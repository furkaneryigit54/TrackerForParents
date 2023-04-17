using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace TrackerForParents
{
    public partial class anaGiris : Form
    {
        public anaGiris()
        {
            InitializeComponent();
        }
         public int ekleyen = 0;
         public List<int> cocuklarID = new List<int>();
        private void anaGiris_Load(object sender, EventArgs e)
        {
            if (ekleyen==1)
            {
                ekleyen = 0;
            }
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();
            if (ekleyen != 0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select KullaniciAd as Kullanıcılar, id from Kullanicilar where tYetkiID=2 and addedBy=" + ekleyen + " ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[1].Visible = false;
            }
            else if (ekleyen == 0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select KullaniciAd as Kullanıcılar, id from Kullanicilar where tYetkiID=2  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[1].Visible = false;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    cocuklarID.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value));
                    cmbKullanicilar.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
            }


            if (cmbKullanicilar.Items.Count >= 1)
            {

                cmbKullanicilar.SelectedIndex = 0;
                cmbKullanicilar.Enabled = true;
            }
            
         
            if (cmbKullanicilar.Items.Count>0)
            {
                
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from history  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView3.DataSource = dt;
                if (dataGridView3.Rows.Count>0)
                {
                    EnFazlaSure();
                    GunlukSure();
                }
                else
                {
                    cmbKullanicilar.Enabled = false;
                    pnlGunler.Visible = false;
                    panel5.Visible = false;
                    panelSaatler.Visible = false;
                    dateTimePicker1.Enabled = false;
                }
            }
            else
            {
                cmbKullanicilar.Enabled = false;
                pnlGunler.Visible = false;
                panel5.Visible = false;
                panelSaatler.Visible = false;
                dateTimePicker1.Enabled = false;
            }
           
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            con.Close();
            dateTimePicker1.Value = DateTime.Now;
        }

        public void EnFazlaSure()
        {
            List<int> kullaniciID = new List<int>();
            dataGridView1.DataSource = null;
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();
            if (ekleyen != 0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from Kullanicilar where tYetkiID=2 and addedBy=" + ekleyen + " ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    kullaniciID.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
                }
            }
            else if (ekleyen == 0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select * from Kullanicilar where tYetkiID=2  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    kullaniciID.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value));
                }
            }

            List<string> site=new List<string>();
            List<string> sure=new List<string>();
            List<string> kullaniciad=new List<string>();
            for (int i = 0; i < kullaniciID.Count; i++)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select Kullanicilar.kullaniciAd,History.Site,max(sure) from history left join Tarayicilar on History.TarayiciID=Tarayicilar.ID left join Kullanicilar on History.KullaniciID=Kullanicilar.ID where  KullaniciID=" + kullaniciID[i] +"", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                if (dataGridView2.Rows.Count>0)
                {
                    kullaniciad.Add(dataGridView2.Rows[0].Cells[0].Value.ToString());
                    site.Add(dataGridView2.Rows[0].Cells[1].Value.ToString());
                    sure.Add(dataGridView2.Rows[0].Cells[2].Value.ToString());

                }
            }
            con.Close();
            
            dataGridView2.DataSource = null;
            dataGridView2.ColumnCount = 3;
            dataGridView2.Columns[0].Name = "Kullanıcı İsmi";
            dataGridView2.Columns[1].Name = "Girilen Site";
            dataGridView2.Columns[2].Name = "Geçirilen Süre";

            for (int i = 0; i < site.Count; i++)
            {
                string sureDeger="";
                if (Convert.ToInt32(sure[i]) <60)
                {
                    sureDeger = sure[i] + " sn";
                }
                else 
                {
                    sureDeger = Convert.ToInt32(Math.Ceiling(decimal.Parse(sure[i]) / 60)) + " dk";
                }
                dataGridView2.Rows.Add(kullaniciad[i], site[i], sureDeger);
            }

            
            dataGridView2.Columns[0].Width = 150;
            dataGridView2.Columns[1].Width = 500;
            dataGridView2.Columns[2].Width = 140;
        }
        long[] gunlukSureler = new long[7];
        public void GunlukSure()
        {
            string tarih = dateTimePicker1.Value.ToShortDateString();
            string[] tarihAyirilmis = tarih.Split(".");

            string ilkTARİH = Convert.ToInt32(tarihAyirilmis[0]).ToString() +"."+ tarihAyirilmis[1] +"."+
                              tarihAyirilmis[2];
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();

            
            using (SQLiteCommand command = new SQLiteCommand("select sum(sure) from history where tarih like '" + ilkTARİH + "%' and KullaniciID=" + cocuklarID[cmbKullanicilar.SelectedIndex] +"", con))
            {
                try
                {
                    gunlukSureler[0] = (long)command.ExecuteScalar();
                }
                catch (Exception e)
                {
                    gunlukSureler[0] = 0;
                }
            }
            
            int tarihGun = Convert.ToInt32(tarihAyirilmis[0]);
            tarihGun = Convert.ToInt32(tarihAyirilmis[0]);
            lblTarih1.Text = tarihGun.ToString() + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih2.Text = Convert.ToString(tarihGun + 1) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih3.Text = Convert.ToString(tarihGun + 2) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih4.Text = Convert.ToString(tarihGun + 3) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih5.Text = Convert.ToString(tarihGun + 4) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih6.Text = Convert.ToString(tarihGun + 5) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            lblTarih7.Text = Convert.ToString(tarihGun + 6) + "." + tarihAyirilmis[1] + "." + tarihAyirilmis[2];
            for (int i = 1; i <= 6; i++)
            {
                try
                {
                    tarihGun++;
                    string queryTarih = tarihGun.ToString() +"."+ tarihAyirilmis[1] +"."+
                                        tarihAyirilmis[2];
                    using (SQLiteCommand command = new SQLiteCommand("select sum(sure) from history where tarih like '" + queryTarih + "%' and KullaniciID=" + cocuklarID[cmbKullanicilar.SelectedIndex] +" ", con))
                    {
                        gunlukSureler[i] = (long)command.ExecuteScalar();
                    }
                }
                catch (Exception e)
                {
                    gunlukSureler[i] = 0;
                }
            }
            con.Close();
            for (int i = 0; i < gunlukSureler.Length; i++)
            {
                double deger;
                if (i == 0)
                {
                   
                    deger = gunlukSureler[i];
                    double location = (deger/3600) * 35;
                    if (location==0)
                    {
                        pnlGun1.Visible = false;
                    }
                    else
                    {
                        pnlGun1.Visible = true;
                    }
                    pnlGun1.Location = new Point(45, 350- Convert.ToInt32(location));
                    if (deger/3600<=2)
                    {
                        pnlGun1.BackColor = Color.FromArgb(44, 186, 0);
                    }else if (deger/3600<=4)
                    {
                        pnlGun1.BackColor = Color.FromArgb(163, 255, 0);
                    }else if (deger/3600<=6)
                    {
                        pnlGun1.BackColor = Color.FromArgb(255, 244, 0);
                    }else if (deger/3600<=8)
                    {
                        pnlGun1.BackColor = Color.FromArgb(255, 167, 0);
                    }else if (deger/3600<=10)
                    {
                        pnlGun1.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun1.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun1.Location = new Point(45, 0);
                    }
                }
                else if (i == 1)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun2.Visible = false;
                    }
                    else
                    {
                        pnlGun2.Visible = true;
                    }
                    pnlGun2.Location = new Point(142, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun2.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun2.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun2.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun2.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun2.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun2.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun2.Location = new Point(142, 0);
                    }
                }
                else if (i == 2)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun3.Visible = false;
                    }
                    else
                    {
                        pnlGun3.Visible = true;
                    }
                    pnlGun3.Location = new Point(240, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun3.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun3.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun3.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun3.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun3.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun3.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun3.Location = new Point(240, 0);
                    }
                }
                else if (i == 3)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun4.Visible = false;
                    }
                    else
                    {
                        pnlGun4.Visible = true;
                    }
                    pnlGun4.Location = new Point(342, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun4.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun4.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun4.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun4.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun4.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun4.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun4.Location = new Point(342, 0);
                    }
                }
                else if (i == 4)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun5.Visible = false;
                    }
                    else
                    {
                        pnlGun5.Visible = true;
                    }
                    pnlGun5.Location = new Point(440, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun5.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun5.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun5.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun5.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun5.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun5.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun5.Location = new Point(440, 0);
                    }
                }
                else if (i == 5)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun6.Visible = false;
                    }
                    else
                    {
                        pnlGun6.Visible = true;
                    }
                    pnlGun6.Location = new Point(537, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun6.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun6.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun6.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun6.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun6.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun6.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun6.Location = new Point(537, 0);
                    }
                }
                else if (i == 6)
                {
                    deger = gunlukSureler[i];
                    double location = (deger / 3600) * 35;
                    if (location == 0)
                    {
                        pnlGun7.Visible = false;
                    }
                    else
                    {
                        pnlGun7.Visible = true;
                    }
                    pnlGun7.Location = new Point(640, 350- Convert.ToInt32(location));
                    if (deger / 3600 <= 2)
                    {
                        pnlGun7.BackColor = Color.FromArgb(44, 186, 0);
                    }
                    else if (deger / 3600 <= 4)
                    {
                        pnlGun7.BackColor = Color.FromArgb(163, 255, 0);
                    }
                    else if (deger / 3600 <= 6)
                    {
                        pnlGun7.BackColor = Color.FromArgb(255, 244, 0);
                    }
                    else if (deger / 3600 <= 8)
                    {
                        pnlGun7.BackColor = Color.FromArgb(255, 167, 0);
                    }
                    else if (deger / 3600 <=10)
                    {
                        pnlGun7.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        pnlGun7.BackColor = Color.FromArgb(255, 0, 0);
                        pnlGun7.Location = new Point(640, 0);
                    }
                }
            }

            
        }
        private void label2_Click(object sender, EventArgs e)       
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GunlukSure();
        }

     
        private void cmbKullanicilar_SelectedIndexChanged(object sender, EventArgs e)
        {
            GunlukSure();
        }

        private void pnlGun1_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[0] % 360;
            toolTip1.SetToolTip(pnlGun1, Convert.ToString(gunlukSureler[0]/3600)+" saat "+ Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun2_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[1] % 3600;
            toolTip1.SetToolTip(pnlGun2, Convert.ToString(gunlukSureler[1] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun3_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[2] % 3600;
            toolTip1.SetToolTip(pnlGun3, Convert.ToString(gunlukSureler[2] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun4_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[3] % 3600;
            toolTip1.SetToolTip(pnlGun4, Convert.ToString(gunlukSureler[3] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun5_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[4] % 3600;
            toolTip1.SetToolTip(pnlGun5, Convert.ToString(gunlukSureler[4] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun6_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[5] % 3600;
            toolTip1.SetToolTip(pnlGun6, Convert.ToString(gunlukSureler[5] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }

        private void pnlGun7_MouseHover(object sender, EventArgs e)
        {
            long dakika = gunlukSureler[6] % 3600;
            toolTip1.SetToolTip(pnlGun7, Convert.ToString(gunlukSureler[6] / 3600) + " saat " + Convert.ToString(dakika/60) + " dakika");
        }
    }
}
