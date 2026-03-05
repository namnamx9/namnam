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

namespace QuanLyCafe_POS
{
    public partial class fLogin : Form
    {
        string connectionString =
@"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCafe_POS;Integrated Security=True";

        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT Role 
                                 FROM Account 
                                 WHERE UserName=@u 
                                 AND Password=@p 
                                 AND isActive=1";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string role = result.ToString();   // lấy role từ DB

                    MessageBox.Show("Đăng nhập thành công!");

                    this.Hide();
                    fMain f = new fMain(username, role);  // truyền username + role
                    f.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }
    }
}