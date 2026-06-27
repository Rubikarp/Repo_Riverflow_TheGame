namespace RiverFlow
{

    /// <summary>
    /// Case du terrain à l'exécution. Le Système 1 ne possède que le terrain : biome, besoin en eau,
    /// obstacle (montagne) et son ouverture par tunnel. L'occupation (rivière, plante) appartient
    /// aux autres systèmes ; le terrain ne fournit que « praticable ou non ».
    /// </summary>
    public sealed class HexCell
    {
        public SHex Coord { get; }
        public EBiome Biome { get; }
        public int RequiredIrrigation { get; }
        public bool IsMountain { get; }
        public bool IsOpened { get; private set; }
 
        /// <summary>Praticable pour un tracé : libre, ou montagne ouverte par un Passage souterrain.</summary>
        public bool IsPassable => !IsMountain || IsOpened;
 
        public HexCell(SHex coord, EBiome biome, int requiredIrrigation, bool isMountain)
        {
            Coord = coord;
            Biome = biome;
            RequiredIrrigation = requiredIrrigation;
            IsMountain = isMountain;
        }
 
        /// <summary>Ouvre l'obstacle (Passage souterrain, récompense S5). Irréversible.</summary>
        public void Open() => IsOpened = true;
    }
}