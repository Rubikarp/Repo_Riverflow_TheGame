using UnityEngine;
using RiverFlow.LD;
using NaughtyAttributes;

namespace RiverFlow.Core
{
    public class MapDrawer : ShaderLink
    {
        [SerializeField] private WorldGrid grid;
        [SerializeField] private SpriteRenderer visual;

        public void UpdateMapVisual(MapData mapData)
        {
            render = visual;

            visual.size = (Vector2)mapData.Size * grid.cellSize;
            UpdateProperty(("_MapTexture", mapData.GenerateMapTexture()));

            ///alt
            //visual.sprite = Sprite.Create(mapData.GenerateMapTexture(), new Rect(Vector2.zero, mapData.size), Vector2.one * 0.5f, grid.cellSize);
        }
    }
}
