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
    public partial class tattendance1 : Form
    {
        int id;
        public tattendance1(int id)
        {
            InitializeComponent();
            this.id = id;
            this.Load += Form_Load;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName,id FROM[dbo].[Person] WHERE Id IN(SELECT Teacher_id FROM[dbo].[Teacher])" , con);
                
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the connection before using it
                var con = Configuration.getInstance().getConnection();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Retrieve student ID and attendance status from the DataGridView
                    string teacherId = row.Cells["id"].Value?.ToString();
                    string attendanceStatus = row.Cells["Attendance"].Value?.ToString();
                    bool  onTime = false;
                    if (row.Cells["ontime"] is DataGridViewCheckBoxCell cell && cell.Value != null && (bool)cell.Value)
                    {
                        onTime = true;
                    }

                    MessageBox.Show("Attendance added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveAttendanceToDatabase(SqlConnection con, string teacherId, string attendanceStatus, bool onTime, int attendanceId)
        {
            // Check if an attendance record already exists for the given teacher ID and attendance ID
            string queryCheck = "SELECT COUNT(*) FROM staffAttendance WHERE teacher_id = @teacherId AND Attendance_id = @attendanceId";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, con);
            cmdCheck.Parameters.AddWithValue("@teacherId", teacherId);
            cmdCheck.Parameters.AddWithValue("@attendanceId", attendanceId);
            int existingRecordsCount = (int)cmdCheck.ExecuteScalar();

            if (existingRecordsCount > 0)
            {
                // If an attendance record exists, update the attendance status and on-time status
                string queryUpdate = "UPDATE staffAttendance SET Attendance_status = @attendanceStatus, Ontime = @onTime WHERE teacher_id = @teacherId AND Attendance_id = @attendanceId";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con);
                cmdUpdate.Parameters.AddWithValue("@teacherId", teacherId);
                cmdUpdate.Parameters.AddWithValue("@attendanceId", attendanceId);
                cmdUpdate.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);
                cmdUpdate.Parameters.AddWithValue("@onTime", onTime);
                cmdUpdate.ExecuteNonQuery();
            }
            else
            {
                // If no attendance record exists, insert a new attendance record
                string queryInsert = "INSERT INTO staffAttendance (teacher_id, Attendance_id, Attendance_status, Ontime) VALUES (@teacherId, @attendanceId, @attendanceStatus, @onTime)";
                SqlCommand cmdInsert = new SqlCommand(queryInsert, con);
                cmdInsert.Parameters.AddWithValue("@teacherId", teacherId);
                cmdInsert.Parameters.AddWithValue("@attendanceId", attendanceId);
                cmdInsert.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);
                cmdInsert.Parameters.AddWithValue("@onTime", onTime);
                cmdInsert.ExecuteNonQuery();
            }
        }



    }
}
