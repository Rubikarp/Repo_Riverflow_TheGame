using System;
using System.Collections.Generic;

namespace RiverFlow
{
    /// <summary>
    /// Coordonnée hexagonale axiale (q, r) ; s = -q - r est implicite (cube).
    /// Source : https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    [Serializable]
    public struct SHex : IEquatable<SHex>
    {
        /// <summary>
        /// Axe x (Q) 1 = droite, -1 = gauche
        /// </summary>
        public int Q;
        /// <summary>
        /// Axe y (R) 1 = haut, -1 = bas
        /// </summary>
        public int R;
        /// <summary>
        /// Axe z (S) implicite
        /// </summary>
        public int S => -Q - R;
 
        public SHex(int q, int r)
        {
            Q = q;
            R = r;
        }
 
        /// <summary>
        /// 6 directions axiales, sens horaire à partir de l'Est.
        /// </summary>
        //     S   R
        //      \ /
        // -Q ───┼─── Q
        //      / \
        //    -R  -S
        private static readonly SHex[] s_directions =
        {
            new SHex(1, 0), // right
            new SHex(1, -1), // bottom-right
            new SHex(0, -1), // bottom-left
            new SHex(-1, 0), // left
            new SHex(-1, 1), // top-left
            new SHex(0, 1) // top right
        };
 
        public static SHex Direction(int index) => s_directions[((index % 6) + 6) % 6];
        public SHex Neighbor(int index) => this + Direction(index);
 
        public IEnumerable<SHex> Neighbors()
        {
            for (int i = 0; i < 6; i++)
            {
                yield return this + s_directions[i];
            }
        }
 
        public int DistanceTo(SHex other)
        {
            return (Math.Abs(Q - other.Q) + Math.Abs(R - other.R) + Math.Abs(S - other.S)) / 2;
        }
 
        /// <summary>Toutes les cases à distance &lt;= range (disque hexagonal). Portée d'irrigation, anneaux de spawn.</summary>
        public IEnumerable<SHex> WithinRange(int range)
        {
            for (int dq = -range; dq <= range; dq++)
            {
                int rMin = Math.Max(-range, -dq - range);
                int rMax = Math.Min(range, -dq + range);
                for (int dr = rMin; dr <= rMax; dr++)
                {
                    yield return new SHex(Q + dq, R + dr);
                }
            }
        }
 
        public static SHex operator +(SHex a, SHex b) => new SHex(a.Q + b.Q, a.R + b.R);
        public static SHex operator -(SHex a, SHex b) => new SHex(a.Q - b.Q, a.R - b.R);
 
        public bool Equals(SHex other) => Q == other.Q && R == other.R;
        public override bool Equals(object obj) => obj is SHex other && Equals(other);
        public override int GetHashCode() => unchecked((Q * 397) ^ R);
        public static bool operator ==(SHex a, SHex b) => a.Equals(b);
        public static bool operator !=(SHex a, SHex b) => !a.Equals(b);
        public override string ToString() => $"Hex({Q},{R})";
    }
}