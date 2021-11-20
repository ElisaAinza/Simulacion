
namespace SimulacionCOVID_19
{
    partial class Simulacion
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
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tiempo = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTiempo = new System.Windows.Forms.Label();
            this.btnIniciarSimulacion = new ns1.BunifuFlatButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar1.Location = new System.Drawing.Point(955, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(19, 412);
            this.vScrollBar1.TabIndex = 0;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(19)))), ((int)(((byte)(67)))));
            this.panel1.Location = new System.Drawing.Point(12, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(940, 408);
            this.panel1.TabIndex = 1;
            // 
            // tiempo
            // 
            this.tiempo.Interval = 1000;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SlateGray;
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblTiempo);
            this.panel2.Controls.Add(this.btnIniciarSimulacion);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(974, 45);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(636, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tiempo Transcurrido:";
            // 
            // lblTiempo
            // 
            this.lblTiempo.AutoSize = true;
            this.lblTiempo.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTiempo.Location = new System.Drawing.Point(832, 16);
            this.lblTiempo.Name = "lblTiempo";
            this.lblTiempo.Size = new System.Drawing.Size(73, 16);
            this.lblTiempo.TabIndex = 1;
            this.lblTiempo.Text = "00:00:00:00";
            // 
            // btnIniciarSimulacion
            // 
            this.btnIniciarSimulacion.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnIniciarSimulacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnIniciarSimulacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIniciarSimulacion.BorderRadius = 0;
            this.btnIniciarSimulacion.ButtonText = "Iniciar";
            this.btnIniciarSimulacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIniciarSimulacion.DisabledColor = System.Drawing.Color.Gray;
            this.btnIniciarSimulacion.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnIniciarSimulacion.Iconcolor = System.Drawing.Color.Transparent;
            this.btnIniciarSimulacion.Iconimage = global::SimulacionCOVID_19.Properties.Resources.tocar;
            this.btnIniciarSimulacion.Iconimage_right = null;
            this.btnIniciarSimulacion.Iconimage_right_Selected = null;
            this.btnIniciarSimulacion.Iconimage_Selected = null;
            this.btnIniciarSimulacion.IconMarginLeft = 0;
            this.btnIniciarSimulacion.IconMarginRight = 0;
            this.btnIniciarSimulacion.IconRightVisible = true;
            this.btnIniciarSimulacion.IconRightZoom = 0D;
            this.btnIniciarSimulacion.IconVisible = true;
            this.btnIniciarSimulacion.IconZoom = 50D;
            this.btnIniciarSimulacion.IsTab = false;
            this.btnIniciarSimulacion.Location = new System.Drawing.Point(0, 0);
            this.btnIniciarSimulacion.Name = "btnIniciarSimulacion";
            this.btnIniciarSimulacion.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnIniciarSimulacion.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.btnIniciarSimulacion.OnHoverTextColor = System.Drawing.Color.White;
            this.btnIniciarSimulacion.selected = false;
            this.btnIniciarSimulacion.Size = new System.Drawing.Size(227, 45);
            this.btnIniciarSimulacion.TabIndex = 0;
            this.btnIniciarSimulacion.Text = "Iniciar";
            this.btnIniciarSimulacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIniciarSimulacion.Textcolor = System.Drawing.Color.White;
            this.btnIniciarSimulacion.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarSimulacion.Click += new System.EventHandler(this.btnIniciarSimulacion_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(19)))), ((int)(((byte)(67)))));
            this.panel3.Controls.Add(this.vScrollBar1);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(974, 412);
            this.panel3.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Teal;
            this.button1.Location = new System.Drawing.Point(233, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 45);
            this.button1.TabIndex = 4;
            this.button1.Text = "INICIAR TRATAMIENTO";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // Simulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 457);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "Simulacion";
            this.Text = "Simulacion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Simulacion_FormClosing);
            this.Load += new System.EventHandler(this.Simulacion_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer tiempo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private ns1.BunifuFlatButton btnIniciarSimulacion;
        private System.Windows.Forms.Label lblTiempo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}