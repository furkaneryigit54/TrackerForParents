using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TrackerForParents
{
    public partial class kullaniciDuzenle : Form
    {
        public kullaniciDuzenle()
        {
            InitializeComponent();
        }

        public int ekleyen = 0;
        private void kullaniciDuzenle_Load(object sender, EventArgs e)
        {
            listele();
        }

        public void listele()
        {
            cmbKullanicilar.Items.Clear();
            txtSifre.Clear();
            txtKullaniciAdi.Clear();
            txtmail.Clear();
            comboBox1.Items.Clear();
            textBox1.Text = "";
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            con.Open();
            //Ebeveyn olarak giriş yapan kişinin sadece kendi eklediği kişileri düzenleyebilmesi için bu kişileri filtreleyerek datagridview'a ekleme
            //Giriş yapan kişi adminse herkesi düzenleyebilir
            if (ekleyen != 1)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from kullanicilar where addedBy=" + ekleyen + "", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else if (ekleyen == 1)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from kullanicilar where addedBy<>0 ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            con.Close();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                cmbKullanicilar.Items.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
            }

            if (cmbKullanicilar.Items.Count<1)
            {
                btnDuzenle.Enabled = false;
                
            }else if (cmbKullanicilar.Items.Count>0)
            {
                cmbKullanicilar.SelectedIndex = 0;
            }
            btnKaydet.Enabled = false;
            btnSil.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;

        }
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            //Combobox'dan seçilen kişinin bilgilerinin ilgili yerlere aktarılmaları
            comboBox1.Items.Clear();
            txtKullaniciAdi.Text = dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[1].Value.ToString();
            txtSifre.Text = dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[2].Value.ToString();
            txtmail.Text= dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[5].Value.ToString();
            degisecekKisiID = Convert.ToInt32(dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[0].Value.ToString());
            string siteler = "";
            if (dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[3].Value.ToString()=="2")
            {
                siteler = dataGridView1.Rows[cmbKullanicilar.SelectedIndex].Cells[6].Value.ToString();
                comboBox1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                label6.Text = "Çocuk";
                label6.Visible = true;
            }
            else
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                label6.Text = "Ebeveyn";
                label6.Visible = true;
            }
            string[] sitelerArray = siteler.Split("-");
            foreach (string s in sitelerArray)
            {
                comboBox1.Items.Add(s);
            }

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i]=="")
                {
                    comboBox1.Items.Remove(i);
                }
            }
            if (comboBox1.Items.Count>0)
            {
                comboBox1.SelectedIndex = 0;
            }
            btnKaydet.Enabled = true;
            btnSil.Enabled = true;
            txtKullaniciAdi.Enabled = true;
            txtSifre.Enabled = true;
            txtmail.Enabled = true;
            
        }

        public int degisecekKisiID = 0;
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            //Bilgileri değişen kişilerin bilgilerinin güncellenmesi sorgusu
            string siteler = "";
            if (comboBox1.Enabled==true&comboBox1.Items.Count>1)
            {
                siteler = comboBox1.Items[0].ToString();
                for (int i = 1; i < comboBox1.Items.Count; i++)
                {
                    siteler = siteler + "-" + comboBox1.Items[i].ToString();
                }
            }else if (comboBox1.Enabled == true & comboBox1.Items.Count == 1)
            {
                siteler=comboBox1.Items[0].ToString();
            }

            string mail = "";
            if (txtmail.Text.Contains("@") & txtmail.Text.Contains(".com"))
            {
                mail = txtmail.Text;
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir e-posta adresi girin!", "UYARI");
                goto don;
            }
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            SQLiteCommand cmd = new SQLiteCommand("update kullanicilar set kullaniciAd=$yeniad, kullaniciSifre=$yenisifre,mail=$yenimail,siteler=$siteler where id=$id", con);
            cmd.Parameters.AddWithValue("$yeniad", txtKullaniciAdi.Text);
            cmd.Parameters.AddWithValue("$yenisifre", txtSifre.Text);
            cmd.Parameters.AddWithValue("$yenimail", mail);
            cmd.Parameters.AddWithValue("$siteler",siteler);
            cmd.Parameters.AddWithValue("$id", degisecekKisiID);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            con.Close();
            listele();
            don: ;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            //Silinecek kişinin silme sorgusu
            DialogResult dialogResult = MessageBox.Show("Bu kullanıcıyı silmek bu kullanıcının bütün verilerini silecektir. Emin Misiniz?", "UYARI", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
                SQLiteCommand cmd = new SQLiteCommand("delete from Kullanicilar where id=$id", con);
                cmd.Parameters.AddWithValue("$id", degisecekKisiID);
                SQLiteCommand cmd2 = new SQLiteCommand("delete from history where KullaniciID=$id", con);
                cmd2.Parameters.AddWithValue("$id", degisecekKisiID);
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    listele();
                }
                catch (Exception)
                {

                }
                con.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void cmbKullanicilar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKullanicilar.SelectedItem!="")
            {
                btnDuzenle.Enabled = true;
            }
            else
            {
                btnDuzenle.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Textbox'a girilen değerin yasaklı siteler listesine eklenmesi
            if (textBox1.Text!=""&!comboBox1.Items.Contains(textBox1.Text))
            {
                comboBox1.Items.Add(textBox1.Text);
            }
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i] == "")
                {
                    comboBox1.Items.Remove(i);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //seçilen combobox değerinin yasaklı siteler listesinden silinmesi
            try
            {
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);
            }finally{}
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i]=="")
                {
                    comboBox1.Items.Remove(i);
                }
            }
        }
    }
}
