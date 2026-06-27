using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RiverFlow.Core
{
    [RequireComponent(typeof(Plant))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantDrawer : MonoBehaviour
    {
        private Plant plant;
        private SpriteRenderer spriteRenderer;

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
                //shrinkFeedback.PlayFeedbacks();
            }
            else
            if(previousState < newState)
            {
                //growFeedback.PlayFeedbacks();
            }
            else
            {
                Debug.LogError("No change in state, c'est cheum");
            }
            previousState = newState;
        }
    }
}
