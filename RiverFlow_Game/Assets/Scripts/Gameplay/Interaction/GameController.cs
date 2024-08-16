using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using RiverFlow.Gameplay.Interaction;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RiverFlow.Core
{
    public class GameController : MonoBehaviour
    {
        [ReadOnly, SerializeField] private InputHandler input;
        [ReadOnly, SerializeField] private WorldGrid grid;
        [Required, SerializeField] private TileMap map;

        [Header("Digging")]
        [SerializeField] public Vector2Int startSelectTile;
        [SerializeField] public Vector3 startSelectTilePos;
        [Space(10)]
        [SerializeField] private Vector2Int endSelectTile;
        [SerializeField] public Vector3 endSelectPos;
        [Space(10)]
        [SerializeField] public Vector3 dragPos;
        [SerializeField] public Vector3 dragVect;

        [Header("Event")]
        public UnityEvent<Vector2Int, Vector2Int> onLink;
        public UnityEvent<Vector2Int> onBreak;
        public UnityEvent<Vector2Int, InputMode> onElementSpawn;

        public void OnPress(InputMode mode, Vector3 pos)
        {
            startSelectTile = grid.PosToTile(pos);
            startSelectTilePos = grid.TileToPos(startSelectTile);
            switch (mode)
            {
                case InputMode.None | InputMode.Dig:
                    break;
                case InputMode.Erase:
                    onBreak?.Invoke(startSelectTile);
                    break;
                case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                    onElementSpawn?.Invoke(startSelectTile, mode);
                    break;
                default:
                    break;
            }
            /*
            switch (mode)
            {

                ///////
                case InputMode.source:
                    if (inventory.sourcesAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                    {
                        element.SpawnWaterSourceAt(grid.PosToTile(input.GetHitPos()));
                        inventory.sourcesAmmount--;
                        input.ChangeMode(InputMode.diging);
                    }
                    break;
                case InputMode.cloud:
                    if (inventory.cloudsAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                    {
                        if (startSelectTile.flowOut.Count < 2 && startSelectTile.flowIn.Count < 2)
                        {
                            if (startSelectTile.ReceivedFlow() > FlowStrenght._00_) //
                            {
                                element.SpawnCloudAt(grid.PosToTile(input.GetHitPos()));
                                inventory.cloudsAmmount--;
                                input.ChangeMode(InputMode.diging);
                            }
                        }
                    }
                    break;
                case InputMode.lake:
                    if (inventory.lakesAmmount > 0 && !startSelectTile.haveElement && startSelectTile.type != TileType.mountain)// 
                    {
                        if (startSelectTile.linkAmount == 2)
                        {
                            if (startSelectTile.ReceivedFlow() > FlowStrenght._00_) //
                            {
                                List<GameTile> testedTileLinks = startSelectTile.GetLinkedTile();
                                GameTile previousTile = startSelectTile.GetNeighbor(startSelectTile.flowIn[0]);
                                GameTile previousLakeTile = previousTile.GetNeighbor(previousTile.flowIn[0]);

                                //check if vertical
                                if ((testedTileLinks[0] == startSelectTile.neighbors[1] && testedTileLinks[1] == startSelectTile.neighbors[5])
                                 || (testedTileLinks[0] == startSelectTile.neighbors[5] && testedTileLinks[1] == startSelectTile.neighbors[1]))
                                {
                                    element.SpawnLakeAt(startSelectTile.gridPos, vertical: true, previousLakeTile);
                                    inventory.lakesAmmount--;
                                    input.ChangeMode(InputMode.diging);
                                }
                                //check if horizontal
                                else
                                if ((testedTileLinks[0] == startSelectTile.neighbors[3] && testedTileLinks[1] == startSelectTile.neighbors[7])
                                 || (testedTileLinks[0] == startSelectTile.neighbors[7] && testedTileLinks[1] == startSelectTile.neighbors[3]))
                                {
                                    element.SpawnLakeAt(startSelectTile.gridPos, vertical: false, previousLakeTile);
                                    inventory.lakesAmmount--;
                                    input.ChangeMode(InputMode.diging);
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            */
        }
        public void OnMaintain(InputMode mode, Vector3 pos)
        {
            dragPos = pos;
            dragVect = (dragPos - startSelectTilePos);

            //Check drag dist
            if (Mathf.Abs(dragVect.x) > grid.cellSize || Mathf.Abs(dragVect.y) > grid.cellSize)
            {
                //Si je d�passe de plus d'une case d'�cart
                if (dragVect.magnitude > (1.5f * grid.cellSize))
                {
                    dragVect = dragVect.normalized * (1.5f * grid.cellSize);
                }
                //Check la ou je touche
                endSelectPos = startSelectTilePos + dragVect;
                endSelectTile = grid.PosToTile(endSelectPos);

                switch (mode)
                {
                    case InputMode.Dig:
                        // Check if there is a plant on the tile
                        if(map.GetPlant(startSelectTile) != null
                            || map.GetPlant(endSelectTile) != null)
                            break;
                        // Check if the tile is a mountain
                        if(map.GetTopology(startSelectTile) == Topology.Mountain
                            || map.GetTopology(endSelectTile) == Topology.Mountain)
                        {
                            // Check Inventaire

                        }

                        onLink?.Invoke(startSelectTile, endSelectTile);
                        break;
                    case InputMode.Erase:
                        onBreak?.Invoke(endSelectTile);
                        break;
                    case InputMode.Cloud | InputMode.Lake | InputMode.Source:
                        //Nothing
                        break;
                    case InputMode.None: break;
                    default: break;
                }

                startSelectTile = endSelectTile;
                startSelectTilePos = grid.TileToPos(endSelectTile);
            }
        }
        public void OnRelease(InputMode mode, Vector3 pos)
        {
            //Clear
            startSelectTile = Vector2Int.zero;
            startSelectTilePos = Vector3.zero;
            //
            endSelectTile = Vector2Int.zero;
            endSelectPos = Vector3.zero;
            //
            dragPos = Vector3.zero;
            dragVect = Vector3.zero;
        }

        private void Start()
        {
            input = InputHandler.Instance;
            grid = WorldGrid.Instance;
        }
        void OnEnable()
        {
            input.onInputPress.AddListener(OnPress);
            input.onInputMaintain.AddListener(OnMaintain);
            input.onInputRelease.AddListener(OnRelease);
        }
        void OnDisable()
        {
            input.onInputPress.RemoveListener(OnPress);
            input.onInputMaintain.RemoveListener(OnMaintain);
            input.onInputRelease.RemoveListener(OnRelease);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            using (new Handles.DrawingScope())
            {
                Handles.color = Color.blue;
                Handles.DrawLine(startSelectTilePos, dragPos, 5f);
                if (startSelectTilePos != Vector3.zero) Handles.DrawWireDisc(startSelectTilePos, Vector3.back, grid.cellSize / 2f);

            }
        }
#endif
    }
}
