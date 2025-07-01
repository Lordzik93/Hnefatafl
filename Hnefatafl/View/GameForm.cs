using Hnefatafl.AI;
using Hnefatafl.Controllers;
using Hnefatafl.Models;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Hnefatafl.Views
{
    /// <summary>
    /// Main game window that displays the board and handles user interaction.
    /// </summary>
    public class GameForm : UserControl
    {
        private const int TileSize = 50; // Size of each square on the board
        private GameController controller;
        private SimpleAI ai;
        private bool vsAI;
        private Position? selected = null;
        private Label lblCurrentPlayer;
        private Label lblStart;

        /// <summary>
        /// Initializes the game form.
        /// </summary>
        /// <param name="playVsAI">True if playing against AI, false for 2-player mode.</param>
        public GameForm(int boardSize, bool playVsAI)
        {
            this.Dock = DockStyle.Fill;
            vsAI = playVsAI;
            controller = new GameController(boardSize);
            controller.BoardUpdated += () => this.Invalidate();
            controller.GameEnded += winner =>
            {
                PlaySound("win.wav");
                MessageBox.Show($"{winner} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            if (vsAI)
                ai = new SimpleAI(controller.Game);

            this.MouseClick += OnMouseClick;

            lblCurrentPlayer = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, boardSize * TileSize + 5),
                AutoSize = true
            };
            this.Controls.Add(lblCurrentPlayer);

            lblStart = new Label
            {
                Text = "Zaczynają: Atakujący",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Location = new Point(200, boardSize * TileSize + 5),
                AutoSize = true
            };
            this.Controls.Add(lblStart);

            UpdateCurrentPlayerLabel();
        }

        private void UpdateCurrentPlayerLabel()
        {
            var player = controller.CurrentPlayer;
            lblCurrentPlayer.Text = $"Ruch gracza: {(player == Player.Attacker ? "Atakujący (czarny)" : "Obrońca (biały/król)")}";
            lblCurrentPlayer.ForeColor = player == Player.Attacker ? Color.Black : Color.DarkBlue;
        }

        /// <summary>
        /// Handles mouse clicks to select and move pieces.
        /// </summary>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / TileSize;
            int y = e.Y / TileSize;
            if (x >= controller.Game.Board.Size || y >= controller.Game.Board.Size) return;

            var pos = new Position(x, y);

            if (selected == null)
            {
                var piece = controller.GetPieceAt(x, y);
                if ((controller.CurrentPlayer == Player.Attacker && piece == PieceType.Attacker) ||
                    (controller.CurrentPlayer == Player.Defender && (piece == PieceType.Defender || piece == PieceType.King)))
                {
                    selected = pos;
                }
            }
            else
            {
                if (controller.TryMove(selected.Value, pos))
                {
                    PlaySound("move.wav");
                    selected = null;

                    if (vsAI && !controller.Game.IsGameOver)
                    {
                        var move = ai.GetMove();
                        if (move.HasValue)
                        {
                            controller.TryMove(move.Value.from, move.Value.to);
                            PlaySound("move.wav");
                        }
                    }
                }
                else
                {
                    selected = null;
                }
            }

            UpdateCurrentPlayerLabel();
        }

        /// <summary>
        /// Draws the game board and pieces.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            int size = controller.Game.Board.Size;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var rect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                    g.FillRectangle(Brushes.BurlyWood, rect);
                    g.DrawRectangle(Pens.Black, rect);

                    var piece = controller.GetPieceAt(x, y);
                    Image img = null;
                    if (piece == PieceType.Attacker) img = Board.AttackerImage;
                    else if (piece == PieceType.Defender) img = Board.DefenderImage;
                    else if (piece == PieceType.King) img = Board.KingImage;

                    if (img != null)
                        g.DrawImage(img, rect);
                    else if (piece != PieceType.None)
                    {
                        Brush brush = piece switch
                        {
                            PieceType.Attacker => Brushes.Black,
                            PieceType.Defender => Brushes.White,
                            PieceType.King => Brushes.Gold,
                            _ => Brushes.Gray
                        };
                        g.FillEllipse(brush, rect.X + 5, rect.Y + 5, TileSize - 10, TileSize - 10);
                    }
                }
            }

            if (selected.HasValue)
            {
                g.DrawRectangle(Pens.Red, selected.Value.X * TileSize, selected.Value.Y * TileSize, TileSize, TileSize);
            }
        }

        /// <summary>
        /// Plays a sound from the Assets folder.
        /// </summary>
        private void PlaySound(string file)
        {
            try
            {
                var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", file);
                if (System.IO.File.Exists(path))
                    new SoundPlayer(path).Play();
            }
            catch
            {
                // Ignore sound errors
            }
        }
    }
}
