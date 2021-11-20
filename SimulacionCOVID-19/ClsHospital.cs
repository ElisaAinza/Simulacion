using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Datos que nos ayudaran a saber las condiciones del hospital
//y asi saber si se pondra brindar una mejor atencion al paciente
//junto con el tratamiento, y saber si se podran lograr los resultados del proyecto.
namespace SimulacionCOVID_19
{
    /*
     * Cantidad de especialistas disponibles 

Capacidad  

Cantidad de tratamientos disponibles 
     */
   public  class ClsHospital
    {
        public int EspacialistasDisponibles
        {
            get;
            set;
        }
        public int CapacidadPacientes
        {
            get;
            set;
        }
        public int CantidadTratamiendosDispo
        {
            get;
            set;
        }
    }
}
