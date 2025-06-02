using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Drawing;
namespace appcaphe1
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=D:\appcaphe1\appcaphe1\users.db;Version=3;";


        public Form1()
        {
            InitializeComponent();
        }

        // Hàm kiểm tra login với username và password
        private bool Login(string username, string password)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE Username = @Username AND Password = @Password";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        private string GetRoleByUsername(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Account FROM users WHERE Username = @Username";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Account"].ToString();
                        }
                        return null;
                    }
                }
            }
        }
        // Sự kiện khi nhấn nút đăng nhập
        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        // Sự kiện cho nút đăng ký (Form2)
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        // Sự kiện khi nhấn vào link quên mật khẩu
     

        // Các sự kiện không liên quan đến đăng nhập (có thể bỏ qua)
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 main = new Form2();
            main.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormForgotPassword forgotForm = new FormForgotPassword();

            if (forgotForm.ShowDialog() == DialogResult.OK)
            {
                FormResetPassword resetForm = new FormResetPassword(forgotForm.Username);
                resetForm.ShowDialog();
            }
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
          
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Kiểm tra đăng nhập với username và password
            if (Login(username, password))
            {
                // Lấy role của người dùng từ Session (dựa trên username)
                string role = GetRoleByUsername(username); // lấy role từ database (cột Account)

                // Nếu có role và là Admin hoặc User, chuyển đến Mainform
                if (role == "Admin" || role == "User")
                {
                    // Lưu username vào session nếu cần
                    // ✅ Lưu vào biến toàn cục trong Program.cs
                    Program.LoggedInUsername = username;
                    Program.LoggedInRole = role;

                    // Chuyển đến Mainform, truyền username vào constructor nếu cần
                    Mainform main = new Mainform(username);
                    main.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Role không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Application.Exit();  // hoặc this.Close()
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("D:\\image.png");
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("D:\\sign-in.png");
        }
    }
}
