using System;
using UnityEngine;
using RiverFlow.Core;

namespace RiverFlow.Gameplay.Interaction
{
    public class Input_Debug : MonoBehaviour
    {
        public TileMap tileMap;
        public PlantSpawner plantSpawner;

        public bool SetExtraFlow = false;
        public bool CreatePlant = false;
        public FlowStrenght value;


        public void onPress(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);

            if (CreatePlant)
                plantSpawner.SpawnPlant(tile);
            if(SetExtraFlow)
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
