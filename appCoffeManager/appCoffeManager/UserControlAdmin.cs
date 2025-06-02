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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace appcaphe1
{
    public partial class UserControlAdmin: UserControl
    {
        public UserControlAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = true;
        }

        private void LoadData()
        {
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\nhanvien.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Staff";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        //button save
       
        private void button3_Click_1(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();             // <-- Dòng này giúp cập nhật ô đang sửa
            dataGridView1.CurrentCell = null;    // <-- Buộc commit dữ liệu mới nhập

            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\nhanvien.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string queryDelete = "DELETE FROM Staff";
                SQLiteCommand cmdDelete = new SQLiteCommand(queryDelete, conn);
                cmdDelete.ExecuteNonQuery();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    string queryInsert = "INSERT INTO Staff (ID, Name, Address, Phone, Position) VALUES (@ID, @Name, @Address, @Phone, @Position)";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", row.Cells["ID"].Value);
                        cmd.Parameters.AddWithValue("@Name", row.Cells["Name"].Value);
                        cmd.Parameters.AddWithValue("@Address", row.Cells["Address"].Value);
                        cmd.Parameters.AddWithValue("@Phone", row.Cells["Phone"].Value?.ToString() ?? ""); // an toàn hơn
                        cmd.Parameters.AddWithValue("@Position", row.Cells["Position"].Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Dữ liệu đã được lưu!");
            }
        }

        private void ResetID(SQLiteConnection conn)
        {
            // Lấy danh sách ID cũ theo thứ tự tăng dần
            string queryGetIDs = "SELECT ID FROM Staff ORDER BY ID";
            List<int> oldIDs = new List<int>();

            using (SQLiteCommand cmd = new SQLiteCommand(queryGetIDs, conn))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    oldIDs.Add(reader.GetInt32(0)); // Lưu danh sách ID hiện tại
                }
            }

            // Cập nhật lại ID mới từ 1, 2, 3,...
            for (int i = 0; i < oldIDs.Count; i++)
            {
                string queryUpdate = "UPDATE Staff SET ID = @NewID WHERE ID = @OldID";
                using (SQLiteCommand cmd = new SQLiteCommand(queryUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@NewID", i + 1);
                    cmd.Parameters.AddWithValue("@OldID", oldIDs[i]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        //button xoa
        private void button2_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\nhanvien.db;Version=3;";

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridView1.SelectedRows[0].Index;
                int idToDelete = Convert.ToInt32(dataGridView1.Rows[selectedIndex].Cells["ID"].Value);

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1️⃣ Xóa nhân viên theo ID
                            string query = "DELETE FROM Staff WHERE ID = @ID";
                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@ID", idToDelete);
                                cmd.ExecuteNonQuery();
                            }

                            // 2️⃣ Đặt lại ID sau khi xóa
                            ResetID(conn);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                        }
                    }
                }

                LoadData(); // 3️⃣ Cập nhật lại DataGridView
            }
        }
        
        //button tim kiem
     

      
        private void UserControl1_Load(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim().ToLower();
            string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\nhanvien.db;Version=3;";


            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập ID hoặc Username để tìm kiếm!", "Thông báo");
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
                if (row.Cells["ID"].Value != null && row.Cells["Name"].Value != null)
                {
                    string id = row.Cells["ID"].Value.ToString();
                    string name = row.Cells["Name"].Value.ToString().ToLower();

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
                MessageBox.Show("Không tìm thấy nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       

        private void button1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = true;
        }
    }
}
