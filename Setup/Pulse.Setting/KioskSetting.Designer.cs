namespace Pulse.Setting
{
    partial class KioskSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KioskSetting));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_liscense = new System.Windows.Forms.Panel();
            this.btn_active = new System.Windows.Forms.Button();
            this.txt_license_key = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.setting_penal = new System.Windows.Forms.Panel();
            this.btn_stop_service = new System.Windows.Forms.Button();
            this.btn_start_service = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbl_copyrigth = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_liscense.SuspendLayout();
            this.setting_penal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pulse.Setting.Properties.Resources.pulse;
            this.pictureBox1.Location = new System.Drawing.Point(223, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 39);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel_liscense
            // 
            this.panel_liscense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_liscense.Controls.Add(this.btn_active);
            this.panel_liscense.Controls.Add(this.txt_license_key);
            this.panel_liscense.Controls.Add(this.label1);
            this.panel_liscense.Location = new System.Drawing.Point(13, 66);
            this.panel_liscense.Name = "panel_liscense";
            this.panel_liscense.Size = new System.Drawing.Size(589, 57);
            this.panel_liscense.TabIndex = 1;
            // 
            // btn_active
            // 
            this.btn_active.Location = new System.Drawing.Point(482, 12);
            this.btn_active.Name = "btn_active";
            this.btn_active.Size = new System.Drawing.Size(75, 27);
            this.btn_active.TabIndex = 2;
            this.btn_active.Text = "Active";
            this.btn_active.UseVisualStyleBackColor = true;
            this.btn_active.Click += new System.EventHandler(this.btn_active_Click);
            // 
            // txt_license_key
            // 
            this.txt_license_key.Location = new System.Drawing.Point(116, 14);
            this.txt_license_key.Name = "txt_license_key";
            this.txt_license_key.Size = new System.Drawing.Size(360, 22);
            this.txt_license_key.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "License Key";
            // 
            // setting_penal
            // 
            this.setting_penal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.setting_penal.Controls.Add(this.btn_start_service);
            this.setting_penal.Controls.Add(this.btn_stop_service);
            this.setting_penal.Enabled = false;
            this.setting_penal.Location = new System.Drawing.Point(13, 138);
            this.setting_penal.Name = "setting_penal";
            this.setting_penal.Size = new System.Drawing.Size(589, 71);
            this.setting_penal.TabIndex = 2;
            // 
            // btn_stop_service
            // 
            this.btn_stop_service.Location = new System.Drawing.Point(331, 13);
            this.btn_stop_service.Name = "btn_stop_service";
            this.btn_stop_service.Size = new System.Drawing.Size(110, 40);
            this.btn_stop_service.TabIndex = 15;
            this.btn_stop_service.Text = "Stop Service";
            this.btn_stop_service.UseVisualStyleBackColor = true;
            this.btn_stop_service.Click += new System.EventHandler(this.btn_stop_service_Click);
            // 
            // btn_start_service
            // 
            this.btn_start_service.Location = new System.Drawing.Point(461, 13);
            this.btn_start_service.Name = "btn_start_service";
            this.btn_start_service.Size = new System.Drawing.Size(110, 40);
            this.btn_start_service.TabIndex = 12;
            this.btn_start_service.Text = "Start Service";
            this.btn_start_service.UseVisualStyleBackColor = true;
            this.btn_start_service.Click += new System.EventHandler(this.btn_start_service_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(17, 229);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "PULSE";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(549, 229);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 17);
            this.label12.TabIndex = 17;
            this.label12.Text = "V0.0.2";
            // 
            // lbl_copyrigth
            // 
            this.lbl_copyrigth.AutoSize = true;
            this.lbl_copyrigth.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_copyrigth.Location = new System.Drawing.Point(244, 227);
            this.lbl_copyrigth.Name = "lbl_copyrigth";
            this.lbl_copyrigth.Size = new System.Drawing.Size(118, 17);
            this.lbl_copyrigth.TabIndex = 3;
            this.lbl_copyrigth.Text = "©Tekcent 2016";
            // 
            // KioskSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 264);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbl_copyrigth);
            this.Controls.Add(this.setting_penal);
            this.Controls.Add(this.panel_liscense);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KioskSetting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PULSE";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_liscense.ResumeLayout(false);
            this.panel_liscense.PerformLayout();
            this.setting_penal.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel_liscense;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_active;
        private System.Windows.Forms.TextBox txt_license_key;
        private System.Windows.Forms.Panel setting_penal;
        private System.Windows.Forms.Button btn_start_service;
        private System.Windows.Forms.Button btn_stop_service;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbl_copyrigth;
    }
}