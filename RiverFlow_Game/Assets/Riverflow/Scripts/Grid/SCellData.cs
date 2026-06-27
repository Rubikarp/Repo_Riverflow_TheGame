using System;

namespace RiverFlow
{
    /// <summary>Une case telle qu'enregistrée dans le niveau (issue de la scène d'édition).</summary>
    [Serializable]
    public struct SCellData
    {
        public int Q;
        public int R;
        public EBiome Biome;
        public bool IsMountain;
    }
}