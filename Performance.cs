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
    public partial class Performance : Form
    {
        string teacherid;
        public Performance(string id)
        {
            InitializeComponent();
            teacherid = id;
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
                SqlCommand cmd = new SqlCommand(@"SELECT p.Id AS Student_id, 
                                                p.FirstName, 
                                                s.RollNumber, 
                                                COALESCE((COALESCE(r.TotalMarksObtained, 0) * 100.0) / NULLIF(e.TotalMarks, 0), 0) AS PercentageObtainedMarks

                                          FROM Person AS p 
                                          JOIN Students AS s ON p.Id = s.Student_Id 
                                          LEFT JOIN 
                                          (
                                              SELECT StudentID, SUM(MarksObtained) AS TotalMarksObtained 
                                              FROM Result 
                                              GROUP BY StudentID
                                          ) AS r ON p.Id = r.StudentID 
                                          LEFT JOIN
                                          (
                                              SELECT SUM(totalmarks) AS TotalMarks
                                              FROM Evaluation
                                          ) AS e ON 1=1
                                          WHERE Teacher_id = @teacherId", con);
                cmd.Parameters.AddWithValue("@teacherId", teacherid);
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
