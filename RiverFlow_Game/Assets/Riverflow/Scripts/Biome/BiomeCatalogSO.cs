using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>
    /// Catalogue des biomes. Pont entre l'enum <see cref="EBiome"/> et ses assets de config.
    /// Implémente <see cref="IBiomeCatalog"/> pour la fabrique de grille (pure), et résout
    /// tuile -> biome pour le baker de niveau.
    /// </summary>
    [CreateAssetMenu(menuName = "RiverFlow/Biome Catalog", fileName = "BiomeCatalog")]
    public sealed class BiomeCatalogSO : ScriptableObject, IBiomeCatalog
    {
        [SerializeField] private SerializedDictionary<EBiome, BiomeSO> _biomes = new ()
        {
            [EBiome.Prairie] = null,
            [EBiome.Steppe] = null,
            [EBiome.Desert] = null,
        };

        public int RequiredIrrigation(EBiome biome)
        {
            return _biomes.TryGetValue(biome, out var so) ? so.RequiredIrrigation : 0;
        }

        public BiomeSO Get(EBiome biome)
        {
            return _biomes.TryGetValue(biome, out var so) ? so : null;
        }

        /// <summary>Retrouve le biome d'une tuile de sol (utilisé par le LevelBaker). null si inconnue.</summary>
        public bool TryResolveBiome(TileBase floorTile, out EBiome biome)
        {
            foreach (var pair in _biomes)
            {
                if (pair.Value != null && pair.Value.FloorTile == floorTile)
                {
                    biome = pair.Key;
                    return true;
                }
            }

            biome = default;
            return false;
        }
    }
}