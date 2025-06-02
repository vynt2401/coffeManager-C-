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
    public partial class UserControlDay: UserControl
    {
        public UserControlDay()
        {
            InitializeComponent();
            VeBieuDoTheoNgay(); 
        }

        private void VeBieuDoTheoNgay()
        {
            string connection = "Data Source=D:\\appcaphe1\\appcaphe1\\stats.db;Version=3;";
            Dictionary<string, double> doanhThuNgay = new Dictionary<string, double>();

            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
        SELECT strftime('%Y-%m-%d', Ngay) AS Ngay, SUM(Tong_tien) AS TongDoanhThu
        FROM thongke
        GROUP BY Ngay
        ORDER BY Ngay";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ngay = reader["Ngay"].ToString(); // yyyy-MM-dd
                        double tong = Convert.ToDouble(reader["TongDoanhThu"]);
                        doanhThuNgay[ngay] = tong;
                    }
                }
            }

            // Xóa dữ liệu cũ
            chart1.Series.Clear();
            chart1.Series.Add("Doanh Thu");

            chart1.Series["Doanh Thu"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // Thêm dữ liệu mới
            foreach (var item in doanhThuNgay)
            {
                chart1.Series["Doanh Thu"].Points.AddXY(item.Key, item.Value);
            }

            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Tổng Doanh Thu (VND)";
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Xoay nhãn ngày cho dễ đọc
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
        }

        private void UserControl_Load(object sender, EventArgs e)
        {
            VeBieuDoTheoNgay();
            string ngay = DateTime.Now.ToString("dd");
             label1.Text = ngay;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlChart userControl = new UserControlChart();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlChart userControl = new UserControlChart();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlChart userControl = new UserControlChart();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
    }
}
