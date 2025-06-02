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
using System.Drawing.Printing;

namespace appcaphe1
{

    public partial class UserControlCheckout: UserControl
    {
        private string tenBan;
        private string connectionStringBill = "Data Source=D:\\appcaphe1\\appcaphe1\\bill.db;Version=3;";
        public UserControlCheckout()
        {
            InitializeComponent();
          

        }
       
        private void label1_Click(object sender, EventArgs e)
        {

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
                dataGridView3.DataSource = dt;
            }
            panel2.Invalidate(); // Gọi lại sự kiện vẽ
        }
        private void TinhTongDonGia()
        {
            double tongDonGia = 0;
            double vorcher = 0;
           
            int giamgia;

            string selectedText = domainUpDown1.Text.Replace("%", ""); // Xóa dấu "%"
           
           


            // Duyệt qua tất cả các dòng trong DataGridView
            foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells["So_luong"].Value != null && row.Cells["Don_gia"].Value != null)
                    {
                        // Lấy giá trị Số lượng và Đơn giá từ các cột tương ứng
                        double soLuong = Convert.ToDouble(row.Cells["So_luong"].Value);
                        double donGia = Convert.ToDouble(row.Cells["Don_gia"].Value);

                        // Cộng dồn vào tổng
                        tongDonGia += soLuong * donGia;
                       
                    }
                }
            LoadDataBill();
            // Hiển thị tổng vào textBox1
            if (int.TryParse(selectedText, out giamgia))        
                vorcher = tongDonGia * 0.01 * giamgia;
                tongDonGia = tongDonGia - vorcher;
                textBox1.Text = tongDonGia.ToString("N0"); // Hiển thị dưới dạng số, có phân cách nghìn
            
        }
       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

     

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
       

        private void UserControlCheckout_Load_1(object sender, EventArgs e)
        {   
            LoadDataBill();
            TinhTongDonGia();
            string ten = UserData.SharedText;
            label1.Text = ten;
         
           
            dataGridView3.Columns["Ten_mon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView3.Columns["Don_gia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
           dataGridView3.Columns["So_luong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
           
          
        }

      

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
               
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
           
        }
       
       
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Arial", 14, FontStyle.Bold);
            Font headerFont = new Font("Arial", 10, FontStyle.Bold);
            Font itemFont = new Font("Arial", 10);
            Brush brush = Brushes.Black;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float spacing = 25;
            double tongTien = 0;

            // Tiêu đề
            e.Graphics.DrawString("HÓA ĐƠN THANH TOÁN", titleFont, brush, x + 100f, y);
            y += 40;

            // Header bảng
            e.Graphics.DrawString("Tên Món", headerFont, brush, x, y);
            e.Graphics.DrawString("SL", headerFont, brush, x + 120f, y);
            e.Graphics.DrawString("Đ.Giá", headerFont, brush, x + 170f, y);
            e.Graphics.DrawString("T.Tiền", headerFont, brush, x + 250f, y);
            y += spacing;

            // Lặp qua các dòng trong DataGridView
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["Ten_mon"].Value != null)
                {
                    string tenMon = row.Cells["Ten_mon"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["So_luong"].Value);
                    double donGia = Convert.ToDouble(row.Cells["Don_gia"].Value);
                    double thanhTien = soLuong * donGia;

                    e.Graphics.DrawString(tenMon, itemFont, brush, x, y);
                    e.Graphics.DrawString(soLuong.ToString(), itemFont, brush, x + 120f, y);
                    e.Graphics.DrawString(donGia.ToString("N0"), itemFont, brush, x + 170f, y);
                    e.Graphics.DrawString(thanhTien.ToString("N0"), itemFont, brush, x + 250f, y);
                    y += spacing;

                    tongTien += thanhTien;
                }
            }

            y += spacing / 2;

            // Giảm giá
            int giamGia = 0;
            if (!int.TryParse(domainUpDown1.Text.Replace("%", "").Trim(), out giamGia))
            {
                giamGia = 0;
            }

            double tienGiam = tongTien * giamGia / 100.0;
            double tongSauGiam = tongTien - tienGiam;

            e.Graphics.DrawString("Giảm Giá:", headerFont, brush, x, y);
            e.Graphics.DrawString(giamGia + "%", itemFont, brush, x + 100f, y);
            y += spacing;

            // Tổng cộng
            e.Graphics.DrawString("Tổng cộng:", headerFont, brush, x, y);
            e.Graphics.DrawString(tongSauGiam.ToString("N0") + " VND", itemFont, brush, x + 100f, y);
            y += spacing * 2;

            // Cảm ơn
            e.Graphics.DrawString("Cảm ơn quý khách!", titleFont, brush, x + 120f, y);
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 8); // Giảm kích thước chữ
            Font fontBold = new Font("Arial", 10, FontStyle.Bold);
            Brush brush = Brushes.Black;

            float x = 5;  // Dịch qua trái thêm
            float y = 5;  // Giảm lề trên
            int spacing = 18; // Khoảng cách giữa các dòng (nhỏ hơn)

            // Tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN THANH TOÁN", fontBold, brush, x + 50, y);
            y += spacing * 2;

            // In thông tin bàn
           
            y += spacing;

            // In tiêu đề cột
            g.DrawString("Tên Món", fontBold, brush, x, y);
            g.DrawString("SL", fontBold, brush, x + 80, y);      // Dịch trái
            g.DrawString("Đ.Giá", fontBold, brush, x + 130, y);
            g.DrawString("T.Tiền", fontBold, brush, x + 180, y);
          
            y += spacing;

            double tongTien = 0;

            // Duyệt qua DataGridView để in món ăn
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells["Ten_mon"].Value != null)
                {
                    string tenMon = row.Cells["Ten_mon"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["So_luong"].Value);
                    double donGia = Convert.ToDouble(row.Cells["Don_gia"].Value);
                    double thanhTien = soLuong * donGia;


                    g.DrawString(tenMon, font, brush, x, y);
                    g.DrawString(soLuong.ToString(), font, brush, x + 90, y);
                    g.DrawString(donGia.ToString("N0"), font, brush, x + 130, y);
                    g.DrawString(thanhTien.ToString("N0"), font, brush, x + 175, y);
                    y += spacing;

                    tongTien += thanhTien;
                   
                }
            }
            int giamGia = 0;
            if (!int.TryParse(domainUpDown1.Text.Replace("%", "").Trim(), out giamGia))
            {
                giamGia = 0;  // Mặc định không giảm giá nếu lỗi
            }

            // Tính tiền sau khi giảm giá
            double tienGiam = tongTien * giamGia / 100.0;
            double tongSauGiam = tongTien - tienGiam;
            // Hiển thị tổng tiền
            g.DrawString("Giảm Giá:", fontBold, brush, x, y);
            g.DrawString(giamGia + "%", fontBold, brush, x + 80, y);

            y += spacing;
           
            g.DrawString("Tổng cộng:", fontBold, brush, x + 80, y);
            g.DrawString(tongSauGiam.ToString("N0") + " VND", fontBold, brush, x + 155, y);
            y += spacing * 2;

            // Lời cảm ơn
            g.DrawString("Cảm ơn quý khách!", fontBold, brush, x + 50, y);
        

        }
      

        private void alphaBlendTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string connectionStringTong = "Data Source=D:\\appcaphe1\\appcaphe1\\tongbill.db;Version=3;";
            string connectionStringStats = "Data Source=D:\\appcaphe1\\appcaphe1\\stats.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionStringTong))
            {
                conn.Open();

                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells["Ten_mon"].Value != null)
                    {
                        string tenBan = UserData.SharedText; // Lấy tên bàn
                        string tenMon = row.Cells["Ten_mon"].Value.ToString();
                        int soLuong = Convert.ToInt32(row.Cells["So_luong"].Value);
                        int donGia = Convert.ToInt32(row.Cells["Don_gia"].Value);

                        string query = "INSERT INTO tong(Ten_ban, Ten_mon, So_luong, Don_Gia) VALUES (@tenBan, @tenMon, @soLuong, @donGia)";

                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@tenBan", tenBan);
                            cmd.Parameters.AddWithValue("@tenMon", tenMon);
                            cmd.Parameters.AddWithValue("@soLuong", soLuong);
                            cmd.Parameters.AddWithValue("@donGia", donGia);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // 🟢 Lưu vào bảng thongke trong stats.db
            using (SQLiteConnection connStats = new SQLiteConnection(connectionStringStats))
            {
                connStats.Open();

                string tenBan = UserData.SharedText;
                double tongTienGoc = 0;
                double tongSauGiam = 0;
                int giamGia = 0;

                // 🔹 Lấy giá trị giảm giá từ domainUpDown1 (chắc chắn lấy đúng)
                if (!int.TryParse(domainUpDown1.Text.Replace("%", "").Trim(), out giamGia))
                {
                    giamGia = 0; // Nếu không đọc được thì mặc định là 0%
                }

                // 🔹 Tính tổng tiền gốc từ DataGridView
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells["So_luong"].Value != null && row.Cells["Don_gia"].Value != null)
                    {
                        double soLuong = Convert.ToDouble(row.Cells["So_luong"].Value);
                        double donGia = Convert.ToDouble(row.Cells["Don_gia"].Value);
                        tongTienGoc += soLuong * donGia;
                    }
                }

                // 🔹 Tính tiền giảm giá
                double tienGiam = tongTienGoc * giamGia / 100.0;
                tongSauGiam = tongTienGoc - tienGiam;  // 💰 Đúng tổng tiền sau giảm giá

                // 🔹 Kiểm tra lại giá trị
                MessageBox.Show($"Tổng tiền gốc: {tongTienGoc}\nGiảm giá: {giamGia}%\nTổng sau giảm: {tongSauGiam}");

                string ngay = DateTime.Now.ToString("yyyy-MM-dd");

                string insertStatsQuery = "INSERT INTO thongke (Ten_ban, Tong_tien, Ngay, Giam_gia) VALUES (@tenBan, @tongSauGiam, @ngay, @giamGia)";

                using (SQLiteCommand cmdStats = new SQLiteCommand(insertStatsQuery, connStats))
                {
                    cmdStats.Parameters.AddWithValue("@tenBan", tenBan);
                    cmdStats.Parameters.AddWithValue("@tongSauGiam", tongSauGiam); // Đảm bảo lưu đúng giá trị
                    cmdStats.Parameters.AddWithValue("@ngay", ngay);
                    cmdStats.Parameters.AddWithValue("@giamGia", giamGia);
                    cmdStats.ExecuteNonQuery();
                }
            }


            TinhTongDonGia();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog(); // Xem trước in
                                        // printDocument.Print(); // Nếu muốn in luôn không cần xem trước
        }
    }
}
