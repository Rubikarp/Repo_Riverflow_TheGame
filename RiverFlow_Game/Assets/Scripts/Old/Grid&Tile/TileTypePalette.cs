using UnityEngine;

namespace RiverFlow.Core
{
    public class TileTypePalette : ScriptableObject
    {
		#region Singleton
		static TileTypePalette _instance;
		public static TileTypePalette instance
		{
			get
			{
				if (_instance != null) return _instance;
				else
				{
					TileTypePalette trs = Resources.Load("TileTypePalette") as TileTypePalette;
					_instance = trs;
					return _instance;
				}
			}
		}
		#endregion

		[Header("Ground")]
        [ColorUsage(true, false)] public Color holedGround = Color.gray;
        [Space(5)]
        [ColorUsage(true, false)] public Color grass = Color.green;
        [ColorUsage(true, false)] public Color clay = Color.red;
        [ColorUsage(true, false)] public Color aride = Color.yellow;
        [ColorUsage(true, false)] public Color mountain = Color.gray;

        [Header("BackUp")]
        [ColorUsage(true, false)] public Color errorMat = Color.magenta;
    }
}
