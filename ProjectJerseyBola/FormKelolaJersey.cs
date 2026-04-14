using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProjectJerseyBola
{
    public partial class FormKelolaJersey : Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        public FormKelolaJersey()
        {
            InitializeComponent();
            TampilkanData();
        }

        // --- METHOD HELPER ---

        void TampilkanData()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT * FROM Jersey";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvJersey.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal nampilin data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        void BersihkanForm()
        {
            txtID.Clear();
            txtNama.Clear();
            txtKlub.Clear();
            txtHarga.Clear();
            txtStok.Clear();
            cbUkuran.SelectedIndex = -1; // Kosongin combobox
            txtID.Focus(); // Bikin kursor kedip-kedip otomatis di kolom ID
        }

        // --- KODINGAN TOMBOL-TOMBOL ---

        // 1. TOMBOL SIMPAN
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            
            if (txtID.Text == "" || txtNama.Text == "" || cbUkuran.Text == "")
            {
                MessageBox.Show("Data belum lengkap cuy!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "INSERT INTO Jersey (KodeJersey, NamaJersey, Klub, Ukuran, Harga, Stok) " +
                               "VALUES (@kode, @nama, @klub, @ukuran, @harga, @stok)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode", txtID.Text);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@klub", txtKlub.Text);
                cmd.Parameters.AddWithValue("@ukuran", cbUkuran.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@stok", txtStok.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Disimpan!", "Sukses");
                TampilkanData();
                BersihkanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Simpan (Mungkin ID/Kode udah ada): " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // 2. TOMBOL UBAH (Edit)
        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "") return; // Gak bisa ubah kalau ID kosong

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "UPDATE Jersey SET NamaJersey=@nama, Klub=@klub, Ukuran=@ukuran, Harga=@harga, Stok=@stok WHERE KodeJersey=@kode";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode", txtID.Text);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@klub", txtKlub.Text);
                cmd.Parameters.AddWithValue("@ukuran", cbUkuran.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@stok", txtStok.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Diupdate!", "Sukses");
                TampilkanData();
                BersihkanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Update: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // 3. TOMBOL HAPUS (Delete)
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "") return;

            if (MessageBox.Show("Yakin mau hapus jersey ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Jersey WHERE KodeJersey=@kode";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kode", txtID.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Berhasil Dihapus!");
                    TampilkanData();
                    BersihkanForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Hapus: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        // 4. TOMBOL CLEAR
        private void btnClear_Click(object sender, EventArgs e)
        {
            BersihkanForm();
        }

        // 5. TOMBOL KEMBALI
        private void btnKembali_Click(object sender, EventArgs e)
        {
            FormMenuUtama menu = new FormMenuUtama();
            menu.Show();
            this.Close();
        }

        // EVENT KLIK DGV
        private void dgvJersey_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dgvJersey.Rows[e.RowIndex];
                txtID.Text = row.Cells["KodeJersey"].Value.ToString();
                txtNama.Text = row.Cells["NamaJersey"].Value.ToString();
                txtKlub.Text = row.Cells["Klub"].Value.ToString();
                cbUkuran.Text = row.Cells["Ukuran"].Value.ToString();
                txtHarga.Text = row.Cells["Harga"].Value.ToString();
                txtStok.Text = row.Cells["Stok"].Value.ToString();
            }
            catch
            {
                // Abaikan kalau salah klik header
            }
        }

        // 7. EVENT PENCARIAN (LIVE SEARCH)
        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT * FROM Jersey WHERE NamaJersey LIKE '%" + txtCari.Text + "%' OR Klub LIKE '%" + txtCari.Text + "%' OR KodeJersey LIKE '%" + txtCari.Text + "%'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvJersey.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Aduh error pas nyari: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}