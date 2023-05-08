using System;
using UnityEngine;
using RiverFlow.Core;

namespace RiverFlow.Gameplay.Interaction
{
    public class Input_Debug : MonoBehaviour
    {
        public TileMap tileMap;
        public FlowStrenght value;

        public void onPress(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);
            tileMap.extraFlow[tileMap.GridPos2ID(tile)] = value;

        }
        public void onMaintain(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);
            /*
            switch (mode)
            {
                case InputMode.Dig:
                    tileMap.topology[tileMap.GridPos2ID(tile)] = Topology.Clay;
                    break;
                case InputMode.Erase:
                    tileMap.topology[tileMap.GridPos2ID(tile)] = Topology.Clay;
                    break;
                case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                    tileMap.topology[tileMap.GridPos2ID(tile)] = Topology.Clay;
                    break;
                default:
                    tileMap.topology[tileMap.GridPos2ID(tile)] = Topology.Clay;
                    break;
            }*/
        }
    }
}
