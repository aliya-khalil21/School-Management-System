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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
using System.Drawing.Text;
using LAB2.DL;
using System.Web.UI.WebControls;

namespace LAB2
{
    public partial class teacheradd : UserControl
    {
        private string class_id;
        public teacheradd()
        { 
            InitializeComponent();
            loadfunction();
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            string gender = "";
            if (checkBox1.Checked)
            {
                gender = "female";
            }
            else if (checkBox2.Checked)
            {
                gender = "male";
            }
            else
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            if (!(STU.checkValidInputs1(textBox2.Text, textBox1.Text, textBox3.Text, textBox6.Text)))
            {
                return;
            }
            try
            {
                var con = Configuration.getInstance().getConnection();

                // Check if the class exists
                SqlCommand cmd = new SqlCommand("AddTeacherWithClass1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClassName", textBox7.Text);
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
                cmd.Parameters.AddWithValue("@ContactDetails", textBox3.Text);
                cmd.Parameters.AddWithValue("@Email", textBox6.Text);
                cmd.Parameters.AddWithValue("@CNIC", textBox5.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);


                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully teacher saved");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Get the data from the clicked row
                string firstName = selectedRow.Cells["FirstName"].Value.ToString();
                string lastName = selectedRow.Cells["LastName"].Value.ToString();
                string contactDetails = selectedRow.Cells["ContactDetails"].Value.ToString();
                string email = selectedRow.Cells["Email"].Value.ToString();
                string cnic = selectedRow.Cells["CNIC"].Value.ToString();
                string class_id = selectedRow.Cells["Class_id"].Value.ToString();
                string address = selectedRow.Cells["Address"].Value.ToString();
               
                textBox5.Text = cnic;
                // Populate respective text boxes with the data
                textBox2.Text = firstName;
                textBox1.Text = lastName;
                textBox3.Text = contactDetails;
                textBox6.Text = email;
                textBox7.Text = class_id;
                textBox4.Text = address;





                textBox5.ReadOnly = true;
            }
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                string firstName = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                string lastName = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                string classID = dataGridView1.Rows[e.RowIndex].Cells["Class_ID"].Value.ToString();

                try
                {
                    var con = Configuration.getInstance().getConnection();

                    // Query to find the teacher ID based on FirstName, LastName, and Class ID
                    SqlCommand findTeacherIdCmd = new SqlCommand(
                        "SELECT t.teacher_Id FROM Teacher t JOIN Person p ON t.teacher_Id = p.Id WHERE p.FirstName = @FirstName AND p.LastName = @LastName AND t.Class_id = @classid", con);
                    findTeacherIdCmd.Parameters.AddWithValue("@FirstName", firstName);
                    findTeacherIdCmd.Parameters.AddWithValue("@LastName", lastName);
                    findTeacherIdCmd.Parameters.AddWithValue("@classid", classID);

                    // Execute the query to get the teacher ID
                    object teacherIdResult = findTeacherIdCmd.ExecuteScalar();

                    if (teacherIdResult != null)
                    {
                        int teacherId = Convert.ToInt32(teacherIdResult);

                        // Update the Students table to set teacher_Id to NULL
                        SqlCommand updateStudentsCmd = new SqlCommand("UPDATE Students SET teacher_Id = NULL WHERE teacher_Id = @TeacherId", con);
                        updateStudentsCmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        updateStudentsCmd.ExecuteNonQuery(); // Execute the update command

                        // Delete the teacher record
                        SqlCommand deleteCmd = new SqlCommand("DeleteTeacherByTeacherId", con);
                        deleteCmd.CommandType = CommandType.StoredProcedure;
                        deleteCmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        deleteCmd.ExecuteNonQuery(); // Execute the stored procedure to delete the teacher record

                        dataGridView1.Rows.RemoveAt(e.RowIndex);

                        MessageBox.Show("Teacher record deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Teacher not found.");
                    }

                    con.Close(); // Close the connection after use
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting teacher record: " + ex.Message);
                }
            }




        }


        private void loadfunction()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM TeacherDetailsView", con);
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

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            string gender = "";
            if (checkBox1.Checked)
            {
                gender = "female";
            }
            else if (checkBox2.Checked)
            {
                gender = "male";
            }
            else
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            if (!(STU.checkValidInputs1(textBox2.Text, textBox1.Text, textBox3.Text, textBox6.Text)))
            {
                return;
            }
            var con = Configuration.getInstance().getConnection();

            // Create a SqlCommand object for the stored procedure
            SqlCommand cmd = new SqlCommand("UpdatePersonAndTeacher", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters to the stored procedure
            cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
            cmd.Parameters.AddWithValue("@ContactDetails", textBox3.Text);
            cmd.Parameters.AddWithValue("@Email", textBox6.Text);
            cmd.Parameters.AddWithValue("@CNIC", textBox5.Text);
            cmd.Parameters.AddWithValue("@Address", textBox4.Text);
            cmd.Parameters.AddWithValue("@Gender", gender);  // Assuming you have 'gender' defined somewhere
           


            // Execute the stored procedure
            cmd.ExecuteNonQuery();

            // Close the connection
           

            MessageBox.Show("Person and Teacher records updated successfully.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tattendance t=new Tattendance();
            t.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            teacherperformancetracker t = new teacherperformancetracker();
            t.Show();
        }
    }
    }

