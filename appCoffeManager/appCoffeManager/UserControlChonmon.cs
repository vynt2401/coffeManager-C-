using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace appcaphe1
{
    public partial class UserControlChonmon : UserControl
    {
        private string tenBan;
        private string connectionStringMenu = "Data Source=D:\\appcaphe1\\appcaphe1\\menu.db;Version=3;";
        private string connectionStringBill = "Data Source=D:\\appcaphe1\\appcaphe1\\bill.db;Version=3;";

        public UserControlChonmon(string tenBan)
        {
            InitializeComponent();
            this.tenBan = tenBan;
        }
        public void SetBan(string tenBan)
        {
            label3.Text = tenBan;
        }
        private void UserControlChonmon_Load(object sender, EventArgs e)
        {
            LoadDataMenu();
            LoadDataBill();
            TinhTongDonGia();
            textBox2.Text = "1"; // Số lượng mặc định
            SetBan(tenBan);

            dataGridView2.MultiSelect = false; // Cho phép chọn một dòng
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Chọn nguyên dòng

            // Tạo cột checkbox "Chọn"
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "Chon";
            checkBoxColumn.HeaderText = "Chọn";
            checkBoxColumn.TrueValue = true;
            checkBoxColumn.FalseValue = false;
            checkBoxColumn.ValueType = typeof(bool);
            dataGridView2.Columns.Add(checkBoxColumn);

            // Đăng ký sự kiện CurrentCellDirtyStateChanged
            dataGridView2.CurrentCellDirtyStateChanged += new EventHandler(dataGridView2_CurrentCellDirtyStateChanged);
        }
        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu đang thay đổi ô checkbox "Chon"
            if (dataGridView2.IsCurrentCellDirty)
            {
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void LoadDataMenu()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionStringMenu))
            {
                conn.Open();
                string query = "SELECT * FROM drink";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView2.DataSource = dt;
            }
        }

        private void LoadDataBill()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionStringBill))
            {
                conn.Open();
                string query = "SELECT * FROM view";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void LuuVaoDatabase(string tenMon, int soLuong, decimal donGia)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionStringBill))
            {
                conn.Open();

                // Kiểm tra xem món đã có trong bill chưa
                string checkQuery = "SELECT So_luong FROM view WHERE Ten_mon = @Ten_mon";
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Ten_mon", tenMon);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null) // Món đã tồn tại, tăng số lượng
                    {
                        int currentQuantity = Convert.ToInt32(result);
                        int newQuantity = currentQuantity + soLuong;

                        string updateQuery = "UPDATE view SET So_luong = @So_luong WHERE Ten_mon = @Ten_mon";
                        using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@So_luong", newQuantity);
                            updateCmd.Parameters.AddWithValue("@Ten_mon", tenMon);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else // Món chưa có, thêm mới
                    {
                        string insertQuery = "INSERT INTO view (Ten_mon, So_luong, Don_gia) VALUES (@Ten_mon, @So_luong, @Don_gia)";
                        using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Ten_mon", tenMon);
                            insertCmd.Parameters.AddWithValue("@So_luong", soLuong);
                            insertCmd.Parameters.AddWithValue("@Don_gia", donGia);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void ResetDatabase()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionStringBill))
            {
                conn.Open();
                string query = "DELETE FROM view";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
        }

        private void TinhTongDonGia()
        {
            double tongDonGia = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["So_luong"].Value != null && row.Cells["Don_gia"].Value != null)
                {
                    double soLuong = Convert.ToDouble(row.Cells["So_luong"].Value);
                    double donGia = Convert.ToDouble(row.Cells["Don_gia"].Value);
                    tongDonGia += soLuong * donGia;
                }
            }
            LoadDataBill();
            textBox4.Text = tongDonGia.ToString("N0");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            TinhTongDonGia();
            UserData.SharedText = label3.Text; // Lưu dữ liệu vào biến static
            bool hasSelected = false;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Chon"].Value != null && (bool)row.Cells["Chon"].Value)
                {
                    hasSelected = true;

                    if (row.Cells["Ten_hang"].Value == null || row.Cells["Gia_ban"].Value == null)
                    {
                        MessageBox.Show("Lỗi: Không lấy được dữ liệu món ăn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    string tenMon = row.Cells["Ten_hang"].Value.ToString();
                    decimal donGia = Convert.ToDecimal(row.Cells["Gia_ban"].Value);

                    int soLuong;
                    if (!int.TryParse(textBox2.Text, out soLuong) || soLuong <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    LuuVaoDatabase(tenMon, soLuong, donGia);
                }
            }

            if (hasSelected)
            {
                TinhTongDonGia();
                LoadDataBill();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            TinhTongDonGia();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            UserControlCheckout userControl = new UserControlCheckout();
            userControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(userControl);
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Chon"].Index && e.RowIndex >= 0)
            {
                bool isChecked = (bool)dataGridView2.Rows[e.RowIndex].Cells["Chon"].Value;

                if (isChecked)
                {
                    // Món được chọn, có thể thực hiện thêm các hành động (hiển thị thông báo, tính tổng giá trị,...)
                }
                else
                {
                    // Món không được chọn
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ResetDatabase();
            LoadDataBill();
            textBox4.Text = "0";
        }

        

       
    }

    public static class UserData
    {
        public static string SharedText = " ";
    }
}
