using NaughtyAttributes;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;
using System;

namespace RiverFlow.Core
{
    [System.Serializable]
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
        public static UnityEvent<Plant> onPlantDeath;

        private TileMap map;
        private TimeManager time;

        [Header("Plant Data")]
        [SerializeField]
        private PlantState _currentState = PlantState.Baby__;
        public PlantState CurrentState
        {
            get => _currentState;
            private set
            {
                if (value == _currentState) return;

                _currentState = value;
                onStateChange?.Invoke(value);
            }
        }
        public UnityEvent onInitEnd;
        public UnityEvent<PlantState> onStateChange;

        [SerializeField, ReadOnly] private float scoreTime = 0f;
        public float IrrigationTime { get; private set; } = 1f;
        [SerializeField, Range(0, 3)] private float growTime = 2f;

        [field: Header("Position Data")]
        [field: SerializeField, ReadOnly] public Topology TopologyOn { get; private set;}
        [SerializeField] private Vector2Int[] neighborPoses;

        private bool IsAlive => CurrentState != PlantState.Dead__;
        public FlowStrenght irrigationLvl => map.GetIrrigation(gridPos);
        public bool IsIrrigated { get; private set; } = false;
        public Plant(Vector2Int pos) : base(pos)
        {
            TopologyOn = map.GetTopology(gridPos);
            neighborPoses = GetNeighbors(pos);
        }

        //public Coroutine ScoreLoop
        public UnityEvent<PlantState> onScore;

        public void Init(Vector2Int pos)
        {
            gridPos = pos;

            map = TileMap.Instance;
            time = TimeManager.Instance;

            TopologyOn = map.GetTopology(gridPos);
            neighborPoses = GetNeighbors(gridPos);

            _currentState = PlantState.Baby__;
            IrrigationTime = 1f;

            onInitEnd?.Invoke();
        }

        public void Update()
        {
            if (!IsAlive) return;

            ScoreGeneration();

            CheckIrrigation();
            if (IsIrrigated)
            {
                //grow
                IrrigationTime += time.DeltaSimulTime * (1f / growTime);
                if (IrrigationTime > 1f)
                {
                    IrrigationTime %= 1f;
                    StateUp();
                }
            }
            else
            {
                //die
                IrrigationTime -= time.DeltaSimulTime * (1f / growTime);
                if (IrrigationTime <= 0f)
                {
                    IrrigationTime += 1f;
                    StateDown();
                }
            }
        }

        private void StateUp()
        {
            if (CurrentState == PlantState.Dead__) return;
            if (CurrentState < PlantState.Senior)
            {
                CurrentState++;
            }
            else
            {
                if (CurrentState == PlantState.Senior)
                //&& CheckSpecialCondition())
                {

                }
            }
        }
        private void StateDown()
        {
            if (CurrentState > PlantState.Baby__)
            {
                CurrentState--;
            }
            else
            {

                CurrentState = PlantState.Dead__;
                onPlantDeath?.Invoke(this);
            }
        }


        private void ScoreGeneration()
        {
            scoreTime += time.DeltaSimulTime;
            if (scoreTime >= 1f)
            {
                scoreTime = 0f;
                onScore.Invoke(CurrentState);
            }
        }

        private void CheckIrrigation()
        {
            switch (TopologyOn)
            {
                case Topology.Grass:
                    IsIrrigated = irrigationLvl >= FlowStrenght._25_;
                    break;
                case Topology.Clay:
                    IsIrrigated = irrigationLvl >= FlowStrenght._50_;
                    break;
                case Topology.Sand:
                    IsIrrigated = irrigationLvl >= FlowStrenght._75_;
                    break;
                default:
                    IsIrrigated = false;
                    break;
            }
        }

        private Vector2Int[] GetNeighbors(Vector2Int pGridPos)
        {
            Vector2Int testedPos = pGridPos;
            List<Vector2Int> neighbors = new(8);

            //square pattern around the tested position
            ///0 1 2
            ///3 X 4
            ///5 6 7
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    testedPos = pGridPos + new Vector2Int(x, y);
                    if (testedPos == pGridPos) continue;
                    if (testedPos.x < 0) continue;
                    if (testedPos.y < 0) continue;
                    if (testedPos.x < map.Size.x) continue;
                    if (testedPos.y < map.Size.y) continue;
                    neighbors.Add(testedPos);
                }
            return neighbors.ToArray();
        }
    }
}
