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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace LAB2
{
    public partial class attendance2 : Form
    {
        public int id;
        public string teacherId;
        public attendance2(int id, String Idlogin)
        {
            InitializeComponent();
           
            this.id = id;
            this.teacherId = Idlogin;
            this.Load += Form_Load;
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

        private void attendance2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var con = Configuration.getInstance().getConnection();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Retrieve student ID and attendance status from the DataGridView
                        string studentId = row.Cells["Student_id"].Value?.ToString();
                        string attendanceStatus = row.Cells["Attendance"].Value?.ToString();

                        // Check if either value is null before saving
                        if (studentId != null && attendanceStatus != null)
                        {
                            // Save the attendance status to the database
                            SaveAttendanceToDatabase(con, studentId, attendanceStatus, id);
                        }
                    }

                    MessageBox.Show("Attendance added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SaveAttendanceToDatabase(SqlConnection con, string studentId, string attendanceStatus, int attendanceId)
        {
            // Check if an attendance record already exists for the given student ID and attendance ID
            string queryCheck = "SELECT COUNT(*) FROM StudentAttendance WHERE Student_id = @studentId AND Attendance_id = @attendanceId";
            SqlCommand cmdCheck = new SqlCommand(queryCheck, con);
            cmdCheck.Parameters.AddWithValue("@studentId", studentId);
            cmdCheck.Parameters.AddWithValue("@attendanceId", attendanceId);
            int existingRecordsCount = (int)cmdCheck.ExecuteScalar();

            if (existingRecordsCount > 0)
            {
                // If an attendance record exists, update the attendance status
                string queryUpdate = "UPDATE StudentAttendance SET Attendance_status = @attendanceStatus WHERE Student_id = @studentId AND Attendance_id = @attendanceId";
                SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con);
                cmdUpdate.Parameters.AddWithValue("@studentId", studentId);
                cmdUpdate.Parameters.AddWithValue("@attendanceId", attendanceId);
                cmdUpdate.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);
                cmdUpdate.ExecuteNonQuery();

            }
            else
            {
                // If no attendance record exists, insert a new attendance record
                string queryInsert = "INSERT INTO StudentAttendance (Student_id, Attendance_id, Attendance_status) VALUES (@studentId, @attendanceId, @attendanceStatus)";
                SqlCommand cmdInsert = new SqlCommand(queryInsert, con);
                cmdInsert.Parameters.AddWithValue("@studentId", studentId);
                cmdInsert.Parameters.AddWithValue("@attendanceId", attendanceId);
                cmdInsert.Parameters.AddWithValue("@attendanceStatus", attendanceStatus);
                cmdInsert.ExecuteNonQuery();

            }
        }
    }
}
