using System;
using UnityEngine;
using RiverFlow.Core;

namespace RiverFlow.Gameplay.Interaction
{
    public class Input_Debug : MonoBehaviour
    {
        public TileMap tileMap;
        public PlantSpawner plantSpawner;
        public ElementSpawner elementSpawner;
        
        [Header("Mode")]
        public bool CreateLake = false;
        public bool CreateCloud = false;
        public bool CreateSource = false;
        [Space]
        public bool CreatePlant = false;
        [Space]
        public bool SetExtraFlow = false;
        public FlowStrenght value = FlowStrenght._00_;
        [Space]
        public bool PaintTopology = false;
        public Topology topology = Topology.Grass;

        public void onPress(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);

            if (CreatePlant)
                plantSpawner.SpawnPlant(tile);

            if(SetExtraFlow)
                tileMap.extraFlow[tileMap.GridPos2ID(tile)] = value;
            
            if(CreateCloud)
                elementSpawner.SpawnCloud(tile);
            if(CreateSource)
                elementSpawner.SpawnSource(tile);
            if(CreateLake)
                elementSpawner.SpawnLake(tile);
        }
        public void onMaintain(InputMode mode, Vector3 pos)
        {
            var tile = WorldGrid.Instance.PosToTile(pos);

            if (PaintTopology)
                tileMap.SetTopology(tile, topology);
        }
    }
}
