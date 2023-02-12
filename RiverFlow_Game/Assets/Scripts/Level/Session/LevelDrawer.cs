using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class LevelDrawer : ShaderLink
    {
        [SerializeField] private LevelHandler level;

        [SerializeField] private SpriteRenderer visual;
        private MaterialPropertyBlock propBlock;

        private void Awake()
        {
            render = (Renderer)visual;
        }

        [Button]
        public void UpdateMapVisual()
        {
            visual.size = level.mapData.size;
            UpdateProperty(("_MapTexture", level.mapData.GenerateMapTexture()));
        }
    }
}
