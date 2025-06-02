using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appcaphe1
{
    public partial class UserControlHome: UserControl
    {
        private string username; // Khai báo biến toàn cục
        private string picture; // Khai báo biến toàn cục
        public UserControlHome()
        {
            InitializeComponent();
        }
        private void LoadUserControl()
        {
            // Xóa UserControl cũ nếu có
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlAdmin userControl = new UserControlAdmin();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
        private void LoadUserControlMenu()
        {
            // Xóa UserControl cũ nếu có
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlMenu userControl = new UserControlMenu();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
        private void LoadUserControlBan()
        {
            // Xóa UserControl cũ nếu có
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlTable userControl = new UserControlTable();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
        private void LoadUserControlChart()
        {
            // Xóa UserControl cũ nếu có
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlChart userControl = new UserControlChart();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void UserControlHome_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;

            if ( role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
            }
            else
            {
                LoadUserControl();
            }



        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            LoadUserControlMenu();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            LoadUserControlBan();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            LoadUserControlChart();
        }

      
    }
}
