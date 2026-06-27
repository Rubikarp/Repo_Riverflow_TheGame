using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>Config d'un biome : son besoin en eau (crans) et son visuel. Unité = celle du débit (S2).</summary>
    [CreateAssetMenu(menuName = "RiverFlow/Biome", fileName = "Biome")]
    public sealed class BiomeSO : ScriptableObject
    {
        [field: SerializeField] public EBiome Biome { get; private set; }

        [field: SerializeField, Range(0, 4), Tooltip("Besoin en Irrigation: Prairie 1 / Steppe 2 / Desert 3")]
        public int RequiredIrrigation{ get; private set; } = 1;

        [field: SerializeField, Required] public TileBase FloorTile { get; private set; }

        [field: SerializeField] public Color DebugColor { get; private set; } = Color.white;
    }
}