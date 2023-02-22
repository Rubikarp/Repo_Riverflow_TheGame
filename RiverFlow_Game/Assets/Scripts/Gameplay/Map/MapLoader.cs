using UnityEngine;
using RiverFlow.LD;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace RiverFlow.Core
{
    public class MapLoader : MonoBehaviour
    {
        public MapData mapToLoad;
        [Space(10)]
        public LevelHandler levelHandler;
        public LevelDrawer levelDrawer;

        private void Awake() => LoadMap();

        [Button]
        public void LoadMap()
        {
            if (mapToLoad is null) return;

            levelHandler.LoadMap(mapToLoad);
            levelDrawer.UpdateMapVisual(mapToLoad);
        }
    }
}