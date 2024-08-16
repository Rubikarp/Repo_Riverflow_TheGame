using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace RiverFlow.Core
{
    public class PlantSpawner : MonoBehaviour
    {
        [SerializeField] private WorldGrid grid;
        [SerializeField] private TileMap map;
        [SerializeField] private TimeManager time;
        [Space(10)]
        [SerializeField] private Plant plantPrefab;

        private void Start()
        {
            map = TileMap.Instance;
            grid = WorldGrid.Instance;
            time = TimeManager.Instance;
        }
        public bool CanSpawnAPlant(Vector2Int spawnPos)
        {
            if(map.GetLinkAmount(spawnPos) > 0)
            {
                return false;
            }
            if(map.GetElement(spawnPos) != null)
            {
                return false;
            }
            if(map.GetPlant(spawnPos) != null)
            {
                return false;
            }
            if (map.GetTopology(spawnPos) == Topology.Mountain)
            {
                return false;
            }
            return true;
        }
        public void SpawnPlant(Vector2Int spawnPos)
        {
            if(!CanSpawnAPlant(spawnPos))
            {
                Debug.Log("Can't spawn a plant at " + spawnPos);
                return;
            }
            Plant plant = Instantiate(plantPrefab, grid.TileToPos(spawnPos), Quaternion.identity, transform);
            plant.Init(spawnPos);

            //rename gameobject with pos
            plant.gameObject.name = "Plant_" + spawnPos.x + "/" + spawnPos.y;

            //add plant to map
            map.SetPlant(spawnPos, plant);
        }
        public void ErasePlant(Vector2Int spawnPos)
        {
            Plant plant = map.GetPlant(spawnPos);
            if(plant == null)
            {
                Debug.Log("No plant to erase at " + spawnPos);
                return;
            }
            map.SetPlant(spawnPos, null);
            Destroy(plant.gameObject);
        }

    }
}
