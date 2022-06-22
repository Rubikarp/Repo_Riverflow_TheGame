using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RiverFlow.Core
{
    public class InputCheck : MonoBehaviour
    {
        public void onPress(InputMode mode, Vector3 pos)
        {
            Debug.Log(pos);
        }
        public void onMaintain(InputMode mode, Vector3 pos)
        {
            Debug.Log(pos);
        }


        private Color ModeColor(InputMode mode)
        {
            switch (mode)
            {
                case InputMode.None: return Color.grey;
                case InputMode.Dig: return Color.green;
                case InputMode.Erase: return Color.red;
                case InputMode.Cloud: return Color.cyan;
                case InputMode.Lake: return Color.yellow;
                case InputMode.Source: return Color.magenta;
                default: return Color.black;
            }
        }

    }
}
