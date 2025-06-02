using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using System.Linq; // Thêm thư viện này để dùng .Last()
using System.Drawing;
namespace appcaphe1
{
    public partial class Form2 : Form
    {
        private static string dbPath = @"D:\appcaphe1\appcaphe1\users.db";
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public Form2()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase(); // Tạo database khi mở Form
        }

      



       

        private void bunifuGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

     

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string username = alphaBlendTextBox1.Text.Trim();
            string password = txtPassword.Text.Trim();
            string createdDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
                return;
            }

            // Mặc định là User
            string accountType = "User";
            if (bunifuCheckbox1.Checked)
            {
                accountType = "Admin";
            }

            // Kiểm tra nếu có chọn Admin


            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO users (Username, Password, Account, CreatedDate) VALUES (@Username, @Password, @Account, @CreatedDate)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Account", accountType);
                    cmd.Parameters.AddWithValue("@CreatedDate", createdDate);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đăng ký thành công!", "Thông báo");
                        Form1 main = new Form1();
                        main.Show();
                        this.Close();
                    }
                    catch (SQLiteException ex) when (ex.ErrorCode == (int)SQLiteErrorCode.Constraint)
                    {
                        MessageBox.Show("Tên người dùng đã tồn tại!", "Lỗi");
                    }
                }
            }
        }

        private void bunifuCheckbox1_OnChange(object sender, EventArgs e)
        {
            
        }

     

        private void label6_Click(object sender, EventArgs e)
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

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
           
            pictureBox4.Image = Image.FromFile("D:\\signup-dark.png");
        }
        

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile("D:\\sign-up.png"); 
        }

        private void alphaBlendTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
