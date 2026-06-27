using UnityEngine;
using Sirenix.OdinInspector;
using Scene = NaughtyAttributes.SceneAttribute;

namespace WorldGame.SceneManagement
{
    public class TransitionTrigger : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField, NaughtyAttributes.ReadOnly] private SceneTransitionService _sceneTransitionService;
        
        [Header("Config")]
        [SerializeField, Scene] private string _targetSceneName;
        [SerializeField] private ETransitionEffect _effect = ETransitionEffect.Fade;
        [SerializeField] private bool _loadingScreenRequired;
        [SerializeField] private bool _pushToHistory = true;
        [SerializeField] private ScriptableObject _payload;
 
        private void Start()
        {
            _sceneTransitionService = SceneTransitionService.Instance;
        }
        
        [Button]
        public void TriggerTransition()
        {
            var request = new STransitionRequest(
                _targetSceneName,
                _effect,
                _loadingScreenRequired,
                _pushToHistory,
                _payload);
            
            _sceneTransitionService.RequestTransition(request);
        }
    }
}