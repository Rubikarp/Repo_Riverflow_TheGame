using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public class RiverManager : MonoBehaviour
    {
        [SerializeField, Required] TileMap map;
        [SerializeField, Required] TimeManager time;
        [SerializeField, Required] LinkHandler link;

        [SerializeField, Required] River riverPrefab;
        [SerializeField] List<River> allRiver;

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

        private River CreateRiver(Vector2Int startTile, Vector2Int endTile) => CreateRiver(new List<Vector2Int>(2) { startTile, endTile });
        private River CreateRiver(List<Vector2Int> tiles)
        {
            River newRiver = Instantiate(riverPrefab, Vector3.zero, Quaternion.identity, transform);
            newRiver.Initialise(tiles);
            return newRiver;
        }
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
        private void OnEnable()
        {
            link.onLink.AddListener(OnLink);
        }
        private void OnDisable()
        {
            link.onLink.RemoveListener(OnLink);
        }
    }
}
