
using System;
using System.Windows.Forms;
using Hnefatafl.Views;

namespace Hnefatafl
{
    public class MainForm : Form
    {
        private Panel contentPanel;

        public MainForm()
        {
            this.Text = "Hnefatafl";
            this.Width = 700;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(contentPanel);

            ShowBoardSizeSelection();
        }

        private void ShowBoardSizeSelection()
        {
            var selector = new BoardSizeForm();
            selector.Dock = DockStyle.Fill;
            selector.GameStartRequested += (size, vsAI) =>
            {
                contentPanel.Controls.Clear();
                var gameForm = new GameForm(size, vsAI);
                gameForm.Dock = DockStyle.Fill;
                contentPanel.Controls.Add(gameForm);
            };
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(selector);
        }
    }
}
