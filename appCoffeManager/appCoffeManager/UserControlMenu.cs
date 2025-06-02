using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace appcaphe1
{
    public partial class UserControlMenu : UserControl
    {
        private string connectionString = "Data Source=D:\\appcaphe1\\appcaphe1\\menu.db;Version=3;";

        public UserControlMenu()
        {
            InitializeComponent();
        }

        private void UserControlMenu_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM drink";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void ResetID(SQLiteConnection conn)
        {
            string queryGetIDs = "SELECT STT FROM drink ORDER BY STT";
            List<int> oldSTTs = new List<int>();

            using (SQLiteCommand cmd = new SQLiteCommand(queryGetIDs, conn))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    oldSTTs.Add(reader.GetInt32(0));
                }
            }

            for (int i = 0; i < oldSTTs.Count; i++)
            {
                string queryUpdate = "UPDATE drink SET STT = @NewSTT WHERE STT = @OldSTT";
                using (SQLiteCommand cmd = new SQLiteCommand(queryUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@NewSTT", i + 1);
                    cmd.Parameters.AddWithValue("@OldSTT", oldSTTs[i]);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // Lưu dữ liệu
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;
            if (role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dataGridView1.EndEdit();
                dataGridView1.CurrentCell = null; // Mẹo nhỏ để force DataGridView cập nhật giá trị ô cuối cùng
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string queryDelete = "DELETE FROM drink";
                    SQLiteCommand cmdDelete = new SQLiteCommand(queryDelete, conn);
                    cmdDelete.ExecuteNonQuery();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string queryInsert = "INSERT INTO drink (STT, Ma_hang, Ten_hang, Gia_ban) VALUES (@STT, @Ma_hang, @Ten_hang, @Gia_ban)";
                        using (SQLiteCommand cmd = new SQLiteCommand(queryInsert, conn))
                        {
                            cmd.Parameters.AddWithValue("@STT", row.Cells["STT"].Value);
                            cmd.Parameters.AddWithValue("@Ma_hang", row.Cells["Ma_hang"].Value);
                            cmd.Parameters.AddWithValue("@Ten_hang", row.Cells["Ten_hang"].Value);
                            cmd.Parameters.AddWithValue("@Gia_ban", row.Cells["Gia_ban"].Value);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được lưu!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) // Xóa món đã chọn
        {
            string username = Program.LoggedInUsername;
            string role = Program.LoggedInRole;

            if (role != "Admin")
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                        {
                            if (selectedRow.Cells["STT"].Value == null) continue;

                            int id = Convert.ToInt32(selectedRow.Cells["STT"].Value);

                            string query = "DELETE FROM drink WHERE STT = @STT";
                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@STT", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Reset ID (nếu cần)
                        ResetID(conn);

                        transaction.Commit();
                        LoadData();
                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e) // Tìm kiếm món
        {

            string searchText = textBox1.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập STT hoặc mã hàng để tìm kiếm!", "Thông báo");
                return;
            }

            bool found = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = false;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["STT"].Value != null && row.Cells["Ma_hang"].Value != null && row.Cells["Ten_hang"].Value != null)
                {
                    string stt = row.Cells["STT"].Value.ToString();
                    string ma = row.Cells["Ma_hang"].Value.ToString().ToLower();
                    string ten = row.Cells["Ten_hang"].Value.ToString();

                    if (stt == searchText || ma == searchText || ten.ToLower().Contains(searchText))
                    {
                        row.Selected = true;
                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                MessageBox.Show("Không tìm thấy đồ uống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem cột được click có phải là cột checkbox không
            if (e.ColumnIndex == dataGridView1.Columns["Chon"].Index && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells["Chon"];
                chk.Value = !(chk.Value == null ? false : (bool)chk.Value);
            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
