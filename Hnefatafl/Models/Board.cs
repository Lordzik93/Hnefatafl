using System;
using System.Drawing;

namespace Hnefatafl.Models
{
    /// <summary>
    /// Represents the game board and its logic.
    /// </summary>
    public class Board
    {
        public int Size { get; private set; }
        public PieceType[,] Grid { get; private set; }

        public static Image BoardImage;
        public static Image AttackerImage;
        public static Image DefenderImage;
        public static Image KingImage;

        static Board()
        {
            try { BoardImage = Image.FromFile("Assets/board.png"); } catch { BoardImage = null; }
            try { AttackerImage = Image.FromFile("Assets/attacker.png"); } catch { AttackerImage = null; }
            try { DefenderImage = Image.FromFile("Assets/defender.png"); } catch { DefenderImage = null; }
            try { KingImage = Image.FromFile("Assets/king.png"); } catch { KingImage = null; }
        }

        public Board(int size)
        {
            Size = size;
            Grid = new PieceType[Size, Size];
            InitializeBoard();
        }

        /// <summary>
        /// Initializes the board with starting positions.
        /// </summary>
        private void InitializeBoard()
        {
            int mid = Size / 2;

            if (Size == 7)
            {
                Grid[mid, mid] = PieceType.King;
                Grid[mid - 1, mid] = Grid[mid + 1, mid] = Grid[mid, mid - 1] = Grid[mid, mid + 1] = PieceType.Defender;

                Grid[0, 3] = Grid[6, 3] = Grid[3, 0] = Grid[3, 6] = PieceType.Attacker;
                Grid[1, 3] = Grid[5, 3] = Grid[3, 1] = Grid[3, 5] = PieceType.Attacker;
            }
            else if (Size == 9)
            {
                Grid[mid, mid] = PieceType.King;
                Grid[mid - 1, mid] = Grid[mid + 1, mid] = Grid[mid, mid - 1] = Grid[mid, mid + 1] = PieceType.Defender;
                Grid[mid - 1, mid - 1] = Grid[mid - 1, mid + 1] = Grid[mid + 1, mid - 1] = Grid[mid + 1, mid + 1] = PieceType.Defender;

                for (int i = 2; i <= 6; i++)
                {
                    Grid[0, i] = Grid[8, i] = Grid[i, 0] = Grid[i, 8] = PieceType.Attacker;
                }
            }
            else
            {
                for (int i = 3; i <= 7; i++)
                {
                    Grid[0, i] = Grid[10, i] = Grid[i, 0] = Grid[i, 10] = PieceType.Attacker;
                }

                Grid[5, 5] = PieceType.King;
                Grid[5, 4] = Grid[5, 6] = Grid[4, 5] = Grid[6, 5] = PieceType.Defender;
                Grid[4, 4] = Grid[4, 6] = Grid[6, 4] = Grid[6, 6] = PieceType.Defender;
            }
        }

        /// <summary>
        /// Validates if a move is legal.
        /// </summary>
        public bool IsValidMove(Position from, Position to, Player player)
        {
            if (!IsInsideBoard(from) || !IsInsideBoard(to)) return false;
            if (Grid[from.X, from.Y] == PieceType.None || Grid[to.X, to.Y] != PieceType.None) return false;

            var piece = Grid[from.X, from.Y];
            if ((player == Player.Attacker && piece != PieceType.Attacker) ||
                (player == Player.Defender && piece == PieceType.Attacker))
                return false;

            if (from.X != to.X && from.Y != to.Y) return false;

            int dx = Math.Sign(to.X - from.X);
            int dy = Math.Sign(to.Y - from.Y);
            int x = from.X + dx, y = from.Y + dy;
            while (x != to.X || y != to.Y)
            {
                if (Grid[x, y] != PieceType.None) return false;
                x += dx;
                y += dy;
            }

            return true;
        }

        /// <summary>
        /// Moves a piece and checks for captures.
        /// </summary>
        public void MovePiece(Position from, Position to)
        {
            var piece = Grid[from.X, from.Y];
            Grid[from.X, from.Y] = PieceType.None;
            Grid[to.X, to.Y] = piece;
            CaptureEnemies(to);
        }

        /// <summary>
        /// Captures enemy pieces adjacent to the moved piece.
        /// </summary>
        private void CaptureEnemies(Position pos)
        {
            var directions = new[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
            foreach (var (dx, dy) in directions)
            {
                int x = pos.X + dx;
                int y = pos.Y + dy;
                int x2 = x + dx;
                int y2 = y + dy;

                if (IsInsideBoard(x2, y2) && Grid[x, y] != PieceType.None && Grid[x2, y2] == Grid[pos.X, pos.Y])
                {
                    if (Grid[x, y] != PieceType.King)
                        Grid[x, y] = PieceType.None;
                }
            }
        }

        public bool IsKingAtCorner(Position pos)
        {
            return Grid[pos.X, pos.Y] == PieceType.King &&
                   ((pos.X == 0 || pos.X == Size - 1) && (pos.Y == 0 || pos.Y == Size - 1));
        }

        public bool IsKingCaptured()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (Grid[x, y] == PieceType.King)
                    {
                        int surrounded = 0;
                        if (x > 0 && Grid[x - 1, y] == PieceType.Attacker) surrounded++;
                        if (x < Size - 1 && Grid[x + 1, y] == PieceType.Attacker) surrounded++;
                        if (y > 0 && Grid[x, y - 1] == PieceType.Attacker) surrounded++;
                        if (y < Size - 1 && Grid[x, y + 1] == PieceType.Attacker) surrounded++;
                        return surrounded >= 4;
                    }
                }
            }
            return false;
        }

        private bool IsInsideBoard(Position pos) => IsInsideBoard(pos.X, pos.Y);
        private bool IsInsideBoard(int x, int y) => x >= 0 && x < Size && y >= 0 && y < Size;
    }
}
