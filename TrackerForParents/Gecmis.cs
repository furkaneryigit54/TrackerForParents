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

namespace TrackerForParents
{
    public partial class Gecmis : Form
    {
        public Gecmis()
        {
            InitializeComponent();
        }

        public int ekleyen = 0;
        private void Gecmis_Load(object sender, EventArgs e)
        {
            //Giriş yapan ebeveynin eklediği çocukların girdiği internet sitelerini görmek için bu kişileri datagridview'a aktarma
            //Giriş yapan admin ise bütün çocukları datagridview'a ekleme
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            con.Open();
            if (ekleyen!=0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select KullaniciAd as Kullanıcılar, id from Kullanicilar where tYetkiID=2 and addedBy="+ekleyen+" ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[1].Visible = false;
            }else if (ekleyen==0)
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select KullaniciAd as Kullanıcılar, id from Kullanicilar where tYetkiID=2  ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[1].Visible = false;
            }
            
            con.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Datagridviewdaki çocukların ismine çift tıklayınca girdiği siteleri datagridview2'ye aktarma
            SQLiteConnection con = new SQLiteConnection("Data Source=TFP.sqlite;Version=3");
            con.Open();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select site as Site,TarayiciAd as 'Tarayıcı İsmi', Tarih, Sure as 'Geçirilen Süre' from history left join Tarayicilar on Tarayicilar.ID=TarayiciID where kullaniciID= " + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value +" ", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
            dataGridView2.Columns[0].Width=400;
            dataGridView2.Columns[2].Width = 110;
            dataGridView2.Columns[3].Width = 64;
        }

        private void dataGridView2_DataSourceChanged(object sender, EventArgs e)
        {
            //datagridview2'ye veri girişi yapıldığında arama tollarını aktif hale getirme
            if (dataGridView2.Rows.Count>0)
            {
                txtSiteArama.Enabled = true;
                btnSiteArama.Enabled = true;
            }
            else
            {
                txtSiteArama.Enabled = false;
                btnSiteArama.Enabled = false;
            }
        }

        private void btnSiteArama_Click(object sender, EventArgs e)
        {
            //Aranmak istenen kelimeyi datagridview2'de arama
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView2.DataSource];
            currencyManager1.SuspendBinding();
            if (txtSiteArama.Text!="")
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    if (!dataGridView2.Rows[i].Cells[0].Value.ToString().ToLower().Contains(txtSiteArama.Text.ToLower()))
                    {
                        dataGridView2.Rows[i].Visible = false;
                    }
                }   
            }
            else
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.Rows[i].Visible = true;
                }
            }
            currencyManager1.ResumeBinding();
        }

    }
}
