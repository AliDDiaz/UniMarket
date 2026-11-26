namespace UniMarket.Presentacion
{
    partial class FrmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.chkMisPublicaciones = new System.Windows.Forms.CheckBox();
            this.flpCatalogo = new System.Windows.Forms.FlowLayoutPanel();
            this.picEngel = new System.Windows.Forms.PictureBox();
            this.pnlBuscar = new System.Windows.Forms.Panel();
            this.picLupa = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picEngel)).BeginInit();
            this.pnlBuscar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLupa)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.BackColor = System.Drawing.Color.Transparent;
            this.lblBuscar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuscar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(63)))), ((int)(((byte)(214)))));
            this.lblBuscar.Location = new System.Drawing.Point(71, 32);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(113, 23);
            this.lblBuscar.TabIndex = 1;
            this.lblBuscar.Text = "¿Qué buscas?";
            // 
            // txtBuscar
            // 
            this.txtBuscar.BackColor = System.Drawing.Color.White;
            this.txtBuscar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBuscar.Location = new System.Drawing.Point(46, 5);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(294, 25);
            this.txtBuscar.TabIndex = 2;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(77)))), ((int)(((byte)(255)))));
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.Location = new System.Drawing.Point(579, 24);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 36);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // chkMisPublicaciones
            // 
            this.chkMisPublicaciones.AutoSize = true;
            this.chkMisPublicaciones.BackColor = System.Drawing.Color.Transparent;
            this.chkMisPublicaciones.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.chkMisPublicaciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(63)))), ((int)(((byte)(214)))));
            this.chkMisPublicaciones.Location = new System.Drawing.Point(744, 24);
            this.chkMisPublicaciones.Name = "chkMisPublicaciones";
            this.chkMisPublicaciones.Size = new System.Drawing.Size(207, 25);
            this.chkMisPublicaciones.TabIndex = 8;
            this.chkMisPublicaciones.Text = "Solo mis publicaciones";
            this.chkMisPublicaciones.UseVisualStyleBackColor = false;
            this.chkMisPublicaciones.CheckedChanged += new System.EventHandler(this.chkMisPublicaciones_CheckedChanged);
            // 
            // flpCatalogo
            // 
            this.flpCatalogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpCatalogo.AutoScroll = true;
            this.flpCatalogo.BackColor = System.Drawing.Color.Lavender;
            this.flpCatalogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.flpCatalogo.Location = new System.Drawing.Point(24, 66);
            this.flpCatalogo.Name = "flpCatalogo";
            this.flpCatalogo.Size = new System.Drawing.Size(1353, 688);
            this.flpCatalogo.TabIndex = 14;
            // 
            // picEngel
            // 
            this.picEngel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picEngel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(196)))), ((int)(((byte)(245)))));
            this.picEngel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picEngel.Image = ((System.Drawing.Image)(resources.GetObject("picEngel.Image")));
            this.picEngel.Location = new System.Drawing.Point(1256, 728);
            this.picEngel.Name = "picEngel";
            this.picEngel.Size = new System.Drawing.Size(81, 78);
            this.picEngel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEngel.TabIndex = 16;
            this.picEngel.TabStop = false;
            this.picEngel.Click += new System.EventHandler(this.picEngel_Click_1);
            // 
            // pnlBuscar
            // 
            this.pnlBuscar.BackColor = System.Drawing.Color.White;
            this.pnlBuscar.Controls.Add(this.picLupa);
            this.pnlBuscar.Controls.Add(this.txtBuscar);
            this.pnlBuscar.Location = new System.Drawing.Point(218, 24);
            this.pnlBuscar.Name = "pnlBuscar";
            this.pnlBuscar.Size = new System.Drawing.Size(343, 35);
            this.pnlBuscar.TabIndex = 0;
            // 
            // picLupa
            // 
            this.picLupa.BackColor = System.Drawing.Color.Transparent;
            this.picLupa.Image = ((System.Drawing.Image)(resources.GetObject("picLupa.Image")));
            this.picLupa.Location = new System.Drawing.Point(3, 0);
            this.picLupa.Name = "picLupa";
            this.picLupa.Size = new System.Drawing.Size(37, 32);
            this.picLupa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLupa.TabIndex = 17;
            this.picLupa.TabStop = false;
            // 
            // FrmPrincipal
            // 
            this.AcceptButton = this.btnBuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1404, 806);
            this.Controls.Add(this.pnlBuscar);
            this.Controls.Add(this.picEngel);
            this.Controls.Add(this.flpCatalogo);
            this.Controls.Add(this.chkMisPublicaciones);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.lblBuscar);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trueque - Tu tienda de confianza";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.Shown += new System.EventHandler(this.FrmPrincipal_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picEngel)).EndInit();
            this.pnlBuscar.ResumeLayout(false);
            this.pnlBuscar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLupa)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.CheckBox chkMisPublicaciones;
        private System.Windows.Forms.FlowLayoutPanel flpCatalogo;
        private System.Windows.Forms.PictureBox picEngel;
        private System.Windows.Forms.Panel pnlBuscar;
        private System.Windows.Forms.PictureBox picLupa;
        public System.Windows.Forms.TextBox txtBuscar;
    }
}