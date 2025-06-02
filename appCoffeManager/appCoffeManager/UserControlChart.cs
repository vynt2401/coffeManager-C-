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
    public partial class UserControlChart: UserControl
    {
        public UserControlChart()
        {
            InitializeComponent();
            VeBieuDoTheoThang();
        }
        private void UserControlChart_Load(object sender, EventArgs e)
        {
            VeBieuDoTheoThang(); // <-- Thêm dòng này
            string month = DateTime.Now.ToString("MM");
            label3.Text = month;
            string monthe = DateTime.Now.ToString("MMMM");
            label1.Text = monthe;
        }
        private void VeBieuDoTheoThang()
        {
            string connection = "Data Source=D:\\appcaphe1\\appcaphe1\\stats.db;Version=3;";
            Dictionary<string, double> doanhThuThang = new Dictionary<string, double>();

            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
    SELECT strftime('%Y-%m', Ngay) AS Thang, SUM(Tong_tien) AS TongDoanhThu
    FROM thongke
    GROUP BY Thang
    ORDER BY Thang";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string thang = reader["Thang"].ToString(); // yyyy-MM
                        double tong = Convert.ToDouble(reader["TongDoanhThu"]);
                        doanhThuThang[thang] = tong;
                    }
                }
            }

            // Xóa dữ liệu cũ
            chart1.Series.Clear();
            chart1.Series.Add("Doanh Thu");

            chart1.Series["Doanh Thu"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // Thêm dữ liệu mới
            foreach (var item in doanhThuThang)
            {
                chart1.Series["Doanh Thu"].Points.AddXY(item.Key, item.Value);
            }

            chart1.ChartAreas[0].AxisX.Title = "Tháng";
            chart1.ChartAreas[0].AxisY.Title = "Tổng Doanh Thu (VND)";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlDay userControl = new UserControlDay();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }

        private void label1_Click(object sender, EventArgs e)
        {

            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlDay userControl = new UserControlDay();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }

        private void label3_Click(object sender, EventArgs e)
        {

            panel1.Controls.Clear();

            // Khởi tạo UserControl mới
            UserControlDay userControl = new UserControlDay();
            userControl.Dock = DockStyle.Fill; // Để UserControl vừa với Panel

            // Thêm UserControl vào Panel
            panel1.Controls.Add(userControl);
        }
    }
}
