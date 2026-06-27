using System.Threading;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RiverFlow
{
    /// <summary>
    /// Rendu du terrain. Peint la grille sur des Tilemaps (sol + obstacles), convertit monde &lt;-&gt; hex,
    /// et anime le dézoom caméra. Ne décide rien seule : reçoit ses ordres du TerrainController.
    /// Le composant Grid d'Unity doit être configuré avec la MÊME orientation que <see cref="_orientation"/>.
    /// </summary>
    public sealed class TerrainView : MonoBehaviour
    {
        [SerializeField, Required] private Tilemap _floorTilemap;
        [SerializeField, Required] private Tilemap _obstacleTilemap;
        [SerializeField, Required] private BiomeCatalogSO _biomes;
        [SerializeField, Required] private TileBase _mountainTile;
        [SerializeField, Required] private Camera _camera;

        public void Paint(HexGrid grid)
        {
            _floorTilemap.ClearAllTiles();
            _obstacleTilemap.ClearAllTiles();
            foreach (var cell in grid.Cells.Values)
            {
                PaintCell(cell);
            }
        }

        public void PaintCell(HexCell cell)
        {
            cell.Coord.ToOffset(out int col, out int row);
            var position = new Vector3Int(col, row, 0);

            var biome = _biomes.Get(cell.Biome);
            _floorTilemap.SetTile(position, biome != null ? biome.FloorTile : null);
            _obstacleTilemap.SetTile(position, cell.IsMountain && !cell.IsOpened ? _mountainTile : null);
        }

        public SHex WorldToHex(Vector3 worldPosition)
        {
            var cell = _floorTilemap.WorldToCell(worldPosition);
            return HexLayout.ToHex(cell.x, cell.y);
        }

        public Vector3 HexToWorld(SHex hex)
        {
            hex.ToOffset(out int col, out int row);
            return _floorTilemap.GetCellCenterWorld(new Vector3Int(col, row, 0));
        }

        /// <summary>Dézoom progressif accompagnant l'agrandissement de la carte (pilier « l'espace comme adversaire »).</summary>
        public async Awaitable AnimateZoomAsync(float orthographicSize, float duration,
            CancellationToken cancellationToken)
        {
            // WaitAsAwaitable : extension DOTween -> Awaitable partagée du projet (voir skill unity-dev).
            await _camera.DOOrthoSize(orthographicSize, duration)
                .SetEase(Ease.InOutSine)
                .WaitAsAwaitable(cancellationToken);
        }
    }
}