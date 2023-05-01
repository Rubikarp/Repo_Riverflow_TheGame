using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverFlow.Core
{
    public class RiverManager : MonoBehaviour
    {
        [SerializeField, Required] LevelHandler level;
        [SerializeField, Required] TimeManager time;
        [SerializeField, Required] LinkHandler link;

        [SerializeField, Required] River riverPrefab;
        [SerializeField] List<River> allRiver;


        public void OnLink(Vector2Int startTile, Vector2Int endTile)
        {
            Debug.Log(startTile);
            Link0To0(startTile, endTile);
            //inventory.digAmmount--;
            if (!time.isPaused)
            {
                //FlowStep();
            }
        }
        
        private void LinkConfirmed(Vector2Int startTile, Vector2Int endTile)
        {
            switch (level.tileGrid[startTile].LinkAmount)
            {
                case 0:
                    switch (level.tileGrid[endTile].LinkAmount)
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
                    switch (level.tileGrid[endTile].LinkAmount)
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
                    switch (level.tileGrid[endTile].LinkAmount)
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
            newRiver.Initialised(startTile, endTile);
            allRiver.Add(newRiver);

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
