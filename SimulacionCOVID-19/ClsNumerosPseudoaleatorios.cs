using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulacionCOVID_19
{
    public class ClsNumerosPseudoaleatorios
    {
        Random alea;
        List<double> datos;
        double a, c, m, semilla;

        public ClsNumerosPseudoaleatorios()
        {
            alea = new Random();
            datos = new List<double>();
            //Elegimos numeros impares 
            a = 16645251;
            c = 101390423;
            m = 99999;
        }
        private double calcularSemilla()
        {
            //valor inicial  de semilla
            double x = 2;
            do
            {
                //buscar numero primo de nueve digitos 
                x = alea.Next(100000001, 999999999);
                //En caso de no encontrar numero primo lo vuelve a buscar
            } while (x % 2 == 0);
            //valor final de la semilla
            return x;
        }
        //Sustituir valores en la formula 
        public List<double> CongruenciaLineal(int cant)
        {
            semilla = calcularSemilla();
            datos = new List<double>();
            //Cantidad de numeros aleatorios a generar
            double x = semilla;
            for (int j = 0; j < cant; j++)
            {
                //formula 
                x = (a * x + c) % m;
                //numero que agarro aleatorio y lo convierte en decimal 
                datos.Add(x / 100000);
            }
            return datos;
        }
    }
}