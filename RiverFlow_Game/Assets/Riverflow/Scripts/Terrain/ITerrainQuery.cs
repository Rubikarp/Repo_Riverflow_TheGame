namespace RiverFlow
{
    /// <summary>
    /// Surface de lecture du terrain
    /// </summary>
    public interface ITerrainQuery
    {
        public bool InBounds(SHex coord);
        public bool IsPassable(SHex coord);
        public int RequiredCransAt(SHex coord);
    }
}