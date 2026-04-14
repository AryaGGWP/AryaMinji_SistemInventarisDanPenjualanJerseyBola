using System;
using System.Windows.Forms;
using System.Data.SqlClient; // Wajib masuk

namespace ProjectJerseyBola
{
    public partial class FormTransaksi : Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormTransaksi()
        {
            InitializeComponent();
            IsiComboBoxJersey();
            txtKode.ReadOnly = true;
            txtNama.ReadOnly = true;
            txtHarga.ReadOnly = true;
            txtStok.ReadOnly = true;
        }

        // --- METHOD BANTUAN ---
        void IsiComboBoxJersey()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT KodeJersey, NamaJersey FROM Jersey";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                cbPilihJersey.Items.Clear();
                cbPilihJersey.Items.Add("-- Pilih Jersey --");

                while (dr.Read())
                {
                    cbPilihJersey.Items.Add(dr["KodeJersey"].ToString() + " - " + dr["NamaJersey"].ToString());
                }
                cbPilihJersey.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal tarik data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void BersihkanTransaksi()
        {
            cbPilihJersey.SelectedIndex = 0;
            txtJumlah.Clear();
            txtTotal.Clear();
        }

        // --- EVENT KETIKA JERSEY DIPILIH ---
        private void cbPilihJersey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPilihJersey.SelectedIndex == 0)
            {
                txtKode.Clear();
                txtNama.Clear();
                txtHarga.Clear();
                txtStok.Clear();
                return;
            }

            string[] pisah = cbPilihJersey.Text.Split('-');
            string kodeDipilih = pisah[0].Trim();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT * FROM Jersey WHERE KodeJersey = @kode";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode", kodeDipilih);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtKode.Text = dr["KodeJersey"].ToString();
                    txtNama.Text = dr["NamaJersey"].ToString();
                    txtHarga.Text = dr["Harga"].ToString();
                    txtStok.Text = dr["Stok"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error nampilin detail: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // --- TOMBOL HITUNG HARGA ---
        private void btnHitung_Click(object sender, EventArgs e)
        {
            if (txtKode.Text == "" || txtJumlah.Text == "")
            {
                MessageBox.Show("Pilih jersey dan masukin jumlah belinya dulu cuy!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int stokTersedia = int.Parse(txtStok.Text);
            int jumlahBeli = int.Parse(txtJumlah.Text);
            int hargaSatuan = int.Parse(txtHarga.Text);

            // Validasi Stok Anti-Mines
            if (jumlahBeli <= 0)
            {
                MessageBox.Show("Jumlah beli gak boleh 0 atau minus bos!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (jumlahBeli > stokTersedia)
            {
                MessageBox.Show("Waduh, stok Jersey kurang/tidak tersedia! Sisa stok cuma: " + stokTersedia, "Stok Kurang", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hitung Total
            txtTotal.Text = (hargaSatuan * jumlahBeli).ToString();
        }

        // --- TOMBOL SIMPAN TRANSAKSI ---
        private void btnSimpanJual_Click(object sender, EventArgs e)
        {
            if (txtKode.Text == "" || txtJumlah.Text == "")
            {
                MessageBox.Show("Hitung dulu transaksinya cuy sebelum disave!", "Peringatan");
                return;
            }

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "UPDATE Jersey SET Stok = Stok - @jumlah WHERE KodeJersey = @kode";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@jumlah", int.Parse(txtJumlah.Text));
                cmd.Parameters.AddWithValue("@kode", txtKode.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Transaksi Berhasil! Stok Jersey otomatis berkurang.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                BersihkanTransaksi();
                IsiComboBoxJersey();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal simpan transaksi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // --- TOMBOL KEMBALI ---
        private void btnKembali_Click(object sender, EventArgs e)
        {
            FormMenuUtama menu = new FormMenuUtama();
            menu.Show();
            this.Close();
        }

        private void txtKode_TextChanged(object sender, EventArgs e) { }
        private void txtNama_TextChanged(object sender, EventArgs e) { }
        private void txtJumlah_TextChanged(object sender, EventArgs e) { }
        private void txtHarga_TextChanged(object sender, EventArgs e) { }
        private void txtStok_TextChanged(object sender, EventArgs e) { }
    }
}