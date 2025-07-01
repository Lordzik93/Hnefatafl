namespace Hnefatafl.Models
{
    /// <summary>
    /// Represents a position on the game board.
    /// </summary>
    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X},{Y})";
    }
}
