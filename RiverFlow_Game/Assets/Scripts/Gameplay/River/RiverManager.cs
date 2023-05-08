using NaughtyAttributes;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public class RiverManager : MonoBehaviour
    {
        [SerializeField, Required] WorldGrid grid;
        [SerializeField, Required] TileMap map;
        [SerializeField, Required] TimeManager time;
        [SerializeField, Required] LinkHandler link;

        [SerializeField, Required] River riverPrefab;
        [SerializeField] List<River> allRiver;

        private void OnEnable()
        {
            link.onLink.AddListener(OnLink);
            link.onBreak.AddListener(OnBreak);
        }
        private void OnDisable()
        {
            link.onLink.RemoveListener(OnLink);
            link.onBreak.RemoveListener(OnBreak);
        }
        private void FixedUpdate()
        {
            foreach (var river in allRiver)
            {
                river.Refresh();
            }
        }

        public void OnLink(Vector2Int startTile, Vector2Int endTile)
        {
            LinkConfirmed(startTile, endTile);
            //inventory.digAmmount--;
            if (!time.isPaused)
            {
                //map.WaterStep();
            }
        }
        private void LinkConfirmed(Vector2Int startTile, Vector2Int endTile)
        {
            switch (map.GetLinkAmount(startTile))
            {
                case 0:
                    switch (map.GetLinkAmount(endTile))
                    {
                        case 0: //in a void
                            Link0To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            Link0To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            Link0To2(startTile, endTile);
                            break;
                    }
                    break;
                case 1:
                    switch (map.GetLinkAmount(endTile))
                    {
                        case 0: //extending the end canal
                            Link1To0(startTile, endTile);
                            break;
                        case 1: //Merging the canals
                            Link1To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            Link1To2(startTile, endTile);
                            break;
                    }
                    break;
                default: // 2 ou +
                    switch (map.GetLinkAmount(endTile))
                    {
                        case 0: //in a void
                            Link2To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            Link2To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            Link2To2(startTile, endTile);
                            break;
                    }
                    break;
            }
        }
        private void OnBreak(Vector2Int erasedTile)
        {
            int tileID = map.GridPos2ID(erasedTile);

            if (map.element[tileID] != null && !(map.element[tileID] is Source) && map.plant[tileID] != null)
            {
                //ErasedElement(erasedTile);
            }
            else
            {
                //ErasedRiverInTile(erasedTile);
            }
            if (!time.isPaused)
            {
                //map.WaterStep();
            }
        }
        private void ErasedRiverInTile(Vector2Int erasedTile)
        {
            //When erasing river not here but riverIn And RiverOut value is tile GridPos !!!

            int linkCount = map.GetLinkAmount(erasedTile);
            int erasedTileID = map.GridPos2ID(erasedTile);
            //inventory.digAmmount += linkCount;

            //TODO : Mountain check

            List<River> tileRivers = map.rivers[erasedTileID];
            List<Vector2Int> impactedTiles = map.riverIn[erasedTileID];
            impactedTiles.AddRange(map.riverOut[erasedTileID]);
            for (int i = 0; i < impactedTiles.Count; i++)
            {
                impactedTiles[i] += erasedTile;
            }
            impactedTiles = impactedTiles.Where(tile => map.GetLinkAmount(tile) >= 2).ToList();

            //foreach (var river in tileRivers)
            for (int i = 0; i < tileRivers.Count; i++)
            {
                if (erasedTile == tileRivers[i].startNode || erasedTile == tileRivers[i].endNode)
                {
                    tileRivers[i].Shorten(erasedTile);
                }
                else
                {
                    var river = tileRivers[i];
                    var newRivers = tileRivers[i].Break(erasedTile);

                    //Suppr old river
                    allRiver.Remove(river);
                    river.UnlinkToGrid();
                    Destroy(river.gameObject);
                    i--;

                    //allRiver add
                    if(newRivers.Item1.Count >= 2)
                    {
                        var addRiver = CreateRiver(newRivers.Item1);
                        addRiver.LinkToGrid();
                        allRiver.Add(addRiver);
                    }
                    if (newRivers.Item2.Count >= 2)
                    {
                        var addRiver = CreateRiver(newRivers.Item2);
                        addRiver.LinkToGrid();
                        allRiver.Add(addRiver);
                    }
                }
            }

            //TODO : Check impacted Tile
            foreach (var impactedTile in impactedTiles)
            {
                if(map.rivers[map.GridPos2ID(impactedTile)].Count >= 2)
                {
                    River river1 = map.rivers[map.GridPos2ID(impactedTile)][0];
                    River river2 = map.rivers[map.GridPos2ID(impactedTile)][1];

                    if (impactedTile == river1.endNode && impactedTile == river2.startNode)
                    {
                        river1.Merge(river2);
                        allRiver.Remove(river2);
                        Destroy(river2.gameObject);
                    }
                    else if (impactedTile == river1.startNode && impactedTile == river2.endNode)
                    {
                        river2.Merge(river1);
                        allRiver.Remove(river1);
                        Destroy(river1.gameObject);
                    }
                }
            }
        }

        private River CreateRiver(Vector2Int startTile, Vector2Int endTile) => CreateRiver(new List<Vector2Int>(2) { startTile, endTile });
        private River CreateRiver(List<Vector2Int> tiles)
        {
            River newRiver = Instantiate(riverPrefab, Vector3.zero, Quaternion.identity, transform);
            newRiver.Initialise(tiles);
            return newRiver;
        }

        #region Link
        private void Link0To0(Vector2Int startTile, Vector2Int endTile)
        {
            var newRiver = CreateRiver(startTile, endTile);
            newRiver.LinkToGrid();
            allRiver.Add(newRiver);
        }
        private void Link0To1(Vector2Int startTile, Vector2Int endTile) => Link1To0(endTile, startTile);
        private void Link1To0(Vector2Int startTile, Vector2Int endTile)
        {
            River river = map.rivers[map.GridPos2ID(startTile)][0];
            river.Extend(startTile, endTile);
        }
        private void Link1To1(Vector2Int startTile, Vector2Int endTile)
        {
            River river1 = map.rivers[map.GridPos2ID(startTile)][0];
            River river2 = map.rivers[map.GridPos2ID(endTile)][0];

            if (river1.CheckForLoop(endTile))
            {
                Debug.LogWarning(" LOOP ", river1);
                return;
            }
            if (river1 == river2)
            {
                Debug.LogError("Error :link river to itself", river1);
                return;
            }
            if (startTile != river1.startNode && startTile != river1.endNode)
            {
                Debug.LogError("Error : try to merge but not from an extremum", river1);
                return;
            }
            if (endTile != river2.startNode && endTile != river2.endNode)
            {
                Debug.LogError("Error : try to merge but not from an extremum", river2);
                return;
            }

            //Merge Cases
            //==>to==>
            if (startTile == river1.endNode && endTile == river2.startNode)
            {
                river1.Extend(startTile, endTile);
                river1.Merge(river2);

                allRiver.Remove(river2);
                Destroy(river2.gameObject);
            }
            //<==to<==
            else if (startTile == river1.startNode && endTile == river2.endNode)
            {
                river2.Extend(endTile, startTile);
                river2.Merge(river1);

                allRiver.Remove(river1);
                Destroy(river1.gameObject);
            }
            //==>to<==
            else if (startTile == river1.endNode && endTile == river2.endNode)
            {
                river2.Reverse();
                river1.Extend(startTile, endTile);
                river1.Merge(river2);

                allRiver.Remove(river2);
                Destroy(river2.gameObject);
            }
            //<==to==>
            else if (startTile == river1.startNode && endTile == river2.startNode)
            {
                river1.Reverse();
                river1.Extend(startTile, endTile);
                river1.Merge(river2);

                allRiver.Remove(river2);
                Destroy(river2.gameObject);
            }
        }
        private void Link0To2(Vector2Int startTile, Vector2Int endTile) => Link2To0(endTile, startTile);
        private void Link2To0(Vector2Int startTile, Vector2Int endTile)
        {
            if (map.element[map.GridPos2ID(startTile)] is Lake)
            {
                //CannotLink(MessageCase.NotInCanal);
                return;
            }

            //If not splited
            if (map.rivers[map.GridPos2ID(startTile)].Count == 1)
            {
                River river = map.rivers[map.GridPos2ID(startTile)][0];
                var newRivers = river.Split(startTile);

                //Suppr old river
                allRiver.Remove(river);
                river.UnlinkToGrid();
                Destroy(river.gameObject);

                //allRiver add
                var addRiver = CreateRiver(newRivers.Item1);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);

                addRiver = CreateRiver(newRivers.Item2);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);
            }

            //Add Branch
            Link0To0(startTile, endTile);
        }
        private void Link1To2(Vector2Int startTile, Vector2Int endTile) => Link2To1(endTile, startTile);
        private void Link2To1(Vector2Int startTile, Vector2Int endTile)
        {
            if (map.element[map.GridPos2ID(startTile)] is Lake)
            {
                //CannotLink(MessageCase.NotInCanal);
                return;
            }

            if (map.rivers[map.GridPos2ID(startTile)].Count == 1)
            {
                var river = map.rivers[map.GridPos2ID(startTile)][0];
                if (river.CheckForLoop(endTile))
                {
                    Debug.LogWarning(" LOOP ", river);
                    return;
                }
            }
            else
            {
                var rivers = map.rivers[map.GridPos2ID(startTile)];//.Where(riv => riv.endNode == startTile);
                foreach (var riv in rivers)
                {
                    if (riv.CheckForLoop(endTile))
                    {
                        Debug.LogWarning(" LOOP ", riv);
                        return;
                    }
                }
            }
            var riverLinked = map.rivers[map.GridPos2ID(endTile)][0];
            if (map.rivers[map.GridPos2ID(startTile)].Contains(riverLinked))
            {
                Debug.LogError("Error :link river to itself", riverLinked);
                return;
            }
            if (riverLinked.CheckForLoop(startTile))
            {
                Debug.LogWarning(" LOOP ", riverLinked);
                return;
            }

            if (map.rivers[map.GridPos2ID(startTile)].Count == 1)
            {
                River river = map.rivers[map.GridPos2ID(startTile)][0];
                var newRivers = river.Split(startTile);

                //Suppr old river
                allRiver.Remove(river);
                river.UnlinkToGrid();
                Destroy(river.gameObject);

                //allRiver add
                var addRiver = CreateRiver(newRivers.Item1);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);

                addRiver = CreateRiver(newRivers.Item2);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);
            }

            //Extend Branch
            Link0To1(startTile, endTile);
        }
        private void Link2To2(Vector2Int startTile, Vector2Int endTile)
        {
            if (map.element[map.GridPos2ID(startTile)] is Lake)
            {
                //CannotLink(MessageCase.NotInCanal);
                return;
            }
            if (map.element[map.GridPos2ID(endTile)] is Lake)
            {
                //CannotLink(MessageCase.NotInCanal);
                return;
            }

            var rivers = map.rivers[map.GridPos2ID(startTile)];//.Where(riv => riv.endNode == startTile);
            foreach (var riv in rivers)
            {
                if (riv.CheckForLoop(endTile))
                {
                    Debug.LogWarning(" LOOP ", riv);
                    return;
                }
            }


            if (map.rivers[map.GridPos2ID(startTile)].Count == 1)
            {
                River river = map.rivers[map.GridPos2ID(startTile)][0];
                var newRivers = river.Split(startTile);

                //Suppr old river
                allRiver.Remove(river);
                river.UnlinkToGrid();
                Destroy(river.gameObject);

                //allRiver add
                var addRiver = CreateRiver(newRivers.Item1);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);

                addRiver = CreateRiver(newRivers.Item2);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);
            }
            if (map.rivers[map.GridPos2ID(endTile)].Count == 1)
            {
                River river = map.rivers[map.GridPos2ID(endTile)][0];
                var newRivers = river.Split(endTile);

                //Suppr old river
                allRiver.Remove(river);
                river.UnlinkToGrid();
                Destroy(river.gameObject);

                //allRiver add
                var addRiver = CreateRiver(newRivers.Item1);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);

                addRiver = CreateRiver(newRivers.Item2);
                addRiver.LinkToGrid();
                allRiver.Add(addRiver);
            }

            //Extend Branch
            Link0To0(startTile, endTile);
        }
        #endregion Link

        private void OnDrawGizmos()
        {
            foreach (var rivers in RiverExtension.linkedRiver)
            {
                for (int i = 0; i < rivers.tiles.Count - 1; i++)
                {
                    Debug.DrawLine(grid.TileToPos(rivers.tiles[i]), grid.TileToPos(rivers.tiles[i + 1]), Color.red);
                }
            }
        }
    }
}
