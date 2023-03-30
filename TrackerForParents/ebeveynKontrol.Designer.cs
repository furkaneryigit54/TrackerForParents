namespace TrackerForParents
{
    partial class ebeveynKontrol
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ebeveynKontrol));
            this.btnKullaniciEkle = new System.Windows.Forms.Button();
            this.btnKullaniciDuzenle = new System.Windows.Forms.Button();
            this.btnGirilenSiteler = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnKullaniciEkle
            // 
            this.btnKullaniciEkle.Location = new System.Drawing.Point(215, 9);
            this.btnKullaniciEkle.Name = "btnKullaniciEkle";
            this.btnKullaniciEkle.Size = new System.Drawing.Size(131, 37);
            this.btnKullaniciEkle.TabIndex = 0;
            this.btnKullaniciEkle.Text = "Kullanıcı Ekle";
            this.btnKullaniciEkle.UseVisualStyleBackColor = true;
            this.btnKullaniciEkle.Click += new System.EventHandler(this.btnKullaniciEkle_Click);
            // 
            // btnKullaniciDuzenle
            // 
            this.btnKullaniciDuzenle.Location = new System.Drawing.Point(215, 56);
            this.btnKullaniciDuzenle.Name = "btnKullaniciDuzenle";
            this.btnKullaniciDuzenle.Size = new System.Drawing.Size(131, 37);
            this.btnKullaniciDuzenle.TabIndex = 1;
            this.btnKullaniciDuzenle.Text = "Kullanıcıları Düzenle";
            this.btnKullaniciDuzenle.UseVisualStyleBackColor = true;
            this.btnKullaniciDuzenle.Click += new System.EventHandler(this.btnKullaniciDuzenle_Click);
            // 
            // btnGirilenSiteler
            // 
            this.btnGirilenSiteler.Location = new System.Drawing.Point(215, 99);
            this.btnGirilenSiteler.Name = "btnGirilenSiteler";
            this.btnGirilenSiteler.Size = new System.Drawing.Size(131, 37);
            this.btnGirilenSiteler.TabIndex = 2;
            this.btnGirilenSiteler.Text = "Girilen Siteler";
            this.btnGirilenSiteler.UseVisualStyleBackColor = true;
            this.btnGirilenSiteler.Click += new System.EventHandler(this.btnGirilenSiteler_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tarih";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(12, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Saat";
            // 
            // ebeveynKontrol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(360, 192);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGirilenSiteler);
            this.Controls.Add(this.btnKullaniciDuzenle);
            this.Controls.Add(this.btnKullaniciEkle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ebeveynKontrol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ebeveyn";
            this.Load += new System.EventHandler(this.ebeveynKontrol_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnKullaniciEkle;
        private Button btnKullaniciDuzenle;
        private Button btnGirilenSiteler;
        private Label label1;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
        private Label label3;
    }
}