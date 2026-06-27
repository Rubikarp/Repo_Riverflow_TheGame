using NUnit.Framework;

namespace RiverFlow.Tests
{
    public class TerrainGridFactoryTests
    {
        private class FakeCatalog : IBiomeCatalog
        {
            public int RequiredIrrigation(EBiome biome)
            {
                switch (biome)
                {
                    case EBiome.Prairie: return 1;
                    case EBiome.Steppe: return 2;
                    case EBiome.Desert: return 3;
                    default: return 0;
                }
            }
        }
 
        [Test]
        public void FromLevel_ConstruitToutesLesCases()
        {
            var level = new SLevelData
            {
                Cells = new[]
                {
                    new SCellData { Q = 0, R = 0, Biome = EBiome.Prairie, IsMountain = false },
                    new SCellData { Q = 1, R = 0, Biome = EBiome.Desert, IsMountain = true }
                }
            };
 
            var grid = TerrainGridFactory.FromLevel(level, new FakeCatalog());
 
            Assert.AreEqual(2, grid.Count);
            Assert.AreEqual(1, grid.RequiredIrrigationAt(new SHex(0, 0)));
            Assert.IsTrue(grid.Get(new SHex(1, 0)).IsMountain);
        }
 
        [Test]
        public void FromLevel_CellsNull_RendUneGrilleVide()
        {
            var grid = TerrainGridFactory.FromLevel(new SLevelData(), new FakeCatalog());
            Assert.AreEqual(0, grid.Count);
        }
    }
}