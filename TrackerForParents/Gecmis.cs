using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
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
        List<int> kullanicilarid=new List<int>();
         List<string> kullanicilarad = new List<string>();
        private void Gecmis_Load(object sender, EventArgs e)
        {
            if (ekleyen==1)
            {
                ekleyen = 0;
            }
            comboBox1.Items.Clear();
            //Giriş yapan ebeveynin eklediği çocukların girdiği internet sitelerini görmek için bu kişileri datagridview'a aktarma
            //Giriş yapan admin ise bütün çocukları datagridview'a ekleme
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
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

            if (dataGridView1.Rows.Count>0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    kullanicilarad.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    comboBox1.Items.Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    kullanicilarid.Add(Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value));
                }
            }

            
            if (comboBox1.Items.Count>=1)
            {
                comboBox1.SelectedIndex = 0;
            }
            con.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
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
        private void btnSiteArama_Click_1(object sender, EventArgs e)
        {
            //Aranmak istenen kelimeyi datagridview2'de arama
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView2.DataSource];
            currencyManager1.SuspendBinding();
            if (txtSiteArama.Text != "")
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView3.DataSource = null;
           dataGridView2.Rows.Clear();
           dataGridView2.Refresh();
            //Datagridviewdaki çocukların ismine çift tıklayınca girdiği siteleri datagridview2'ye aktarma
            SQLiteConnection con = new SQLiteConnection("Data Source=\"C:\\TFPDB\\TFP.sqlite\";Version=3");
            con.Open();
            int id = kullanicilarid[comboBox1.SelectedIndex];
            SQLiteDataAdapter da = new SQLiteDataAdapter("select site as Site,TarayiciAd as 'Tarayıcı İsmi', Tarih, Sure as 'Geçirilen Süre' from history left join Tarayicilar on Tarayicilar.ID=TarayiciID where kullaniciID= " + id+ " order by History.ID desc ", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            con.Close();
            dataGridView2.ColumnCount = 3;
            dataGridView2.Columns[0].Name = "Site";
            dataGridView2.Columns[1].Name = "Tarih";
            dataGridView2.Columns[2].Name = "Geçirilen Süre";
            dataGridView2.Columns[0].Width = 490;
            dataGridView2.Columns[1].Width = 160;
            dataGridView2.Columns[2].Width = 125;
            foreach (DataGridViewColumn column in dataGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                    
                    string sureDeger = dataGridView3.Rows[i].Cells[3].Value.ToString();
                    if (Convert.ToInt32(sureDeger) < 60)
                    {
                        sureDeger =sureDeger + " sn";
                    }
                    else
                    {
                        sureDeger = Convert.ToInt32(Math.Ceiling(decimal.Parse(sureDeger) / 60)) + " dk";
                    }

                    dataGridView2.Rows.Add(dataGridView3.Rows[i].Cells[0].Value,
                        dataGridView3.Rows[i].Cells[2].Value, sureDeger);
            }
        }

        private void txtSiteArama_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSiteArama_Enter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(68, 215, 182);
        }

        private void txtSiteArama_Leave(object sender, EventArgs e)
        {
            panel1.BackColor=Color.FromArgb(84, 86, 95);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
