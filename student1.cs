using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRUD_Operations;
using LAB2.BL;
using LAB2.DL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LAB2
{
    public partial class student1 : Form
    {
        private string parentId;
        public student1(string parentId)
        {
            InitializeComponent();
            this.parentId = parentId;
            loadfunction();

        }

        private void tableLayoutPanel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

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
            if (!(STU.checkValidInputs(textBox2.Text, textBox1.Text, textBox9.Text, textBox8.Text,textBox6.Text)))
            {
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("InsertStudentAndEnrollment", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters with values
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
                cmd.Parameters.AddWithValue("@ContactDetails", textBox9.Text);
                cmd.Parameters.AddWithValue("@Email", textBox8.Text);
                cmd.Parameters.AddWithValue("@CNIC", textBox5.Text);
                cmd.Parameters.AddWithValue("@Address", textBox4.Text); // Corrected parameter name to match stored procedure
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dateTimePicker1.Text));
                int year = dateTimePicker2.Value.Year;
                cmd.Parameters.AddWithValue("@AcademicYear", year);
                cmd.Parameters.AddWithValue("@RollNumber", textBox6.Text);
                cmd.Parameters.AddWithValue("@Class", textBox7.Text);
                cmd.Parameters.AddWithValue("@ParentId", parentId); // Assuming parentId is defined somewhere in your code

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student information saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

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
                string rollnumber= selectedRow.Cells["Rollnumber"].Value.ToString();
                
                string address = selectedRow.Cells["Address"].Value.ToString();

                textBox5.Text = cnic;
                // Populate respective text boxes with the data
                textBox2.Text = firstName;
                textBox1.Text = lastName;
                textBox9.Text = contactDetails;
                textBox8.Text = email;
                textBox4.Text = address;
                textBox6.Text = rollnumber;

            }
                if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Get the data from the selected row
                string firstName = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                string lastName = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                string rollNumber = dataGridView1.Rows[e.RowIndex].Cells["RollNumber"].Value.ToString();

                try
                {
                    var con = Configuration.getInstance().getConnection();

                    // Query to find the student ID based on FirstName and LastName from the Person table
                    SqlCommand findStudentIdCmd = new SqlCommand(
                        "SELECT s.Student_Id FROM Students s JOIN Person p ON s.Student_Id = p.Id WHERE p.FirstName = @FirstName AND p.LastName = @LastName AND s.Rollnumber = @RollNumber", con);
                    findStudentIdCmd.Parameters.AddWithValue("@FirstName", firstName);
                    findStudentIdCmd.Parameters.AddWithValue("@LastName", lastName);
                    findStudentIdCmd.Parameters.AddWithValue("@RollNumber", rollNumber);

                    // Execute the query to get the student ID
                    object studentIdResult = findStudentIdCmd.ExecuteScalar();

                    if (studentIdResult != null)
                    {
                        int studentId = Convert.ToInt32(studentIdResult);

                        SqlCommand deleteCmd = new SqlCommand("DeleteStudentAndEnrollment", con);
                        deleteCmd.CommandType = CommandType.StoredProcedure;
                        deleteCmd.Parameters.AddWithValue("@StudentId", studentId);

                        // Execute the stored procedure to delete the student record
                        deleteCmd.ExecuteNonQuery();

                        // Remove the row from the DataGridView
                        dataGridView1.Rows.RemoveAt(e.RowIndex);

                        MessageBox.Show("Student record deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Student not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void loadfunction()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM StudentDetailsView", con);
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

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
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
            if (!(STU.checkValidInputs(textBox2.Text, textBox1.Text, textBox9.Text, textBox8.Text, textBox6.Text)))
            {
                return;
            }
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Updatestudentdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters with values
                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox1.Text);
                cmd.Parameters.AddWithValue("@ContactDetails", textBox9.Text);
                cmd.Parameters.AddWithValue("@Email", textBox8.Text);
                cmd.Parameters.AddWithValue("@CNIC", textBox5.Text);
                cmd.Parameters.AddWithValue("@Address", textBox4.Text); // Corrected parameter name to match stored procedure
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(dateTimePicker1.Text));
                int year = dateTimePicker2.Value.Year;
                cmd.Parameters.AddWithValue("@AcademicYear", year);
                cmd.Parameters.AddWithValue("@RollNumber",textBox6.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Student information saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            promotestudent p=new promotestudent();
            p.Show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Establish database connection
                var con = Configuration.getInstance().getConnection();

                // SQL query to fetch Person ID based on matching first name or last name
                SqlCommand cmd = new SqlCommand(@"
            DECLARE @PersonID INT;
            SELECT @PersonID = ID
            FROM Person
            WHERE FirstName = @FirstName OR LastName = @LastName;

            IF @PersonID IS NOT NULL
            BEGIN
                SELECT p.*, 'Yes' AS IsParent
                FROM Person p
                INNER JOIN students pa ON p.ID = pa.student_id
                WHERE p.ID = @PersonID;
            END", con);

                // Create SqlCommand object

                // Add parameters for first and last name
                cmd.Parameters.AddWithValue("@FirstName", textBox3.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox3.Text);



                // Execute the query and read the results
                SqlDataReader reader = cmd.ExecuteReader();

                // Check if data is returned
                if (reader.HasRows)
                {
                    // Read the first row (assuming there's only one result expected)
                    reader.Read();

                    // Construct the message to display
                    StringBuilder message = new StringBuilder();
                    message.AppendLine("Person Details:");
                    message.AppendLine($"First Name: {reader["FirstName"]}");
                    message.AppendLine($"Last Name: {reader["LastName"]}");
                    message.AppendLine($"Gender: {reader["Gender"]}");
                    message.AppendLine($"CNIC: {reader["cnic"]}");
                    message.AppendLine($"Date_of_birth: {reader["cnic"]}");


                    // Close the reader and connection
                    reader.Close();


                    // Show the message box with the details
                    MessageBox.Show(message.ToString(), "Person Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Close the reader and connection
                    reader.Close();


                    // Inform user that no data was found
                    MessageBox.Show("No matching person found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
