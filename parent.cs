using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB2
{
    public partial class parent : Form
    {
        string id;
        public parent(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlertParents alert = new AlertParents(id);
            this.Hide();
            alert.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Announcement1 announcement = new Announcement1();
            this.Hide();
            announcement.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ParentCommunication pcom = new ParentCommunication(id);
            this.Hide();
            pcom.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChildResult cresult = new ChildResult(id);
            this.Hide();
            cresult.ShowDialog();
            this.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
