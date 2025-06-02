using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace appcaphe1
{
    public partial class FormForgotPassword : Form
    {
        public string Username { get; private set; }

       
        public FormForgotPassword()
        {
            InitializeComponent();
        }

        // Hàm kiểm tra username có tồn tại trong database không
        private bool CheckUsernameExists(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=D:\\appcaphe1\\appcaphe1\\users.db;Version=3;"))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@Username";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0; // Trả về true nếu username tồn tại
                }
            }
        }

        // Xử lý khi nhấn nút "Tiếp tục" để kiểm tra username
     

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
        }

        private void FormForgotPassword_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CheckUsernameExists(username))
            {
                // Nếu username tồn tại, chuyển sang form Reset Password
                FormResetPassword resetForm = new FormResetPassword(username);
                resetForm.Show();
                this.Close(); // Đóng form hiện tại
            }
            else
            {
                MessageBox.Show("Tên người dùng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form1 main = new Form1();
            main.Show();
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Form1 main = new Form1();
            main.Show();
            this.Close();
        }
    }
}
