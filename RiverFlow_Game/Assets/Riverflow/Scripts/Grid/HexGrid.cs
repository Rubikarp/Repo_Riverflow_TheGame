using System.Collections.Generic;

namespace RiverFlow
{
    /// <summary>
    /// Grille hexagonale runtime : source de vérité du terrain. Indexée par coordonnée axiale.
    /// Pure C# : testable en EditMode, sans dépendance Tilemap.
    /// </summary>
    public sealed class HexGrid
    {
        private readonly Dictionary<SHex, HexCell> _cells = new Dictionary<SHex, HexCell>();
 
        public IReadOnlyDictionary<SHex, HexCell> Cells => _cells;
        public int Count => _cells.Count;
 
        public void AddCell(HexCell cell) => _cells[cell.Coord] = cell;
        public bool Remove(SHex coord) => _cells.Remove(coord);
        public bool Contains(SHex coord) => _cells.ContainsKey(coord);
 
        public HexCell Get(SHex coord) => _cells.TryGetValue(coord, out var cell) ? cell : null;
        public bool TryGet(SHex coord, out HexCell cell) => _cells.TryGetValue(coord, out cell);
 
        public bool IsPassable(SHex coord) => _cells.TryGetValue(coord, out var cell) && cell.IsPassable;
 
        public int RequiredIrrigationAt(SHex coord) => _cells.TryGetValue(coord, out var cell) ? cell.RequiredIrrigation : 0;
 
        public IEnumerable<HexCell> PassableNeighbors(SHex coord)
        {
            foreach (var neighbor in coord.Neighbors())
            {
                HexCell cell;
                if (_cells.TryGetValue(neighbor, out cell) && cell.IsPassable)
                {
                    yield return cell;
                }
            }
        }
    }
}