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
    public partial class viewAttendance1 : Form
    {
        int ID;
        string teacherId;
        string Date;
        public viewAttendance1(int id, string Idlogin, string date)
        {
            InitializeComponent();
            this.ID = id;
            this.teacherId = Idlogin;
            this.Date = date;
            this.Load += Form_Load;
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT s.Student_Id, p.FirstName, Attendance_status FROM Person AS p JOIN Students AS s ON p.Id = s.Student_Id JOIN StudentAttendance AS sa ON s.Student_Id = sa.Student_id WHERE sa.Attendance_id = @attendanceId AND s.Teacher_id = @teacherId", con);
                cmd.Parameters.AddWithValue("@teacherId", teacherId);
                cmd.Parameters.AddWithValue("@attendanceId", ID);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                
                  

                    SqlCommand cmd = new SqlCommand("SELECT s.Student_Id, p.FirstName, sa.Attendance_status FROM Person AS p JOIN Students AS s ON p.Id = s.Student_Id JOIN StudentAttendance AS sa ON s.Student_Id = sa.Student_id WHERE sa.Attendance_id = @attendanceId AND s.Teacher_id = @teacherId AND sa.Attendance_status = 'absent'", con);
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
                    cmd.Parameters.AddWithValue("@attendanceId", ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string studentId = row["Student_Id"].ToString();

                        // Retrieve parent ID based on student ID
                        string parentId = GetParentId(con, studentId);

                        // Insert student ID and parent ID into the "Attendance_Alerts" table
                        InsertAttendanceAlert(con, studentId, parentId);
                    }

                    MessageBox.Show("Attendance alerts send successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private string GetParentId(SqlConnection con, string studentId)
        {
            string query = "SELECT Parent_id FROM Students WHERE Student_Id = @studentId";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@studentId", studentId);
            object result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : null;
        }

        private void InsertAttendanceAlert(SqlConnection con, string studentId, string parentId)
        {
            int idalert = GenerateUniqueId();
            string msg = "Respect parent your child is Absent";
            string query = "INSERT INTO Attendance_Alerts (Alert_id, id, Student_id, Date, Message) VALUES (@alertid , @parentId ,@studentId , @date, @msg)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@alertid", idalert);
            cmd.Parameters.AddWithValue("@parentId", parentId);
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@date", Date);
            cmd.Parameters.AddWithValue("@msg", msg);
            cmd.ExecuteNonQuery();
        }

        private int GenerateUniqueId()
        {
            using (var con = Configuration.getInstance().getConnection())
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                string query = "SELECT MAX(Alert_id) FROM Attendance_Alerts";
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
        }

    }
}
