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
                var margen = 0;
                switch (p.EnfermedadesCronicas.Count)
                {
                    case 1:
                        margen = 2;
                        break;
                    case 2:
                        margen = 3;
                        break;
                    case 3:
                        margen = 4;
                        break;
                    default:
                        margen = 1;
                        break;
                }
                p.Oxigenacion = alea.Next(p.Oxigenacion - margen, p.Oxigenacion > (101 - margen ) ? 101 : p.Oxigenacion + margen );
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
                    p.Sintomas.Add(Sintomas[i]);
                }
            }
        }
    }
}
