using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using UnityEngine.Events;
using RiverFlow.Gameplay.Interaction;

namespace RiverFlow.Core
{
    public class LinkHandler : MonoBehaviour
    {
        [ReadOnly, SerializeField] private InputHandler input;
        [ReadOnly, SerializeField] private WorldGrid grid;
        //[Required, SerializeField] private MapHandler map;

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

        public void OnPress(InputMode mode, Vector3 pos)
        {
            startSelectTile = grid.PosToTile(pos);
            startSelectTilePos = grid.TileToPos(startSelectTile);
            switch (mode)
            {
                case InputMode.None:
                    break;
                case InputMode.Dig:
                    break;
                case InputMode.Erase:
                    onBreak?.Invoke(startSelectTile);
                    break;
                case InputMode.Cloud:
                    break;
                case InputMode.Lake:
                    break;
                case InputMode.Source:
                    break;
                default:
                    break;
            }
            /*
            switch (mode)
            {

                case InputMode.diging:
                    if (startSelectTile.linkAmount > 2 || startSelectTile.flowOut.Count >= 2)
                    {
                        startSelectTile.flowOut.Add(startSelectTile.flowOut[0]);
                        startSelectTile.flowOut.RemoveAt(0);
                    }
                    break;
                case InputMode.eraser:
                    if (startSelectTile.linkAmount > 0 || startSelectTile.haveElement)
                    {
                        if (lastSelectedTile != startSelectTile)
                        {
                            lastSelectedTile = startSelectTile;
                            onBreak?.Invoke(startSelectTile);
                        }
                    }
                    break;
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
                //Check if i jump a tile
                //TODO : rework 
                if (dragVect.x > (1.5f * grid.cellSize) || dragVect.y > (1.5f * grid.cellSize))
                {
                    dragVect = dragVect.normalized * (1.45f * grid.cellSize);
                }

                endSelectPos = pos;
                endSelectTile = grid.PosToTile(pos);

                switch (mode)
                {
                    case InputMode.Dig:
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


        private void Awake()
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

        private void OnDrawGizmos()
        {
            using (new Handles.DrawingScope())
            {
                Handles.color = Color.blue;
                Handles.DrawLine(startSelectTilePos, dragPos, 5f);
                if (startSelectTilePos != Vector3.zero) Handles.DrawWireDisc(startSelectTilePos, Vector3.back, grid.cellSize / 2f);

            }
        }
    }
}
