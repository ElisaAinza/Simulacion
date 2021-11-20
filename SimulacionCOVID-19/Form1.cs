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
    public partial class Form1 : Form
    {
        bool vacuna; //La utilizaremos para saber si la persona ya recibio la vacuna
        int edad;// Para saber en que rango de edad se encuentra el paciente
        string[] enfermedadesCronicas = new string[] // Este Arreglo lo vamos a utilizar para saber si el paciente cuenta con alguna enfermedad
        //cronica de las que mecionamos, ya que estas son las que llevan a compliaciones al virus 
        {
          "Asma", "Cancer","Enfermedad Pulmonar Constructiva Cronica", "Fibrosis", "Diabetes", "Enfermedades del Corazon", "VID/SIDA", "Presion arterial alta"
        };
        //Variables para sacar el IMC y asi saber para saber que tipo de nutricion que  lleva la persona 
        double altura;
        double peso;
        double IMC;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = @"¿Cómo afecta el Covid-19 al cuerpo humano__Trim.mp4";
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Informacion frm =  new Informacion();
            frm.ShowDialog();


        }
    }
}
