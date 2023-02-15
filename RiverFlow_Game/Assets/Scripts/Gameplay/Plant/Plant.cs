using UnityEngine;

namespace RiverFlow.Core
{
    public enum PlantState
    {
        Dead__ = 0,
        Agony_ = 1,
        Baby__ = 2,
        Young_ = 3,
        Adult_ = 4,
        Senior = 5,
    }

    public class Plant : TileObject
    {
        [Header("Plant Data")]
        public PlantState currentState = PlantState.Young_;
        private bool IsAlive => currentState != PlantState.Dead__;

        public Plant() : base(1) { }

        //public Coroutine ScoreLoop
        //public UnityEvent<PlantState> onScore;
    }
}
