using UnityEngine;
using RiverFlow.LD;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class MapLoader : MonoBehaviour
    {
        public MapData mapToLoad;
        [Space(10)]
        public TileMap level;
        public MapDrawer levelDrawer;

        private void Awake() => LoadMap();

        [Button]
        public void LoadMap()
        {
            if (mapToLoad is null) return;

            level.LoadMap(mapToLoad);
            levelDrawer.UpdateMapVisual(mapToLoad);
        }
    }
}
