using System;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlantType", menuName = "Riverflow/NewPlantType")]
    public class PlantType : ScriptableObject
    {
        [Header("Plant Sprite")]
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite deadSprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite agonySprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite babySprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite youngSprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite adultSprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite seniorSprite;
        [SerializeField, ShowAssetPreview(32, 32)] private Sprite fruitSprite;

        [field: Space(10)]
        [field: Header("Plant's flowers")]
        [field: SerializeField] public Sprite[] flowers { get; private set; }
        public Sprite StateSprite(PlantState state)
        {
            switch (state)
            {
                case PlantState.Dead__:
                    return deadSprite;
                case PlantState.Agony_:
                    return agonySprite;
                case PlantState.Baby__:
                    return babySprite;
                case PlantState.Young_:
                    return youngSprite;
                case PlantState.Adult_:
                    return adultSprite;
                case PlantState.Senior:
                    return seniorSprite;
                default:
                    return null;
            }
        }

    }
}
