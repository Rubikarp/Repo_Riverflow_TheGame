using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class DayTime : MonoBehaviour
    {
        [SerializeField] private TimeManager time;

        [field: SerializeField, Header("Parameter")] public int dayIndex { get; private set; }
        public float dayDuration = 3 * 60f;
        [field: SerializeField, ProgressBar("dayDuration", EColor.Yellow)] public float progression;

        public UnityEvent<int> onDayChange = new UnityEvent<int>();

        private void Start()
        {
            dayIndex = Mathf.FloorToInt(time.gameTime / dayDuration);
        }

        private void OnEnable()
        {
            Start();
            time.onGameTimeChange.AddListener(OnTimeUpdate);
        }
        private void OnDisable() => time.onGameTimeChange.RemoveListener(OnTimeUpdate);

        private void OnTimeUpdate(float elipsedTime)
        {
            progression = time.gameTime % dayDuration;

            var newdayIndex = Mathf.FloorToInt(time.gameTime / dayDuration);
            if (dayIndex < newdayIndex)
            {
                dayIndex = newdayIndex;
                onDayChange?.Invoke(dayIndex);
            }
        }

    }
}
