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
    public partial class ParentCommunication : Form
    {
        string parentId;
        public ParentCommunication(string parentId)
        {
            InitializeComponent();
            this.parentId = parentId;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Message = textBox1.Text.Trim();
            string message = "(SentByParent) : " + Message;

            try
            {

                if (!string.IsNullOrEmpty(parentId))
                {
                    InsertIntoParentTeacher(parentId, message);
                    MessageBox.Show("Send message successfully.");
                }
                else
                {
                    MessageBox.Show("Parent not found for the selected student.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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
                SqlCommand cmd = new SqlCommand("SELECT Timestamp, Message  FROM Parent_Teacher_Communication where Parent_id = @parentId", con);
                cmd.Parameters.AddWithValue("@parentId", parentId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["Timestamp"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void InsertIntoParentTeacher(string parentId, string message)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
              
                DateTime timestamp = DateTime.Now;
                string teacherId = GetTeacherIdByParentId();
                int Communicationid = GenerateUniqueId();
                SqlCommand insertCmd = new SqlCommand("INSERT INTO Parent_Teacher_Communication (Communication_id, Parent_id, Teacher_id, Message, Timestamp) VALUES (@Communicationid, @Parentid, @Teacherid, @Message, @Timestamp)", con);
                insertCmd.Parameters.AddWithValue("@Communicationid", Communicationid);
                insertCmd.Parameters.AddWithValue("@Parentid", parentId);
                insertCmd.Parameters.AddWithValue("@Teacherid", teacherId);
                insertCmd.Parameters.AddWithValue("@Message", message);
                insertCmd.Parameters.AddWithValue("@Timestamp", timestamp);

                insertCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error to send message: " + ex.Message);
            }
        }
        public string GetTeacherIdByParentId()
        {
            string teacherId = null;

            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT Teacher_id FROM Students WHERE Parent_id = @parentID", con);
                cmd.Parameters.AddWithValue("@parentID", parentId);
                
                teacherId = cmd.ExecuteScalar()?.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return teacherId;
        }


        private int GenerateUniqueId()
        {
            var con = Configuration.getInstance().getConnection();



            SqlCommand cmd = new SqlCommand("SELECT MAX(Communication_id) FROM Parent_Teacher_Communication", con);
               
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
