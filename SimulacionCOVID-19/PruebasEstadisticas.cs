using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulacionCOVID_19
{
    public class PruebasEstadisticas
    {

        public List<double> datos;
        //Declaracion de varibles necesarias para todos los procedimientos
        private double media, ds, rango, tamIntervalo, chiCuadrada, valorIndependencia, valorAleatoriedad;
        public double uniformidadDeseada, independenciaDeseada, aleatoriedadDeseada;
        private double[,] TablaChiCuadrada;
        private int cantDatos, cantInter;
        private bool uniforme, aleatorio, independiente;
        //tabla chi cuadrada con significancia 0.5
        private double[] tablaChi05 = {
            3.8415,
            5.9915,
            7.8147,
            9.4877,
            11.0705,
            12.5916,
            14.0671,
            15.5073,
            16.9190,
            18.3070,
            19.6752,
            21.0261,
            22.3620,
            23.6848,
            24.9958,
            26.2962,
            27.5871,
            28.8693,
            30.1435,
            31.4104,
            32.6706,
            33.9245,
            35.1725,
            36.4150,
            37.6525,
            38.8851,
            40.1133,
            41.3372,
            42.5569,
            43.7730,
            44.9853,
            46.1942
        };

        //getter variables privadas (Regresa valor de variable con el nombre asignado )
        public double Avg => media;
        public double StdDev => ds;
        public double Range => rango;
        public double IntervalSize => tamIntervalo;
        public double ChiSq => chiCuadrada;
        public double IndValue => valorIndependencia;
        public double RandValue => valorAleatoriedad;
        public double[,] ChiSqTable => TablaChiCuadrada;
        public int DataCount => cantDatos;
        public int IntervalCount => cantInter;
        public bool Uniforme => uniforme;
        public bool Aleatorio => aleatorio;
        public bool Independiente => independiente;
        //Constructor que recibe la lista de numeros a probar 
        public PruebasEstadisticas(List<double> datos)
        {
            //Asigna datos recibidos a locales
            this.datos = datos;
            //Formula para sacar media (Suma del lo datos entre el total de datos)
            //La media la utilizamos para aleatoriedad
            media = datos.Sum() / datos.Count;
            cantDatos = datos.Count;

            cantInter = CalcularCantIntervalos();
            rango = CalcularRango();
            tamIntervalo = CalcularTamIntervalos();
            TablaChiCuadrada = generarTablaChiCuadrada();
            //Se manda a llamar metodos 
            comprobarUniformidad();
            comprobarAleatoriedad();
            comprobarIndependencia();
        }
        //Calcular rango para calcular el tamaño de intervalos 
        private double CalcularRango()
        {
            return datos.Max() - datos.Min();
        }

        private int CalcularCantIntervalos()
        {
            //Raiz cuadrada de la cantidad de datos (Ceiling Rendondear limite mayor)
            return (int)Math.Ceiling(Math.Sqrt(cantDatos));
        }
        //Cuantos datos tendran los intervalos 
        private double CalcularTamIntervalos()
        {
            return rango / cantInter;
        }
        private double[,] generarTablaChiCuadrada()
        {
            int contador;
            double inferior, superior;
            double sumatoria = 0;
            double sumatoriaDatos = 0; //debug
            double[,] tabla = new double[cantInter, 6];
            double fe = tamIntervalo * cantDatos;
            //Recorre ña canridad  de intervalos calculados anteriormente 
            for (int i = 0; i < cantInter; i++)
            {
                //Enumerando Intervalos 
                tabla[i, 0] = i + 1;
                //Definiendo limites inferiores 
                tabla[i, 1] = (i == 0) ? 0 : tabla[i - 1, 1] + tamIntervalo;
                //Definiendo limites superiores
                tabla[i, 2] = (i == cantInter - 1) ? 1 : tabla[i, 1] + tamIntervalo;
                //Definiendo frecuencia observada
                //Cantidad de numeros de la cantidad de datos generados dentro del intervalo actual (limite inferior y superior)
                contador = 0;

                inferior = tabla[i, 1];
                superior = tabla[i, 2];
                //666 cantidad de datos 
                for (int j = 0; j < cantDatos; j++)
                {
                    //Si es el ultimo intervalo se compara con el limite inferior 
                    if (i == cantInter - 1)
                    {
                        if (datos[j] >= inferior) contador++;
                    }
                    //si no se compara si el numero esta enre limite inferior y superior
                    else
                    {
                        if (datos[j] >= inferior && datos[j] < superior) contador++;
                    }
                }
                //Asigna la frecuencia observada
                tabla[i, 3] = contador;



                //Definiendo Frecuencia Esperada
                //En caso de que el ultimo intervalo sea de mayor tamaño se calcula la frecuencia esperada correspondiente 
                tabla[i, 4] = (i == cantInter - 1) ? (cantDatos - ((cantInter - 1) * fe)) : fe;


                //Calculando la parte interna de la sumatoria
                //Formula: (observada - esperada ) ^2 / esperada
                tabla[i, 5] = Math.Pow(tabla[i, 3] - tabla[i, 4], 2) / tabla[i, 4];


                //Actualizando sumatoria
                //datos generados con la formulaD
                sumatoria += tabla[i, 5];
                //Suma de datos = 666
                sumatoriaDatos += tabla[i, 3];
            }
            //redondear a 4 decimales (resultados de la sumatoria)
            chiCuadrada = Math.Round(sumatoria, 4);
            return tabla;
        }
        //
        private void comprobarUniformidad()
        {
            //cantidad de intervalos -1
            uniformidadDeseada = tablaChi05[cantInter - 2];
            //uniforme 
            uniforme = (chiCuadrada < uniformidadDeseada);
        }
        //Corridas por arriba y debajo de la media ( distribucion chi cuadrada )
        private void comprobarAleatoriedad()
        {

            //-=0 || += 1
            string signos = "";
            //registrando datos 666
            foreach (var dato in datos)
            {
                //comparar dato con la media 
                if (dato < media) signos += "0";
                else signos += "1";
            }
            string corridas = "";
            //registrando corridas{
            //recorriendo string  de signos 
            for (int i = 0; i < signos.Length - 1; i++)
            {

                corridas += signos[i];
                //Si el siguiente signo es diferente al actual se colo un separador en este caso -
                if (!signos[i].Equals(signos[i + 1])) corridas += "-";
            }
            //generar arreglo con string de corridas 
            string[] ArregloCorridas = corridas.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);


            //obteniendo la corrida mas larga
            //Orderna el arreglo por tamaño de corrida y selecciona la longitud de la primera corrida 
            int corridaMasLarga = ArregloCorridas.OrderByDescending(e => e.Length).FirstOrDefault().Length;


            //arreglo de contadores 
            int[] cantCorridasPorTamaño = new int[corridaMasLarga];
            //Clasificando corridas por tamaños
            //Cuenta cuantas corridas de cada tamaño existen 
            foreach (var corrida in ArregloCorridas)
            {
                cantCorridasPorTamaño[corrida.Length - 1]++;
            }
            double chi = 0;
            //se crea otra tabla de chi cuadrada 
            //3= observada, esperada y sumatoria para generar la siguiente tabla de chi cuadrada 
            double[,] tablaChiCuadrada = new double[corridaMasLarga, 3];
            //Generando tabla chi cuadrada
            for (int i = 0; i < corridaMasLarga; i++)
            {
                //frecuencia observada
                tablaChiCuadrada[i, 0] = cantCorridasPorTamaño[i];
                //esperada (cantidad de datos - el tamaño de la corrida+ 3 / 2 ^ tamaño de la corrida + 1)
                tablaChiCuadrada[i, 1] = (signos.Length - (i + 1) + 3) / Math.Pow(2, (i + 1) + 1);
                //parte interior sumatoria con Formula: (observada - esperada ) ^2 / esperada 
                tablaChiCuadrada[i, 2] = Math.Pow(tablaChiCuadrada[i, 0] - tablaChiCuadrada[i, 1], 2) / tablaChiCuadrada[i, 1];
                //sumatoria  de resultados de la parte interior
                chi += tablaChiCuadrada[i, 2];
            }
            //valor de la sumatoria final redondeado a 4 decimales
            valorAleatoriedad = Math.Round(chi, 4);
            //comparar en la tabla
            aleatoriedadDeseada = tablaChi05[tablaChiCuadrada.GetLength(0) - 2];

            aleatorio = (valorAleatoriedad < aleatoriedadDeseada);
        }
        private void comprobarIndependencia()
        {
            List<string> resultado = new List<string>();
            int[] numerosRepetidos;
            int[] cartas;
            string num;
            string concatenado;
            //Recorriendo los 666 datos generados
            foreach (var dato in datos)
            {
                cartas = new int[5];
                numerosRepetidos = new int[10];
                concatenado = "";
                double partedecimal = dato - Math.Truncate(dato);
                //Convirtiendo la parte decimal a string con 5 digitos rellenados con 0
                if (partedecimal <= 0.00009)
                    num = $"0000{partedecimal.ToString()[0]}";
                else
                    num = partedecimal.ToString().Substring(2).PadRight(5, '0');

                //Recorro el numero generado de 5 digitos 
                //Contando la cantidad que cada numero  del 0 al 9 se repite
                foreach (char digito in num)
                {

                    numerosRepetidos[int.Parse(digito.ToString())]++;
                }
                //Registrando cuantas cartas de cada valor se obtuvieron del 1 al 5 
                foreach (int cant in numerosRepetidos)
                {
                    if (cant > 0)
                    {
                        cartas[cant - 1]++;
                    }
                }
                //Registrando la mano  (Concatenar el digitos anteriores )
                for (int i = 0; i < cartas.Length; i++)
                {
                    concatenado += cartas[i].ToString();
                }
                resultado.Add(concatenado);
            }
            //7 combinaciones diferentes 
            /* PAR 31000
             * DOS PARES 12000
             * TERCIA 20100  
             * POKER  10010 
             * QUINTILLA 00001
             * FULL 01100
             * TODAS DIFERENTES 50000
             * */
            int[] frecuenciasObservadas = new int[7];
            foreach (var mano in resultado)
            {
                //se cuenta con 666 manos y se analiza cuantas hay de cada una 
                //Contando cuantas de cada mano se obtuvieron
                switch (mano)
                {
                    case "31000":
                        frecuenciasObservadas[0]++;
                        break;
                    case "12000":
                        frecuenciasObservadas[1]++;
                        break;
                    case "20100":
                        frecuenciasObservadas[2]++;
                        break;
                    case "10010":
                        frecuenciasObservadas[3]++;
                        break;
                    case "00001":
                        frecuenciasObservadas[4]++;
                        break;
                    case "01100":
                        frecuenciasObservadas[5]++;
                        break;
                    case "50000":
                        frecuenciasObservadas[6]++;
                        break;
                }
            }
            //Generando tabla chi cuadrada
            //sumatoria
            double chi = 0;
            //tabla a generar 
            double[,] tablaXi = new double[frecuenciasObservadas.Length, 3];
            //Columna frecuencia esperada
            tablaXi[0, 1] = .50400 * cantDatos; //Par (50.4 % de posibilidad)
            tablaXi[1, 1] = .10800 * cantDatos; //Dos Pares(10.8 % de posibilidad)
            tablaXi[2, 1] = .07200 * cantDatos; //Tercia(7.2 % de posibilidad)
            tablaXi[3, 1] = .00450 * cantDatos; //Poker( 0.45 % de posibilidad)
            tablaXi[4, 1] = .00010 * cantDatos; //Quintilla(0.01% de posibilidad)
            tablaXi[5, 1] = .00900 * cantDatos; //FULL(0.9 % de posibilidad)
            tablaXi[6, 1] = .30240 * cantDatos; //Todas diferentes(30.24 % de posibilidad)

            //Recorro frecuencias observadas
            for (int i = 0; i < frecuenciasObservadas.Length; i++)
            {
                //Frecuencia observada
                tablaXi[i, 0] = frecuenciasObservadas[i];
                //parte interior sumatoria Formula: (observada - esperada ) ^2 / esperada 
                tablaXi[i, 2] = Math.Pow(tablaXi[i, 0] - tablaXi[i, 1], 2) / tablaXi[i, 1];
                //sumatoria 
                chi += tablaXi[i, 2];
            }
            //se redondea a 4 decimales
            valorIndependencia = Math.Round(chi, 4);
            //Comparo valores en la tabla 
            independenciaDeseada = tablaChi05[tablaXi.GetLength(0) - 2];

            independiente = (valorIndependencia < independenciaDeseada);
        }

    }
}
