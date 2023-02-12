using Karprod;
using UnityEngine;
using RiverFlow.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.LD
{
    [CreateAssetMenu(fileName = "NewMap", menuName = "LevelDesign/NewMap")]
    public class MapData : ScriptableObject
    {
        public Vector2Int Size { get => size; set => size = value; }
        public Vector2Int size = new Vector2Int(8, 8);

        public TileTopology GetTopology(int x, int y) => topology[x + (size.y - 1 - y) * size.x];
        public TileTopology[] topology = new TileTopology[8 * 8];

        //Accesseurs

        [Button]
        public void MapToTextureChannel() => TextureGenerator.Create("newMapTexture", GenerateMapTexture(), TextureType.PNG);

        public Texture2D GenerateMapTexture()
        {
            Texture2D mapTexture = TextureGenerator.Generate(size, true);
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    switch (GetTopology(x, y))
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
            //Save la modification
            mapTexture.Apply();

            mapTexture.filterMode = FilterMode.Point;
            mapTexture.wrapMode = TextureWrapMode.Clamp;

            return mapTexture;
        }
        public void LoadMapTexture(Texture2D texture)
        {
            size = new Vector2Int(texture.width, texture.height);
            topology = new TileTopology[size.x * size.y];

            Color readPixel;
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    readPixel = texture.GetPixel(x, y).linear;
                    Debug.Log(readPixel);

                    if (readPixel.a < .1) topology[x + ((size.y - 1 - y) * size.x)] = TileTopology.Mountain;
                    else if (readPixel.r > .8) topology[x + ((size.y - 1 - y) * size.x)] = TileTopology.Sand;
                    else if (readPixel.g > .8) topology[x + ((size.y - y - 1) * size.x)] = TileTopology.Grass;
                    else if (readPixel.b > .8) topology[x + ((size.y - 1 - y) * size.x)] = TileTopology.Clay;
                    else topology[x + (y * size.x)] = TileTopology.Mountain;
                }
            }
        }

#if UNITY_EDITOR
        public TileTopology selectedType;
        public Color[] typePalette = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
        {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };
#endif
    }
}