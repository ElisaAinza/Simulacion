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
    public partial class Informacion : Form
    {
       
        public Informacion()
        {
            InitializeComponent();
        }

        private void Informacion_Load(object sender, EventArgs e)
        {

        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImc_Click(object sender, EventArgs e)
        {
            CALCULOIMC frm = new CALCULOIMC();
            frm.ShowDialog();

        }

        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            if (switchVacuna.Value || switchNovacuna.Value)
            {
                if (checkFemenino.Checked || checkMasculino.Checked)
                {
                    if (Convert.ToInt32(txtMinimo.Text)>= 18 && Convert.ToInt32(txtMaximo.Text)<=85)
                    {
                        Simulacion frm1 = new Simulacion();
                        try
                        {
                            frm1.rangoMinimino = Convert.ToInt32(txtMinimo.Text);
                            frm1.rangoMaximo = Convert.ToInt32(txtMaximo.Text);
                            frm1.Femenino = checkFemenino.Checked;
                            frm1.Masculino = checkMasculino.Checked;
                            frm1.Diabetes = switchdiabetes.Value;
                            frm1.Hipertension = switchHipertension.Value;
                            frm1.Obesidad = switchObesidad.Value;
                            frm1.Vacunado = switchVacuna.Value;
                            frm1.noVacunado = switchNovacuna.Value;
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Llenar los campos de rango minimo y maximo");
                        }

                        frm1.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Rango menor permitido: 5 Rango mayor permitido: 85");
                    }
                   
                }
                else
                {
                    MessageBox.Show("Seleccione las variantes del Genero");
                }
                
            }
            else
            {
                MessageBox.Show("Seleccione las variantes de vacunacion ");
            }
           
           
        }

    }
}
