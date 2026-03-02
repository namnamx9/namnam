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

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        string connectionString =
@"Data Source=.\SQLEXPRESS;
Initial Catalog=QuanLyCafe;
Integrated Security=True;
TrustServerCertificate=True;";
        public fTableManager()
        {
            InitializeComponent();
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM TableFood";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Button btn = new Button();
                    btn.Width = 100;
                    btn.Height = 100;

                    string tableName = reader["TableName"].ToString();
                    string status = reader["Status"].ToString();

                    btn.Text = tableName + Environment.NewLine + status;

                    if (status == "Trống")
                        btn.BackColor = Color.Aqua;
                    else
                        btn.BackColor = Color.LightPink;

                    flpTable.Controls.Add(btn);
                }

                reader.Close();
            }
        }
        private void fTableManager_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
