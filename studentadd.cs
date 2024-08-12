using CRUD_Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using LAB2.DL;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;

namespace LAB2
{
    public partial class studentadd : UserControl
    {
        public studentadd()
        {
            InitializeComponent();
            load();
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
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
            if (!(STU.checkValidInputs1(textBox2.Text, textBox8.Text, textBox6.Text, textBox7.Text)))
            {
                return;
            }
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("InsertPersonAndParent", con);  // Corrected stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;  // Specify that it's a stored procedure

                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox8.Text);  // Assuming this is the Last Name field
                cmd.Parameters.AddWithValue("@ContactDetails", textBox6.Text);
                cmd.Parameters.AddWithValue("@Email", textBox7.Text);
                cmd.Parameters.AddWithValue("@Cnic", textBox5.Text);
                cmd.Parameters.AddWithValue("@address", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Relationship", textBox1.Text);  // Corrected parameter name

                cmd.ExecuteNonQuery();
                MessageBox.Show("parent saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
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
                string address = selectedRow.Cells["Address"].Value.ToString();
                string relationship = selectedRow.Cells["Relationship"].Value.ToString();

                // Populate respective text boxes with the data
                textBox2.Text = firstName;
                textBox8.Text = lastName;
                textBox6.Text = contactDetails;
                textBox7.Text = email;
                textBox5.Text = cnic;
                textBox5.ReadOnly = true;
                textBox4.Text = address;

                textBox1.Text = relationship;
            }
            if (e.ColumnIndex == 0 && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell)
            {
                // Get the data from the clicked row
                string firstName = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                string lastName = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                string contactDetails = dataGridView1.Rows[e.RowIndex].Cells["ContactDetails"].Value.ToString();
                string email = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                string cnic = dataGridView1.Rows[e.RowIndex].Cells["CNIC"].Value.ToString();
                string gender = dataGridView1.Rows[e.RowIndex].Cells["Gender"].Value.ToString();
                string relationship = dataGridView1.Rows[e.RowIndex].Cells["Relationship"].Value.ToString();

                var con = Configuration.getInstance().getConnection();
                string query = "SELECT Id FROM Person WHERE FirstName = @FirstName AND LastName = @LastName AND ContactDetails = @ContactDetails AND Email = @Email AND CNIC = @CNIC AND Gender = @Gender";

                // Create a command with parameters
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@ContactDetails", contactDetails);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@CNIC", cnic);
                cmd.Parameters.AddWithValue("@Gender", gender);
                object result = cmd.ExecuteScalar();

                if (result != null) // Check if the result is not null
                {
                    string parentId = result.ToString(); // Convert the result to string

                    // Pass parentId to buttonOpenChildForm_Click method
                    buttonOpenChildForm_Click(parentId);
                    

                }
                else
                {
                    MessageBox.Show("Parent ID not found for the selected data.");
                }
            }
            if (e.ColumnIndex == 1 && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell)
            {
                // Get the data from the clicked row
                string firstName = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                string lastName = dataGridView1.Rows[e.RowIndex].Cells["LastName"].Value.ToString();
                string contactDetails = dataGridView1.Rows[e.RowIndex].Cells["ContactDetails"].Value.ToString();
                string email = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                string cnic = dataGridView1.Rows[e.RowIndex].Cells["CNIC"].Value.ToString();
                string gender = dataGridView1.Rows[e.RowIndex].Cells["Gender"].Value.ToString();
                string relationship = dataGridView1.Rows[e.RowIndex].Cells["Relationship"].Value.ToString();

                var con = Configuration.getInstance().getConnection();

                // Check if the parent ID exists
                string parentIdQuery = "SELECT Id FROM Person WHERE FirstName = @FirstName AND LastName = @LastName AND ContactDetails = @ContactDetails AND Email = @Email AND CNIC = @CNIC AND Gender = @Gender";
                SqlCommand parentIdCmd = new SqlCommand(parentIdQuery, con);
                parentIdCmd.Parameters.AddWithValue("@FirstName", firstName);
                parentIdCmd.Parameters.AddWithValue("@LastName", lastName);
                parentIdCmd.Parameters.AddWithValue("@ContactDetails", contactDetails);
                parentIdCmd.Parameters.AddWithValue("@Email", email);
                parentIdCmd.Parameters.AddWithValue("@CNIC", cnic);
                parentIdCmd.Parameters.AddWithValue("@Gender", gender);
                object parentIdResult = parentIdCmd.ExecuteScalar();

                if (parentIdResult != null) // Check if the parent ID is found
                {
                    string parentId = parentIdResult.ToString(); // Convert the parent ID to string

                    // Retrieve the student ID for the parent
                    string studentIdQuery = "SELECT student_id FROM students WHERE parent_id = @ParentId";
                    SqlCommand studentIdCmd = new SqlCommand(studentIdQuery, con);
                    studentIdCmd.Parameters.AddWithValue("@ParentId", parentId);
                    object studentIdResult = studentIdCmd.ExecuteScalar();

                    if (studentIdResult != null) // Check if the student ID is found
                    {
                        string studentId = studentIdResult.ToString(); // Convert the student ID to string

                        // Store the student ID for later use
                        string deleteEnrollmentsQuery = "DELETE FROM Enrollments WHERE student_id = @StudentId";
                        SqlCommand deleteEnrollmentsCmd = new SqlCommand(deleteEnrollmentsQuery, con);
                        deleteEnrollmentsCmd.Parameters.AddWithValue("@StudentId", studentId);
                        deleteEnrollmentsCmd.ExecuteNonQuery();

                        string deleteStudentQuery = "DELETE FROM students WHERE student_id = @StudentId";
                        SqlCommand deleteStudentCmd = new SqlCommand(deleteStudentQuery, con);
                        deleteStudentCmd.Parameters.AddWithValue("@StudentId", studentId);
                        deleteStudentCmd.ExecuteNonQuery();

                        // Now delete data from Person using the stored student ID
                        string deletePersonQuery = "DELETE FROM Person WHERE Id = @StudentId";
                        SqlCommand deletePersonCmd = new SqlCommand(deletePersonQuery, con);
                        deletePersonCmd.Parameters.AddWithValue("@StudentId", studentId);
                        deletePersonCmd.ExecuteNonQuery();

                        // Delete the parent
                        string deleteParentQuery = "DELETE FROM Parent WHERE id = @ParentId";
                        SqlCommand deleteParentCmd = new SqlCommand(deleteParentQuery, con);
                        deleteParentCmd.Parameters.AddWithValue("@ParentId", parentId);
                        deleteParentCmd.ExecuteNonQuery();

                        string deleteParentpersonQuery = "DELETE FROM Person WHERE id = @ParentId";
                        SqlCommand deleteParent1Cmd = new SqlCommand(deleteParentpersonQuery, con);
                        deleteParent1Cmd.Parameters.AddWithValue("@ParentId", parentId);
                        deleteParent1Cmd.ExecuteNonQuery();

                        MessageBox.Show("Enrollments, associated student, parent, and corresponding Person data deleted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Student ID not found for the selected parent.");
                    }
                }
                else
                {
                    MessageBox.Show("Parent ID not found for the selected data.");
                }
            }
        }

