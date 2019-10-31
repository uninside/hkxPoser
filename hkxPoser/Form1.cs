using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hkxPoser
{
    public partial class MainWindow : Form
    {
        internal Viewer _viewer = null;
        private Timer _playtimer = new Timer();
        Settings _settings;

        public MainWindow(Settings settings, string loadfilePath = "")
        {
            InitializeComponent();

            _settings = settings;

            this.ClientSize = settings.ClientSize;
            _viewer = new Viewer(settings);
            
            _viewer.LoadAnimationEvent += delegate(object sender, EventArgs args)
            {
                trackBar1.Maximum = _viewer.GetNumFrames()-1;
                trackBar1.Value = 0;
            };

            int ret = _viewer.PreLoadAnimation(ref loadfilePath);
            if(ret != 0)
            {
                loadfilePath = "";
            }

            if (_viewer.InitializeGraphics(this, loadfilePath))
            {
                timer1.Enabled = true;
            }

            

            RepeatCheckbox.Checked = settings.Repeat;
            BoneViewingCheckBox.Checked = settings.BoneViewing;
            _viewer.BoneViewing = BoneViewingCheckBox.Checked;
            RepeatCheckbox.CheckedChanged += RepeatCheckbox_CheckedChanged;
            _playtimer.Interval = 1;
            _playtimer.Tick += Playtimer_Tick;
        }

        private void RepeatCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Repeat = RepeatCheckbox.Checked;
            _settings.Dump();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _viewer.Update();
            _viewer.Render();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewer.command_man.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _viewer.command_man.Redo();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "hkx files|*.hkx";
            dialog.FilterIndex = 0;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string source_file = dialog.FileName;
                _viewer.LoadAnimation(source_file);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "out.hkx";
            dialog.Filter = "hkx files|*.hkx";
            dialog.FilterIndex = 0;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dest_file = dialog.FileName;
                _viewer.SaveAnimation(dest_file);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _viewer.SetCurrentPose(trackBar1.Value);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (_playtimer.Enabled)
            {
                PlayButton.Text = "\u25B6";
                _playtimer.Stop();
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string source_file in (string[])e.Data.GetData(DataFormats.FileDrop))
                    _viewer.LoadAnimation(source_file);
            }
        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
        }

        

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Button thisButton = sender as Button;
            
            if(!_playtimer.Enabled)
            {
                
                thisButton.Text = "\u275A\u275A";
                _playtimer.Start();
            }
            else
            {
                thisButton.Text = "\u25B6";
                _playtimer.Stop();
            }

        }

        private void Playtimer_Tick(object sender, EventArgs e)
        {
            if (trackBar1.Maximum == trackBar1.Value)
            {
                if (RepeatCheckbox.Checked)
                {
                    trackBar1.Value = 0;
                }
                else
                {
                    PlayButton.Text = "\u25B6";
                    _playtimer.Stop();
                }
            }
            else 
            { 
                trackBar1.Value += 1;
            }

        }

        private void WindowResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                return;

            _settings.ClientSize = this.ClientSize;
            _settings.Dump();
        }

        private void BoneViewingCheckbox_Changed(object sender, EventArgs e)
        {
            _viewer.BoneViewing = BoneViewingCheckBox.Checked;
            _settings.BoneViewing = BoneViewingCheckBox.Checked;
            _settings.Dump();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();

            about.ShowDialog();



        }
    }
}
