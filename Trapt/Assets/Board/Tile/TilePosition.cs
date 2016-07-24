class TilePosition
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public override int GetHashCode()
    {
        return this.X.GetHashCode() ^ this.Y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        TilePosition other = (TilePosition)obj;
        if (other == null)
        {
            return false;
        }


        return
            this.X == other.X &&
            this.Y == other.Y;
    }

    public TilePosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}