        private void buttonOpenChildForm_Click(string parentId)
        {
            // Create an instance of student1 form and pass parentId
            student1 childForm = new student1(parentId);
            childForm.Show();
        }





        private void tableLayoutPanel8_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel11_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
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
            if (!(STU.checkValidInputs1(textBox2.Text, textBox8.Text, textBox6.Text, textBox7.Text)))
            {
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("UpdatePersonAndParent", con);  // Corrected stored procedure name
                cmd.CommandType = CommandType.StoredProcedure;  // Specify that it's a stored procedure

                cmd.Parameters.AddWithValue("@FirstName", textBox2.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox8.Text);  // Assuming this is the Last Name field
                cmd.Parameters.AddWithValue("@ContactDetails", textBox6.Text);
                cmd.Parameters.AddWithValue("@Email", textBox7.Text);
                cmd.Parameters.AddWithValue("@Cnic", textBox5.Text);
                cmd.Parameters.AddWithValue("@address", textBox4.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Relationship", textBox1.Text);  // Corrected parameter name

                cmd.ExecuteNonQuery();
                MessageBox.Show("parent  update  successfully.");
            }
            catch (Exception ex)
            {


            }
        }
        private void load()
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT * FROM PersonParentView", con);
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

        private void button3_Click(object sender, EventArgs e)
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
                INNER JOIN Parent pa ON p.ID = pa.id
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

                    message.AppendLine($"Is Parent: {reader["IsParent"]}");

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

        // Function to check if a person is a parent based on their ID
        private bool IsParent(int personID)
        {
           
            return false; // Placeholder logic, replace with actual implementation
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
