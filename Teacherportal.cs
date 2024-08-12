using LAB2.DL;
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
    public partial class Teacherportal : Form
    {
        string id;
        public Teacherportal(string Id)
        {
            InitializeComponent();

            this.id = Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            attendance1 AddAttendance = new attendance1(id);
            this.Hide();
            AddAttendance.ShowDialog();
            this.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Evaluation Addassignment = new Evaluation(id);
            this.Hide();
            Addassignment.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Announcement1 announcement = new Announcement1();
            this.Hide();
            announcement.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PTcommunication communication = new PTcommunication(id);
            this.Hide();
            communication.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Results results = new Results(id);
            this.Hide();
            results.ShowDialog();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Performance results = new Performance(id);
            this.Hide();
            results.ShowDialog();
            this.Show();
        }
    }
}
