using System.Linq;
using NUnit.Framework;

namespace RiverFlow.Tests
{
    public class HexGridTests
    {
        private static HexGrid BuildGrid()
        {
            var grid = new HexGrid();
            grid.AddCell(new HexCell(new SHex(0, 0), EBiome.Prairie, 1, false));
            grid.AddCell(new HexCell(new SHex(1, 0), EBiome.Desert, 3, false));
            grid.AddCell(new HexCell(new SHex(2, 0), EBiome.Steppe, 2, true)); // montagne
            return grid;
        }
 
        [Test]
        public void IsPassable_Montagne_EstFalse()
        {
            Assert.IsFalse(BuildGrid().IsPassable(new SHex(2, 0)));
        }
 
        [Test]
        public void IsPassable_HorsGrille_EstFalse()
        {
            Assert.IsFalse(BuildGrid().IsPassable(new SHex(9, 9)));
        }
 
        [Test]
        public void Open_MontagneAvecTunnel_DevientPraticable()
        {
            var grid = BuildGrid();
            grid.Get(new SHex(2, 0)).Open();
            Assert.IsTrue(grid.IsPassable(new SHex(2, 0)));
        }
 
        [Test]
        public void RequiredCransAt_RendLeBesoinDuBiome()
        {
            Assert.AreEqual(3, BuildGrid().RequiredIrrigationAt(new SHex(1, 0)));
        }
 
        [Test]
        public void PassableNeighbors_IgnoreLesMontagnesFermees()
        {
            var grid = BuildGrid();
            var neighbors = grid.PassableNeighbors(new SHex(1, 0)).Select(c => c.Coord).ToList();
            Assert.Contains(new SHex(0, 0), neighbors);
            Assert.IsFalse(neighbors.Contains(new SHex(2, 0)), "montagne fermee exclue");
        }
    }
}