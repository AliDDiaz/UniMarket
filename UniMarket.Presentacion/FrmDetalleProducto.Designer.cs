namespace UniMarket.Presentacion
{
    partial class FrmDetalleProducto
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
            this.picProducto = new System.Windows.Forms.PictureBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblTrueque = new System.Windows.Forms.Label();
            this.txtTrueque = new System.Windows.Forms.TextBox();
            this.btnProponerTrueque = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picProducto)).BeginInit();
            this.SuspendLayout();
            // 
            // picProducto
            // 
            this.picProducto.Location = new System.Drawing.Point(25, 61);
            this.picProducto.Name = "picProducto";
            this.picProducto.Size = new System.Drawing.Size(243, 284);
            this.picProducto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProducto.TabIndex = 0;
            this.picProducto.TabStop = false;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Indigo;
            this.lblTitulo.Location = new System.Drawing.Point(274, 44);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(216, 28);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Nombre del producto";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecio.ForeColor = System.Drawing.Color.Magenta;
            this.lblPrecio.Location = new System.Drawing.Point(317, 72);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(30, 23);
            this.lblPrecio.TabIndex = 2;
            this.lblPrecio.Text = "$0";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(305, 138);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(103, 23);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripcion";
            // 
            // lblCantidad
            // 
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidad.ForeColor = System.Drawing.Color.DarkMagenta;
            this.lblCantidad.Location = new System.Drawing.Point(305, 288);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(123, 23);
            this.lblCantidad.TabIndex = 4;
            this.lblCantidad.Text = "Disponibles: 0";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.BackColor = System.Drawing.Color.White;
            this.txtDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescripcion.Location = new System.Drawing.Point(309, 164);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ReadOnly = true;
            this.txtDescripcion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescripcion.Size = new System.Drawing.Size(302, 109);
            this.txtDescripcion.TabIndex = 5;
            this.txtDescripcion.TabStop = false;
            // 
            // lblTrueque
            // 
            this.lblTrueque.AutoSize = true;
            this.lblTrueque.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTrueque.ForeColor = System.Drawing.Color.Indigo;
            this.lblTrueque.Location = new System.Drawing.Point(305, 318);
            this.lblTrueque.Name = "lblTrueque";
            this.lblTrueque.Size = new System.Drawing.Size(270, 23);
            this.lblTrueque.TabIndex = 6;
            this.lblTrueque.Text = "El vendedor acepta trueque por:";
            // 
            // txtTrueque
            // 
            this.txtTrueque.BackColor = System.Drawing.Color.White;
            this.txtTrueque.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTrueque.Location = new System.Drawing.Point(309, 346);
            this.txtTrueque.Multiline = true;
            this.txtTrueque.Name = "txtTrueque";
            this.txtTrueque.ReadOnly = true;
            this.txtTrueque.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTrueque.Size = new System.Drawing.Size(302, 56);
            this.txtTrueque.TabIndex = 7;
            this.txtTrueque.TabStop = false;
            // 
            // btnProponerTrueque
            // 
            this.btnProponerTrueque.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.btnProponerTrueque.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProponerTrueque.FlatAppearance.BorderSize = 0;
            this.btnProponerTrueque.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProponerTrueque.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnProponerTrueque.ForeColor = System.Drawing.Color.White;
            this.btnProponerTrueque.Location = new System.Drawing.Point(309, 414);
            this.btnProponerTrueque.Name = "btnProponerTrueque";
            this.btnProponerTrueque.Size = new System.Drawing.Size(200, 38);
            this.btnProponerTrueque.TabIndex = 8;
            this.btnProponerTrueque.Text = "Proponer trueque";
            this.btnProponerTrueque.UseVisualStyleBackColor = false;
            this.btnProponerTrueque.Click += new System.EventHandler(this.btnProponerTrueque_Click);
            // 
            // FrmDetalleProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(678, 463);
            this.Controls.Add(this.btnProponerTrueque);
            this.Controls.Add(this.txtTrueque);
            this.Controls.Add(this.lblTrueque);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.picProducto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDetalleProducto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detalles del producto";
            this.Load += new System.EventHandler(this.FrmDetalleProducto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picProducto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picProducto;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblTrueque;
        private System.Windows.Forms.TextBox txtTrueque;
        private System.Windows.Forms.Button btnProponerTrueque;
    }
}