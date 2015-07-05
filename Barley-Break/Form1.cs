using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barley_Break
{
    public partial class Form1 : Form
    {
        Map map;

        public Form1()
        {
            InitializeComponent();
            CreateInterface();
        }

        private void buttonAI_Click(object sender, EventArgs e)
        {
            AI ai = new AI(map);
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            restart();
        }

        public void restart()
        {
            Controls.Remove(map);
            CreateInterface();
        }

        public void CreateInterface()
        {
            map = new Map();
            buttonRestart.Width = map.Width / 2;
            buttonAI.Width = buttonRestart.Width;
            textBox1.Width = buttonRestart.Width;
            buttonApply.Width = buttonRestart.Width;
            buttonRestart.Location = new Point(0, map.Size.Height + 3);
            buttonAI.Location = new Point(buttonRestart.Width, buttonRestart.Location.Y);
            textBox1.Location = new Point(0, buttonRestart.Location.Y + buttonRestart.Height + 1);
            buttonApply.Location = new Point(textBox1.Width, textBox1.Location.Y);
            this.Size = new Size(map.Size.Width + 16, textBox1.Location.Y + textBox1.Height + 3 + 40);
            Controls.Add(map);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            int n;
            if (Int32.TryParse(textBox1.Text, out n))
            {
                if (n > 12 || n < 3) { MessageBox.Show("Минимум 3 и максимум 12!"); }
                else
                {
                    Map.CountCells = n;
                    restart();
                }
            }
        }
    }
}
