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

namespace LAB2
{
    public partial class Tattendance : Form
    {
        public Tattendance()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "CallAttendance" && e.ColumnIndex == dataGridView1.Columns["CallAttendance"].Index)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Attendance_id"].Value);
                    tattendance1 form4 = new tattendance1(id);
                    this.Hide();
                    form4.ShowDialog();
                    this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "ViewAttendance" && e.ColumnIndex == dataGridView1.Columns["ViewAttendance"].Index)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Attendance_id"].Value);
                    string date = dataGridView1.Rows[e.RowIndex].Cells["Attendance_date"].Value?.ToString();
                    viewattendance form4 = new viewattendance(id, date);
                    this.Hide();
                    form4.ShowDialog();
                    this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private int GenerateUniqueId()
        {
            var con = Configuration.getInstance().getConnection();
                string query = "SELECT MAX(Attendance_id) FROM Attendance";
                SqlCommand cmd = new SqlCommand(query, con);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int maxId;
                    if (int.TryParse(result.ToString(), out maxId))
                    {

                        return maxId + 1;
                    }
                }
                return 1;
            
        }

            private void button1_Click(object sender, EventArgs e)
           {
            try
            {
                DateTime attendanceDate = dateTimePicker1.Value.Date;

                int id = GenerateUniqueId();
                var con = Configuration.getInstance().getConnection();

                SqlCommand cmd = new SqlCommand("INSERT INTO Attendance (Attendance_id, Attendance_date) VALUES (@id, @attendanceDate)", con);
                 cmd.Parameters.AddWithValue("@id", id);
                 cmd.Parameters.AddWithValue("@attendanceDate", attendanceDate);
                 cmd.ExecuteNonQuery();

                    MessageBox.Show("Attendance added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Attendance", con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
           
        }
    }
}