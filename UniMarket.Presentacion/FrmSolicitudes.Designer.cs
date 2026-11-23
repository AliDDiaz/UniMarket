namespace UniMarket.Presentacion
{
    partial class FrmSolicitudes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.lblTituloForm = new System.Windows.Forms.Label();
            this.tblContenedor = new System.Windows.Forms.TableLayoutPanel();
            this.pnlColRecibidas = new System.Windows.Forms.Panel();
            this.flpRecibidas = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubRecibidas = new System.Windows.Forms.Label();
            this.pnlColEnviadas = new System.Windows.Forms.Panel();
            this.flpEnviadas = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubEnviadas = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.tblContenedor.SuspendLayout();
            this.pnlColRecibidas.SuspendLayout();
            this.pnlColEnviadas.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.panelHeader.Controls.Add(this.btnActualizar);
            this.panelHeader.Controls.Add(this.lblTituloForm);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(920, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTituloForm
            // 
            this.lblTituloForm.AutoSize = true;
            this.lblTituloForm.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblTituloForm.ForeColor = System.Drawing.Color.White;
            this.lblTituloForm.Location = new System.Drawing.Point(20, 16);
            this.lblTituloForm.Name = "lblTituloForm";
            this.lblTituloForm.Size = new System.Drawing.Size(222, 23);
            this.lblTituloForm.TabIndex = 0;
            this.lblTituloForm.Text = "📋 Historial de Solicitudes";
            // 
            // btnActualizar
            // 
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Location = new System.Drawing.Point(775, 11);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(125, 34);
            this.btnActualizar.TabIndex = 1;
            this.btnActualizar.Text = "↻ Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // tblContenedor
            // 
            this.tblContenedor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.tblContenedor.ColumnCount = 2;
            this.tblContenedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblContenedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblContenedor.Controls.Add(this.pnlColRecibidas, 0, 0);
            this.tblContenedor.Controls.Add(this.pnlColEnviadas, 1, 0);
            this.tblContenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblContenedor.Location = new System.Drawing.Point(0, 56);
            this.tblContenedor.Name = "tblContenedor";
            this.tblContenedor.Padding = new System.Windows.Forms.Padding(12);
            this.tblContenedor.RowCount = 1;
            this.tblContenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblContenedor.Size = new System.Drawing.Size(920, 484);
            this.tblContenedor.TabIndex = 1;
            // 
            // pnlColRecibidas
            // 
            this.pnlColRecibidas.Controls.Add(this.flpRecibidas);
            this.pnlColRecibidas.Controls.Add(this.lblSubRecibidas);
            this.pnlColRecibidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColRecibidas.Location = new System.Drawing.Point(15, 15);
            this.pnlColRecibidas.Name = "pnlColRecibidas";
            this.pnlColRecibidas.Padding = new System.Windows.Forms.Padding(4);
            this.pnlColRecibidas.Size = new System.Drawing.Size(442, 454);
            this.pnlColRecibidas.TabIndex = 0;
            // 
            // lblSubRecibidas
            // 
            this.lblSubRecibidas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubRecibidas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubRecibidas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.lblSubRecibidas.Location = new System.Drawing.Point(4, 4);
            this.lblSubRecibidas.Name = "lblSubRecibidas";
            this.lblSubRecibidas.Size = new System.Drawing.Size(434, 32);
            this.lblSubRecibidas.TabIndex = 0;
            this.lblSubRecibidas.Text = "📥 Recibidas (Quieren comprarte)";
            this.lblSubRecibidas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpRecibidas
            // 
            this.flpRecibidas.AutoScroll = true;
            this.flpRecibidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRecibidas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRecibidas.Location = new System.Drawing.Point(4, 36);
            this.flpRecibidas.Name = "flpRecibidas";
            this.flpRecibidas.Padding = new System.Windows.Forms.Padding(2, 6, 6, 6);
            this.flpRecibidas.Size = new System.Drawing.Size(434, 414);
            this.flpRecibidas.TabIndex = 1;
            this.flpRecibidas.WrapContents = false;
            // 
            // pnlColEnviadas
            // 
            this.pnlColEnviadas.Controls.Add(this.flpEnviadas);
            this.pnlColEnviadas.Controls.Add(this.lblSubEnviadas);
            this.pnlColEnviadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColEnviadas.Location = new System.Drawing.Point(463, 15);
            this.pnlColEnviadas.Name = "pnlColEnviadas";
            this.pnlColEnviadas.Padding = new System.Windows.Forms.Padding(4);
            this.pnlColEnviadas.Size = new System.Drawing.Size(442, 454);
            this.pnlColEnviadas.TabIndex = 1;
            // 
            // lblSubEnviadas
            // 
            this.lblSubEnviadas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubEnviadas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubEnviadas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.lblSubEnviadas.Location = new System.Drawing.Point(4, 4);
            this.lblSubEnviadas.Name = "lblSubEnviadas";
            this.lblSubEnviadas.Size = new System.Drawing.Size(434, 32);
            this.lblSubEnviadas.TabIndex = 0;
            this.lblSubEnviadas.Text = "📤 Enviadas (Tus pedidos)";
            this.lblSubEnviadas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpEnviadas
            // 
            this.flpEnviadas.AutoScroll = true;
            this.flpEnviadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpEnviadas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpEnviadas.Location = new System.Drawing.Point(4, 36);
            this.flpEnviadas.Name = "flpEnviadas";
            this.flpEnviadas.Padding = new System.Windows.Forms.Padding(2, 6, 6, 6);
            this.flpEnviadas.Size = new System.Drawing.Size(434, 414);
            this.flpEnviadas.TabIndex = 1;
            this.flpEnviadas.WrapContents = false;
            // 
            // FrmSolicitudes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 540);
            this.Controls.Add(this.tblContenedor);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSolicitudes";
            this.Text = "Solicitudes - Trueque";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.tblContenedor.ResumeLayout(false);
            this.pnlColRecibidas.ResumeLayout(false);
            this.pnlColEnviadas.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Label lblTituloForm;
        private System.Windows.Forms.TableLayoutPanel tblContenedor;
        private System.Windows.Forms.Panel pnlColRecibidas;
        private System.Windows.Forms.FlowLayoutPanel flpRecibidas;
        private System.Windows.Forms.Label lblSubRecibidas;
        private System.Windows.Forms.Panel pnlColEnviadas;
        private System.Windows.Forms.FlowLayoutPanel flpEnviadas;
        private System.Windows.Forms.Label lblSubEnviadas;
    }
}