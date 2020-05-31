using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Risk_game_AI
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            
            InitializeComponent();

        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var gs = Game_state.Instance;
            gs.SetAgents(comboBox1.SelectedIndex, comboBox2.SelectedIndex);

            if (radioButton1.Checked)
                gs.SetMap(1);
            else
                gs.SetMap(2);

            USA_map game = new USA_map();
            game.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
