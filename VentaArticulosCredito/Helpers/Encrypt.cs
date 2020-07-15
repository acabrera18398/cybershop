using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace VentaArticulosCredito.Helpers
{
    public class Encrypt
    {
        public static byte[] Clave = Encoding.ASCII.GetBytes("Vent@Cr3d1T0");
        public static byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");

        /// <summary>
        /// Encripta la contraseña para enviarse a SQL
        /// </summary>
        /// <param name="Cadena">Contraseña</param>
        /// <returns>Cadena encriptada</returns>
        public static string Encripta(string Cadena)
        {

            byte[] inputBytes = Encoding.ASCII.GetBytes(Cadena);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }

            return Convert.ToBase64String(encripted);
        }

        /// <summary>
        /// Desencripta la contraseña
        /// </summary>
        /// <param name="Cadena">Contraseña</param>
        /// <returns>Cadena descentriptada</returns>
        public static string Desencripta(string Cadena)
        {
            byte[] inputBytes = Convert.FromBase64String(Cadena);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }

            return textoLimpio;
        }
    }
}