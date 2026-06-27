using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RiverFlow
{
    /// <summary>
    /// Pilote du terrain. Construit la grille depuis le LevelSO, la possède, et l'expose en lecture
    /// aux autres systèmes via <see cref="ITerrainQuery"/> (sans coupler les Controllers entre eux).
    /// Reçoit de S5 l'ouverture d'obstacle (tunnel) et l'agrandissement de carte.
    /// </summary>
    public sealed class TerrainController : MonoBehaviour, ITerrainQuery
    {
        [SerializeField, Required] private LevelSO _level;
        [SerializeField, Required] private BiomeCatalogSO _biomes;
        [SerializeField, Required] private TerrainView _view;

        [Title("Events")] public UnityEvent OnGridBuilt;
        public UnityEvent OnGridChanged;

        private HexGrid _grid;

        public HexGrid Grid => _grid;
        public SLevelData Level => _level.Level;

        [Button]
        public void LoadLevel()
        {
            _grid = TerrainGridFactory.FromLevel(_level.Level, _biomes);
            _view.Paint(_grid);
            OnGridBuilt.Invoke();
        }

        // ---- ITerrainQuery : surface lue par S2 / S3 / S5 ----

        public bool InBounds(SHex coord) => _grid != null && _grid.Contains(coord);

        public bool IsPassable(SHex coord) => _grid != null && _grid.IsPassable(coord);

        public int RequiredCransAt(SHex coord) => _grid != null ? _grid.RequiredIrrigationAt(coord) : 0;

        // ---- Mutations déclenchées par S5 ----

        /// <summary>Passage souterrain (récompense) : ouvre une montagne pour la rendre traversable.</summary>
        public void OpenObstacle(SHex coord)
        {
            if (_grid == null || !_grid.TryGet(coord, out var cell) || !cell.IsMountain)
            {
                return;
            }

            cell.Open();
            _view.PaintCell(cell);
            OnGridChanged.Invoke();
        }

        /// <summary>
        /// Agrandit la carte (déclenché par S5). La LOI de croissance (rythme, ampleur, génération
        /// des biomes au-delà du niveau peint) reste À DÉFINIR — non chiffrée dans le GDD.
        /// </summary>
        public void AddCells(IEnumerable<SCellData> cells)
        {
            foreach (var data in cells)
            {
                var coord = new SHex(data.Q, data.R);
                if (_grid.Contains(coord))
                {
                    continue;
                }

                var cell = new HexCell(coord, data.Biome, _biomes.RequiredIrrigation(data.Biome), data.IsMountain);
                _grid.AddCell(cell);
                _view.PaintCell(cell);
            }

            OnGridChanged.Invoke();
        }

        public SHex WorldToHex(Vector3 worldPosition) => _view.WorldToHex(worldPosition);

        public Vector3 HexToWorld(SHex hex) => _view.HexToWorld(hex);
    }
}