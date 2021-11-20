using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization;
using System.Diagnostics;

namespace SimulacionCOVID_19
{
    public partial class Simulacion : Form
    {
        public int rangoMinimino;
        public int rangoMaximo;
        public bool Masculino;
        public bool Femenino;
        public bool Diabetes;
        public bool Hipertension;
        public bool Obesidad;
        public bool Vacunado;
        public bool noVacunado;
        //Poblacion de mexico 126014024, con 99 % confiabilidad y con margen de error de 5% = 666
        public int CantidadPacientes = 666;// muestra poblacion de mexico 
        List<ClsPaciente> Pacientes;
        ClsCovid covid;
        System.Timers.Timer tiempoTranscurrido;
        DateTime inicio;
        string[] enfermedadesCronicas = new string[]
        {
            "Diabetes", "Hipertencion", "Obesidad"

        };

        bool terminar = true;

        public Simulacion()
        {
            InitializeComponent();
            Pacientes = new List<ClsPaciente> ();
        }
        public void InicioSimulacion()
        {
            tiempoTranscurrido = new System.Timers.Timer(500);
            inicio = DateTime.Now;
            tiempoTranscurrido.Elapsed += HandleTimer;
            tiempoTranscurrido.Start();
        }

        private void HandleTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                Invoke(new Action(() => lblTiempo.Text = (e.SignalTime - inicio).ToString()));
                covid.oxigenacion();
                if (terminar)
                {
                    //Detener simulacion, parar hilos y tiempo
                    tiempoTranscurrido.Stop();
                
                }
            }
            catch (Exception)
            {
            }
        }
        private void Simulacion_Load(object sender, EventArgs e)
        {
            CrearPersonas();
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel1.Top = -(e.NewValue * panel1.Height / 100);

        }
        public void CrearPersonas()
        {
            Random alea = new Random(); //Variable aleatorias
            //Clase con metodos para generacion de numeros
            ClsNumerosPseudoaleatorios numPseudoaleatorios = new ClsNumerosPseudoaleatorios();
            //Genrar alturas hasta cumplir con las 3 características
            List<double> alturas;
            PruebasEstadisticas pruebasEstadisticas;
            do
            {
                //Utilizando la clase numeros pseudoaletorios para generar los valores en las alturas de los pacientes 
                alturas = numPseudoaleatorios.CongruenciaLineal(CantidadPacientes);
                //Realizando pruebas
                pruebasEstadisticas = new PruebasEstadisticas(alturas);
            } while (!pruebasEstadisticas.Aleatorio || !pruebasEstadisticas.Uniforme || !pruebasEstadisticas.Independiente);

            //For para generar las variables aleatrias de los datos de los pacientes a simular 
            for (int i = 0; i < CantidadPacientes; i++)
            {
                //Agarramos los datos generados en la clase Paciente y los inicializamos 
                ClsPaciente paciente = new ClsPaciente();
                //Agarramos datos aleatorios a partir del rango minimo al rango maximo +1 para agrrar todos los datos 
                paciente.Edad = alea.Next(rangoMinimino, rangoMaximo + 1);
                //2= Femenino y masculino
                if (Masculino && Femenino)
                {
                    paciente.Genero = alea.Next(2);
                }
                else
                {
                    paciente.Genero = Masculino ? 1 : 0;
                }
                //Asignarle al numero aleatorio  un numero entre 1.44 y 1.84 (rangos de estaturas en tabla IMC)
                paciente.Estatura = alturas[i] * .40 + 1.44;
                //Arreglo de pacientes 
                Pacientes.Add(paciente);
                if (Vacunado && noVacunado )
                {
                    paciente.Vacuna = alea.Next(2) == 0 ? null : new ClsVacuna();                
                }
                else
                {
                    paciente.Vacuna = Vacunado ? new ClsVacuna() : null  ;
                }
               
            }

            if (Obesidad)
            {
                RepObesidadAlea();

            }
            else
            {
                //Si no tiene obesidad asigna peso 
                for (int i = 0; i < Pacientes.Count; i++)
                {
                    // generar un peso que al calcular el IMC me de resultado < 30 
                    int rangoMaximoPeso = Convert.ToInt32(30 * Pacientes[i].Estatura * Pacientes[i].Estatura * 100);
                    Pacientes[i].Peso = alea.Next(3840, rangoMaximoPeso) / 100;
                }
            }

            if (Hipertension)
            {
                RepHipertensionAlea();

            }

            if (Diabetes)
            {
                RepDiabetesAlea();

            }
            covid = new ClsCovid(ref Pacientes) { done = actualizarEtiquetas };
            covid.asignarSintomas();
            ClasificarPacientes();

            var test = Pacientes.Where(e => e.EnfermedadesCronicas.Any()).ToList();
        }
        public void ClasificarPacientes()
        {
            var jovenes = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25).ToList();
            var jovenesCronicosM = jovenes.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var jovenesCronicosF = jovenes.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var jovenesSanosM = jovenes.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var jovenesSanosF = jovenes.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            if(jovenesCronicosM.Count() > 0)
            {
                //creacion de controles
                panel1.Height += 350;
                var container = new Panel() {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "JOVENES CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatJovenesCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + jovenesCronicosM.Count(),
                    Name = "lblCantJovenesCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + jovenesCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvgJovenesCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + jovenesCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgJovenesCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadJovenesCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label() {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasJovenesCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasJovenesCronicosM"
                };

                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadJovenesCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadJovenesCronicosM"
                };

                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasJovenesCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasJovenesCronicosM"
                };

                var lstResumenSintomas = jovenesCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);

                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesJovenesCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesJovenesCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + jovenesCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgJovenesCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbJovenesCronicosM"
                };
                //peso promedio
                var pesoAvg = jovenesCronicosM.Average(p => p.Peso);
                var estaturaAvg = jovenesCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;
                //agregar controles

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);
                
                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);
                
                container.Controls.Add(paciente);
                

                panel1.Controls.Add(container);
            }
            if (jovenesCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "JOVENES CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatJovenesCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + jovenesCronicosF.Count(),
                    Name = "lblCantJovenesCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + jovenesCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvgJovenesCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + jovenesCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgJovenesCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadJovenesCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasJovenesCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasJovenesCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasJovenesCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasJovenesCronicosF"
                };
                var lstResumenSintomas = jovenesCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);

                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadJovenesCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadJovenesCronicosF"
                };
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesJovenesCronicosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesJovenesCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + jovenesCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgJovenesCronicosM"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbJovenesCronicosF"
                };

                var pesoAvg = jovenesCronicosF.Average(p => p.Peso);
                var estaturaAvg = jovenesCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (jovenesSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "JOVENES SANOS (MASCULINO)",
                    Name = "lblCatJovenesM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + jovenesSanosM.Count(),
                    Name = "lblCantJovenesM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + jovenesSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvgJovenesM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + jovenesSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgJovenesM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + jovenesSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgJovenesM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadJovenesM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasJovenesM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasJovenesM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };

                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadJovenesM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadJovenesM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasJovenesM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasJovenesM"
                };
                var lstResumenSintomas = jovenesSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesJovenesM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesJovenesM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbJovenesM"
                };

                var pesoAvg = jovenesSanosM.Average(p => p.Peso);
                var estaturaAvg = jovenesSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (jovenesSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "JOVENES SANOS (FEMENINO)",
                    Name = "lblCatJovenesF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + jovenesSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgJovenesM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + jovenesSanosF.Count(),
                    Name = "lblCantJovenesF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + jovenesSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvgJovenesF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + jovenesSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgJovenesF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadJovenesF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasJovenesF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasJovenesF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadJovenesF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadJovenesF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasJovenesF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasJovenesF"
                };
                var lstResumenSintomas = jovenesSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);

                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesJovenesF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesJovenesF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbJovenesF"
                };

                var pesoAvg = jovenesSanosF.Average(p => p.Peso);
                var estaturaAvg = jovenesSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }

            var adultosJovenes = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40).ToList();
            var adultosJovenesCronicosM = adultosJovenes.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosJovenesCronicosF = adultosJovenes.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosJovenesSanosM = adultosJovenes.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosJovenesSanosF = adultosJovenes.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            if (adultosJovenesCronicosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS JOVENES CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatAdultosJovenesCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosJovenesCronicosM.Count(),
                    Name = "lblCantAdultosJovenesCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosJovenesCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosJovenesCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosJovenesCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosJovenesCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosJovenesCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosJovenesCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosJovenesCronicosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosJovenesCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosJovenesCronicosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosJovenesCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosJovenesCronicosM"
                };
                var lstResumenSintomas = adultosJovenesCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosJovenesCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosJovenesCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosJovenesCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosJovenesCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosJovenesCronicosM"
                };

                var pesoAvg = adultosJovenesCronicosM.Average(p => p.Peso);
                var estaturaAvg = adultosJovenesCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosJovenesCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS JOVENES CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatAdultosJovenesCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosJovenesCronicosF.Count(),
                    Name = "lblCantAdultosJovenesCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosJovenesCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosJovenesCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosJovenesCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosJovenesCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosJovenesCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosJovenesCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosJovenesCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosJovenesCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosJovenesCronicosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosJovenesCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosJovenesCronicosF"
                };
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosJovenesCronicosF"
                };
                var lstResumenSintomas = adultosJovenesCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosJovenesCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosJovenesCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosJovenesCronicosF"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosJovenesCronicosF"
                };

                var pesoAvg = adultosJovenesCronicosF.Average(p => p.Peso);
                var estaturaAvg = adultosJovenesCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosJovenesSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS JOVENES SANOS (MASCULINO)",
                    Name = "lblCatAdultosJovenesM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosJovenesSanosM.Count(),
                    Name = "lblCantAdultosJovenesM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosJovenesSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosJovenesM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosJovenesSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosJovenesM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosJovenesSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosJovenesM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosJovenesM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosJovenesM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosJovenesM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosJovenesM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosJovenesM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosJovenesM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosJovenesM"
                };
                var lstResumenSintomas = adultosJovenesSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosJovenesM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosJovenesM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosJovenesM"
                };

                var pesoAvg = adultosJovenesSanosM.Average(p => p.Peso);
                var estaturaAvg = adultosJovenesSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosJovenesSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS JOVENES SANOS (FEMENINO)",
                    Name = "lblCatAdultosJovenesF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosJovenesSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosJovenesM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosJovenesSanosF.Count(),
                    Name = "lblCantAdultosJovenesF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosJovenesSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosJovenesF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosJovenesSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosJovenesF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosJovenesF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosJovenesF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosJovenesF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosJovenesF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosJovenesF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosJovenesF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosJovenesF"
                };
                var lstResumenSintomas = adultosJovenesSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosJovenesF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosJovenesF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosJovenesF"
                };

                var pesoAvg = adultosJovenesSanosF.Average(p => p.Peso);
                var estaturaAvg = adultosJovenesSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }

            var adultos = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55).ToList();
            var adultosCronicosM = adultos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosCronicosF = adultos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosSanosM = adultos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosSanosF = adultos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
           
            if (adultosCronicosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatAdultosCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosCronicosM.Count(),
                    Name = "lblCantAdultosCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosCronicosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosCronicosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosCronicosM"
                };
                var lstResumenSintomas = adultosCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosCronicosM"
                };

                var pesoAvg = adultosCronicosM.Average(p => p.Peso);
                var estaturaAvg = adultosCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatAdultosCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosCronicosF.Count(),
                    Name = "lblCantAdultosCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosCronicosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosCronicosF"
                };
                var lstResumenSintomas = adultosCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosCronicosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosCronicosF"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosCronicosF"
                };

                var pesoAvg = adultosCronicosF.Average(p => p.Peso);
                var estaturaAvg = adultosCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS  SANOS (MASCULINO)",
                    Name = "lblCatAdultosM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosSanosM.Count(),
                    Name = "lblCantAdultosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosM"
                };
                var lstResumenSintomas = adultosSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosM"
                };

                var pesoAvg = adultosSanosM.Average(p => p.Peso);
                var estaturaAvg = adultosSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS  SANOS (FEMENINO)",
                    Name = "lblCatAdultosF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosSanosF.Count(),
                    Name = "lblCantAdultosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosF"
                };
                var lstResumenSintomas = adultosSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosF"
                };

                var pesoAvg = adultosSanosF.Average(p => p.Peso);
                var estaturaAvg = adultosSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);

                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }

            var adultosMayores = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65).ToList();
            var adultosMayoresCronicosM = adultosMayores.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosMayoresCronicosF = adultosMayores.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosMayoresSanosM = adultosMayores.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosMayoresSanosF = adultosMayores.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            if (adultosMayoresCronicosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS MAYORES CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatAdultosMayoresCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosMayoresCronicosM.Count(),
                    Name = "lblCantAdultosMayoresCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosMayoresCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosMayoresCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosMayoresCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosMayoresCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosMayoresCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosMayoresCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosMayoresCronicosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosMayoresCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosMayoresCronicosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosMayoresCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosMayoresCronicosM"
                };
                var lstResumenSintomas = adultosMayoresCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosMayoresCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosMayoresCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosMayoresCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosMayoresCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosMayoresCronicosM"
                };

                var pesoAvg = adultosMayoresCronicosM.Average(p => p.Peso);
                var estaturaAvg = adultosMayoresCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosMayoresCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS MAYORES CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatAdultosMayoresCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosMayoresCronicosF.Count(),
                    Name = "lblCantAdultosMayoresCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosMayoresCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosMayoresCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosMayoresCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosMayoresCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosMayoresCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosMayoresCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosMayoresCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosMayoresCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosMayoresCronicosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosMayoresCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosMayoresCronicosF"
                };
                var lstResumenSintomas = adultosMayoresCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosMayoresCronicosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosMayoresCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosMayoresCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosMayoresCronicosF"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosMayoresCronicosF"
                };

                var pesoAvg = adultosMayoresCronicosF.Average(p => p.Peso);
                var estaturaAvg = adultosMayoresCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosMayoresSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS MAYORES SANOS (MASCULINO)",
                    Name = "lblCatAdultosMayoresM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosMayoresSanosM.Count(),
                    Name = "lblCantAdultosMayoresM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosMayoresSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosMayoresM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosMayoresSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosMayoresM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosMayoresSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosMayoresM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosMayoresM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosMayoresM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosMayoresM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosMayoresM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosMayoresM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosMayoresM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosMayoresM"
                };
                var lstResumenSintomas = adultosMayoresSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosMayoresM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosMayoresM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosMayoresM"
                };

                var pesoAvg = adultosMayoresSanosM.Average(p => p.Peso);
                var estaturaAvg = adultosMayoresSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (adultosMayoresSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ADULTOS MAYORES SANOS (FEMENINO)",
                    Name = "lblCatAdultosMayoresF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + adultosMayoresSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgAdultosMayoresM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + adultosMayoresSanosF.Count(),
                    Name = "lblCantAdultosMayoresF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + adultosMayoresSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvgAdultosMayoresF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + adultosMayoresSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgAdultosMayoresF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadAdultosMayoresF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasAdultosMayoresF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasAdultosMayoresF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadAdultosMayoresF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadAdultosMayoresF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasAdultosMayoresF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasAdultosMayoresF"
                };
                var lstResumenSintomas = adultosMayoresSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesAdultosMayoresF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesAdultosMayoresF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbAdultosMayoresF"
                };

                var pesoAvg = adultosMayoresSanosF.Average(p => p.Peso);
                var estaturaAvg = adultosMayoresSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            var ancianos = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75).ToList();
            var ancianosCronicosM = ancianos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var ancianosCronicosF = ancianos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var ancianosSanosM = ancianos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var ancianosSanosF = ancianos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            if (ancianosCronicosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ANCIANOS CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatancianosCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + ancianosCronicosM.Count(),
                    Name = "lblCantancianosCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + ancianosCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvgancianosCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + ancianosCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgancianosCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadancianosCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasancianosCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasancianosCronicosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadancianosCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadancianosCronicosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasancianosCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasancianosCronicosM"
                };
                var lstResumenSintomas = ancianosCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesancianosCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesancianosCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + ancianosCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgancianosCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbancianosCronicosM"
                };


                var pesoAvg = ancianosCronicosM.Average(p => p.Peso);
                var estaturaAvg = ancianosCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (ancianosCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ANCIANOS CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatancianosCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + ancianosCronicosF.Count(),
                    Name = "lblCantancianosCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + ancianosCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvgancianosCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + ancianosCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgancianosCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadancianosCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasancianosCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasancianosCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadancianosCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadancianosCronicosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasancianosCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasancianosCronicosF"
                };
                var lstResumenSintomas = ancianosCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesancianosCronicosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesancianosCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + ancianosCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgancianosCronicosF"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbancianosCronicosF"
                };

                var pesoAvg = ancianosCronicosF.Average(p => p.Peso);
                var estaturaAvg = ancianosCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (ancianosSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ANCIANOS SANOS (MASCULINO)",
                    Name = "lblCatancianosM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + ancianosSanosM.Count(),
                    Name = "lblCantancianosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + ancianosSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvgancianosM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + ancianosSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvgancianosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + ancianosSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgancianosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadancianosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasancianosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasancianosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadancianosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadancianosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasancianosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasancianosM"
                };
                var lstResumenSintomas = ancianosSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesancianosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesancianosM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbancianosM"
                };

                var pesoAvg = ancianosSanosM.Average(p => p.Peso);
                var estaturaAvg = ancianosSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (ancianosSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "ANCIANOS SANOS (FEMENINO)",
                    Name = "lblCatancianosF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + ancianosSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvgancianosM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + ancianosSanosF.Count(),
                    Name = "lblCantancianosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + ancianosSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvgancianosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + ancianosSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvgancianosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadancianosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelasancianosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelasancianosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadancianosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadancianosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomasancianosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomasancianosF"
                };
                var lstResumenSintomas = ancianosSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacionesancianosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacionesancianosF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pbancianosF"
                };

                var pesoAvg = ancianosSanosF.Average(p => p.Peso);
                var estaturaAvg = ancianosSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            var longevos = Pacientes.Where(p => p.Edad >= 75).ToList();
            var longevosCronicosM = longevos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var longevosCronicosF = longevos.Where(p => p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var longevosSanosM = longevos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var longevosSanosF = longevos.Where(p => p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            if (longevosCronicosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "LONGEVOS CON ENFERMEDADES CRÓNICAS (MASCULINO)",
                    Name = "lblCatlongevosCronicosM",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + longevosCronicosM.Count(),
                    Name = "lblCantlongevosCronicosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + longevosCronicosM.Average(e => e.Peso),
                    Name = "lblPesoAvglongevosCronicosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + longevosCronicosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvglongevosCronicosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadlongevosCronicosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelaslongevosCronicosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelaslongevosCronicosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadlongevosCronicosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadlongevosCronicosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomaslongevosCronicosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomaslongevosCronicosM"
                };
                var lstResumenSintomas = longevosCronicosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacioneslongevosCronicosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacioneslongevosCronicosM",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + longevosCronicosM.Average(e => e.Estatura),
                    Name = "lblPesoAvglongevosCronicosM"
                };
                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pblongevosCronicosM"

                };

                var pesoAvg = longevosCronicosM.Average(p => p.Peso);
                var estaturaAvg = longevosCronicosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);


                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (longevosCronicosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "LONGEVOS CON ENFERMEDADES CRÓNICAS (FEMENINO)",
                    Name = "lblCatlongevosCronicosF",
                    Height = 50
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + longevosCronicosF.Count(),
                    Name = "lblCantlongevosCronicosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + longevosCronicosF.Average(e => e.Peso),
                    Name = "lblPesoAvglongevosCronicosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + longevosCronicosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvglongevosCronicosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadlongevosCronicosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelaslongevosCronicosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelaslongevosCronicosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadlongevosCronicosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadlongevosCronicosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomaslongevosCronicosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomaslongevosCronicosF"
                };
                var lstResumenSintomas = longevosCronicosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacioneslongevosCronicosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacioneslongevosCronicosF",
                    Height = 90
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + longevosCronicosF.Average(e => e.Estatura),
                    Name = "lblPesoAvglongevosCronicosF"
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pblongevosCronicosF"
                };

                var pesoAvg = longevosCronicosF.Average(p => p.Peso);
                var estaturaAvg = longevosCronicosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);

                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (longevosSanosM.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "LONGEVOS SANOS (MASCULINO)",
                    Name = "lblCatlongevosM",
                    Height = 38
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + longevosSanosM.Count(),
                    Name = "lblCantlongevosM"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + longevosSanosM.Average(e => e.Peso),
                    Name = "lblPesoAvglongevosM"
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + longevosSanosM.Average(e => e.Estatura),
                    Name = "lblPesoAvglongevosM"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + longevosSanosM.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvglongevosM"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadlongevosM"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelaslongevosM"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelaslongevosM"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadlongevosM"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadlongevosM"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomaslongevosM"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomaslongevosM"
                };
                var lstResumenSintomas = longevosSanosM.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacioneslongevosM"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacioneslongevosM",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pblongevosM"
                };

                var pesoAvg = longevosSanosM.Average(p => p.Peso);
                var estaturaAvg = longevosSanosM.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.hombreObeso : Properties.Resources.hombreSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);
                container.Controls.Add(panelGraficas);
                

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);



                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
            if (longevosSanosF.Count() > 0)
            {
                panel1.Height += 350;
                var container = new Panel()
                {
                    Dock = DockStyle.Top,
                    ForeColor = Color.White,
                    Height = 350
                };
                var lblCategoria = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Comic Sans MS", 20.35f, FontStyle.Bold),
                    Text = "LONGEVOS SANOS (FEMENINO)",
                    Name = "lblCatlongevosF",
                    Height = 38
                };
                var lblEstatura = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "ESTATURA PROMEDIO: " + longevosSanosF.Average(e => e.Estatura),
                    Name = "lblPesoAvglongevosM"
                };
                var lblCantP = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "CANTIDAD DE PACIENTES: " + longevosSanosF.Count(),
                    Name = "lblCantlongevosF"
                };
                var lblPeso = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "PESO PROMEDIO: " + longevosSanosF.Average(e => e.Peso),
                    Name = "lblPesoAvglongevosF"
                };
                var lblOxigenacion = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "OXIGENACION PROMEDIO: " + longevosSanosF.Average(e => e.Oxigenacion),
                    Name = "lblOxigenacionAvglongevosF"
                };
                var lblEfectividad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "EFECTIVIDAD: ",
                    Name = "lbleEfectivadlongevosF"
                };
                var panelGraficas = new Panel()
                {
                    Dock = DockStyle.Top,
                    Height = 170
                };
                var panelGraficaSecuelas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSecuelas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SECUELAS",
                    Name = "lblSecuelaslongevosF"
                };
                var graficaSecuelas = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartSecuelaslongevosF"
                };
                var panelGraficaMortalidad = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitMortalidad = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "MORTALIDAD",
                    Name = "lblMortalidadlongevosF"
                };
                var graficaMortalidad = new Chart()
                {
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Palette = ChartColorPalette.SeaGreen,
                    Name = "chartMortalidadlongevosF"
                };
                var panelListaSintomas = new Panel()
                {
                    Dock = DockStyle.Left,
                    Width = 174
                };
                var lblTitSintomas = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Regular),
                    Text = "SINTOMAS",
                    Name = "lblSintomaslongevosF"
                };
                var listaSintomas = new ListBox()
                {
                    Dock = DockStyle.Fill,
                    Name = "lstSintomaslongevosF"
                };
                var lstResumenSintomas = longevosSanosF.SelectMany(p => p.Sintomas).GroupBy(p => p).Select(p => $"{p.Key}: {p.Count()} personas").ToArray();
                listaSintomas.Items.AddRange(lstResumenSintomas);
                var lblRecomendaciones = new Label()
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Palatino Linotype", 11.35f, FontStyle.Bold),
                    Text = "RECOMENDACIONES",
                    Name = "lblRecomendacioneslongevosF"
                };
                var panelRecomendaciones = new Panel()
                {
                    Dock = DockStyle.Top,
                    Name = "panelRecomendacioneslongevosF",
                    Height = 90
                };

                var paciente = new PictureBox()
                {
                    Dock = DockStyle.Left,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 210,
                    Name = "pblongevosF"
                };

                var pesoAvg = longevosSanosF.Average(p => p.Peso);
                var estaturaAvg = longevosSanosF.Average(p => p.Estatura);
                var IMCAvg = pesoAvg / Math.Pow(estaturaAvg, 2);
                paciente.Image = IMCAvg >= 30 ? Properties.Resources.mujerObesa : Properties.Resources.mujerSaludable;

                panelGraficas.Controls.Add(panelGraficaSecuelas);
                panelGraficas.Controls.Add(panelGraficaMortalidad);
                panelGraficas.Controls.Add(panelListaSintomas);

                container.Controls.Add(panelGraficas);

                container.Controls.Add(lblOxigenacion);
                container.Controls.Add(lblPeso);
                container.Controls.Add(lblEstatura);
                container.Controls.Add(lblCantP);
                container.Controls.Add(lblCategoria);

                panelGraficaSecuelas.Controls.Add(graficaSecuelas);
                panelGraficaSecuelas.Controls.Add(lblTitSecuelas);

                panelGraficaMortalidad.Controls.Add(graficaMortalidad);
                panelGraficaMortalidad.Controls.Add(lblTitMortalidad);

                panelListaSintomas.Controls.Add(listaSintomas);
                panelListaSintomas.Controls.Add(lblTitSintomas);

                container.Controls.Add(paciente);


                panel1.Controls.Add(container);
            }
        }
        public void RepDiabetesAlea()
        {
            int cant = Convert.ToInt32(Math.Round(Pacientes.Count * .206));
            //Asignar diabetes a la cantidad de personas 
            for (int i = 0; i < cant; i++)
            {
                Pacientes[i].EnfermedadesCronicas.Add("Diabetes");

            }
        }
        public void RepHipertensionAlea()
        {

            Pacientes = Sortear(Pacientes);

            int cant = Convert.ToInt32(Math.Round(Pacientes.Count * .255));
            for (int i = 0; i < cant; i++)
            {
                Pacientes[i].EnfermedadesCronicas.Add( "Hipertensión");
               
            }
        }
        public void RepObesidadAlea()
        {
            Random alea = new Random();
            Pacientes = Sortear(Pacientes);


            int cant = Convert.ToInt32(Math.Round(Pacientes.Count * .2749));
            
            for (int i = 0; i < cant; i++)
            {
                //asigna obesidad a la cantidad 
                Pacientes[i].EnfermedadesCronicas.Add("Obesidad");
                //buscar que me de un resultado mayor a 30 
                int rangoMinimoPeso = Convert.ToInt32(30 * Pacientes[i].Estatura * Pacientes[i].Estatura * 100);
                //asigno el peso que encontre de manera aleatoria 135.41 peso maximo que maneja tabla IMC 
                Pacientes[i].Peso = alea.Next(rangoMinimoPeso, 13541) / 100;
            }
            //El restante del porcentaje de la poblacion que no tiene obesidad (38.40) rango minimo que maneja la tala 
            for (int i = cant; i < Pacientes.Count; i++)
            {
                int rangoMaximoPeso = Convert.ToInt32(30 * Pacientes[i].Estatura * Pacientes[i].Estatura * 100);
                Pacientes[i].Peso = alea.Next(3840, rangoMaximoPeso) / 100;
            }
        }
        /*
         * Metodo para revolver la lista 
         * */
        public List<ClsPaciente> Sortear(List<ClsPaciente> ListaPacientes)
        {
            //cantidad de pacientes 
            int Cantidad = ListaPacientes.Count;
            //temporal ya que se estara agregando pacientes 
            List<ClsPaciente> temp = new List<ClsPaciente>();
            Random alea = new Random();
            //Seguir moviendolo mientras exista pacientes 
            while (Cantidad>0)
            {
                //Escojo un paciente aletorio
                int i= alea.Next(Cantidad);
                //lo agrego al temporal
                temp.Add(ListaPacientes[i]);
                //se lo quito al anterior
                ListaPacientes.RemoveAt(i);
                //y reviso cuantos quedan 
                Cantidad = ListaPacientes.Count;
            }
            return temp;
        }

        Task animacionContagio;
        Task cambiarOxigenacion;
        private void btnIniciarSimulacion_Click(object sender, EventArgs e)
        {
            terminar = !terminar;
            if (terminar) lblTiempo.Text = "00:00:00:00";
            btnIniciarSimulacion.Text = terminar ? "Iniciar" : "Detener";
            animacionContagio = new Task(entradaCovid);
            cambiarOxigenacion = animacionContagio.ContinueWith((data) => {
                InicioSimulacion();
            });
            animacionContagio.Start();
        }

        public void entradaCovid()
        {
            //se mandan a llamar las categorias que se estan considerando
            #region declaracionCategorias
            var jovenesSanosF = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var jovenesSanosM = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var jovenesCronicosF = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var jovenesCronicosM = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosJovenesSanosF = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosJovenesSanosM = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosJovenesCronicosF = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosJovenesCronicosM = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosSanosF = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosSanosM = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosCronicosF = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosCronicosM = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosMayoresSanosF = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosMayoresSanosM = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosMayoresCronicosF = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosMayoresCronicosM = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var ancianosSanosF = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var ancianosSanosM = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var ancianosCronicosF = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var ancianosCronicosM = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var longevosSanosF = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var longevosSanosM = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var longevosCronicosF = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var longevosCronicosM = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            #endregion

            double avgIMC;

            //JovenesM
            if (jovenesSanosM.Count > 0)
            {
                avgIMC = jovenesSanosM.Average(e => e.Peso) / Math.Pow(jovenesSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbJovenesM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (jovenesCronicosM.Count > 0)
            {
                avgIMC = jovenesCronicosM.Average(e => e.Peso) / Math.Pow(jovenesCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbJovenesCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //JovenesF
            if (jovenesSanosF.Count > 0)
            {
                avgIMC = jovenesSanosF.Average(e => e.Peso) / Math.Pow(jovenesSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbJovenesF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            if (jovenesCronicosF.Count > 0)
            {
                avgIMC = jovenesCronicosF.Average(e => e.Peso) / Math.Pow(jovenesCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbJovenesCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            //AdultosJovenesM
            if (adultosJovenesSanosM.Count > 0)
            {
                avgIMC = adultosJovenesSanosM.Average(e => e.Peso) / Math.Pow(adultosJovenesSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosJovenesM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (adultosJovenesCronicosM.Count > 0)
            {
                avgIMC = adultosJovenesCronicosM.Average(e => e.Peso) / Math.Pow(adultosJovenesCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosJovenesCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //AdultosJovenesF
            if (adultosJovenesSanosF.Count > 0)
            {
                avgIMC = adultosJovenesSanosF.Average(e => e.Peso) / Math.Pow(adultosJovenesSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosJovenesF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            if (adultosJovenesCronicosF.Count > 0)
            {
                avgIMC = adultosJovenesCronicosF.Average(e => e.Peso) / Math.Pow(adultosJovenesCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosJovenesCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            //AdultosM
            if (adultosSanosM.Count > 0)
            {
                avgIMC = adultosSanosM.Average(e => e.Peso) / Math.Pow(adultosSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (adultosCronicosM.Count > 0)
            {
                avgIMC = adultosCronicosM.Average(e => e.Peso) / Math.Pow(adultosCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //AdultosF
            if (adultosSanosF.Count > 0)
            {
                avgIMC = adultosSanosF.Average(e => e.Peso) / Math.Pow(adultosSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            if (adultosCronicosF.Count > 0)
            {
                avgIMC = adultosCronicosF.Average(e => e.Peso) / Math.Pow(adultosCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            //AdultosMayoresM
            if (adultosMayoresSanosM.Count > 0)
            {
                avgIMC = adultosMayoresSanosM.Average(e => e.Peso) / Math.Pow(adultosMayoresSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosMayoresM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (adultosMayoresCronicosM.Count > 0)
            {
                avgIMC = adultosMayoresCronicosM.Average(e => e.Peso) / Math.Pow(adultosMayoresCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosMayoresCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //AdultosMayoresF
            if (adultosMayoresSanosF.Count > 0)
            {
                avgIMC = adultosMayoresSanosF.Average(e => e.Peso) / Math.Pow(adultosMayoresSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosMayoresF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }

            if (adultosMayoresCronicosF.Count > 0)
            {
                avgIMC = adultosMayoresCronicosF.Average(e => e.Peso) / Math.Pow(adultosMayoresCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbAdultosMayoresCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            //AncianosM
            if (ancianosSanosM.Count > 0)
            {
                avgIMC = ancianosSanosM.Average(e => e.Peso) / Math.Pow(ancianosSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbancianosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (ancianosCronicosM.Count > 0)
            {
                avgIMC = ancianosCronicosM.Average(e => e.Peso) / Math.Pow(ancianosCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbancianosCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //AncianosF
            if (ancianosSanosF.Count > 0)
            {
                avgIMC = ancianosSanosF.Average(e => e.Peso) / Math.Pow(ancianosSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbancianosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            if (ancianosCronicosF.Count > 0)
            {
                avgIMC = ancianosCronicosF.Average(e => e.Peso) / Math.Pow(ancianosCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pbancianosCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            //LongevosM
            if (longevosSanosM.Count > 0)
            {
                avgIMC = longevosSanosM.Average(e => e.Peso) / Math.Pow(longevosSanosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pblongevosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            if (longevosCronicosM.Count > 0)
            {
                avgIMC = longevosCronicosM.Average(e => e.Peso) / Math.Pow(longevosCronicosM.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pblongevosCronicosM", true)[0]).Image = avgIMC > 30? Properties.Resources.hombreObeso1 : Properties.Resources.HombreSaludable1;
            }
            //LongevosF
            if (longevosSanosF.Count > 0)
            {
                avgIMC = longevosSanosF.Average(e => e.Peso) / Math.Pow(longevosSanosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pblongevosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }
            if (longevosCronicosF.Count > 0)
            {
                avgIMC = longevosCronicosF.Average(e => e.Peso) / Math.Pow(longevosCronicosF.Average(e => e.Estatura), 2);
                ((PictureBox)Controls.Find("pblongevosCronicosF", true)[0]).Image = avgIMC > 30? Properties.Resources.mujerObesa1 : Properties.Resources.mujerSaludable1;
            }

            Thread.Sleep(4000);
        }

        public void actualizarEtiquetas()
        {
            //se mandan a llamar las categorias que se estan considerando
            #region declaracionCategorias
            var jovenesSanosF = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var jovenesSanosM = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var jovenesCronicosF = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var jovenesCronicosM = Pacientes.Where(p => p.Edad >= 18 && p.Edad < 25 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosJovenesSanosF = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosJovenesSanosM = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosJovenesCronicosF = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosJovenesCronicosM = Pacientes.Where(p => p.Edad >= 25 && p.Edad < 40 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosSanosF = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosSanosM = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosCronicosF = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosCronicosM = Pacientes.Where(p => p.Edad >= 40 && p.Edad < 55 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var adultosMayoresSanosF = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var adultosMayoresSanosM = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var adultosMayoresCronicosF = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var adultosMayoresCronicosM = Pacientes.Where(p => p.Edad >= 55 && p.Edad < 65 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var ancianosSanosF = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var ancianosSanosM = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var ancianosCronicosF = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var ancianosCronicosM = Pacientes.Where(p => p.Edad >= 65 && p.Edad < 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            var longevosSanosF = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 0).ToList();
            var longevosSanosM = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count == 0 && p.Genero == 1).ToList();
            var longevosCronicosF = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 0).ToList();
            var longevosCronicosM = Pacientes.Where(p => p.Edad >= 75 && p.EnfermedadesCronicas.Count > 0 && p.Genero == 1).ToList();
            #endregion
            // cambia las etiquetas 
            //Oxigenacion para forma
            int avgOx;
            //Oxigenacion para imagen
            int avgOxImg;
            double avgIMC;
            try
            {
                //jovenes
                if (jovenesSanosF.Count > 0)
                {

                    avgOx = (int)Math.Round(jovenesSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(jovenesSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = jovenesSanosF.Average(e => e.Peso) / Math.Pow(jovenesSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)Controls.Find("lblOxigenacionAvgJovenesF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)Controls.Find("pbJovenesF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);

                    }));
                }
                if (jovenesSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(jovenesSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(jovenesSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = jovenesSanosM.Average(e => e.Peso) / Math.Pow(jovenesSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgJovenesM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbJovenesM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));
                }
                if (jovenesCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(jovenesCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(jovenesCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = jovenesCronicosF.Average(e => e.Peso) / Math.Pow(jovenesCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgJovenesCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbJovenesCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (jovenesCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(jovenesCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(jovenesCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = jovenesCronicosM.Average(e => e.Peso) / Math.Pow(jovenesCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgJovenesCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbJovenesCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                //adultos jovenes
                if (adultosJovenesSanosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosJovenesSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosJovenesSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosJovenesSanosF.Average(e => e.Peso) / Math.Pow(adultosJovenesSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosJovenesF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosJovenesF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosJovenesSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosJovenesSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosJovenesSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosJovenesSanosM.Average(e => e.Peso) / Math.Pow(adultosJovenesSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosJovenesM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx; ;
                        ((PictureBox)this.Controls.Find("pbAdultosJovenesM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosJovenesCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosJovenesCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosJovenesCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosJovenesCronicosF.Average(e => e.Peso) / Math.Pow(adultosJovenesCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosJovenesCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx; ;
                        ((PictureBox)this.Controls.Find("pbAdultosJovenesCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosJovenesCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosJovenesCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosJovenesCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosJovenesCronicosM.Average(e => e.Peso) / Math.Pow(adultosJovenesCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosJovenesCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosJovenesCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                //adultos
                if (adultosSanosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosSanosF.Average(e => e.Peso) / Math.Pow(adultosSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosSanosM.Average(e => e.Peso) / Math.Pow(adultosSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx; ;
                        ((PictureBox)this.Controls.Find("pbAdultosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosCronicosF.Average(e => e.Peso) / Math.Pow(adultosCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));
                }
                if (adultosCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosCronicosM.Average(e => e.Peso) / Math.Pow(adultosCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                //adultos mayores
                if (adultosMayoresSanosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosMayoresSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosMayoresSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosMayoresSanosF.Average(e => e.Peso) / Math.Pow(adultosMayoresSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosMayoresF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosMayoresF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosMayoresSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosMayoresSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosMayoresSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosMayoresSanosM.Average(e => e.Peso) / Math.Pow(adultosMayoresSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosMayoresM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosMayoresM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));


                }
                if (adultosMayoresCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosMayoresCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosMayoresCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosMayoresCronicosF.Average(e => e.Peso) / Math.Pow(adultosMayoresCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosMayoresCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosMayoresCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (adultosMayoresCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(adultosMayoresCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(adultosMayoresCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = adultosMayoresCronicosM.Average(e => e.Peso) / Math.Pow(adultosMayoresCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgAdultosMayoresCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbAdultosMayoresCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                //ancianos
                if (ancianosSanosF.Count > 0)
                {
                    avgOx = (int)Math.Round(ancianosSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(ancianosSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = ancianosSanosF.Average(e => e.Peso) / Math.Pow(ancianosSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgancianosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbancianosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (ancianosSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(ancianosSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(ancianosSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = ancianosSanosM.Average(e => e.Peso) / Math.Pow(ancianosSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgancianosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbancianosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (ancianosCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(ancianosCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(ancianosCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = ancianosCronicosF.Average(e => e.Peso) / Math.Pow(ancianosCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgancianosCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbancianosCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (ancianosCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(ancianosCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(ancianosCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = ancianosCronicosM.Average(e => e.Peso) / Math.Pow(ancianosCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvgancianosCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pbancianosCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                //longevos
                if (longevosSanosF.Count > 0)
                {
                    avgOx = (int)Math.Round(longevosSanosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(longevosSanosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = longevosSanosF.Average(e => e.Peso) / Math.Pow(longevosSanosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvglongevosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pblongevosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
                if (longevosSanosM.Count > 0)
                {
                    avgOx = (int)Math.Round(longevosSanosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(longevosSanosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = longevosSanosM.Average(e => e.Peso) / Math.Pow(longevosSanosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvglongevosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pblongevosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }

                if (longevosCronicosF.Count > 0)
                {
                    avgOx = (int)Math.Round(longevosCronicosF.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(longevosCronicosF.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = longevosCronicosF.Average(e => e.Peso) / Math.Pow(longevosCronicosF.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvglongevosCronicosF", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pblongevosCronicosF", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("mujer" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }

                if (longevosCronicosM.Count > 0)
                {
                    avgOx = (int)Math.Round(longevosCronicosM.Average(e => e.Oxigenacion));
                    avgOxImg = (int)Math.Round(longevosCronicosM.Average(e => e.Oxigenacion) / 5) * 5;
                    avgIMC = longevosCronicosM.Average(e => e.Peso) / Math.Pow(longevosCronicosM.Average(e => e.Estatura), 2);
                    Invoke(new Action(() => {
                        ((Label)this.Controls.Find("lblOxigenacionAvglongevosCronicosM", true)[0]).Text = "OXIGENACIÓN PROMEDIO: " + avgOx;
                        ((PictureBox)this.Controls.Find("pblongevosCronicosM", true)[0]).Image = (Bitmap)Properties.Resources.ResourceManager.GetObject("hombre" + (avgIMC > 30 ? "O" : "") + avgOxImg);
                    }));

                }
            }
            catch (Exception)
            {

            }
            
        }

        private void Simulacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Parar tareas si la forma se cierra
            try
            {
                animacionContagio.Dispose();
                cambiarOxigenacion.Dispose();

            }
            catch (Exception)
            {
            }
        }
    }
}
