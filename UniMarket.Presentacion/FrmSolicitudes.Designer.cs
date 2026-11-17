namespace UniMarket.Presentacion
{
    partial class FrmSolicitudes
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
            this.tabSolicitudes = new System.Windows.Forms.TabControl();
            this.tabRecibidas = new System.Windows.Forms.TabPage();
            this.tabEnviadas = new System.Windows.Forms.TabPage();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnRechazar = new System.Windows.Forms.Button();
            this.dgvRecibidas = new System.Windows.Forms.DataGridView();
            this.dgvEnviadas = new System.Windows.Forms.DataGridView();
            this.tabSolicitudes.SuspendLayout();
            this.tabRecibidas.SuspendLayout();
            this.tabEnviadas.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecibidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnviadas)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSolicitudes
            // 
            this.tabSolicitudes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSolicitudes.Controls.Add(this.tabRecibidas);
            this.tabSolicitudes.Controls.Add(this.tabEnviadas);
            this.tabSolicitudes.Location = new System.Drawing.Point(12, 33);
            this.tabSolicitudes.Name = "tabSolicitudes";
            this.tabSolicitudes.SelectedIndex = 0;
            this.tabSolicitudes.Size = new System.Drawing.Size(930, 375);
            this.tabSolicitudes.TabIndex = 0;
            // 
            // tabRecibidas
            // 
            this.tabRecibidas.Controls.Add(this.dgvRecibidas);
            this.tabRecibidas.Controls.Add(this.pnlAcciones);
            this.tabRecibidas.Location = new System.Drawing.Point(4, 25);
            this.tabRecibidas.Name = "tabRecibidas";
            this.tabRecibidas.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecibidas.Size = new System.Drawing.Size(922, 346);
            this.tabRecibidas.TabIndex = 0;
            this.tabRecibidas.Text = "Recibidas";
            this.tabRecibidas.UseVisualStyleBackColor = true;
            // 
            // tabEnviadas
            // 
            this.tabEnviadas.Controls.Add(this.dgvEnviadas);
            this.tabEnviadas.Location = new System.Drawing.Point(4, 25);
            this.tabEnviadas.Name = "tabEnviadas";
            this.tabEnviadas.Padding = new System.Windows.Forms.Padding(3);
            this.tabEnviadas.Size = new System.Drawing.Size(922, 346);
            this.tabEnviadas.TabIndex = 1;
            this.tabEnviadas.Text = "Enviadas";
            this.tabEnviadas.UseVisualStyleBackColor = true;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(96, 500);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(116, 57);
            this.btnActualizar.TabIndex = 1;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Location = new System.Drawing.Point(803, 500);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(116, 57);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Controls.Add(this.btnRechazar);
            this.pnlAcciones.Controls.Add(this.btnAceptar);
            this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAcciones.Location = new System.Drawing.Point(3, 243);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(916, 100);
            this.pnlAcciones.TabIndex = 0;
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(66, 25);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(99, 43);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnRechazar
            // 
            this.btnRechazar.Location = new System.Drawing.Point(765, 25);
            this.btnRechazar.Name = "btnRechazar";
            this.btnRechazar.Size = new System.Drawing.Size(99, 43);
            this.btnRechazar.TabIndex = 1;
            this.btnRechazar.Text = "Rechazar";
            this.btnRechazar.UseVisualStyleBackColor = true;
            this.btnRechazar.Click += new System.EventHandler(this.btnRechazar_Click);
            // 
            // dgvRecibidas
            // 
            this.dgvRecibidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecibidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecibidas.Location = new System.Drawing.Point(3, 3);
            this.dgvRecibidas.Name = "dgvRecibidas";
            this.dgvRecibidas.RowHeadersWidth = 51;
            this.dgvRecibidas.RowTemplate.Height = 24;
            this.dgvRecibidas.Size = new System.Drawing.Size(916, 240);
            this.dgvRecibidas.TabIndex = 1;
            // 
            // dgvEnviadas
            // 
            this.dgvEnviadas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnviadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnviadas.Location = new System.Drawing.Point(3, 3);
            this.dgvEnviadas.Name = "dgvEnviadas";
            this.dgvEnviadas.RowHeadersWidth = 51;
            this.dgvEnviadas.RowTemplate.Height = 24;
            this.dgvEnviadas.Size = new System.Drawing.Size(916, 340);
            this.dgvEnviadas.TabIndex = 0;
            // 
            // FrmSolicitudes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(954, 610);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.tabSolicitudes);
            this.MinimizeBox = false;
            this.Name = "FrmSolicitudes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mis solicitudes";
            this.tabSolicitudes.ResumeLayout(false);
            this.tabRecibidas.ResumeLayout(false);
            this.tabEnviadas.ResumeLayout(false);
            this.pnlAcciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecibidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnviadas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSolicitudes;
        private System.Windows.Forms.TabPage tabRecibidas;
        private System.Windows.Forms.TabPage tabEnviadas;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.DataGridView dgvRecibidas;
        private System.Windows.Forms.Button btnRechazar;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.DataGridView dgvEnviadas;
    }
}