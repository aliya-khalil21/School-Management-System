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
    public partial class AlertParents : Form
    {
        string parentid;
        public AlertParents(string parentid)
        {
            InitializeComponent();
            this.Load += Form_Load;
            this.parentid = parentid;
        }
        private void Form_Load(object sender, EventArgs e)
        {
            try
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand(@"SELECT Date, Message 
                                  FROM Attendance_Alerts 
                                  WHERE Student_id IN (SELECT Student_Id 
                                                        FROM Students 
                                                        WHERE Parent_id = @parentID)", con);
                cmd.Parameters.AddWithValue("@parentID", parentid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
