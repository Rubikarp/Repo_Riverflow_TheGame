using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldGame.SceneManagement
{
    [Serializable]
    public struct STransitionRequest
    {
        [Scene]
        public string SceneName;
        public ETransitionEffect Effect;
        public bool LoadingScreenRequired;
 
        /// <summary>
        /// Si false, la transition n'est pas poussée dans l'historique (splash, cinématique, etc.).
        /// </summary>
        public bool PushToHistory;
 
        /// <summary>
        /// Payload optionnel transmis à la scène de destination via ScenePayloadChannel.
        /// </summary>
        public ScriptableObject Payload;
 
        public STransitionRequest(
            string sceneName,
            ETransitionEffect effect = ETransitionEffect.Fade,
            bool loadingScreenRequired = false,
            bool pushToHistory = true,
            ScriptableObject payload = null)
        {
            SceneName = sceneName;
            Effect = effect;
            LoadingScreenRequired = loadingScreenRequired;
            PushToHistory = pushToHistory;
            Payload = payload;
        }
 
        public static STransitionRequest Invalid => new STransitionRequest(string.Empty, pushToHistory: false);
 
        public bool IsValid => !string.IsNullOrEmpty(SceneName);
    }
}