using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hnefatafl.Views
{
    /// <summary>
    /// Main menu form for selecting game mode and viewing rules.
    /// </summary>
    public class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            this.Text = "Hnefatafl - Main Menu";
            this.ClientSize = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            var btnVsPlayer = new Button { Text = "Play vs Player", Location = new Point(80, 30), Size = new Size(140, 30) };
            var btnVsAI = new Button { Text = "Play vs AI", Location = new Point(80, 70), Size = new Size(140, 30) };
            var btnRules = new Button { Text = "Game Rules", Location = new Point(80, 110), Size = new Size(140, 30) };

            btnVsPlayer.Click += (s, e) => { new GameForm(false).Show(); };
            btnVsAI.Click += (s, e) => { new GameForm(true).Show(); };
            btnRules.Click += (s, e) =>
            {
                MessageBox.Show("Hnefatafl is a strategy board game...\n\nGoal:\n- Defenders must help the king escape to a corner.\n- Attackers must capture the king.\n\nRules:\n- Pieces move orthogonally any number of spaces.\n- Captures are made by surrounding an enemy piece on two opposite sides.", "Game Rules");
            };

            this.Controls.Add(btnVsPlayer);
            this.Controls.Add(btnVsAI);
            this.Controls.Add(btnRules);
        }
    }
}
