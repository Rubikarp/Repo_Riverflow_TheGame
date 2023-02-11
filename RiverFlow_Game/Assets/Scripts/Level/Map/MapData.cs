using UnityEngine;
using NaughtyAttributes;
using RiverFlow.Core;
using System.Collections;
using System.Collections.Generic;
using Karprod;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.LD
{
    [CreateAssetMenu(fileName = "NewMap", menuName = "LevelDesign/NewMap")]
    public class MapData : ScriptableObject
    {
        public Vector2Int size = new Vector2Int(8, 8);
        public TileTopology[] topology = new TileTopology[8 * 8];
        public TileTopology[,] Topology
        {
            get
            {
                TileTopology[,] result = new TileTopology[size.x, size.y];
                for (int x = 0; x < size.x; x++)
                {
                    for (int y = 0; y < size.y; y++)
                    {
                        result[x, y] = topology[x + ((size.y - y - 1) * size.x)];
                    }
                }
                return result;
            }
        }

        //Accesseurs
        public Vector2Int Size => size;

        [Button]
        public void MapToTextureChannel() => TextureGenerator.Create("newMapTexture", GenerateMapTexture(), TextureType.PNG);

        public Texture2D GenerateMapTexture()
        {
            Texture2D mapTexture = TextureGenerator.Generate(size, true);
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    switch (Topology[x, y])
                    {
                        case TileTopology.Grass:
                            mapTexture.SetPixel(x, y, Color.green);
                            break;
                        case TileTopology.Clay:
                            mapTexture.SetPixel(x, y, Color.blue);
                            break;
                        case TileTopology.Sand:
                            mapTexture.SetPixel(x, y, Color.red);
                            break;
                        case TileTopology.Mountain:
                            mapTexture.SetPixel(x, y, Color.clear);
                            break;
                        default:
                            mapTexture.SetPixel(x, y, Color.clear);
                            break;
                    }

            mapTexture.filterMode = FilterMode.Point;
            mapTexture.wrapMode = TextureWrapMode.Clamp;
            //Save la modification
            mapTexture.Apply();

            return mapTexture;
        }

#if UNITY_EDITOR
        public TileTopology selectedType;
        public Color[] typePalette = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
        {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };
#endif
    }
}