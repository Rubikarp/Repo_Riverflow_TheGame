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
        public TileType[] topology = new TileType[8 * 8];
        public TileType[,] Topology
        {
            get
            {
                TileType[,] result = new TileType[size.x, size.y];
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
        public Vector2Int Size
        {
            get => size;
        }

#if UNITY_EDITOR
        public TileType selectedType;
        public Color[] typePalette = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
        {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };
#endif
        [Button]
        public void MapToTextureChannel()
        {
            Texture2D mapTexture = TextureGenerator.Generate(size, true);
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    switch (Topology[x, y])
                    {
                        case TileType.Grass:
                            mapTexture.SetPixel(x, y, new Color(0, 1, 0, 1));
                            break;
                        case TileType.Clay:
                            mapTexture.SetPixel(x, y, new Color(0, 0, 1, 1));
                            break;
                        case TileType.Sand:
                            mapTexture.SetPixel(x, y, new Color(1, 0, 0, 1));
                            break;
                        case TileType.Mountain:
                            mapTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                            break;
                        default:
                            mapTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                            break;
                    }
                }
            }
            //Save la modification
            mapTexture.Apply();

            TextureGenerator.Create("newMapTexture", mapTexture, TextureType.PNG);
        }
    }
}