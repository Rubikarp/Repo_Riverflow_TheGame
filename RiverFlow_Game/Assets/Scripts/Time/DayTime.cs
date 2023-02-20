using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow.Core
{
    public class DayTime : MonoBehaviour
    {
        private TimeManager time;

        [Header("Parameter")]
        public int _dayIndex = 0;
        public int dayIndex { get=>_dayIndex; private set =>_dayIndex = value; }
        public float dayDuration = 3 * 60f;

        public UnityEvent onDayChange;

        private void Awake()
        {
            time = TimeManager.Instance;
        }

        void OnEnable()
        {
            time.onGameTimeChange.AddListener(OnTimeUpdate);
            _dayIndex = Mathf.FloorToInt(time.gameTime / dayDuration);
        }
        private void OnDisable()
        {
            time.onGameTimeChange.RemoveListener(OnTimeUpdate);
        }

        private void OnTimeUpdate()
        {
            var newdayIndex = Mathf.FloorToInt(time.gameTime / dayDuration);
            if(dayIndex < newdayIndex)
            {
                dayIndex = newdayIndex;
                onDayChange?.Invoke();
            }
        }
    }
}
