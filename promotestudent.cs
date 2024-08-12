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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB2
{
    public partial class promotestudent : Form
    {
        public promotestudent()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("PromoteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters with values
                cmd.Parameters.AddWithValue("@FromMarks", textBox2.Text);
                cmd.Parameters.AddWithValue("@ToMarks", textBox1.Text);
                cmd.Parameters.AddWithValue("@Class", textBox3.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student information saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
