using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using appcaphe1;


public class DatabaseHelper
{
    private static string dbPath = "database.db";  // Đường dẫn file database
    private static string connectionString = $"Data Source={dbPath};Version=3;";

    // Hàm tạo database và bảng nếu chưa có
    public static void InitializeDatabase()
    {
        if (!File.Exists(dbPath))
        {
            SQLiteConnection.CreateFile(dbPath);
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT UNIQUE,
                        Password TEXT
                    )";
                using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Database & Table Created!", "Thông báo");
        }
    }

    // Hàm đăng ký user
    public static void RegisterUser(string username, string password)
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Đăng ký thành công!", "Thông báo");
                 
                }
                catch (Exception)
                {
                    MessageBox.Show("Tên người dùng đã tồn tại!", "Lỗi");
                }
            }
        }
    }

    // Hàm hiển thị danh sách user
    public static void ShowUsers()
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM Users";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                string users = "Danh sách tài khoản:\n";
                while (reader.Read())
                {
                    users += $" ID | {reader["Id"]} | Username: {reader["Username"]}\n";
                }
                MessageBox.Show(users, "User List");
            }
        }
    }
}
