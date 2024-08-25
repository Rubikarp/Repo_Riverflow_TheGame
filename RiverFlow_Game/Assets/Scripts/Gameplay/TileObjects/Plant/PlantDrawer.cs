using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System;

namespace RiverFlow.Core
{
    [RequireComponent(typeof(Plant))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantDrawer : MonoBehaviour
    {
        private Plant plant;
        private SpriteRenderer spriteRenderer;
        public MMF_Player growFeedback;
        public MMF_Player shrinkFeedback;

        [Header("Visual")]
        private SpriteRenderer sprRender;
        public PlantVisualData visuals;

        private PlantState previousState = PlantState.Baby__;

        private void Awake()
        {
            plant = GetComponent<Plant>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void OnEnable()
        {
            visuals = PlantVisualData.Instance;

            plant.onStateChange.AddListener(OnStateChange);
            plant.onInitEnd.AddListener(Init);
        }
        private void OnDisable()
        {
            plant.onStateChange.RemoveListener(OnStateChange);
            plant.onInitEnd.RemoveListener(Init);
        }

        private void Init()
        {
            spriteRenderer.sprite = visuals.GetSprite(plant.CurrentState, plant.TopologyOn);
        }
        private void OnStateChange(PlantState newState)
        {
            spriteRenderer.sprite = visuals.GetSprite(newState, plant.TopologyOn);
            if(previousState > newState)
            {
                shrinkFeedback.PlayFeedbacks();
            }
            else
            if(previousState < newState)
            {
                growFeedback.PlayFeedbacks();
            }
            else
            {
                Debug.LogError("No change in state, c'est cheum");
            }
            previousState = newState;
        }
    }
}
