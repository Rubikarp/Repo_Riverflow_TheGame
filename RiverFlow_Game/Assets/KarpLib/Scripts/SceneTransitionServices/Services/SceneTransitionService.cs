using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    /// <summary>
    /// Orchestrateur principal. À placer sur un GameObject persistant (DontDestroyOnLoad).
    /// Implémente ISceneTransitionService (API publique) et ITransitionExecutor (injecté dans les commandes).
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneTransitionService : PersistentSingleton<SceneTransitionService>, ISceneTransitionService, ITransitionExecutor
    {
        [Header("Effects")]
        [SerializeField] private List<TransitionEffectEntry> _effects;

        [Header("Loading Screen")]
        [SerializeField] private LoadingScreenController _loadingScreen;

        [Header("Events")]
        public UnityEvent<STransitionRequest> OnTransitionStarted;
        public UnityEvent OnTransitionCompleted;

        private readonly SceneHistoryStack _historyStack = new SceneHistoryStack();
        private STransitionRequest _currentRequest;
        private bool _isTransitioning;

        public bool CanGoBack => _historyStack.CanGoBack && !_isTransitioning;
        
        [Button]
        public void RequestTransition(STransitionRequest request)
        {
            _ = TransitionToAsync(request, destroyCancellationToken);
        }
        public async Awaitable TransitionToAsync(STransitionRequest request, CancellationToken ct)
        {
            if (_isTransitioning)
            {
                Debug.LogWarning("[SceneTransitionService] Transition déjà en cours — requête ignorée.");
                return;
            }

            var command = new GoToSceneCommand(request, _currentRequest, this);

            if (request.PushToHistory)
            {
                _historyStack.Push(command);
            }

            await command.ExecuteAsync(ct);
        }

        [Button]
        public void RequestGoBack()
        {
            _ = GoBackAsync(destroyCancellationToken);
        } 
        public async Awaitable GoBackAsync(CancellationToken ct)
        {
            if (!CanGoBack)
            {
                Debug.LogWarning("[SceneTransitionService] GoBack impossible : pile vide ou transition en cours.");
                return;
            }

            var command = _historyStack.Pop();
            await command.UndoAsync(ct);
        }
        /// <summary>
        /// Implémente ITransitionExecutor — appelé par GoToSceneCommand.Execute et GoToSceneCommand.Undo.
        /// </summary>
        public async Awaitable ExecuteTransitionAsync(STransitionRequest request, CancellationToken ct)
        {
            _isTransitioning = true;
            OnTransitionStarted.Invoke(request);

            var effect = ResolveEffect(request.Effect);

            await effect.PlayInAsync(ct);

            if (request.LoadingScreenRequired)
            {
                await _loadingScreen.Show(ct);
            }

            var op = SceneManager.LoadSceneAsync(request.SceneName);
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                if (request.LoadingScreenRequired)
                {
                    _loadingScreen.UpdateProgress(op.progress);
                }

                await Awaitable.NextFrameAsync(ct);
            }

            op.allowSceneActivation = true;

            // Attendre un frame que la scène soit activée.
            await Awaitable.NextFrameAsync(ct);

            _currentRequest = request;

            if (request.LoadingScreenRequired)
            {
                await _loadingScreen.Hide(ct);
            }

            await effect.PlayOutAsync(ct);

            _isTransitioning = false;
            OnTransitionCompleted.Invoke();
        }

        private ITransitionEffect ResolveEffect(ETransitionEffect effectType)
        {
            foreach (var entry in _effects)
            {
                if (entry.Type == effectType && entry.Effect != null)
                {
                    return entry.Effect;
                }
            }

            Debug.LogWarning($"[SceneTransitionService] Effet '{effectType}' non trouvé — fallback sur premier effet disponible.");
            return _effects.Count > 0 ? _effects[0].Effect : null;
        }

        [System.Serializable]
        private class TransitionEffectEntry
        {
            public ETransitionEffect Type;
            public GameObject EffectObject;

            public ITransitionEffect Effect => EffectObject.GetComponents<MonoBehaviour>().ToList().Where(x => x is ITransitionEffect).FirstOrDefault() as ITransitionEffect;
        }
    }
}