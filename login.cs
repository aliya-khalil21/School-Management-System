using CRUD_Operations;
using LAB2.DL;
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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
        private bool CheckAdminCredentials(string cnic, string id)
        {
            bool exists = false;
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE Admin_Id = @id AND CNIC = @cnic", con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cnic", cnic);

               
                int count = (int)cmd.ExecuteScalar();
                exists = (count > 0);

              
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking credentials: " + ex.Message);
            }

            return exists;
        }

        private bool CheckTeacherCredentials(string cnic, string id)
        {
            bool exists = false;
            try
            {

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Person AS p JOIN Teacher AS t ON p.Id = t.Teacher_id WHERE p.CNIC = @cnic AND p.Id = @id AND t.Teacher_id = @tid", con);


                cmd.Parameters.AddWithValue("@tid", id);
                cmd.Parameters.AddWithValue("@cnic", cnic);
                cmd.Parameters.AddWithValue("@id", id);

                int count = (int)cmd.ExecuteScalar();
                exists = (count > 0);
            }




            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking credentials: " + ex.Message);
            }

            return exists;
        }

        private bool CheckParentCredentials(string cnic, string id)
        {

            bool exists = false;
            try
            {

                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Person AS p JOIN Parent AS t ON p.Id = t.Id WHERE p.CNIC = @cnic AND p.Id = @id AND t.Id = @tid", con);


                cmd.Parameters.AddWithValue("@tid", id);
                cmd.Parameters.AddWithValue("@cnic", cnic);
                cmd.Parameters.AddWithValue("@id", id);

                int count = (int)cmd.ExecuteScalar();
                exists = (count > 0);
            }




            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking credentials: " + ex.Message);
            }

            return exists;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedRole = comboBox1.SelectedItem?.ToString();
            string Cnic = textBox2.Text;
            string Id = textBox1.Text;

            int cnic;
            int id;

            if (string.IsNullOrEmpty(selectedRole) || string.IsNullOrEmpty(Cnic) || string.IsNullOrEmpty(Id))
            {
                MessageBox.Show("Please provide valid data for all fields.");
                return;
            }

            if (!int.TryParse(Id, out id) || !int.TryParse(Cnic, out cnic))
            {
                MessageBox.Show("Please enter valid data.");
                return;
            }

            if (selectedRole == "Teacher")
            {
                bool teacherExists = CheckTeacherCredentials(Cnic, Id);

                if (teacherExists)
                {
                    MessageBox.Show("Login Successful!");
                    Teacherportal teacher = new Teacherportal(Id);
                    this.Hide();
                    teacher.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Login Failed!");
                }
            }
            if (selectedRole == "Admin")
            {
                bool teacherExists = CheckAdminCredentials(Cnic, Id);

                if (teacherExists)
                {
                    MessageBox.Show("Login Successful!");
                    Admin teacher = new Admin();
                    this.Hide();
                    teacher.ShowDialog();
                    this.Show();
                }
            }
            if (selectedRole == "Parent")
            {
                bool teacherExists = CheckParentCredentials(Cnic, Id);

                if (teacherExists)
                {
                    MessageBox.Show("Login Successful!");
                    parent teacher = new parent(Id);
                    this.Hide();
                    teacher.ShowDialog();
                    this.Show();

                }
                else
                {
                    MessageBox.Show("Login Failed!");
                }
            }
        }
    }
}
