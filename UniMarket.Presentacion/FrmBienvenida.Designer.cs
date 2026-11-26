namespace UniMarket.Presentacion
{
    partial class FrmBienvenida
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBienvenida));
            this.picBienvenida = new System.Windows.Forms.PictureBox();
            this.btnComenzar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.timerBienvenida = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBienvenida)).BeginInit();
            this.SuspendLayout();
            // 
            // picBienvenida
            // 
            this.picBienvenida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBienvenida.Image = ((System.Drawing.Image)(resources.GetObject("picBienvenida.Image")));
            this.picBienvenida.Location = new System.Drawing.Point(0, 0);
            this.picBienvenida.Name = "picBienvenida";
            this.picBienvenida.Size = new System.Drawing.Size(732, 453);
            this.picBienvenida.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBienvenida.TabIndex = 0;
            this.picBienvenida.TabStop = false;
            // 
            // btnComenzar
            // 
            this.btnComenzar.BackColor = System.Drawing.Color.Transparent;
            this.btnComenzar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComenzar.FlatAppearance.BorderSize = 0;
            this.btnComenzar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnComenzar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnComenzar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComenzar.Location = new System.Drawing.Point(419, 353);
            this.btnComenzar.Name = "btnComenzar";
            this.btnComenzar.Size = new System.Drawing.Size(245, 34);
            this.btnComenzar.TabIndex = 1;
            this.btnComenzar.UseVisualStyleBackColor = false;
            this.btnComenzar.Click += new System.EventHandler(this.btnComenzar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Location = new System.Drawing.Point(681, 22);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(29, 23);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // timerBienvenida
            // 
            this.timerBienvenida.Interval = 20;
            this.timerBienvenida.Tick += new System.EventHandler(this.timerBienvenida_Tick);
            // 
            // FrmBienvenida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(732, 453);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnComenzar);
            this.Controls.Add(this.picBienvenida);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBienvenida";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bienvenida";
            ((System.ComponentModel.ISupportInitialize)(this.picBienvenida)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBienvenida;
        private System.Windows.Forms.Button btnComenzar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Timer timerBienvenida;
    }
}