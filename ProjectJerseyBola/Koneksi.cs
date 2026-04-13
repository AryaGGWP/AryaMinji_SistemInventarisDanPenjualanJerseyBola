using System;
using System.Data.SqlClient; // Ini wajib buat manggil SQL Server

namespace ProjectJerseyBola // Pastikan namespace ini sama kayak nama project lu
{
    class Koneksi
    {
        // Method buat nyambungin ke database
        public SqlConnection GetConn()
        {
            // WARNING: Ganti tulisan di dalem kutip ini pakai Connection String lu sendiri!
            SqlConnection conn = new SqlConnection(@"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True");
            return conn;
        }
    }
}