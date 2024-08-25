using UnityEngine;

namespace RiverFlow.Core
{
    public class ElementSpawner : MonoBehaviour
    {
        [SerializeField] private WorldGrid grid;
        [SerializeField] private TileMap map;
        [SerializeField] private TimeManager time;
        [Space(10)]
        [SerializeField] private LevelInventory inventory;
        [Space(10)]
        [SerializeField] private Lake lakePrefab;
        [SerializeField] private Cloud cloudPrefab;
        [SerializeField] private Source sourcePrefab;

        private void Start()
        {
            map = TileMap.Instance;
            grid = WorldGrid.Instance;
            time = TimeManager.Instance;
        }

        public bool CanSpawnCloud(Vector2Int spawnPos)
        {
            if (inventory.cloudsAmmount <= 0)
            {
                Debug.Log("No clouds in inventory");
                return false;
            }
            if(map.GetElement(spawnPos) != null)
            {
                Debug.Log("There is already an element at " + spawnPos);
                return false;
            }
            if(map.GetPlant(spawnPos) != null)
            {
                Debug.Log("There is already a plant at " + spawnPos);
                return false;
            }
            if (map.GetTopology(spawnPos) == Topology.Mountain)
            {
                Debug.Log("Can't spawn a cloud on a mountain");
                return false;
            }
            return true;
        }
        public void SpawnCloud(Vector2Int spawnPos)
        {
            if (!CanSpawnCloud(spawnPos))
            {
                Debug.Log("Can't spawn an element at " + spawnPos);
                return;
            }
            Cloud cloud = Instantiate(cloudPrefab, grid.TileToPos(spawnPos), Quaternion.identity, transform);
            //rename gameobject with pos
            cloud.gameObject.name = "Cloud_" + spawnPos.x + "/" + spawnPos.y;

            //add element to map
            map.SetElement(spawnPos, cloud);

            //remove from inventory
            inventory.cloudsAmmount--;
        }

        public bool CanSpawnLake(Vector2Int spawnPos)
        {
            if (inventory.lakesAmmount <= 0)
            {
                Debug.Log("No lakes in inventory");
                return false;
            }
            if (map.GetElement(spawnPos) != null)
            {
                Debug.Log("There is already an element at " + spawnPos);
                return false;
            }
            if (map.GetPlant(spawnPos) != null)
            {
                Debug.Log("There is already a plant at " + spawnPos);
                return false;
            }
            if (map.GetTopology(spawnPos) == Topology.Mountain)
            {
                Debug.Log("Can't spawn a lake on a mountain");
                return false;
            }
            return true;
        }
        public void SpawnLake(Vector2Int spawnPos)
        {
            if (!CanSpawnLake(spawnPos))
            {
                Debug.Log("Can't spawn an element at " + spawnPos);
                return;
            }
            Lake lake = Instantiate(lakePrefab, grid.TileToPos(spawnPos), Quaternion.identity, transform);
            //rename gameobject with pos
            lake.gameObject.name = "Lake_" + spawnPos.x + "/" + spawnPos.y;

            //add element to map
            map.SetElement(spawnPos, lake);

            //remove from inventory
            inventory.lakesAmmount--;
        }

        public bool CanSpawnSource(Vector2Int spawnPos)
        {
            if (inventory.sourcesAmmount <= 0)
            {
                Debug.Log("No sources in inventory");
                return false;
            }
            if (map.GetElement(spawnPos) != null)
            {
                Debug.Log("There is already an element at " + spawnPos);
                return false;
            }
            if (map.GetPlant(spawnPos) != null)
            {
                Debug.Log("There is already a plant at " + spawnPos);
                return false;
            }
            if (map.GetTopology(spawnPos) == Topology.Mountain)
            {
                Debug.Log("Can't spawn a source on a mountain");
                return false;
            }
            return true;
        }
        public void SpawnSource(Vector2Int spawnPos)
        {
            if (!CanSpawnSource(spawnPos)) return;
            
            Source source = Instantiate(sourcePrefab, grid.TileToPos(spawnPos), Quaternion.identity, transform);
            //rename gameobject with pos
            source.gameObject.name = "Source_" + spawnPos.x + "/" + spawnPos.y;

            //add element to map
            map.SetElement(spawnPos, source);

            //remove from inventory
            inventory.sourcesAmmount--;
        }

        public void EraseElement(Vector2Int spawnPos)
        {
            Element element = map.GetElement(spawnPos);
            if(element == null)
            {
                Debug.Log("No element to erase at " + spawnPos);
                return;
            }
            map.SetElement(spawnPos, null);

            //Regain in inventory
            switch(element)
            {
                case Lake lake:
                    inventory.lakesAmmount++;
                    break;
                case Cloud cloud:
                    inventory.cloudsAmmount++;
                    break;
                case Source source:
                    inventory.sourcesAmmount++;
                    break;
            }
            Destroy(element.gameObject);
        }
    }
}
