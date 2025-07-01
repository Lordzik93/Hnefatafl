using System;

namespace Hnefatafl.Models
{
    /// <summary>
    /// Represents the two players in the game.
    /// </summary>
    public enum Player { Attacker, Defender }

    /// <summary>
    /// Main game logic controller.
    /// </summary>
    public class Game
    {
        public Board Board { get; private set; }
        public Player CurrentPlayer { get; private set; }
        public bool IsGameOver { get; private set; }
        public Player? Winner { get; private set; }

        public Game()
        {
            Board = new Board();
            CurrentPlayer = Player.Defender;
            IsGameOver = false;
        }

        /// <summary>
        /// Attempts to make a move from one position to another.
        /// </summary>
        public bool MakeMove(Position from, Position to)
        {
            if (IsGameOver || !Board.IsValidMove(from, to, CurrentPlayer))
                return false;

            Board.MovePiece(from, to);
            CheckVictory(to);
            CurrentPlayer = CurrentPlayer == Player.Attacker ? Player.Defender : Player.Attacker;
            return true;
        }

        /// <summary>
        /// Checks if the game has been won after a move.
        /// </summary>
        private void CheckVictory(Position kingPos)
        {
            if (Board.IsKingAtCorner(kingPos))
            {
                IsGameOver = true;
                Winner = Player.Defender;
            }
            else if (Board.IsKingCaptured())
            {
                IsGameOver = true;
                Winner = Player.Attacker;
            }
        }

        /// <summary>
        /// Resets the game to initial state.
        /// </summary>
        public void Reset()
        {
            Board = new Board();
            CurrentPlayer = Player.Defender;
            IsGameOver = false;
            Winner = null;
        }
    }
}
