using CRUD_Operations;
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

namespace LAB2
{
    public partial class PTcommunication : Form
    {
        string teacherId;
        public PTcommunication(string id)
        {
            InitializeComponent();
            teacherId = id;
            this.Load += Form_Load;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "StartChat")
            {
                string studentId = dataGridView1.Rows[e.RowIndex].Cells["Student_id"].Value?.ToString();

                PTcommunication2 communication = new PTcommunication2(studentId, teacherId);
                this.Hide();
                communication.ShowDialog();
                this.Show();

            }
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Student_id, FirstName,RollNumber  FROM Person AS p JOIN Students AS s ON p.Id = s.Student_Id WHERE Teacher_id = @teacherId", con);
                cmd.Parameters.AddWithValue("@teacherId", teacherId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
