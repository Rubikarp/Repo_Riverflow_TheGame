using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public class RiverManager : MonoBehaviour
    {
        [SerializeField, Required] DataGrid<TileData> map;
        [SerializeField, Required] TimeManager time;
        [SerializeField, Required] LinkHandler link;

        private void OnLink(Vector2Int startTile, Vector2Int endTile)
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
            switch (map.GetData(startTile).LinkAmount)
            {
                case 0:
                    switch (map.GetData(endTile).LinkAmount)
                    {
                        case 0: //in a void
                            Link0To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            //Link1To0(endTile, startTile);
                            break;
                        default: //x >= 2
                            //Link2To0(endTile, startTile);
                            break;
                    }
                    break;
                case 1:
                    switch (map.GetData(endTile).LinkAmount)
                    {
                        case 0: //in a void
                            //Link1To0(startTile, endTile);
                            break;
                        case 1: //extending the end canal
                            //Link1To1(startTile, endTile);
                            break;
                        default: //x >= 2
                            //Link2To1(endTile, startTile);
                            break;
                    }
                    break;
                default: // 2 ou +
                    switch (map.GetData(endTile).LinkAmount)
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
            throw new NotImplementedException();
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
