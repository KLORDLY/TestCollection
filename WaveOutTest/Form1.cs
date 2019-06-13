using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveOutTest
{
    public partial class Form1 : Form
    {
        WaveOutClass waveOutClass = new WaveOutClass();
        WaveOutClass waveOutClass1 = new WaveOutClass();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            waveOutClass.playSounds(@"D:\StuffOnGithub\PublicStuff\TestCollection\WaveOutTest\Kalimba.wav");
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            waveOutClass.Pause();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            waveOutClass.Restart();
        }

        private void btnGetPos_Click(object sender, EventArgs e)
        {
            waveOutClass.GetPosition();
        }

        private void btnStart2_Click(object sender, EventArgs e)
        {
            waveOutClass1.playSounds(@"F:\Work\Test\WaveOutTest\WaveOutTest\Maid with the Flaxen Hair.wav");
        }

        private void btnPause2_Click(object sender, EventArgs e)
        {
            waveOutClass1.Pause();
        }

        private void btnRestart2_Click(object sender, EventArgs e)
        {
            waveOutClass1.Restart();
        }

        private void btnPos2_Click(object sender, EventArgs e)
        {
            waveOutClass1.GetPosition();
        }
    }
}
