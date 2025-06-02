using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;

namespace appcaphe1
{
    public partial class UserControlStats : UserControl
    {
        private string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\stats.db;Version=3;";
        public UserControlStats()
        {
            InitializeComponent();
        }
        private DateTime currentDate = DateTime.Today;
        private void UserControlStats_Load(object sender, EventArgs e)
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;
            if (role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
           

                LoadDataMenu();
                dateTimePicker1.Value = DateTime.Today; // Đặt mặc định là hôm nay
                LoadThongKeTheoNgay(DateTime.Today);    // Load dữ liệu hôm nay

                currentDate = DateTime.Today;
                dateTimePicker1.Value = currentDate;
                LoadThongKeTheoNgay(currentDate);
            }
        }
        private void LoadDataMenu()
        {
         
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM thongke";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void LoadThongKeTheoNgay(DateTime selectedDate)
        {
            string ngayChon = selectedDate.ToString("yyyy-MM-dd");

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Ten_ban, Tong_tien, Giam_gia, Ngay FROM thongke WHERE Ngay = @ngay";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ngay", ngayChon);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }
      
        

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            currentDate = dateTimePicker1.Value;
            LoadThongKeTheoNgay(currentDate);
            LoadThongKeTheoNgay(dateTimePicker1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(1);
            dateTimePicker1.Value = currentDate;
            LoadThongKeTheoNgay(currentDate);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(-1);
            dateTimePicker1.Value = currentDate;
            LoadThongKeTheoNgay(currentDate);
        }
        private void label2_Click(object sender, EventArgs e)
        {

            currentDate = currentDate.AddDays(-1);
            dateTimePicker1.Value = currentDate;
            LoadThongKeTheoNgay(currentDate);
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        
            // Xóa UserControl cũ nếu có
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlChart userControl = new UserControlChart();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        
    }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

            currentDate = currentDate.AddDays(1);
            dateTimePicker1.Value = currentDate;
            LoadThongKeTheoNgay(currentDate);
        }
    }
    }

      


