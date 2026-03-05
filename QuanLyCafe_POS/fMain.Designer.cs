namespace QuanLyCafe_POS
{
    partial class fMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnAccount = new System.Windows.Forms.Button();
            this.btnFood = new System.Windows.Forms.Button();
            this.btnTable = new System.Windows.Forms.Button();
            this.btnVoucher = new System.Windows.Forms.Button();
            this.btnBill = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.flpTable1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelMenu.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.Gainsboro;
            this.panelMenu.Controls.Add(this.lblTitle);
            this.panelMenu.Controls.Add(this.btnDashboard);
            this.panelMenu.Controls.Add(this.btnAccount);
            this.panelMenu.Controls.Add(this.btnFood);
            this.panelMenu.Controls.Add(this.btnTable);
            this.panelMenu.Controls.Add(this.btnVoucher);
            this.panelMenu.Controls.Add(this.btnBill);
            this.panelMenu.Controls.Add(this.panelBottom);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(200, 600);
            this.panelMenu.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(40, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(97, 16);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ADMIN PANEL";
            // 
            // btnDashboard
            // 
            this.btnDashboard.Location = new System.Drawing.Point(40, 40);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(143, 23);
            this.btnDashboard.TabIndex = 1;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnAccount
            // 
            this.btnAccount.Location = new System.Drawing.Point(40, 80);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(143, 23);
            this.btnAccount.TabIndex = 2;
            this.btnAccount.Text = "Quản lý tài khoản";
            this.btnAccount.Click += new System.EventHandler(this.btnAccount_Click);
            // 
            // btnFood
            // 
            this.btnFood.Location = new System.Drawing.Point(40, 120);
            this.btnFood.Name = "btnFood";
            this.btnFood.Size = new System.Drawing.Size(143, 23);
            this.btnFood.TabIndex = 3;
            this.btnFood.Text = "Quản lý món";
            this.btnFood.Click += new System.EventHandler(this.btnFood_Click);
            // 
            // btnTable
            // 
            this.btnTable.Location = new System.Drawing.Point(40, 160);
            this.btnTable.Name = "btnTable";
            this.btnTable.Size = new System.Drawing.Size(143, 23);
            this.btnTable.TabIndex = 4;
            this.btnTable.Text = "Quản lý bàn";
            this.btnTable.Click += new System.EventHandler(this.btnTable_Click);
            // 
            // btnVoucher
            // 
            this.btnVoucher.Location = new System.Drawing.Point(40, 202);
            this.btnVoucher.Name = "btnVoucher";
            this.btnVoucher.Size = new System.Drawing.Size(143, 23);
            this.btnVoucher.TabIndex = 6;
            this.btnVoucher.Text = "Voucher";
            this.btnVoucher.Click += new System.EventHandler(this.btnVoucher_Click);
            // 
            // btnBill
            // 
            this.btnBill.Location = new System.Drawing.Point(43, 243);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(143, 23);
            this.btnBill.TabIndex = 7;
            this.btnBill.Text = "Hóa đơn";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnLogout);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 520);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(200, 80);
            this.panelBottom.TabIndex = 8;
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogout.Location = new System.Drawing.Point(0, 0);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 80);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Đăng xuất";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.flpTable1);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(200, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(800, 600);
            this.panelContent.TabIndex = 0;
            // 
            // flpTable1
            // 
            this.flpTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTable1.Location = new System.Drawing.Point(0, 0);
            this.flpTable1.Name = "flpTable1";
            this.flpTable1.Size = new System.Drawing.Size(800, 600);
            this.flpTable1.TabIndex = 0;
            // 
            // fMain
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Name = "fMain";
            this.Text = "Admin Panel";
            this.Load += new System.EventHandler(this.fMain_Load);
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnAccount;
        private System.Windows.Forms.Button btnFood;
        private System.Windows.Forms.Button btnTable;
        private System.Windows.Forms.Button btnVoucher;
        private System.Windows.Forms.Button btnBill;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel flpTable1;
    }
}