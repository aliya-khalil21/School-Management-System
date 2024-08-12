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
    public partial class ChildResult : Form
    {
        string parentId;
        public ChildResult(string parentId)
        {
            InitializeComponent();
            this.Load += Form_Load;
            this.parentId = parentId;
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
                                                COALESCE(r.MarksObtained, 0) AS MarksObtained,
                                                COALESCE(e.TotalMarks, 0) AS TotalMarks
                                          FROM Person AS p 
                                          JOIN Students AS s ON p.Id = s.Student_Id 
                                          LEFT JOIN 
                                          (
                                              SELECT StudentID, SUM(MarksObtained) AS MarksObtained 
                                              FROM Result 
                                              GROUP BY StudentID
                                          ) AS r ON p.Id = r.StudentID 
                                          LEFT JOIN
                                          (
                                              SELECT SUM(totalmarks) AS TotalMarks
                                              FROM Evaluation
                                          ) AS e ON 1=1
                                          WHERE Parent_id = @parentId", con);
                cmd.Parameters.AddWithValue("@parentId", parentId);
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
