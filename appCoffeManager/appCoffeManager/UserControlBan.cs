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
    public partial class UserControlTable: UserControl
    {
        public UserControlTable()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\soban.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM tab";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        

        private void UserControlBancs_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;

            if (role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dataGridView1.AllowUserToAddRows = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\tab.db;Version=3;";
            string searchText = textBox1.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập STT va tên bàn để tìm kiếm!", "Thông báo");
                return;
            }

            bool found = false;

            // Bỏ chọn tất cả các hàng trước đó
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["STT"].Value != null && row.Cells["Ten_ban"].Value != null)
                {
                    string id = row.Cells["STT"].Value.ToString();
                    string name = row.Cells["Ten_ban"].Value.ToString().ToLower();

                    if (id == searchText || name.Contains(searchText))
                    {
                        row.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;
            if (role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\soban.db;Version=3;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string queryDelete = "DELETE FROM tab";
                    SQLiteCommand cmdDelete = new SQLiteCommand(queryDelete, conn);
                    cmdDelete.ExecuteNonQuery();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string queryInsert = "INSERT INTO tab (STT, Ten_ban, Trang_thai) VALUES (@STT, @Ten_ban, @Trang_thai)";
                        using (SQLiteCommand cmd = new SQLiteCommand(queryInsert, conn))
                        {
                            cmd.Parameters.AddWithValue("@STT", row.Cells["STT"].Value);
                            cmd.Parameters.AddWithValue("@Ten_ban", row.Cells["Ten_ban"].Value);
                            cmd.Parameters.AddWithValue("@Trang_thai", row.Cells["Trang_thai"].Value);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được lưu!");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      
    }
}
