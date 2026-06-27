using UnityEngine;
using System.Threading;

namespace WorldGame.SceneManagement
{
    /// <summary>
    /// Encapsule une transition vers une scène.
    /// Mémorise la requête d'origine pour permettre le Undo (retour arrière).
    /// </summary>
    public class GoToSceneCommand : ISceneCommand
    {
        private readonly STransitionRequest _request;
        private readonly ITransitionExecutor _executor;
 
        // Capturée au moment du Execute ; vide avant le premier appel.
        private STransitionRequest _previousRequest;
        private bool _hasBeenExecuted;
 
        public bool CanUndo => _request.PushToHistory && _hasBeenExecuted && _previousRequest.IsValid;
 
        public GoToSceneCommand(STransitionRequest request, STransitionRequest previousRequest, ITransitionExecutor executor)
        {
            _request = request;
            _previousRequest = previousRequest;
            _executor = executor;
        }
 
        public async Awaitable ExecuteAsync(CancellationToken ct)
        {
            _hasBeenExecuted = true;
            await _executor.ExecuteTransitionAsync(_request, ct);
        }
 
        /// <summary>
        /// Rejoue la requête inverse. PushToHistory forcé à false pour éviter une boucle infinie.
        /// </summary>
        public async Awaitable UndoAsync(CancellationToken ct)
        {
            var backRequest = new STransitionRequest(
                _previousRequest.SceneName,
                _previousRequest.Effect,
                _previousRequest.LoadingScreenRequired,
                pushToHistory: false,
                _previousRequest.Payload);
 
            await _executor.ExecuteTransitionAsync(backRequest, ct);
        }
    }
}