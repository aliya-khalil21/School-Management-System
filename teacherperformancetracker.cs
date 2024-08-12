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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LAB2
{
    public partial class teacherperformancetracker : Form
    {

        
        public teacherperformancetracker()
        {
            InitializeComponent();
            LoadData();


        }



        private void LoadData()
        {
          

                try
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("SELECT p.FirstName AS TeacherFirstName, p.LastName AS TeacherLastName, c.Class_Name AS AssignedClass, AVG(r.MarksObtained * 100.0 / ev.totalmarks) AS AveragePercentageMarks FROM Teacher t JOIN Person p ON t.Teacher_id = p.Id JOIN Class c ON t.Class_Id = c.Class_Id JOIN Enrollments en ON en.grade_level = c.Class_Name JOIN Result r ON en.student_id = r.StudentID JOIN Evaluation ev ON r.Evaluation_id = ev.id GROUP BY p.FirstName, p.LastName, c.Class_Name\r\n", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
           }

            
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

          
        }
    }
}

