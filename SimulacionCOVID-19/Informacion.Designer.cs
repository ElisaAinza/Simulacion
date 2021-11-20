
namespace SimulacionCOVID_19
{
    partial class Informacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Informacion));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.switchObesidad = new ns1.BunifuiOSSwitch();
            this.switchHipertension = new ns1.BunifuiOSSwitch();
            this.switchdiabetes = new ns1.BunifuiOSSwitch();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSiVacuna = new System.Windows.Forms.Label();
            this.LBLNoVacuna = new System.Windows.Forms.Label();
            this.lblPreguntaVacuna = new System.Windows.Forms.Label();
            this.switchNovacuna = new ns1.BunifuiOSSwitch();
            this.switchVacuna = new ns1.BunifuiOSSwitch();
            this.label1 = new System.Windows.Forms.Label();
            this.checkMasculino = new System.Windows.Forms.CheckBox();
            this.checkFemenino = new System.Windows.Forms.CheckBox();
            this.lblRangoEdad = new ns1.BunifuCustomLabel();
            this.bunifuElipse1 = new ns1.BunifuElipse(this.components);
            this.bunifuDragControl1 = new ns1.BunifuDragControl(this.components);
            this.btnclose = new System.Windows.Forms.Button();
            this.bunifuElipse2 = new ns1.BunifuElipse(this.components);
            this.btnEmpezar = new ns1.BunifuFlatButton();
            this.txtMinimo = new ns1.BunifuMaterialTextbox();
            this.txtMaximo = new ns1.BunifuMaterialTextbox();
            this.btnImc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SimulacionCOVID_19.Properties.Resources.giphy;
            this.pictureBox1.Location = new System.Drawing.Point(0, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(435, 411);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // switchObesidad
            // 
            this.switchObesidad.BackColor = System.Drawing.Color.Transparent;
            this.switchObesidad.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchObesidad.BackgroundImage")));
            this.switchObesidad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchObesidad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchObesidad.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.switchObesidad.Location = new System.Drawing.Point(243, 252);
            this.switchObesidad.Name = "switchObesidad";
            this.switchObesidad.OffColor = System.Drawing.Color.Gray;
            this.switchObesidad.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchObesidad.Size = new System.Drawing.Size(43, 25);
            this.switchObesidad.TabIndex = 35;
            this.switchObesidad.Value = false;
            // 
            // switchHipertension
            // 
            this.switchHipertension.BackColor = System.Drawing.Color.Transparent;
            this.switchHipertension.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchHipertension.BackgroundImage")));
            this.switchHipertension.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchHipertension.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchHipertension.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.switchHipertension.Location = new System.Drawing.Point(243, 196);
            this.switchHipertension.Name = "switchHipertension";
            this.switchHipertension.OffColor = System.Drawing.Color.Gray;
            this.switchHipertension.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchHipertension.Size = new System.Drawing.Size(43, 25);
            this.switchHipertension.TabIndex = 34;
            this.switchHipertension.Value = false;
            // 
            // switchdiabetes
            // 
            this.switchdiabetes.BackColor = System.Drawing.Color.Transparent;
            this.switchdiabetes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchdiabetes.BackgroundImage")));
            this.switchdiabetes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchdiabetes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchdiabetes.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.switchdiabetes.Location = new System.Drawing.Point(243, 165);
            this.switchdiabetes.Name = "switchdiabetes";
            this.switchdiabetes.OffColor = System.Drawing.Color.Gray;
            this.switchdiabetes.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchdiabetes.Size = new System.Drawing.Size(43, 25);
            this.switchdiabetes.TabIndex = 33;
            this.switchdiabetes.Value = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Sitka Banner", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(117, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 18);
            this.label5.TabIndex = 32;
            this.label5.Text = "OBESIDAD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Sitka Banner", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(117, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "HIPERTENSIÓN";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Sitka Banner", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(117, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 18);
            this.label3.TabIndex = 30;
            this.label3.Text = "DIABETES";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(116, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 21);
            this.label2.TabIndex = 29;
            this.label2.Text = "¿ENFERMEDAD CRÓNICA?";
            // 
            // lblSiVacuna
            // 
            this.lblSiVacuna.AutoSize = true;
            this.lblSiVacuna.BackColor = System.Drawing.Color.Transparent;
            this.lblSiVacuna.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblSiVacuna.Location = new System.Drawing.Point(119, 330);
            this.lblSiVacuna.Name = "lblSiVacuna";
            this.lblSiVacuna.Size = new System.Drawing.Size(17, 13);
            this.lblSiVacuna.TabIndex = 28;
            this.lblSiVacuna.Text = "SI";
            // 
            // LBLNoVacuna
            // 
            this.LBLNoVacuna.AutoSize = true;
            this.LBLNoVacuna.BackColor = System.Drawing.Color.Transparent;
            this.LBLNoVacuna.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LBLNoVacuna.Location = new System.Drawing.Point(287, 330);
            this.LBLNoVacuna.Name = "LBLNoVacuna";
            this.LBLNoVacuna.Size = new System.Drawing.Size(23, 13);
            this.LBLNoVacuna.TabIndex = 27;
            this.LBLNoVacuna.Text = "NO";
            // 
            // lblPreguntaVacuna
            // 
            this.lblPreguntaVacuna.AutoSize = true;
            this.lblPreguntaVacuna.BackColor = System.Drawing.Color.Transparent;
            this.lblPreguntaVacuna.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPreguntaVacuna.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblPreguntaVacuna.Location = new System.Drawing.Point(133, 277);
            this.lblPreguntaVacuna.Name = "lblPreguntaVacuna";
            this.lblPreguntaVacuna.Size = new System.Drawing.Size(166, 21);
            this.lblPreguntaVacuna.TabIndex = 26;
            this.lblPreguntaVacuna.Text = "¿SE HA VACUNADO?";
            // 
            // switchNovacuna
            // 
            this.switchNovacuna.BackColor = System.Drawing.Color.Transparent;
            this.switchNovacuna.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchNovacuna.BackgroundImage")));
            this.switchNovacuna.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchNovacuna.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchNovacuna.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.switchNovacuna.Location = new System.Drawing.Point(224, 318);
            this.switchNovacuna.Name = "switchNovacuna";
            this.switchNovacuna.OffColor = System.Drawing.Color.Gray;
            this.switchNovacuna.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchNovacuna.Size = new System.Drawing.Size(43, 25);
            this.switchNovacuna.TabIndex = 25;
            this.switchNovacuna.Value = false;
            // 
            // switchVacuna
            // 
            this.switchVacuna.BackColor = System.Drawing.Color.Transparent;
            this.switchVacuna.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("switchVacuna.BackgroundImage")));
            this.switchVacuna.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.switchVacuna.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchVacuna.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.switchVacuna.Location = new System.Drawing.Point(160, 318);
            this.switchVacuna.Name = "switchVacuna";
            this.switchVacuna.OffColor = System.Drawing.Color.Gray;
            this.switchVacuna.OnColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(202)))), ((int)(((byte)(94)))));
            this.switchVacuna.Size = new System.Drawing.Size(43, 25);
            this.switchVacuna.TabIndex = 24;
            this.switchVacuna.Value = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Sitka Small", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(158, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 31);
            this.label1.TabIndex = 23;
            this.label1.Text = "GÉNERO";
            // 
            // checkMasculino
            // 
            this.checkMasculino.AutoSize = true;
            this.checkMasculino.BackColor = System.Drawing.Color.Transparent;
            this.checkMasculino.Font = new System.Drawing.Font("Sitka Display", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkMasculino.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkMasculino.Location = new System.Drawing.Point(224, 109);
            this.checkMasculino.Name = "checkMasculino";
            this.checkMasculino.Size = new System.Drawing.Size(110, 27);
            this.checkMasculino.TabIndex = 22;
            this.checkMasculino.Text = "MASCULINO";
            this.checkMasculino.UseVisualStyleBackColor = false;
            // 
            // checkFemenino
            // 
            this.checkFemenino.AutoSize = true;
            this.checkFemenino.BackColor = System.Drawing.Color.Transparent;
            this.checkFemenino.Font = new System.Drawing.Font("Sitka Display", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkFemenino.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkFemenino.Location = new System.Drawing.Point(101, 109);
            this.checkFemenino.Name = "checkFemenino";
            this.checkFemenino.Size = new System.Drawing.Size(102, 27);
            this.checkFemenino.TabIndex = 21;
            this.checkFemenino.Text = "FEMENINO";
            this.checkFemenino.UseVisualStyleBackColor = false;
            // 
            // lblRangoEdad
            // 
            this.lblRangoEdad.AutoSize = true;
            this.lblRangoEdad.BackColor = System.Drawing.Color.Transparent;
            this.lblRangoEdad.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRangoEdad.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblRangoEdad.Location = new System.Drawing.Point(156, 19);
            this.lblRangoEdad.Name = "lblRangoEdad";
            this.lblRangoEdad.Size = new System.Drawing.Size(139, 21);
            this.lblRangoEdad.TabIndex = 20;
            this.lblRangoEdad.Text = "RANGO DE EDAD";
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 80;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this.pictureBox1;
            this.bunifuDragControl1.Vertical = true;
            // 
            // btnclose
            // 
            this.btnclose.BackColor = System.Drawing.Color.Red;
            this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnclose.ForeColor = System.Drawing.Color.Red;
            this.btnclose.Location = new System.Drawing.Point(396, 19);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(23, 23);
            this.btnclose.TabIndex = 36;
            this.btnclose.UseVisualStyleBackColor = false;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // bunifuElipse2
            // 
            this.bunifuElipse2.ElipseRadius = 80;
            this.bunifuElipse2.TargetControl = this.btnclose;
            // 
            // btnEmpezar
            // 
            this.btnEmpezar.Activecolor = System.Drawing.Color.Blue;
            this.btnEmpezar.BackColor = System.Drawing.Color.Transparent;
            this.btnEmpezar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEmpezar.BorderRadius = 0;
            this.btnEmpezar.ButtonText = "EMPEZAR";
            this.btnEmpezar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmpezar.DisabledColor = System.Drawing.Color.Gray;
            this.btnEmpezar.Font = new System.Drawing.Font("Palatino Linotype", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpezar.Iconcolor = System.Drawing.Color.Transparent;
            this.btnEmpezar.Iconimage = global::SimulacionCOVID_19.Properties.Resources.tocar;
            this.btnEmpezar.Iconimage_right = null;
            this.btnEmpezar.Iconimage_right_Selected = null;
            this.btnEmpezar.Iconimage_Selected = null;
            this.btnEmpezar.IconMarginLeft = 0;
            this.btnEmpezar.IconMarginRight = 0;
            this.btnEmpezar.IconRightVisible = true;
            this.btnEmpezar.IconRightZoom = 0D;
            this.btnEmpezar.IconVisible = true;
            this.btnEmpezar.IconZoom = 80D;
            this.btnEmpezar.IsTab = false;
            this.btnEmpezar.Location = new System.Drawing.Point(137, 361);
            this.btnEmpezar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEmpezar.Name = "btnEmpezar";
            this.btnEmpezar.Normalcolor = System.Drawing.Color.Transparent;
            this.btnEmpezar.OnHovercolor = System.Drawing.Color.WhiteSmoke;
            this.btnEmpezar.OnHoverTextColor = System.Drawing.Color.Black;
            this.btnEmpezar.selected = false;
            this.btnEmpezar.Size = new System.Drawing.Size(149, 37);
            this.btnEmpezar.TabIndex = 37;
            this.btnEmpezar.Text = "EMPEZAR";
            this.btnEmpezar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEmpezar.Textcolor = System.Drawing.Color.White;
            this.btnEmpezar.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmpezar.Click += new System.EventHandler(this.btnEmpezar_Click);
            // 
            // txtMinimo
            // 
            this.txtMinimo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(19)))), ((int)(((byte)(67)))));
            this.txtMinimo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMinimo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtMinimo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtMinimo.HintForeColor = System.Drawing.Color.White;
            this.txtMinimo.HintText = "Rango Minimo";
            this.txtMinimo.isPassword = false;
            this.txtMinimo.LineFocusedColor = System.Drawing.Color.Blue;
            this.txtMinimo.LineIdleColor = System.Drawing.Color.Gray;
            this.txtMinimo.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.txtMinimo.LineThickness = 3;
            this.txtMinimo.Location = new System.Drawing.Point(42, 52);
            this.txtMinimo.Margin = new System.Windows.Forms.Padding(4);
            this.txtMinimo.Name = "txtMinimo";
            this.txtMinimo.Size = new System.Drawing.Size(149, 28);
            this.txtMinimo.TabIndex = 38;
            this.txtMinimo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // txtMaximo
            // 
            this.txtMaximo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(19)))), ((int)(((byte)(67)))));
            this.txtMaximo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaximo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtMaximo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtMaximo.HintForeColor = System.Drawing.Color.White;
            this.txtMaximo.HintText = "Rango Maximo";
            this.txtMaximo.isPassword = false;
            this.txtMaximo.LineFocusedColor = System.Drawing.Color.Blue;
            this.txtMaximo.LineIdleColor = System.Drawing.Color.Gray;
            this.txtMaximo.LineMouseHoverColor = System.Drawing.Color.Blue;
            this.txtMaximo.LineThickness = 3;
            this.txtMaximo.Location = new System.Drawing.Point(224, 52);
            this.txtMaximo.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaximo.Name = "txtMaximo";
            this.txtMaximo.Size = new System.Drawing.Size(174, 28);
            this.txtMaximo.TabIndex = 39;
            this.txtMaximo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // btnImc
            // 
            this.btnImc.BackColor = System.Drawing.Color.Transparent;
            this.btnImc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImc.Font = new System.Drawing.Font("MS PGothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImc.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnImc.Location = new System.Drawing.Point(164, 224);
            this.btnImc.Name = "btnImc";
            this.btnImc.Size = new System.Drawing.Size(75, 23);
            this.btnImc.TabIndex = 40;
            this.btnImc.Text = "¿IMC?";
            this.btnImc.UseVisualStyleBackColor = false;
            this.btnImc.Click += new System.EventHandler(this.btnImc_Click);
            // 
            // Informacion
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(174)))), ((int)(((byte)(254)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(435, 405);
            this.Controls.Add(this.btnImc);
            this.Controls.Add(this.txtMaximo);
            this.Controls.Add(this.txtMinimo);
            this.Controls.Add(this.btnEmpezar);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.switchObesidad);
            this.Controls.Add(this.switchHipertension);
            this.Controls.Add(this.switchdiabetes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSiVacuna);
            this.Controls.Add(this.LBLNoVacuna);
            this.Controls.Add(this.lblPreguntaVacuna);
            this.Controls.Add(this.switchNovacuna);
            this.Controls.Add(this.switchVacuna);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkMasculino);
            this.Controls.Add(this.checkFemenino);
            this.Controls.Add(this.lblRangoEdad);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Informacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Informacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private ns1.BunifuiOSSwitch switchObesidad;
        private ns1.BunifuiOSSwitch switchHipertension;
        private ns1.BunifuiOSSwitch switchdiabetes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSiVacuna;
        private System.Windows.Forms.Label LBLNoVacuna;
        private System.Windows.Forms.Label lblPreguntaVacuna;
        private ns1.BunifuiOSSwitch switchNovacuna;
        private ns1.BunifuiOSSwitch switchVacuna;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkMasculino;
        private System.Windows.Forms.CheckBox checkFemenino;
        private ns1.BunifuCustomLabel lblRangoEdad;
        private ns1.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.Button btnclose;
        private ns1.BunifuDragControl bunifuDragControl1;
        private ns1.BunifuElipse bunifuElipse2;
        private ns1.BunifuMaterialTextbox txtMaximo;
        private ns1.BunifuMaterialTextbox txtMinimo;
        private ns1.BunifuFlatButton btnEmpezar;
        private System.Windows.Forms.Button btnImc;
    }
}