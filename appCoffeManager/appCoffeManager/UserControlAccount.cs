using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace appcaphe1
{
    public partial class UserControlAccount: UserControl
    {
        public UserControlAccount()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\users.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM users";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
                dataGridView2.Columns["CreatedDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        private void UserControlAdmin_Load(object sender, EventArgs e)
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;
            if (role != "Admin")
            { 
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                LoadData();
            }
        }

       

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\users.db;Version=3;";
            string searchText = textBox1.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập ID hoặc Username để tìm kiếm!", "Thông báo");
                return;
            }

            bool found = false;

            // Bỏ chọn tất cả các hàng trước đó
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["ID"].Value != null && row.Cells["Username"].Value != null)
                {
                    string id = row.Cells["ID"].Value.ToString();
                    string name = row.Cells["Username"].Value.ToString().ToLower();

                    if (id == searchText || name.Contains(searchText))
                    {
                        row.Selected = true;
                        dataGridView2.FirstDisplayedScrollingRowIndex = row.Index;
                        found = true;
                        break; // Dừng lại khi tìm thấy dòng đầu tiên khớp
                    }
                }
            }

            if (!found)
            {
                MessageBox.Show("Không tìm thấy Account!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
