using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace appcaphe1
{
    public partial class FormResetPassword : Form
    {
        private string username;

        public FormResetPassword(string username)
        {
            InitializeComponent();
            this.username = username; // Lưu username
        }
        private void UpdatePassword(string username, string newPassword)
        {
            string dbPath = "D:\\appcaphe1\\appcaphe1\\users.db"; // Đảm bảo đường dẫn đúng

            if (!System.IO.File.Exists(dbPath))
            {
                MessageBox.Show("File database không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();

                string updateQuery = "UPDATE Users SET Password=@NewPassword WHERE Username=@Username";
                using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    cmd.Parameters.AddWithValue("@Username", username);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Mật khẩu đã được đặt lại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        conn.Close(); // Đóng kết nối trước khi mở Form1

                        Form1 main = new Form1();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản hoặc lỗi cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    

        

        private void textBoxConfirmPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNewPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormResetPassword_Load(object sender, EventArgs e)
        {

        }

        private void textBoxNewPassword_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text.Trim();
            string confirmPassword = textBoxConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {

                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            UpdatePassword(username, newPassword);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            FormForgotPassword main = new FormForgotPassword();
            main.Show();
            this.Close();
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
            FormForgotPassword main = new FormForgotPassword();
            main.Show();
            this.Close();

        }
    }
}
