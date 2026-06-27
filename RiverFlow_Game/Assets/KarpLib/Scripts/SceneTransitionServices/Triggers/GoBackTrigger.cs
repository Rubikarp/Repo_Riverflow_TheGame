using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    public class GoBackTrigger : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField, ReadOnly] private SceneTransitionService _sceneTransitionService;
        
        private void Start()
        {
            _sceneTransitionService = SceneTransitionService.Instance;
        }

        public bool CanGoBack => _sceneTransitionService.CanGoBack;
        public void GoBack()
        {
            if (!CanGoBack)
            {
                Debug.LogWarning("Cannot go back, no previous scene available");
                return;
            }
            _sceneTransitionService.RequestGoBack();
        }
        public void CancelGoBack()
        {
            //_sceneTransitionService.CancelGoBack();
        }
    }
}