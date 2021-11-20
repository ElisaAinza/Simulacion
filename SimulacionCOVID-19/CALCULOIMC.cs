using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulacionCOVID_19
{
    public partial class CALCULOIMC : Form
    {
        public CALCULOIMC()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                var peso = Convert.ToDouble(txtPeso.text);
                //metros
                var estatura = Convert.ToDouble(txtEstatura.text);
                var IMC = peso / (estatura * estatura);
                if (IMC < 18.5)
                {
                    lblResultado.Text = " BAJO PESO";
                    pictureBox2.Image = Properties.Resources.bajopeso;
                }
                else if (IMC >= 18.5 && IMC < 25)
                {
                    lblResultado.Text = " PESO NORMAL";
                    pictureBox2.Image = Properties.Resources.pesonormal;
                }
                else if (IMC >= 25 && IMC < 30)
                {
                    lblResultado.Text = " SOBREPESO";
                    pictureBox2.Image = Properties.Resources.sobrepeso;
                }
                else if (IMC >= 30)
                {
                    lblResultado.Text = " OBESIDAD";
                    pictureBox2.Image = Properties.Resources.obesidad;
                }
            }
            catch (Exception)
            {

                
            }
           
        }
    }
}
