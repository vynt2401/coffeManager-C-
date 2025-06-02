using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace appcaphe1
{
   
    public partial class Mainform: Form
    {
        
        private string currentUsername;


        public event EventHandler CloseRequested;
        public Mainform(string username)
        {
            InitializeComponent();
            ShowUserControlOrder(); // Hiển thị UserControlOrder khi mở Mainform
            currentUsername = username;


        }
        private UserControlOrder userControlOrder;
        private UserControlCheckout userControlCheckout;
        public void ShowUserControlOrder()
        {
            panel2.Controls.Clear();
            UserControlOrder orderControl = new UserControlOrder();
            panel2.Controls.Add(orderControl);
            orderControl.Dock = DockStyle.Fill;
        }
        private string LayUsernameTuDatabase()
        {
            string username = "";
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\users.db;Version=3;";

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Username FROM Users LIMIT 1"; // Lấy username đầu tiên

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            username = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return username;
        }
        
        private void Mainform_Load(object sender, EventArgs e)
        {
            LoadUserControlHome(); // load usercontrolhome dau tien
          
            pictureBox3.Image = Image.FromFile("D:\\yhome.png");
        }

        private void button1_Click(object sender, EventArgs e)
        {
           //trong
        }

        //button home
        private void button5_Click(object sender, EventArgs e)
        {
            
        }

      
       
        private void LoadUserControlHome()
        {
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlHome userControl = new UserControlHome();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }
        private void LoadUserControlAdmin1()
        {
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlAdmin userControl = new UserControlAdmin();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }
        
        private void LoadUserControlAdmin()
        {
            // Xóa UserControl cũ nếu có
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlAccount userControl = new UserControlAccount();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }
        private void LoadUserControlStats()
        {
            // Xóa UserControl cũ nếu có
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlStats   userControl = new UserControlStats();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }

        private void LoadUserControlOrder()
        {
            // Xóa UserControl cũ nếu có
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlOrder userControl = new UserControlOrder();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }
        private void LoadUserControlCheckout()
        {
            // Xóa UserControl cũ nếu có
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlCheckout userControl = new UserControlCheckout();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }


        private void LoadUserControlHelp()
        {
            // Xóa UserControl cũ nếu có
            panel2.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlHelp userControl = new UserControlHelp();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel2.Controls.Add(userControl);
        }
      
      
         
       
        private void button8_Click(object sender, EventArgs e)
        {

           
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

     

      

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
         
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            FormInformation main = new FormInformation(currentUsername);
            main.Show();
            
        }

      

        private void reset()
        {
            pictureBox3.Image = Properties.Resources.home;
            button6.Image = Properties.Resources.admin1;
            button7.Image = Properties.Resources.stats;
            button8.Image = Properties.Resources.order1;
            button9.Image = Properties.Resources.checout1;
            button10.Image = Properties.Resources.help1;
        }
        private void pictureBox3_Click_4(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.home2;
            LoadUserControlHome();
            reset();
            pictureBox3.Image = Properties.Resources.yhome;

        }



        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.admin;
            LoadUserControlAdmin();
            reset();
            button6.Image = Properties.Resources.yadmin;

        }

   

        private void pictureBox3_Click_2(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.statistics;
            LoadUserControlStats();
            reset();

            button7.Image = Properties.Resources.ystats;

        }

    
        

        private void button8_Click_2(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.order;
            LoadUserControlOrder();
            reset();

            button8.Image = Properties.Resources.yorder;

        }

     

        private void button9_Click_2(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.checkout;
            reset();
            button9.Image = Properties.Resources.ycheckout;
            LoadUserControlCheckout();


        }

       

        private void pictureBox3_Click_3(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.help;
            LoadUserControlHelp();
            reset();

            button10.Image = Properties.Resources.yhelp;

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có chắc không? ", " ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // Hành động khi người dùng nhấn OK
                Form1 main = new Form1();
                main.Show();
                this.Hide();
            }
        }

        private void bunifuGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

       
    }
}
