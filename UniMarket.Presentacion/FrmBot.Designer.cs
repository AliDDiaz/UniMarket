namespace UniMarket.Presentacion
{
    partial class FrmBot
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
            this.lblTituloBot = new System.Windows.Forms.Label();
            this.pbAvatarBot = new System.Windows.Forms.PictureBox();
            this.flpMensajes = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatarBot)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(26)))), ((int)(((byte)(94)))));
            this.panelHeader.Controls.Add(this.btnActualizar);
            this.panelHeader.Controls.Add(this.lblTituloBot);
            this.panelHeader.Controls.Add(this.pbAvatarBot);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1013, 80);
            this.panelHeader.TabIndex = 0;
            // 
            // btnActualizar
            // 
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(77)))), ((int)(((byte)(255)))));
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Location = new System.Drawing.Point(843, 18);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(128, 43);
            this.btnActualizar.TabIndex = 2;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // lblTituloBot
            // 
            this.lblTituloBot.AutoSize = true;
            this.lblTituloBot.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloBot.ForeColor = System.Drawing.Color.White;
            this.lblTituloBot.Location = new System.Drawing.Point(96, 26);
            this.lblTituloBot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloBot.Name = "lblTituloBot";
            this.lblTituloBot.Size = new System.Drawing.Size(394, 28);
            this.lblTituloBot.TabIndex = 1;
            this.lblTituloBot.Text = "Trueque Bot — Centro de Notificaciones";
            // 
            // pbAvatarBot
            // 
            this.pbAvatarBot.Location = new System.Drawing.Point(20, 12);
            this.pbAvatarBot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbAvatarBot.Name = "pbAvatarBot";
            this.pbAvatarBot.Size = new System.Drawing.Size(60, 55);
            this.pbAvatarBot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAvatarBot.TabIndex = 0;
            this.pbAvatarBot.TabStop = false;
            // 
            // flpMensajes
            // 
            this.flpMensajes.AutoScroll = true;
            this.flpMensajes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.flpMensajes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMensajes.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMensajes.Location = new System.Drawing.Point(0, 80);
            this.flpMensajes.Margin = new System.Windows.Forms.Padding(0);
            this.flpMensajes.Name = "flpMensajes";
            this.flpMensajes.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.flpMensajes.Size = new System.Drawing.Size(1013, 535);
            this.flpMensajes.TabIndex = 1;
            this.flpMensajes.WrapContents = false;
            this.flpMensajes.SizeChanged += new System.EventHandler(this.flpMensajes_SizeChanged);
            // 
            // FrmBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1013, 615);
            this.Controls.Add(this.flpMensajes);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FrmBot";
            this.Text = "Trueque Bot";
            this.Load += new System.EventHandler(this.FrmBot_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatarBot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Label lblTituloBot;
        private System.Windows.Forms.PictureBox pbAvatarBot;
        private System.Windows.Forms.FlowLayoutPanel flpMensajes;
    }
}