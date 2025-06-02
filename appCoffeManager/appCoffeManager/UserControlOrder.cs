using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace appcaphe1
{
  
    public partial class UserControlOrder : UserControl
    {

        private string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\soban.db;Version=3;";
        private string banDangChon = ""; // Lưu bàn đang chọn

        public UserControlOrder()
        {
            InitializeComponent();
        }

        private void UserControlOrder_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.AutoScroll = true;
            LoadBanTuDatabase(); // Gọi hàm tạo bàn từ DB
        }

        private void LoadBanTuDatabase()
        {
            flowLayoutPanel1.Controls.Clear(); // Xóa bàn cũ nếu có

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Ten_ban, Trang_Thai FROM tab";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tenBan = reader["Ten_ban"].ToString();
                            string trangThai = reader["Trang_Thai"].ToString();

                            Button btnBan = new Button();
                            btnBan.Text = $"{tenBan}\n({trangThai})";
                            btnBan.Width = 100;
                            btnBan.Height = 60;
                            btnBan.Font = new Font("Arial", 9, FontStyle.Bold);
                            btnBan.Margin = new Padding(10);
                            btnBan.Tag = tenBan; // Lưu tên bàn vào Tag

                            // Đổi màu theo trạng thái
                            if (trangThai == "Có khách")
                            {
                                btnBan.BackColor = Color.Red;
                                btnBan.ForeColor = Color.White;
                            }
                            else
                            {
                                btnBan.BackColor = Color.White;
                                btnBan.ForeColor = Color.Black;
                            }

                            btnBan.Click += BtnBan_Click;
                            flowLayoutPanel1.Controls.Add(btnBan);
                        }
                    }
                }
            }
        }

        // Khi click vào bàn, chuyển sang "Có khách" nếu đang "Trống"
        private void BtnBan_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string tenBan = btn.Tag.ToString();
                banDangChon = tenBan; // Lưu tên bàn được chọn

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Lấy trạng thái hiện tại của bàn
                    string queryCheck = "SELECT Trang_Thai FROM tab WHERE Ten_ban = @Ten_ban";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryCheck, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ten_ban", tenBan);
                        string trangThai = cmd.ExecuteScalar()?.ToString();

                        string newTrangThai = (trangThai == "Trống") ? "Có khách" : "Trống";

                        // Cập nhật trạng thái bàn trong database
                        string queryUpdate = "UPDATE tab SET Trang_Thai = @NewTrangThai WHERE Ten_ban = @Ten_ban";
                        using (SQLiteCommand cmdUpdate = new SQLiteCommand(queryUpdate, conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@NewTrangThai", newTrangThai);
                            cmdUpdate.Parameters.AddWithValue("@Ten_ban", tenBan);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // Cập nhật ngay trên giao diện
                        btn.Text = $"{tenBan}\n({newTrangThai})";
                        btn.BackColor = (newTrangThai == "Có khách") ? Color.Red : Color.White;
                        btn.ForeColor = (newTrangThai == "Có khách") ? Color.White : Color.Black;
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(banDangChon))
            {
                UserControlChonmon ucChonMon = new UserControlChonmon(banDangChon);

                ucChonMon.SetBan(banDangChon); // Truyền tên bàn qua UserControl

                panel1.Controls.Clear();
                panel1.Controls.Add(ucChonMon);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Xử lý khi nhấn nút "Đặt món"

        // Sự kiện giữ nguyên theo yêu cầu



    }
}
