namespace RiverFlow.Core
{
    public class Lake : Element
    {
        public Lake() : base(FlowStrenght._100_, 3) { }

        public void LinkToTile(TileData[] tiles)
        {

            for (int i = 0; i < tilesOn.Length; i++)
            {
                tilesOn[i].element = this;
                tilesOn[i] = tiles[i];
            }
        }
        public override void UnlinkTile()
        {
            for (int i = 0; i < tilesOn.Length; i++)
            {
                tilesOn[i].element = null;
                tilesOn[i] = null;
            }
        }
    }
}
