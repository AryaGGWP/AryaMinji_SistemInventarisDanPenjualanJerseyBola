using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectJerseyBola
{
    public partial class FormKelolaJersey : Form
    {
        string connectionString = @"Data Source=IDEAPAD-ARYA\BANGDIO; Initial Catalog=DB_Jersey; User ID=sa; Password=123; TrustServerCertificate=True";

        // --- TAMBAHAN UCP 2: Variabel buat Binding ---
        BindingSource bs = new BindingSource();

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
                // VIEW
                string query = "SELECT * FROM vw_DataJersey";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                bs.DataSource = dt;
                dgvJersey.DataSource = bs;

                if (bindingNavigator1 != null)
                {
                    bindingNavigator1.BindingSource = bs;
                }
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
            cbUkuran.SelectedIndex = -1;
            txtID.Focus();
        }

        // --- REVISI UCP 2: METHOD STORED PROCEDURE ---
        void EksekusiSP_ManageJersey(string aksi)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_ManageJersey", conn);
                cmd.CommandType = CommandType.StoredProcedure; // Kasih tau SQL kalau ini SP

                // Parameter wajib
                cmd.Parameters.AddWithValue("@Action", aksi);
                cmd.Parameters.AddWithValue("@Kode", txtID.Text);

                // Kalau Hapus, cuma butuh Kode. Kalau Insert/Update butuh semuanya.
                if (aksi != "DELETE")
                {
                    cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                    cmd.Parameters.AddWithValue("@Klub", txtKlub.Text);
                    cmd.Parameters.AddWithValue("@Harga", int.Parse(txtHarga.Text));
                    cmd.Parameters.AddWithValue("@Stok", int.Parse(txtStok.Text));
                    cmd.Parameters.AddWithValue("@Ukuran", cbUkuran.Text);
                }

                cmd.ExecuteNonQuery(); // Tembak ke database

                MessageBox.Show("Mantap! Aksi " + aksi + " berhasil cuy.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TampilkanData();
                BersihkanForm();
            }
            catch (SqlException ex)
            {
                // Nangkep pesan error lemparan dari THROW di Stored Procedure
                MessageBox.Show("Gagal eksekusi ke database:\n" + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ada error sistem: " + ex.Message, "Error Sistem");
            }
            finally
            {
                conn.Close();
            }
        }

        // --- KODINGAN TOMBOL-TOMBOL ---

        // 1. TOMBOL SIMPAN (Panggil SP INSERT)
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "" || txtNama.Text == "" || cbUkuran.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Data belum lengkap cuy!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EksekusiSP_ManageJersey("INSERT");
        }

        // 2. TOMBOL UBAH (Panggil SP UPDATE)
        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Pilih data jersey di tabel dulu yang mau diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult konfirmasi = MessageBox.Show("Yakin nih data jersey '" + txtNama.Text + "' mau diubah?", "Konfirmasi Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi == DialogResult.Yes)
            {
                EksekusiSP_ManageJersey("UPDATE");
            }
        }

        // 3. TOMBOL HAPUS (Panggil SP DELETE)
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Pilih data jersey di tabel dulu yang mau dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult konfirmasi = MessageBox.Show("HATI-HATI! Yakin mau hapus jersey '" + txtNama.Text + "' selamanya?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (konfirmasi == DialogResult.Yes)
            {
                EksekusiSP_ManageJersey("DELETE");
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
                txtHarga.Text = row.Cells["Harga"].Value.ToString();
                txtStok.Text = row.Cells["Stok"].Value.ToString();
                cbUkuran.Text = row.Cells["Ukuran"].Value.ToString();
            }
            catch
            {
                // Abaikan kalau salah klik header
            }
        }

        // 7. EVENT PENCARIAN (Panggil SP Search dari Form Cek Stok)
        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                // REVISI UCP 2: Pake Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_CariJersey", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", txtCari.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                bs.DataSource = dt; // Update BindingSource
                dgvJersey.DataSource = bs;
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

        private void txtHarga_TextChanged(object sender, EventArgs e) { }

        private void txtHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            // validasi harus angka
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Tolak kalau ada simbol
            }
        }

        private void bindingNavigator1_BindingContextChanged(object sender, EventArgs e)
        {

        }

        private void FormKelolaJersey_Load(object sender, EventArgs e)
        {
            // Kasih komentar biar dia nggak nimpa VIEW
            // this.jerseyTableAdapter.Fill(this.dB_JerseyDataSet.Jersey);
        }
    }
}