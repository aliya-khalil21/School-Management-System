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
    public partial class Evaluation : Form
    {
        string teacherid;
        public Evaluation(string id)
        {
            InitializeComponent();
            this.teacherid = id;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string title = textBox1.Text.Trim();
            string marks = textBox3.Text.Trim();
            //string weightage = textBox4.Text.Trim();

            // Check if required fields are empty
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(marks))//|| string.IsNullOrEmpty(weightage))
            {
                MessageBox.Show("Please provide valid data for all fields.");
                return;
            }

            // Validate marks and weightage format
            int marksValue;
            if (!int.TryParse(marks, out marksValue)) //|| !int.TryParse(weightage, out weightageValue))
            {
                MessageBox.Show("Marks and weightage must be integers.");
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
               
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Evaluation WHERE name = @name", con);
                checkCmd.Parameters.AddWithValue("@name", title);
                int existingRecordCount = (int)checkCmd.ExecuteScalar();

                if (existingRecordCount > 0)
                {

                    SqlCommand cmd = new SqlCommand("UPDATE Evaluation SET totalmarks = @TotalMarks WHERE name = @Title", con);

                    cmd.Parameters.AddWithValue("@TotalMarks", marksValue);
                    cmd.Parameters.AddWithValue("@Title", title);

                    // Execute the query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update.");
                    }
                }
                else
                {
                    MessageBox.Show("Assessment with the provided title does not exist.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text.Trim();
            string marks = textBox3.Text.Trim();
            //string weightage = textBox4.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(marks))//|| string.IsNullOrEmpty(weightage))
            {
                MessageBox.Show("Please provide valid data for all fields.");
                return;
            }

            int marksValue, weightageValue;
            if (!int.TryParse(marks, out marksValue)) //|| !int.TryParse(weightage, out weightageValue))
            {
                MessageBox.Show("Marks and weightage must be integers.");
                return;
            }

            try
            {
                var con = Configuration.getInstance().getConnection();
                // Open the connection

                SqlCommand cmd = new SqlCommand("INSERT INTO Evaluation (name, totalmarks) VALUES (@Title, @TotalMarks)", con);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@TotalMarks", marksValue);
                //cmd.Parameters.AddWithValue("@TotalWeightage", weightageValue);

                // Execute the query
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("New assessment added successfully.");

                    // Clear input fields after adding data
                    textBox1.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                }
                else
                {
                    MessageBox.Show("Failed to add new assessment.");
                }
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
                Form_Load(sender, e); // Call the Form_Load method
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("select id, name, totalmarks from Evaluation", con);

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
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "EvaluateStudents" && e.ColumnIndex == dataGridView1.Columns["EvaluateStudents"].Index)
            {
                try
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value?.ToString();
                    int totalMarks = 0;

                    if (dataGridView1.Rows[e.RowIndex].Cells["totalmarks"].Value != null)
                    {
                        int.TryParse(dataGridView1.Rows[e.RowIndex].Cells["totalmarks"].Value.ToString(), out totalMarks);
                    }

                    addmarks form4 = new addmarks(id, teacherid, totalMarks);
                    this.Hide();
                    form4.ShowDialog();
                    this.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Delete" && e.ColumnIndex == dataGridView1.Columns["Delete"].Index)
            {
                string evaluationId = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();

                DeleteEvaluation(evaluationId);
            }
        }
        private void DeleteEvaluation(string evaluationId)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                

                SqlCommand deleteCmd = new SqlCommand("DELETE FROM Evaluation WHERE id = @evaluationId", con);
                deleteCmd.Parameters.AddWithValue("@evaluationId", evaluationId);

                int rowsAffected = deleteCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Evaluation deleted successfully.");

                }
                else
                {
                    MessageBox.Show("No evaluation found with the specified ID.");
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
