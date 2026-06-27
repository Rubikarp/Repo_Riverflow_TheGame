using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>
    /// Lecture partagée des Tilemaps de la scène d'édition vers les données pures d'un niveau.
    /// Utilisée par <see cref="LevelSaver"/> et <see cref="LevelBaker"/>. Éditeur uniquement.
    /// </summary>
    public static class LevelBakeUtility
    {
        public static SLevelData ReadLevel(
            Tilemap floor,
            Tilemap obstacle,
            Tilemap sources,
            BiomeCatalogSO biomes,
            string levelName,
            int pointThreshold)
        {
            var cells = new List<SCellData>();
 
            floor.CompressBounds();
            foreach (var position in floor.cellBounds.allPositionsWithin)
            {
                var floorTile = floor.GetTile(position);
                if (floorTile == null)
                {
                    continue;
                }
 
                if (!biomes.TryResolveBiome(floorTile, out var biome))
                {
                    Debug.LogWarning($"Tuile de sol non mappee a un biome en {position} — case ignoree.");
                    continue;
                }
 
                var hex = HexLayout.ToHex(position.x, position.y);
                bool isMountain = obstacle != null && obstacle.GetTile(position) != null;
                cells.Add(new SCellData { Q = hex.Q, R = hex.R, Biome = biome, IsMountain = isMountain });
            }
 
            return new SLevelData
            {
                Name = levelName,
                Cells = cells.ToArray(),
                Sources = ReadSources(sources).ToArray(),
                unlockNextLevelThreshold = pointThreshold
            };
        }
 
        private static List<SHex> ReadSources(Tilemap sources)
        {
            var result = new List<SHex>();
            if (sources == null)
            {
                return result;
            }
 
            sources.CompressBounds();
            foreach (var position in sources.cellBounds.allPositionsWithin)
            {
                if (sources.GetTile(position) != null)
                {
                    result.Add(HexLayout.ToHex(position.x, position.y));
                }
            }
 
            return result;
        }
    }
}