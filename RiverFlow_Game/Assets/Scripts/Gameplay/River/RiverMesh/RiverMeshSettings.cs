using UnityEngine;

namespace RiverFlow.Core
{
    public class RiverMeshSettings : SingletonSCO<RiverMeshSettings>
	{
		[Header("River Data")]
		[Range(0.01f, 4)] public float scale = 1;
		[Range(0.01f, 4)] public float lakeScale = 1;
		[Range(1, 16)] public int subdividePerSegment = 1;
	}
}
