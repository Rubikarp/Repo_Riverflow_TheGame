using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow.Core
{
    public class TimeManager : SingletonMonoBehaviour<TimeManager>
    {
        [Header("Parameter")]
        private float _gameSpeed = 1f;
        public float gameSpeed 
        { 
            get => _gameSpeed;

            private set 
            {
                if(isPaused && value > 0f) { onResume?.Invoke(); }
                if(!isPaused && value <= 0f) { onPause?.Invoke(); }
                _gameSpeed = value;
            } 
        }
        [field : SerializeField] public float gameTime { get; private set; }

        public UnityEvent<float> onGameTimeChange = new UnityEvent<float>();
        public UnityEvent onPause = new UnityEvent();
        public UnityEvent onResume = new UnityEvent();

        public bool isPaused { get => gameSpeed <= 0f; }
        public float DeltaSimulTime => Time.deltaTime * _gameSpeed;

        [Button] public void Pause() => gameSpeed = 0f;
        [Button] public void Speedx1() => gameSpeed = 1f;
        [Button] public void Speedx2() => gameSpeed = 2f;
        [Button] public void Speedx3() => gameSpeed = 3f;

        [Button]
        public void ToglePause() => gameSpeed = isPaused ? 0f : 1f;
        public void SetPause(bool state) => gameSpeed = state ? 0f : 1f;
        public void SetSpeed(float speed) => gameSpeed = speed;
        
        public void OnEnable()=> Reset();
        public void Reset()
        {
            gameTime = 0f;
            Speedx1();
        }

        private void Update()
        {
            gameTime += DeltaSimulTime;
            onGameTimeChange?.Invoke(DeltaSimulTime);
        }
    }
}
