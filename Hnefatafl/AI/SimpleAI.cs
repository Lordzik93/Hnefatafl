using Hnefatafl.Models;
using System;
using System.Collections.Generic;

namespace Hnefatafl.AI
{
    /// <summary>
    /// Simple AI that selects a random valid move.
    /// </summary>
    public class SimpleAI
    {
        private Game game;

        /// <summary>
        /// Initializes the AI with the current game state.
        /// </summary>
        public SimpleAI(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Gets a random valid move for the current player.
        /// </summary>
        /// <returns>A tuple of from and to positions, or null if no move is available.</returns>
        public (Position from, Position to)? GetMove()
        {
            var board = game.Board;
            var player = game.CurrentPlayer;
            var moves = new List<(Position, Position)>();

            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    var piece = board.Grid[x, y];
                    if ((player == Player.Attacker && piece != PieceType.Attacker) ||
                        (player == Player.Defender && piece == PieceType.Attacker) ||
                        piece == PieceType.None)
                        continue;

                    var from = new Position(x, y);
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (Math.Abs(dx) + Math.Abs(dy) != 1) continue;
                            for (int i = 1; i < board.Size; i++)
                            {
                                int nx = x + dx * i;
                                int ny = y + dy * i;
                                var to = new Position(nx, ny);
                                if (!board.IsValidMove(from, to, player)) break;
                                moves.Add((from, to));
                            }
                        }
                    }
                }
            }

            if (moves.Count == 0) return null;
            var rand = new Random();
            return moves[rand.Next(moves.Count)];
        }
    }
}
