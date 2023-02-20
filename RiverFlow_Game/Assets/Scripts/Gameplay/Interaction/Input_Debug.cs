using UnityEngine;
using RiverFlow.Core;

namespace RiverFlow.Gameplay.Interaction
{
    public class Input_Debug : MonoBehaviour
    {
        public LevelHandler map;
        public void onPress(InputMode mode, Vector3 pos)
        {
        }
        public void onMaintain(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);
            switch (mode)
            {
                case InputMode.Dig:
                    map.tileGrid.GetTile(tile).topology = TileTopology.Clay;
                    break;
                case InputMode.Erase:
                    map.tileGrid.GetTile(tile).topology = TileTopology.Sand;
                    break;
                case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                    map.tileGrid.GetTile(tile).topology = TileTopology.Mountain;
                    break;
                default:
                    map.tileGrid.GetTile(tile).topology = TileTopology.Grass;
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
