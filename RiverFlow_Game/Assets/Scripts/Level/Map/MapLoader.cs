using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiverFlow.LD;

namespace RiverFlow.Core
{
    public class MapLoader : MonoBehaviour
    {
        public MapData mapToLoad;
        public SpriteRenderer background;
        void Awake()
        {
            LevelHandler.Instance.LoadMap(mapToLoad);
            background.sprite = Sprite.Create(mapToLoad.GenerateMapTexture(), new Rect(Vector2.zero, mapToLoad.size), Vector2.one * .5f, 1f);
        }
    }
}
