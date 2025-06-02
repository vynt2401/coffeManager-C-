using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace appcaphe1
{
    public partial class FormInformation : Form
    {
        private string connectionString = @"Data Source=D:\appcaphe1\appcaphe1\users.db;Version=3;";
        //Ham tao database
        private string loggedInUser; // Lưu username đã đăng nhập
      

        public FormInformation(string username)
        {
            InitializeComponent();
            loggedInUser = username; // Nhận username từ Form1
           

        }
        private void label1_Click(object sender, EventArgs e)
        {
           
        }
        private void FormInformation_Load(object sender, EventArgs e)
        {
            LoadUserInfo(); // Gọi hàm lấy dữ liệu từ database
        }

        private void LoadUserInfo()
        {
            string connectionString = @"Data Source=D:\appcaphe1\appcaphe1\users.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Username, Password, Account, CreatedDate FROM users WHERE Username = @Username";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", loggedInUser);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox4.Text = reader["Username"].ToString();
                            textBox1.Text = reader["Password"].ToString(); // Đúng cột Password
                            textBox3.Text = reader["Account"].ToString();
                            textBox2.Text = reader["CreatedDate"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {
            Mainform main = new Mainform(loggedInUser); // ✅ truyền username đúng
            main.Show();
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Mainform main = new Mainform(loggedInUser); // ✅ truyền username đúng
            main.Show();
            this.Close();
        }
    }
}
