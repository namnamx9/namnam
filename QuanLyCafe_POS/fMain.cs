using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe_POS
{
    public partial class fMain : Form
    {
        string currentUser;
        string currentRole;

        string connectionString =
        @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyCafe_POS;Integrated Security=True";
        string selectedImagePath = "";
        string imageFolder;


        FlowLayoutPanel flpFood;
        ListBox lbBill;
        Label lblTotal;
        Button btnPay;


        int currentTableId = -1;
        int currentBillId = -1;
        public fMain(string username, string role)
        {
            InitializeComponent();
            currentUser = username;
            currentRole = role;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Xin chào " + currentUser);
            btnDashboard.PerformClick();
            imageFolder = Application.StartupPath + "\\Images\\Food\\";
            Directory.CreateDirectory(imageFolder);
        }

        // =============================
        // CLEAR PANEL
        // =============================
        private void ClearPanel()
        {
            panelContent.Controls.Clear();
        }

        // =============================
        // DASHBOARD
        // =============================
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            ClearPanel();

            int revenue = 0;
            int billCount = 0;
            int tableUsing = 0;
            int employeeWorking = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string q1 = @"SELECT ISNULL(SUM(TotalMoney),0)
                              FROM Bill
                              WHERE CAST(DateCheckOut AS DATE) = CAST(GETDATE() AS DATE)
                              AND Status = 1";
                revenue = Convert.ToInt32(new SqlCommand(q1, conn).ExecuteScalar());

                string q2 = @"SELECT COUNT(*)
                              FROM Bill
                              WHERE CAST(DateCheckOut AS DATE) = CAST(GETDATE() AS DATE)";
                billCount = Convert.ToInt32(new SqlCommand(q2, conn).ExecuteScalar());

                string q3 = @"SELECT COUNT(*) FROM TableFood WHERE Status = N'Có người'";
                tableUsing = Convert.ToInt32(new SqlCommand(q3, conn).ExecuteScalar());

                string q4 = @"SELECT COUNT(*) FROM Employee WHERE IsWorking = 1";
                employeeWorking = Convert.ToInt32(new SqlCommand(q4, conn).ExecuteScalar());
            }

            CreateStatBox("Doanh thu hôm nay", revenue.ToString("N0") + " VND", 50, 80);
            CreateStatBox("Số hóa đơn", billCount.ToString(), 350, 80);
            CreateStatBox("Bàn đang sử dụng", tableUsing.ToString(), 50, 250);
            CreateStatBox("Nhân viên đang làm", employeeWorking.ToString(), 350, 250);
        }

        // =============================
        // TẠO BOX DASHBOARD
        // =============================
        private void CreateStatBox(string title, string value, int x, int y)
        {
            Panel box = new Panel();
            box.Size = new Size(250, 120);
            box.Location = new Point(x, y);
            box.BackColor = Color.LightBlue;
            box.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTitle.Location = new Point(10, 10);
            lblTitle.AutoSize = true;

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblValue.Location = new Point(10, 50);
            lblValue.AutoSize = true;

            box.Controls.Add(lblTitle);
            box.Controls.Add(lblValue);

            panelContent.Controls.Add(box);
        }

        // ================= QUẢN LÝ TÀI KHOẢN =================
        private void btnAccount_Click(object sender, EventArgs e)
        {
            ClearPanel();

            Label lblTitle = new Label()
            {
                Text = "QUẢN LÝ TÀI KHOẢN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(20, 70),
                Size = new Size(850, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // ===== LABEL =====
            Label lblUser = new Label() { Text = "Username", Location = new Point(20, 330) };
            Label lblPass = new Label() { Text = "Password", Location = new Point(180, 330) };
            Label lblDisplay = new Label() { Text = "DisplayName", Location = new Point(340, 330) };
            Label lblHoTen = new Label() { Text = "Họ tên", Location = new Point(500, 330) };

            // ===== TEXTBOX =====
            TextBox txtUser = new TextBox() { Location = new Point(20, 350), Width = 150 };
            TextBox txtPass = new TextBox() { Location = new Point(180, 350), Width = 150, PasswordChar = '*' };
            TextBox txtDisplay = new TextBox() { Location = new Point(340, 350), Width = 150 };
            TextBox txtHoTen = new TextBox() { Location = new Point(500, 350), Width = 200 };

            ComboBox cbRole = new ComboBox()
            {
                Location = new Point(720, 350),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cbRole.Items.Add("Admin");
            cbRole.Items.Add("Staff");
            cbRole.SelectedIndex = 1;

            CheckBox chkActive = new CheckBox()
            {
                Text = "Hoạt động",
                Location = new Point(20, 390),
                Checked = true
            };

            Button btnAdd = new Button() { Text = "Thêm", Location = new Point(20, 430) };
            Button btnUpdate = new Button() { Text = "Sửa", Location = new Point(120, 430) };
            Button btnDelete = new Button() { Text = "Xóa", Location = new Point(220, 430) };

            panelContent.Controls.AddRange(new Control[]
            {
        lblTitle, dgv,
        lblUser, lblPass, lblDisplay, lblHoTen,
        txtUser, txtPass, txtDisplay, txtHoTen,
        cbRole, chkActive,
        btnAdd, btnUpdate, btnDelete
            });

            LoadAccountData(dgv);

            dgv.CellClick += (s, ev) =>
            {
                if (dgv.CurrentRow == null) return;

                txtUser.Text = dgv.CurrentRow.Cells["UserName"].Value?.ToString();
                txtPass.Text = dgv.CurrentRow.Cells["Password"].Value?.ToString();
                txtDisplay.Text = dgv.CurrentRow.Cells["DisplayName"].Value?.ToString();
                txtHoTen.Text = dgv.CurrentRow.Cells["HoTen"].Value?.ToString();

                // ===== SỬA LỖI NULL ROLE =====
                object roleValue = dgv.CurrentRow.Cells["Role"].Value;
                if (roleValue != DBNull.Value && roleValue != null)
                    cbRole.SelectedIndex = Convert.ToInt32(roleValue);
                else
                    cbRole.SelectedIndex = 1;

                // ===== SỬA LỖI NULL isActive =====
                object activeValue = dgv.CurrentRow.Cells["isActive"].Value;
                if (activeValue != DBNull.Value && activeValue != null)
                    chkActive.Checked = Convert.ToBoolean(activeValue);
                else
                    chkActive.Checked = true;
            };

            btnAdd.Click += (s, ev) =>
                AddAccount(dgv, txtUser, txtPass, txtDisplay, txtHoTen, cbRole, chkActive);

            btnUpdate.Click += (s, ev) =>
                UpdateAccount(dgv, txtUser, txtPass, txtDisplay, txtHoTen, cbRole, chkActive);

            btnDelete.Click += (s, ev) =>
                DeleteAccount(dgv);
        }

        private void LoadAccountData(DataGridView dgv)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Account", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
        }

        private void AddAccount(DataGridView dgv,
    TextBox txtUser, TextBox txtPass,
    TextBox txtDisplay, TextBox txtHoTen,
    ComboBox cbRole, CheckBox chkActive)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Không được để trống Username hoặc Password");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Account
            (UserName, Password, DisplayName, HoTen, Role, isActive)
            VALUES (@u, @p, @d, @h, @r, @a)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPass.Text.Trim());
                    cmd.Parameters.AddWithValue("@d", txtDisplay.Text.Trim());
                    cmd.Parameters.AddWithValue("@h", txtHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@r", cbRole.SelectedIndex);
                    cmd.Parameters.AddWithValue("@a", chkActive.Checked);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm thành công!");
                LoadAccountData(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void UpdateAccount(DataGridView dgv,
    TextBox txtUser, TextBox txtPass,
    TextBox txtDisplay, TextBox txtHoTen,
    ComboBox cbRole, CheckBox chkActive)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE Account SET
                Password=@p,
                DisplayName=@d,
                HoTen=@h,
                Role=@r,
                isActive=@a
                WHERE UserName=@u";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", txtUser.Text);
                    cmd.Parameters.AddWithValue("@p", txtPass.Text);
                    cmd.Parameters.AddWithValue("@d", txtDisplay.Text);
                    cmd.Parameters.AddWithValue("@h", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@r", cbRole.SelectedIndex);
                    cmd.Parameters.AddWithValue("@a", chkActive.Checked);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Cập nhật thành công!");
                LoadAccountData(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void DeleteAccount(DataGridView dgv)
        {
            if (dgv.CurrentRow == null) return;

            int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Account WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Xóa thành công!");
                LoadAccountData(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        // Quản lý món //
        private void btnFood_Click(object sender, EventArgs e)
        {
            ClearPanel();

            Label lblTitle = new Label()
            {
                Text = "QUẢN LÝ MÓN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            DataGridView dgv = new DataGridView()
            {
                Location = new Point(20, 70),
                Size = new Size(900, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            Label lblName = new Label() { Text = "Tên món", Location = new Point(20, 340) };
            TextBox txtName = new TextBox() { Location = new Point(20, 360), Width = 150 };

            Label lblPrice = new Label() { Text = "Giá", Location = new Point(200, 340) };
            TextBox txtPrice = new TextBox() { Location = new Point(200, 360), Width = 100 };

            Label lblCategory = new Label() { Text = "Loại", Location = new Point(330, 340) };
            ComboBox cboCategory = new ComboBox()
            {
                Location = new Point(330, 360),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            CheckBox chkAvailable = new CheckBox()
            {
                Text = "Còn bán",
                Location = new Point(500, 360),
                Checked = true
            };

            PictureBox picFood = new PictureBox()
            {
                Location = new Point(650, 340),
                Size = new Size(120, 120),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            Button btnChooseImage = new Button()
            {
                Text = "Chọn ảnh",
                Location = new Point(650, 470)
            };

            Button btnAdd = new Button() { Text = "Thêm", Location = new Point(20, 420) };
            Button btnUpdate = new Button() { Text = "Sửa", Location = new Point(110, 420) };
            Button btnDelete = new Button() { Text = "Xóa", Location = new Point(200, 420) };

            Label lblSearch = new Label() { Text = "Tìm kiếm", Location = new Point(400, 420) };
            TextBox txtSearch = new TextBox() { Location = new Point(400, 440), Width = 200 };

            panelContent.Controls.AddRange(new Control[] {
        lblTitle, dgv,
        lblName, txtName,
        lblPrice, txtPrice,
        lblCategory, cboCategory,
        chkAvailable,
        picFood, btnChooseImage,
        btnAdd, btnUpdate, btnDelete,
        lblSearch, txtSearch
    });

            LoadCategory(cboCategory);
            LoadFood(dgv);

            // ===== EVENTS =====

            dgv.CellClick += (s, ev) =>
            {
                if (dgv.CurrentRow == null) return;

                txtName.Text = dgv.CurrentRow.Cells["FoodName"].Value?.ToString();
                txtPrice.Text = dgv.CurrentRow.Cells["Price"].Value?.ToString();

                // ===== IMAGE =====
                object imgValue = dgv.CurrentRow.Cells["ImagePath"].Value;

                if (imgValue != DBNull.Value && imgValue != null)
                    selectedImagePath = imgValue.ToString();
                else
                    selectedImagePath = "";

                // ===== AVAILABLE =====
                object availableValue = dgv.CurrentRow.Cells["isAvailable"].Value;

                if (availableValue != DBNull.Value && availableValue != null)
                    chkAvailable.Checked = Convert.ToBoolean(availableValue);
                else
                    chkAvailable.Checked = true;

                if (!string.IsNullOrEmpty(selectedImagePath) && File.Exists(selectedImagePath))
                    picFood.Image = Image.FromFile(selectedImagePath);
            };

            btnChooseImage.Click += (s, ev) =>
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.png;*.jpeg";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName);
                    string newPath = Path.Combine(imageFolder, fileName);

                    ResizeAndSaveImage(ofd.FileName, newPath, 300, 300);

                    selectedImagePath = newPath;
                    picFood.Image = Image.FromFile(newPath);
                }
            };

            btnAdd.Click += (s, ev) =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"INSERT INTO Food
            (FoodName,idCategory,Price,isAvailable,ImagePath)
            VALUES (@name,@cat,@price,@active,@img)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@cat", cboCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@price", float.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@active", chkAvailable.Checked);
                    cmd.Parameters.AddWithValue("@img", selectedImagePath);

                    cmd.ExecuteNonQuery();
                }

                LoadFood(dgv);
            };

            btnUpdate.Click += (s, ev) =>
            {
                if (dgv.CurrentRow == null) return;

                int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE Food SET
                FoodName=@name,
                idCategory=@cat,
                Price=@price,
                isAvailable=@active,
                ImagePath=@img
                WHERE Id=@id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@cat", cboCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@price", float.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@active", chkAvailable.Checked);
                    cmd.Parameters.AddWithValue("@img", selectedImagePath);

                    cmd.ExecuteNonQuery();
                }

                LoadFood(dgv);
            };

            btnDelete.Click += (s, ev) =>
            {
                if (dgv.CurrentRow == null) return;

                int id = Convert.ToInt32(dgv.CurrentRow.Cells["Id"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Food WHERE Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                LoadFood(dgv);
            };

            txtSearch.TextChanged += (s, ev) =>
            {
                LoadFood(dgv, txtSearch.Text);
            };
        }
        // ================= LOAD CATEGORY =================
        private void LoadCategory(ComboBox cbo)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id, CategoryName FROM Category", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cbo.DataSource = dt;
                cbo.DisplayMember = "CategoryName";
                cbo.ValueMember = "Id";
            }
        }

        // ================= LOAD FOOD =================
        private void LoadFood(DataGridView dgv, string keyword = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
        SELECT f.Id,
               f.FoodName,
               c.CategoryName,
               f.Price,
               f.isAvailable,
               f.ImagePath
        FROM Food f
        JOIN Category c ON f.idCategory = c.Id
        WHERE f.FoodName LIKE @key";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgv.DataSource = dt;
            }
        }

        // ================= RESIZE IMAGE =================
        private void ResizeAndSaveImage(string originalPath, string savePath, int w, int h)
        {
            using (Image original = Image.FromFile(originalPath))
            {
                Bitmap resized = new Bitmap(w, h);

                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.DrawImage(original, 0, 0, w, h);
                }

                resized.Save(savePath);
            }
        }
        private void Food_Click(object sender, EventArgs e)
        {
            if (currentTableId == -1)
            {
                MessageBox.Show("Chọn bàn trước!");
                return;
            }

            Button btn = sender as Button;
            int foodId = Convert.ToInt32(btn.Tag);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1️⃣ Tìm bill đang mở
                SqlCommand billCmd = new SqlCommand(
                    "SELECT id FROM Bill WHERE idTable=@id AND Status=0", conn);
                billCmd.Parameters.AddWithValue("@id", currentTableId);

                object billObj = billCmd.ExecuteScalar();

                if (billObj == null)
                {
                    // tạo bill mới
                    SqlCommand create = new SqlCommand(
                        @"INSERT INTO Bill(idTable, DateCheckIn, Status, Discount, TotalMoney)
                  OUTPUT INSERTED.id
                  VALUES(@id, GETDATE(), 0, 0, 0)", conn);

                    create.Parameters.AddWithValue("@id", currentTableId);
                    currentBillId = (int)create.ExecuteScalar();

                    // đổi trạng thái bàn
                    SqlCommand updateTable = new SqlCommand(
                        "UPDATE TableFood SET status=N'Có người' WHERE Id=@id", conn);
                    updateTable.Parameters.AddWithValue("@id", currentTableId);
                    updateTable.ExecuteNonQuery();
                }
                else
                {
                    currentBillId = Convert.ToInt32(billObj);
                }

                // 2️⃣ Lấy giá món
                SqlCommand priceCmd = new SqlCommand(
                    "SELECT Price FROM Food WHERE Id=@f", conn);
                priceCmd.Parameters.AddWithValue("@f", foodId);
                decimal price = Convert.ToDecimal(priceCmd.ExecuteScalar());

                // 3️⃣ Kiểm tra món đã tồn tại chưa
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT id, Quantity FROM BillInfo WHERE idBill=@b AND idFood=@f", conn);
                checkCmd.Parameters.AddWithValue("@b", currentBillId);
                checkCmd.Parameters.AddWithValue("@f", foodId);

                SqlDataReader reader = checkCmd.ExecuteReader();

                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    int quantity = Convert.ToInt32(reader["Quantity"]);
                    reader.Close();

                    SqlCommand update = new SqlCommand(
                        "UPDATE BillInfo SET Quantity=@q WHERE id=@id", conn);
                    update.Parameters.AddWithValue("@q", quantity + 1);
                    update.Parameters.AddWithValue("@id", id);
                    update.ExecuteNonQuery();
                }
                else
                {
                    reader.Close();

                    SqlCommand insert = new SqlCommand(
                        "INSERT INTO BillInfo(idBill,idFood,Quantity,Price) VALUES(@b,@f,1,@p)", conn);
                    insert.Parameters.AddWithValue("@b", currentBillId);
                    insert.Parameters.AddWithValue("@f", foodId);
                    insert.Parameters.AddWithValue("@p", price);
                    insert.ExecuteNonQuery();
                }
            }

            LoadBill(currentTableId);
            LoadTableButtons();
        }
        private void LoadBill(int tableId)
        {
            if (lbBill == null)
            {
                MessageBox.Show("lbBill is null");
                return;
            }
            lbBill.Items.Clear();
            currentBillId = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand billCmd = new SqlCommand(
                    "SELECT id FROM Bill WHERE idTable=@id AND Status=0", conn);
                billCmd.Parameters.AddWithValue("@id", tableId);

                object billObj = billCmd.ExecuteScalar();
                if (billObj == null)
                {
                    lblTotal.Text = "Tổng: 0 VND";
                    return;
                }

                currentBillId = Convert.ToInt32(billObj);

                SqlCommand cmd = new SqlCommand(@"
            SELECT f.FoodName, bi.Quantity, bi.Price
            FROM BillInfo bi
            JOIN Food f ON bi.idFood = f.Id
            WHERE bi.idBill = @id", conn);

                cmd.Parameters.AddWithValue("@id", currentBillId);

                SqlDataReader reader = cmd.ExecuteReader();

                decimal total = 0;

                while (reader.Read())
                {
                    string name = reader["FoodName"].ToString();
                    int quantity = Convert.ToInt32(reader["Quantity"]);
                    decimal price = Convert.ToDecimal(reader["Price"]);

                    decimal lineTotal = quantity * price;
                    total += lineTotal;

                    lbBill.Items.Add($"{name} x{quantity} = {lineTotal:N0}");
                }

                reader.Close();

                // cập nhật TotalMoney vào Bill
                SqlCommand updateTotal = new SqlCommand(
                    "UPDATE Bill SET TotalMoney=@t WHERE id=@id", conn);
                updateTotal.Parameters.AddWithValue("@t", total);
                updateTotal.Parameters.AddWithValue("@id", currentBillId);
                updateTotal.ExecuteNonQuery();

                lblTotal.Text = "Tổng: " + total.ToString("N0") + " VND";
            }
        }
        private void BtnPay_Click(object sender, EventArgs e)
        {
            if (currentBillId == -1) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand pay = new SqlCommand(
                    @"UPDATE Bill
              SET Status=1,
                  DateCheckOut=GETDATE()
              WHERE id=@id", conn);

                pay.Parameters.AddWithValue("@id", currentBillId);
                pay.ExecuteNonQuery();

                SqlCommand table = new SqlCommand(
                    "UPDATE TableFood SET status=N'Trống' WHERE Id=@id", conn);

                table.Parameters.AddWithValue("@id", currentTableId);
                table.ExecuteNonQuery();
            }

            MessageBox.Show("Thanh toán thành công!");

            currentBillId = -1;
            currentTableId = -1;

            lbBill.Items.Clear();
            lblTotal.Text = "Tổng: 0 VND";

            LoadTableButtons();
        }

        private void LoadTableButtons()
        {
            flpTable1.Controls.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM TableFood", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Button btn = new Button();
                    btn.Width = 120;
                    btn.Height = 80;

                    int tableId = (int)reader["id"];
                    string tableName = reader["TableName"].ToString();
                    string status = reader["status"].ToString();

                    btn.Text = tableName + Environment.NewLine + status;
                    btn.Tag = tableId; // lưu id để dùng sau
                    btn.Click += Table_Click;

                    if (status == "Trống")
                        btn.BackColor = Color.LightGreen;
                    else
                        btn.BackColor = Color.OrangeRed;

                    flpTable1.Controls.Add(btn);
                }
            }
        }

        private void Table_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentTableId = Convert.ToInt32(btn.Tag);

            LoadBill(currentTableId);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();

            // ===== LEFT: bàn =====
            flpTable1.Dock = DockStyle.Left;
            flpTable1.Width = 500;
            panelContent.Controls.Add(flpTable1);

            // ===== RIGHT: bill =====
            lbBill = new ListBox();
            lbBill.Width = 300;
            lbBill.Height = 300;
            lbBill.Location = new Point(520, 20);
            panelContent.Controls.Add(lbBill);

            lblTotal = new Label();
            lblTotal.Text = "Tổng: 0 VND";
            lblTotal.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotal.Location = new Point(520, 330);
            panelContent.Controls.Add(lblTotal);

            btnPay = new Button();
            btnPay.Text = "Thanh toán";
            btnPay.Location = new Point(520, 370);
            btnPay.Click += BtnPay_Click;
            panelContent.Controls.Add(btnPay);

            LoadTableButtons();
        }

        private void btnVoucher_Click(object sender, EventArgs e)
        {

        }
    }
    }