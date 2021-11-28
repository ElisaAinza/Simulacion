using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionCOVID_19
{

    class ClsCovid
    {
        string[] Sintomas = { "Fiebre", "Tos", "Dolor de cuerpo", "Baja Oxigenacion"};
        List<ClsPaciente> pacientes;
        Random alea = new Random();
        public Action done;
        //Recibe a los pacientes y les cambia la oxigenacion mediante el metodo
        public  ClsCovid (ref List<ClsPaciente> pacientes)
        {
            this.pacientes = pacientes;
        }
        public void oxigenacion()
        {
            foreach (var p in pacientes) {
                switch (p.EnfermedadesCronicas.Count)
                {
                    case 1:
                        if (p.Sintomas.Contains("Baja Oxigenacion"))
                        {
                            p.Oxigenacion = alea.Next(70, 100);
                        }
                        else
                        {
                            p.Oxigenacion = alea.Next(95, 100);
                        }
                        break;
                    case 2:
                        if (p.Sintomas.Contains("Baja Oxigenacion"))
                        {
                            p.Oxigenacion = alea.Next(65, 100);
                        }
                        else
                        {
                            p.Oxigenacion = alea.Next(95, 100);
                        }
                        break;
                    case 3:
                        if (p.Sintomas.Contains("Baja Oxigenacion"))
                        {
                            p.Oxigenacion = alea.Next(60, 100);
                        }
                        else
                        {
                            p.Oxigenacion = alea.Next(95, 100);
                        }
                        break;
                    default:
                        if (p.Sintomas.Contains("Baja Oxigenacion"))
                        {
                            p.Oxigenacion = alea.Next(80, 100);
                        }
                        else
                        {
                            p.Oxigenacion = alea.Next(97, 100);
                        }
                        break;

                }

                p.HistorialOxigenacion.Add(p.Oxigenacion);
            }
            done();
        }
        public void asignarSintomas()
        {
            foreach (var p in pacientes)
            {
                int cantSintomas = alea.Next(Sintomas.Length + 1);
                for (int i = 0; i < cantSintomas; i++)
                {
                    var sintoma = Sintomas[alea.Next(Sintomas.Length)];
                    if (p.Sintomas.Contains(sintoma))
                    {
                        i--;
                    }
                    else { 
                        p.Sintomas.Add(sintoma);
                    }
                }
            }
        }
    }
}
