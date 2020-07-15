using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaArticulosCredito.Helpers
{
    public class ValidarNIT
    {
        /// <summary>
        /// Valida si el nit ingresado por el usuario es válido
        /// </summary>
        /// <param name="nit">NIT</param>
        /// <returns>True o false para válido o invalido</returns>
        public static Boolean ValidaNIT(string nit)
        {
            Boolean valido = false;

            //Valida que se haya ingresado un guión (-)
            if (!nit.Contains("-")) 
                return false;

            //Separa el nit por el guión (-)
            var separarNIT = nit.Split('-');
            int suma = 0;
            int posicion = 2;
            int resta = 0;
            int multiplicacion = 0;
            int residuo = 0;
            int verificador = 0;

            try
            {
                //Asigna las posiciones respectivas a los números
                //a la izquierda del guión (-) empezando de derecha a izquierda
                //desde dos, se multiplica el número por la pocisión y se suman 
                //todas las multiplicaciones
                for (int i = separarNIT[0].Length - 1; i >= 0; i--)
                {
                    suma += (Convert.ToInt32(separarNIT[0].Substring(i, 1)) * posicion);

                    //suma uno a la posición
                    posicion += 1;
                }

                //Resta a la suma el resultado de dividir la suma entre 11, tomar
                //el valor entero y multiplicarlo por 11
                resta = (suma - (Convert.ToInt32((suma / 11).ToString().Split('.')[0]) * 11));

                //Divide la resta entre 11, tomando el valor entero
                //y lo multiplica por 11
                multiplicacion = Convert.ToInt32((resta / 11).ToString().Split('.')[0]) * 11;

                //Resta la multiplicacion a la resta
                residuo = (resta - multiplicacion);

                //Resta el residuo a 11
                verificador = (11 - residuo);

                //Si el verificador del nit es K, la operación debe ser igual a 10
                if (separarNIT[1].ToUpper().Equals("K"))
                {
                    if (verificador.ToString().Equals("10"))
                        valido = true;
                }
                else
                {
                    if (verificador.ToString().Equals(separarNIT[1]))
                        valido = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return valido;
        }
    }
}