
using System;
using System.Windows.Forms;
using Hnefatafl.Views;

namespace Hnefatafl
{
    /// <summary>
    /// Form that allows the user to select board size and game mode.
    /// </summary>
    public class BoardSizeForm : Form
    {
        private ComboBox cmbBoardSize;
        private RadioButton rdoVsPlayer;
        private RadioButton rdoVsAI;
        private Button btnStart;

        public BoardSizeForm()
        {
            this.Text = "Wybór planszy";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new System.Drawing.Size(300, 200);

            Label lblSize = new Label
            {
                Text = "Wybierz rozmiar planszy:",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblSize);

            cmbBoardSize = new ComboBox
            {
                Location = new System.Drawing.Point(20, 45),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbBoardSize.Items.AddRange(new object[] { "7x7", "9x9", "11x11" });
            cmbBoardSize.SelectedIndex = 2; // default 11x11
            this.Controls.Add(cmbBoardSize);

            rdoVsPlayer = new RadioButton
            {
                Text = "Gracz vs Gracz",
                Location = new System.Drawing.Point(20, 80),
                Checked = true
            };
            this.Controls.Add(rdoVsPlayer);

            rdoVsAI = new RadioButton
            {
                Text = "Gracz vs Komputer",
                Location = new System.Drawing.Point(20, 105)
            };
            this.Controls.Add(rdoVsAI);

            btnStart = new Button
            {
                Text = "Rozpocznij grę",
                Location = new System.Drawing.Point(20, 140),
                Width = 120
            };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            int boardSize = 11;
            switch (cmbBoardSize.SelectedItem.ToString())
            {
                case "7x7": boardSize = 7; break;
                case "9x9": boardSize = 9; break;
                case "11x11": boardSize = 11; break;
            }

            bool vsAI = rdoVsAI.Checked;

            // Start game form with selected options
            GameForm gameForm = new GameForm(vsAI, boardSize);
            gameForm.Show();
            this.Hide();
        }
    }
}
