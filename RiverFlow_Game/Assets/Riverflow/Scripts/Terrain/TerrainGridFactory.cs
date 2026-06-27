namespace RiverFlow
{
    /// <summary>
    /// Construit une <see cref="HexGrid"/> à partir des données de niveau et d'un catalogue de biomes.
    /// Fonction pure : aucun MonoBehaviour, testable en EditMode.
    /// </summary>
    public static class TerrainGridFactory
    {
        public static HexGrid FromLevel(SLevelData level, IBiomeCatalog biomes)
        {
            var grid = new HexGrid();
            if (level.Cells == null)
            {
                return grid;
            }

            foreach (var data in level.Cells)
            {
                var coord = new SHex(data.Q, data.R);
                grid.AddCell(new HexCell(coord, data.Biome, biomes.RequiredIrrigation(data.Biome), data.IsMountain));
            }

            return grid;
        }
    }
}