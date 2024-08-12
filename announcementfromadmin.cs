using CRUD_Operations;
using LAB2.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB2
{
    public partial class announcementfromadmin : UserControl
    {
        public announcementfromadmin()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Events(Event_name, Event_date, Description, Admin_id) VALUES(@eventName, @eventdate, @description, (SELECT Admin_Id FROM Admin))", con);
                cmd.Parameters.AddWithValue("@eventName", textBox2.Text);
                cmd.Parameters.AddWithValue("@description", textBox1.Text);
                cmd.Parameters.AddWithValue("@eventdate", DateTime.Parse(dateTimePicker1.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Announcement added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        }
}
