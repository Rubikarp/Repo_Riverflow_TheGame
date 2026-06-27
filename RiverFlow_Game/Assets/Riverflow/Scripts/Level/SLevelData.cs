using System;

namespace RiverFlow
{
    /// <summary>
    /// Données pures d'un niveau : la scène d'édition est peinte à la main puis « bakée » dans ces
    /// données, portées par un LevelSO et consommées en partie. Pure C# → construction testable.
    /// </summary>
    [Serializable]
    public struct SLevelData
    {
        public string Name;
        public SCellData[] Cells;
        public SHex[] Sources;
 
        /// <summary>Palier de points pour débloquer le niveau suivant (S6, décision hors GDD v0.2).</summary>
        public int unlockNextLevelThreshold;
    }
}