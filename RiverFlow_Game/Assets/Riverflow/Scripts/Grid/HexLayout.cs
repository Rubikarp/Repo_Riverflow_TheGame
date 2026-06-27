namespace RiverFlow
{
    /// <summary>Orientation des hexagones. DÉCISION ENCORE OUVERTE — défaut PointyTop, à confirmer.</summary>
    public enum EHexOrientation
    {
        PointyTop,
        FlatTop
    }
    
    /// <summary>
    /// Conversion entre la coordonnée axiale du modèle et la coordonnée offset (col, row) du Tilemap Unity.
    /// C'est la SEULE pièce du Système 1 qui dépend de l'orientation pointy/flat-top : tout le reste
    /// (voisinage, distance, portée) est axial et orientation-agnostique.
    /// Pointy-top utilise l'offset odd-r ; flat-top l'offset odd-q (convention Red Blob Games).
    /// L'alignement exact avec le repère du Tilemap Unity se calibre par un test de round-trip en éditeur.
    /// </summary>
    public static class HexLayout
    {
        private const EHexOrientation GRID_ORIENTATION = EHexOrientation.PointyTop;
 
        public static void ToOffset(this SHex hex, out int col, out int row)
        {
            if (GRID_ORIENTATION == EHexOrientation.PointyTop)
            {
                col = hex.Q + (hex.R - (hex.R & 1)) / 2;
                row = hex.R;
            }
            else
            {
                col = hex.Q;
                row = hex.R + (hex.Q - (hex.Q & 1)) / 2;
            }
        }
 
        public static SHex ToHex(int col, int row)
        {
            if (GRID_ORIENTATION == EHexOrientation.PointyTop)
            {
                int q = col - (row - (row & 1)) / 2;
                return new SHex(q, row);
            }
            else
            {
                int r = row - (col - (col & 1)) / 2;
                return new SHex(col, r);
            }
        }
    }
}