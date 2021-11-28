using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionCOVID_19
{
    
    public class ClsPaciente
    {
        public int Edad //Rango de 18-85 años. 
        {
            get;
            set;
        }
        public int Genero //Rango de 0-1 siendo 0 el género femenino y 1 el género masculino. 
        {
            get;
            set;
        }
        public List<string> EnfermedadesCronicas
        {
            get;
            set;
        } = new List<string>();
        public List<string> Secuelas
        {
            get;
            set;
        } = new List<string>();

        public List<int> HistorialOxigenacion
        {
            get;
            set;
        } = new List<int>();
        public List<string> Sintomas
        {
            get;
            set;
        } = new List<string>();
        public double Peso
        {
            get;
            set;
        }
        public double Estatura 
        {
            get;
            set;
        }
        public ClsVacuna Vacuna
        {
            get;
            set;
        }
        public int CantidadEnfermedades
        {
            get;
            set;
        }
        public int Oxigenacion
        {
            get;
            set;
        } = new Random().Next(95, 101);
    }
}
