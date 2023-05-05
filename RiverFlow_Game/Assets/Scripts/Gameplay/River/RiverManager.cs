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


        public void OnLink(Vector2Int startTile, Vector2Int endTile)
        {
            LinkConfirmed(startTile, endTile);
            //inventory.digAmmount--;
            if (!time.isPaused)
            {
                //FlowStep();
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
                            //Link2To0(endTile, startTile);
                            break;
                    }
                    break;
                case 1:
                    switch (map.GetLinkAmount(endTile))
                    {
                        case 0: //extending the end canal
                            Link1To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            Link1To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            //Link2To1(endTile, startTile);
                            break;
                    }
                    break;
                default: // 2 ou +
                    switch (map.GetLinkAmount(endTile))
                    {
                        case 0: //in a void
                            //Link2To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            //Link2To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            //Link2To2(startTile, endTile);
                            break;
                    }
                    break;
            }
        }
        
        private void Link0To0(Vector2Int startTile, Vector2Int endTile)
        {
            River newRiver = Instantiate(riverPrefab, Vector3.zero, Quaternion.identity, transform);
            newRiver.Initialise(startTile, endTile);
            newRiver.LinkToGrid();
            allRiver.Add(newRiver);
        }

        private void Link0To1(Vector2Int startTile, Vector2Int endTile) => Link1To0(endTile, startTile);
        private void Link1To0(Vector2Int startTile, Vector2Int endTile)
        {
            River river = map.rivers[map.GridPos2ID(startTile)][0];
            river.Extend(endTile);
        }
        private void Link1To1(Vector2Int startTile, Vector2Int endTile)
        {
            River river1 = map.rivers[map.GridPos2ID(startTile)][0];
            River river2 = map.rivers[map.GridPos2ID(endTile)][0];

            if (river1.startNode != startTile || river1.endNode != startTile)
            {
                Debug.LogError("Error : try to merge but not from an extremum", river1);
                return;
            }
            if (river2.startNode != startTile || river2.endNode != startTile)
            {
                Debug.LogError("Error : try to merge but not from an extremum", river2);
                return;
            }

            //Made sure startTile is the endTile
            if (river1.endNode != startTile || river2.startNode != endTile)
            {
                if(river1.endNode != startTile)
                {
                    //Merge river1 into 2
                }
                else
                {
                    //Merge river2 into 1
                }
            }
            else
            {
                river1.Reverse();
                //Merge river1 into 2

            }

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
