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
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.dgvPublicaciones = new System.Windows.Forms.DataGridView();
            this.chkMisPublicaciones = new System.Windows.Forms.CheckBox();
            this.btnSolicitar = new System.Windows.Forms.Button();
            this.btnSolicitudes = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPublicaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(627, 18);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(36, 16);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Hola";
            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new System.Drawing.Point(59, 123);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new System.Drawing.Size(49, 16);
            this.lblBuscar.TabIndex = 1;
            this.lblBuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(134, 120);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(100, 22);
            this.txtBuscar.TabIndex = 2;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(258, 108);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(93, 46);
            this.btnBuscar.TabIndex = 3;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(258, 447);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(93, 46);
            this.btnEliminar.TabIndex = 4;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(81, 447);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(93, 46);
            this.btnEditar.TabIndex = 5;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // dgvPublicaciones
            // 
            this.dgvPublicaciones.AllowUserToAddRows = false;
            this.dgvPublicaciones.AllowUserToDeleteRows = false;
            this.dgvPublicaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPublicaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPublicaciones.Location = new System.Drawing.Point(47, 183);
            this.dgvPublicaciones.MultiSelect = false;
            this.dgvPublicaciones.Name = "dgvPublicaciones";
            this.dgvPublicaciones.ReadOnly = true;
            this.dgvPublicaciones.RowHeadersWidth = 51;
            this.dgvPublicaciones.RowTemplate.Height = 24;
            this.dgvPublicaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPublicaciones.Size = new System.Drawing.Size(699, 227);
            this.dgvPublicaciones.TabIndex = 7;
            this.dgvPublicaciones.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPublicaciones_CellContentClick);
            // 
            // chkMisPublicaciones
            // 
            this.chkMisPublicaciones.AutoSize = true;
            this.chkMisPublicaciones.Location = new System.Drawing.Point(376, 119);
            this.chkMisPublicaciones.Name = "chkMisPublicaciones";
            this.chkMisPublicaciones.Size = new System.Drawing.Size(168, 20);
            this.chkMisPublicaciones.TabIndex = 8;
            this.chkMisPublicaciones.Text = "Solo mis publicaciones";
            this.chkMisPublicaciones.UseVisualStyleBackColor = true;
            this.chkMisPublicaciones.CheckedChanged += new System.EventHandler(this.chkMisPublicaciones_CheckedChanged);
            // 
            // btnSolicitar
            // 
            this.btnSolicitar.Location = new System.Drawing.Point(653, 447);
            this.btnSolicitar.Name = "btnSolicitar";
            this.btnSolicitar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSolicitar.Size = new System.Drawing.Size(93, 46);
            this.btnSolicitar.TabIndex = 9;
            this.btnSolicitar.Text = "Solicitar compra";
            this.btnSolicitar.UseVisualStyleBackColor = true;
            this.btnSolicitar.Click += new System.EventHandler(this.btnSolicitar_Click);
            // 
            // btnSolicitudes
            // 
            this.btnSolicitudes.Location = new System.Drawing.Point(494, 447);
            this.btnSolicitudes.Name = "btnSolicitudes";
            this.btnSolicitudes.Size = new System.Drawing.Size(93, 46);
            this.btnSolicitudes.TabIndex = 10;
            this.btnSolicitudes.Text = "Mis solicitudes";
            this.btnSolicitudes.UseVisualStyleBackColor = true;
            this.btnSolicitudes.Click += new System.EventHandler(this.btnSolicitudes_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, -79);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(888, 645);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // FrmPrincipal
            // 
            this.AcceptButton = this.btnBuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 542);
            this.Controls.Add(this.btnSolicitudes);
            this.Controls.Add(this.btnSolicitar);
            this.Controls.Add(this.chkMisPublicaciones);
            this.Controls.Add(this.dgvPublicaciones);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UniMarket";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPublicaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblBuscar;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.DataGridView dgvPublicaciones;
        private System.Windows.Forms.CheckBox chkMisPublicaciones;
        private System.Windows.Forms.Button btnSolicitar;
        private System.Windows.Forms.Button btnSolicitudes;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}