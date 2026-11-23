namespace UniMarket.Presentacion
{
    partial class FrmPrincipal2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnMycuenta = new System.Windows.Forms.Button();
            this.btnHistorial = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblBadgeBot = new System.Windows.Forms.Label();
            this.btnBot = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblBadgeCarrito = new System.Windows.Forms.Label();
            this.btnCarrito = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelContenedor = new System.Windows.Forms.Panel();
            this.timerBot = new System.Windows.Forms.Timer(this.components);
            this.notifyIconBot = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Indigo;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnSalir);
            this.panel1.Controls.Add(this.btnMycuenta);
            this.panel1.Controls.Add(this.btnHistorial);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 798);
            this.panel1.TabIndex = 0;
            // 
            // btnSalir
            // 
            this.btnSalir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(0, 638);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(201, 54);
            this.btnSalir.TabIndex = 5;
            this.btnSalir.Text = "CERRAR SESIÓN";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnMycuenta
            // 
            this.btnMycuenta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMycuenta.FlatAppearance.BorderSize = 0;
            this.btnMycuenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMycuenta.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMycuenta.ForeColor = System.Drawing.Color.White;
            this.btnMycuenta.Image = ((System.Drawing.Image)(resources.GetObject("btnMycuenta.Image")));
            this.btnMycuenta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMycuenta.Location = new System.Drawing.Point(-4, 524);
            this.btnMycuenta.Name = "btnMycuenta";
            this.btnMycuenta.Size = new System.Drawing.Size(212, 54);
            this.btnMycuenta.TabIndex = 4;
            this.btnMycuenta.Text = "    MI CUENTA";
            this.btnMycuenta.UseVisualStyleBackColor = true;
            this.btnMycuenta.Click += new System.EventHandler(this.btnMycuenta_Click);
            // 
            // btnHistorial
            // 
            this.btnHistorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistorial.FlatAppearance.BorderSize = 0;
            this.btnHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorial.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorial.ForeColor = System.Drawing.Color.White;
            this.btnHistorial.Image = ((System.Drawing.Image)(resources.GetObject("btnHistorial.Image")));
            this.btnHistorial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistorial.Location = new System.Drawing.Point(-5, 406);
            this.btnHistorial.Name = "btnHistorial";
            this.btnHistorial.Size = new System.Drawing.Size(246, 65);
            this.btnHistorial.TabIndex = 2;
            this.btnHistorial.Text = "    MIS SOLICITUDES";
            this.btnHistorial.UseVisualStyleBackColor = true;
            this.btnHistorial.Click += new System.EventHandler(this.btnHistorial_Click);
            // 
            // button2
            // 
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(0, 323);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(208, 54);
            this.button2.TabIndex = 2;
            this.button2.Text = "    PUBLICAR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Indigo;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(-3, 217);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 54);
            this.button1.TabIndex = 1;
            this.button1.Text = "INICIO";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblBadgeBot
            // 
            this.lblBadgeBot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblBadgeBot.Font = new System.Drawing.Font("Segoe UI Light", 8.5F);
            this.lblBadgeBot.ForeColor = System.Drawing.Color.White;
            this.lblBadgeBot.Location = new System.Drawing.Point(1113, 32);
            this.lblBadgeBot.Name = "lblBadgeBot";
            this.lblBadgeBot.Size = new System.Drawing.Size(22, 22);
            this.lblBadgeBot.TabIndex = 7;
            this.lblBadgeBot.Text = "1";
            this.lblBadgeBot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBadgeBot.Visible = false;
            // 
            // btnBot
            // 
            this.btnBot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBot.FlatAppearance.BorderSize = 0;
            this.btnBot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBot.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBot.ForeColor = System.Drawing.Color.White;
            this.btnBot.Location = new System.Drawing.Point(1068, 33);
            this.btnBot.Name = "btnBot";
            this.btnBot.Size = new System.Drawing.Size(56, 51);
            this.btnBot.TabIndex = 6;
            this.btnBot.Text = "🔔";
            this.btnBot.UseVisualStyleBackColor = true;
            this.btnBot.Click += new System.EventHandler(this.btnBot_Click_1);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Indigo;
            this.panel2.Controls.Add(this.lblBadgeCarrito);
            this.panel2.Controls.Add(this.btnCarrito);
            this.panel2.Controls.Add(this.lblBadgeBot);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnBot);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(225, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1300, 112);
            this.panel2.TabIndex = 1;
            // 
            // lblBadgeCarrito
            // 
            this.lblBadgeCarrito.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.lblBadgeCarrito.Font = new System.Drawing.Font("Segoe UI Light", 8.5F);
            this.lblBadgeCarrito.ForeColor = System.Drawing.Color.White;
            this.lblBadgeCarrito.Location = new System.Drawing.Point(1204, 34);
            this.lblBadgeCarrito.Name = "lblBadgeCarrito";
            this.lblBadgeCarrito.Size = new System.Drawing.Size(22, 22);
            this.lblBadgeCarrito.TabIndex = 8;
            this.lblBadgeCarrito.Text = "0";
            this.lblBadgeCarrito.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBadgeCarrito.Visible = false;
            // 
            // btnCarrito
            // 
            this.btnCarrito.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCarrito.FlatAppearance.BorderSize = 0;
            this.btnCarrito.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCarrito.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCarrito.ForeColor = System.Drawing.Color.White;
            this.btnCarrito.Location = new System.Drawing.Point(1164, 38);
            this.btnCarrito.Name = "btnCarrito";
            this.btnCarrito.Size = new System.Drawing.Size(51, 51);
            this.btnCarrito.TabIndex = 7;
            this.btnCarrito.Text = "🛒";
            this.btnCarrito.UseVisualStyleBackColor = true;
            this.btnCarrito.Click += new System.EventHandler(this.btnCarrito_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(215)))), ((int)(((byte)(250)))));
            this.label3.Location = new System.Drawing.Point(526, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 28);
            this.label3.TabIndex = 1;
            this.label3.Text = "Tu tienda de confianza";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(215)))), ((int)(((byte)(250)))));
            this.label5.Location = new System.Drawing.Point(760, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 28);
            this.label5.TabIndex = 3;
            this.label5.Text = "---------";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(215)))), ((int)(((byte)(250)))));
            this.label4.Location = new System.Drawing.Point(436, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 28);
            this.label4.TabIndex = 2;
            this.label4.Text = "---------";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(504, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(304, 81);
            this.label2.TabIndex = 0;
            this.label2.Text = "TRUEQUE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Indigo;
            this.flowLayoutPanel1.Controls.Add(this.panelContenedor);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(225, 112);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(24, 8, 4, 8);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1300, 686);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // panelContenedor
            // 
            this.panelContenedor.BackColor = System.Drawing.Color.Lavender;
            this.panelContenedor.Location = new System.Drawing.Point(27, 11);
            this.panelContenedor.Name = "panelContenedor";
            this.panelContenedor.Size = new System.Drawing.Size(1282, 675);
            this.panelContenedor.TabIndex = 0;
            // 
            // timerBot
            // 
            this.timerBot.Enabled = true;
            this.timerBot.Interval = 8000;
            this.timerBot.Tick += new System.EventHandler(this.timerBot_Tick);
            // 
            // notifyIconBot
            // 
            this.notifyIconBot.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconBot.Icon")));
            this.notifyIconBot.Text = "notifyIcon1";
            this.notifyIconBot.Visible = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 188);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // FrmPrincipal2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1525, 798);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmPrincipal2";
            this.Text = "UniMarket";
            this.Load += new System.EventHandler(this.FrmPrincipal2_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnMycuenta;
        private System.Windows.Forms.Button btnHistorial;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelContenedor;
        private System.Windows.Forms.Timer timerBot;
        private System.Windows.Forms.NotifyIcon notifyIconBot;
        private System.Windows.Forms.Button btnBot;
        private System.Windows.Forms.Label lblBadgeBot;
        private System.Windows.Forms.Button btnCarrito;
        private System.Windows.Forms.Label lblBadgeCarrito;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}