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
    public partial class addmarks : Form
    {
        string assessmentid;
        string teacherId;
        int totalMarks;
        public addmarks(string id, string teacherId, int totalmarks)
        {
           InitializeComponent();
            this.assessmentid = id;
            this.teacherId = teacherId;
            this.totalMarks = totalmarks;
            this.Load += Form_Load;
        }

        private void addmarks_Load(object sender, EventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string studentId = row.Cells["Student_Id"].Value?.ToString();
                    string marks = row.Cells["ObtainMarks"].Value?.ToString();

                    if (!string.IsNullOrEmpty(marks))
                    {
                        int enteredMarks = Convert.ToInt32(marks);
                        if (enteredMarks > totalMarks)
                        {
                            MessageBox.Show("Entered marks cannot exceed total marks.");
                            row.Cells["ObtainMarks"].Value = DBNull.Value;
                        }
                    }

                    AddStudentMarksToResult(assessmentid, studentId, marks);
                }
            }

            MessageBox.Show("Marks are saved successfully");
        }
        private void AddStudentMarksToResult(string assessmentid, string studentId, string marks)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
               

                SqlCommand insertCmd = new SqlCommand("INSERT INTO Result (Evaluation_id, StudentID, MarksObtained) VALUES (@Evaluation_id, @StudentID, @MarksObtained)", con);
                insertCmd.Parameters.AddWithValue("@Evaluation_id", assessmentid);
                insertCmd.Parameters.AddWithValue("@StudentID", studentId);
                insertCmd.Parameters.AddWithValue("@MarksObtained", string.IsNullOrEmpty(marks) ? (object)DBNull.Value : marks);

                insertCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to add attendance: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
