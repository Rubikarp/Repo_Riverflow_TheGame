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
                    map.tileGrid.SetTopo(Topology.Clay, tile);
                    break;
                case InputMode.Erase:
                    map.tileGrid.SetTopo(Topology.Sand, tile);
                    break;
                case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                    map.tileGrid.SetTopo(Topology.Mountain, tile);
                    break;
                default:
                    map.tileGrid.SetTopo(Topology.Grass, tile);
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
