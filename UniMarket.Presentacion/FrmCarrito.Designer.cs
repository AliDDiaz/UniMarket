namespace UniMarket.Presentacion
{
    partial class FrmCarrito
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
            this.lblTituloCarrito = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnConfirmarCompra = new System.Windows.Forms.Button();
            this.btnVaciar = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.flpItemsCarrito = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.panelHeader.Controls.Add(this.lblTituloCarrito);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(747, 68);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTituloCarrito
            // 
            this.lblTituloCarrito.AutoSize = true;
            this.lblTituloCarrito.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloCarrito.ForeColor = System.Drawing.Color.White;
            this.lblTituloCarrito.Location = new System.Drawing.Point(24, 20);
            this.lblTituloCarrito.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloCarrito.Name = "lblTituloCarrito";
            this.lblTituloCarrito.Size = new System.Drawing.Size(257, 28);
            this.lblTituloCarrito.TabIndex = 0;
            this.lblTituloCarrito.Text = "🛒 Tu Carrito de Compras";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.White;
            this.panelFooter.Controls.Add(this.btnConfirmarCompra);
            this.panelFooter.Controls.Add(this.btnVaciar);
            this.panelFooter.Controls.Add(this.lblTotal);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 449);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(747, 105);
            this.panelFooter.TabIndex = 1;
            // 
            // btnConfirmarCompra
            // 
            this.btnConfirmarCompra.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmarCompra.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.btnConfirmarCompra.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirmarCompra.FlatAppearance.BorderSize = 0;
            this.btnConfirmarCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmarCompra.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnConfirmarCompra.ForeColor = System.Drawing.Color.White;
            this.btnConfirmarCompra.Location = new System.Drawing.Point(456, 49);
            this.btnConfirmarCompra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirmarCompra.Name = "btnConfirmarCompra";
            this.btnConfirmarCompra.Size = new System.Drawing.Size(267, 43);
            this.btnConfirmarCompra.TabIndex = 2;
            this.btnConfirmarCompra.Text = "Confirmar solicitudes";
            this.btnConfirmarCompra.UseVisualStyleBackColor = false;
            this.btnConfirmarCompra.Click += new System.EventHandler(this.btnConfirmarCompra_Click_1);
            // 
            // btnVaciar
            // 
            this.btnVaciar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnVaciar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVaciar.FlatAppearance.BorderSize = 0;
            this.btnVaciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVaciar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnVaciar.ForeColor = System.Drawing.Color.White;
            this.btnVaciar.Location = new System.Drawing.Point(24, 52);
            this.btnVaciar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnVaciar.Name = "btnVaciar";
            this.btnVaciar.Size = new System.Drawing.Size(167, 39);
            this.btnVaciar.TabIndex = 1;
            this.btnVaciar.Text = "Vaciar carrito";
            this.btnVaciar.UseVisualStyleBackColor = false;
            this.btnVaciar.Click += new System.EventHandler(this.btnVaciar_Click_1);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.lblTotal.Location = new System.Drawing.Point(24, 15);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(94, 28);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Total: $0";
            // 
            // flpItemsCarrito
            // 
            this.flpItemsCarrito.AutoScroll = true;
            this.flpItemsCarrito.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.flpItemsCarrito.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpItemsCarrito.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpItemsCarrito.Location = new System.Drawing.Point(0, 68);
            this.flpItemsCarrito.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flpItemsCarrito.Name = "flpItemsCarrito";
            this.flpItemsCarrito.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.flpItemsCarrito.Size = new System.Drawing.Size(747, 381);
            this.flpItemsCarrito.TabIndex = 2;
            this.flpItemsCarrito.WrapContents = false;
            // 
            // FrmCarrito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 554);
            this.Controls.Add(this.flpItemsCarrito);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCarrito";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Carrito - Trueque";
            this.Load += new System.EventHandler(this.FrmCarrito_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTituloCarrito;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnConfirmarCompra;
        private System.Windows.Forms.Button btnVaciar;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.FlowLayoutPanel flpItemsCarrito;
    }
}