using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionCOVID_19
{
    /*Datos de los pacientes 

Resultado del tratamiento  

Cálculos 

Graficación 

 
*/
    public class ClsEstadisticas
    {
        public ClsPaciente DatosPAciente
        {
            get;
            set;
        }
        public double EfectividadTratamiendo
        {
            get;
           
        }
        public DataTable Calculo
        {
            get;
            
        }
        public DataTable Grafica
        {
            get;
        }

    }
}
