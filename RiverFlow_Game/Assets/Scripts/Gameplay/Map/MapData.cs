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

        // Define an indexer (https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/?redirectedfrom=MSDN)
        public Topology[] topology = new Topology[8 * 8];
        public Topology this[Vector2Int pos] { get { return this[pos.x, pos.y]; } }
        public Topology this[int x, int y] { get { return topology[x + (y * (size.x-1))]; } }


        [Button]
        public void MapToTextureChannel() => TextureGenerator.Create("newMapTexture", GenerateMapTexture(), TextureType.PNG);

        public Texture2D GenerateMapTexture()
        {
            Texture2D mapTexture = TextureGenerator.Generate(size, true);
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    switch (this[x, y])
                    {
                        case Topology.Grass:
                            mapTexture.SetPixel(x, y, Color.green);
                            break;
                        case Topology.Clay:
                            mapTexture.SetPixel(x, y, Color.blue);
                            break;
                        case Topology.Sand:
                            mapTexture.SetPixel(x, y, Color.red);
                            break;
                        case Topology.Mountain:
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
            topology = new Topology[(size.x) * (size.y + 1)];

            Color readPixel;
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                {
                    readPixel = texture.GetPixel(x, y).linear;
                    Debug.Log(readPixel);

                    if (readPixel.a < .1)
                        topology[x + (y * (size.x - 1))] = Topology.Mountain;
                    else if (readPixel.r > .8)
                        topology[x + (y * (size.x - 1))] = Topology.Sand;
                    else if (readPixel.g > .8)
                        topology[x + (y * (size.x - 1))] = Topology.Grass;
                    else if (readPixel.b > .8)
                        topology[x + (y * (size.x - 1))] = Topology.Clay;
                    else
                        topology[x + (y * (size.x - 1))] = Topology.Mountain;
                }

        }

#if UNITY_EDITOR
        public Topology selectedType;
        public Color[] typePalette = new Color[5 /*System.Enum.GetValues(typeof(TileType)).Length*/]
        {Color.magenta, Color.green, Color.red, Color.yellow, Color.grey };
#endif
    }
}