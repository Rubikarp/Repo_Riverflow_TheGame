using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RiverFlow.Core
{
    public class InputCheck : MonoBehaviour
    {
        public MapHandler map;
        public void onPress(InputMode mode, Vector3 pos)
        {
        }
        public void onMaintain(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);
            switch (mode)
            {
                case InputMode.Dig:
                    map.topology.Tiles[tile.x, tile.y].type = TileType.Clay;
                    break;
                case InputMode.Erase:
                    map.topology.Tiles[tile.x, tile.y].type = TileType.Sand;
                    break;
                case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                    map.topology.Tiles[tile.x, tile.y].type = TileType.Mountain;
                    break;
                default:
                    map.topology.Tiles[tile.x, tile.y].type = TileType.Grass;
                    break;
            }
        }


        private Color ModeColor(InputMode mode)
        {
            switch (mode)
            {
                case InputMode.None: return Color.grey;
                case InputMode.Dig: return Color.green;
                case InputMode.Erase: return Color.red;
                case InputMode.Cloud: return Color.cyan;
                case InputMode.Lake: return Color.yellow;
                case InputMode.Source: return Color.magenta;
                default: return Color.black;
            }
        }

    }
}
