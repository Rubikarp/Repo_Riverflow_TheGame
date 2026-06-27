using System.Linq;
using NUnit.Framework;

namespace RiverFlow.Tests
{
    public class SHexTests
    {
        [Test]
        public void Neighbors_RetourneSixVoisinsDistincts()
        {
            var hex = new SHex(0, 0);
            Assert.AreEqual(6, hex.Neighbors().Distinct().Count());
        }

        [Test]
        public void DistanceTo_VoisinImmediat_Vaut1()
        {
            var hex = new SHex(3, -2);
            Assert.AreEqual(1, hex.DistanceTo(hex.Neighbor(0)));
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 2, -1, 2)]
        [TestCase(-1, 1, 1, -1, 2)]
        public void DistanceTo_RespecteLaMetriqueCube(int aq, int ar, int bq, int br, int expected)
        {
            Assert.AreEqual(expected, new SHex(aq, ar).DistanceTo(new SHex(bq, br)));
        }

        [TestCase(0, 1)]
        [TestCase(1, 7)]
        [TestCase(2, 19)]
        [TestCase(3, 37)]
        public void WithinRange_CompteLeDisqueHexagonal(int range, int expectedCount)
        {
            // 1 + 3*range*(range+1)
            Assert.AreEqual(expectedCount, new SHex(0, 0).WithinRange(range).Count());
        }

        [Test]
        public void Addition_CombineLesCoordonnees()
        {
            Assert.AreEqual(new SHex(3, -1), new SHex(1, 0) + new SHex(2, -1));
        }
        
        public void OffsetRoundTrip_EstIdentite()
        {
            for (int q = -10; q <= 10; q++)
            {
                for (int r = -10; r <= 10; r++)
                {
                    var source = new SHex(q, r);
                    HexLayout.ToOffset(source, out int col, out int row);
                    Assert.AreEqual(source, HexLayout.ToHex(col, row), $"round-trip casse en {source}");
                }
            }
        }
    }
}