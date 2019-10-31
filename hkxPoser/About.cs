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
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void AboutMouseDown_event(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lb = sender as LinkLabel;
            System.Diagnostics.Process.Start(lb.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lb = sender as LinkLabel;
            System.Diagnostics.Process.Start(lb.Text);

        }
    }
}
