using Hnefatafl.Models;
using System;

namespace Hnefatafl.Controllers
{
    /// <summary>
    /// Controls the game logic and acts as a bridge between the model and the view.
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// Instance of the game logic.
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// Event triggered when the board state is updated.
        /// </summary>
        public event Action BoardUpdated;

        /// <summary>
        /// Event triggered when the game ends.
        /// </summary>
        public event Action<Player?> GameEnded;

        /// <summary>
        /// Initializes a new game controller.
        /// </summary>
        public GameController(int boardSize)
        {
            Game = new Game(boardSize);
        }

        /// <summary>
        /// Resets the game to its initial state.
        /// </summary>
        public void ResetGame()
        {
            Game.Reset();
            BoardUpdated?.Invoke();
        }

        /// <summary>
        /// Attempts to make a move from one position to another.
        /// </summary>
        /// <param name="from">Starting position</param>
        /// <param name="to">Target position</param>
        /// <returns>True if the move was successful</returns>
        public bool TryMove(Position from, Position to)
        {
            bool moved = Game.MakeMove(from, to);
            if (moved)
            {
                BoardUpdated?.Invoke();
                if (Game.IsGameOver)
                    GameEnded?.Invoke(Game.Winner);
            }
            return moved;
        }

        /// <summary>
        /// Gets the piece type at a specific board position.
        /// </summary>
        public PieceType GetPieceAt(int x, int y) => Game.Board.Grid[x, y];

        /// <summary>
        /// Gets the current player.
        /// </summary>
        public Player CurrentPlayer => Game.CurrentPlayer;
    }
}
