using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using VentaArticulosCredito.DBEntities;
using VentaArticulosCredito.Models.Login;

namespace VentaArticulosCredito.Helpers
{
    public class Token
    {

        /// <summary>
        /// Ruta del archivo de Tokens
        /// </summary>
        public static string ruta = AppDomain.CurrentDomain.BaseDirectory + @"Content\Tokens\Tokens.xml";

        /// <summary>
        /// Guarda la solicitud con su token y usuario
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="token">Token</param>
        public static void GuardarToken(string correo, string token)
        {
            XDocument xml = XDocument.Load(ruta);
            XElement nodo = new XElement("Token");
            nodo.Add(new XElement("correo", correo));
            nodo.Add(new XElement("valorToken", token));
            nodo.Add(new XElement("fechaEmision", DateTime.Now.ToString("dd/MM/yyyy hh:mm")));
            nodo.Add(new XElement("fechaExpiracion", DateTime.Now.AddHours(24.00).ToString("dd/MM/yyyy hh:mm")));
            nodo.Add(new XElement("estado", 0));
            xml.Element("Tokens").Add(nodo);
            xml.Save(ruta);
        }

        /// <summary>
        /// Valida si el token es válido
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="token">Token</param>
        /// <returns>True o False</returns>
        public static Boolean LeerToken(string correo, string token)
        {
            Boolean valido = false;
            XDocument xml = XDocument.Load(ruta);

            try
            {
                List<Models.Token> listaTokens = xml.Descendants("Token").Select(t => new Models.Token
                {
                    correo = t.Element("correo").Value,
                    token = t.Element("valorToken").Value,
                    fechaEmision = Convert.ToDateTime(t.Element("fechaEmision").Value),
                    fechaExpiracion = Convert.ToDateTime(t.Element("fechaExpiracion").Value),
                    estado = Convert.ToInt32(t.Element("estado").Value)
                }).ToList();

                if (listaTokens.Count > 0)
                    if (listaTokens.Where(t => t.correo == correo && t.token == token && t.fechaEmision <= DateTime.Now && t.fechaExpiracion >= DateTime.Now && t.estado == 0).FirstOrDefault() != null)
                        valido = true;
            }
            catch (Exception ex)
            {

            }

            return (valido);
        }

        /// <summary>
        /// Valida si el usuario ya tiene un token válido
        /// </summary>
        /// <param name="usuario">Usuario</param>
        /// <param name="token">Token</param>
        /// <returns>True o False</returns>
        public static Boolean LeerToken(string correo)
        {
            Boolean valido = false;
            XDocument xml = XDocument.Load(ruta);

            try
            {
                List<Models.Token> listaTokens = xml.Descendants("Token").Select(t => new Models.Token
                {
                    correo = t.Element("correo").Value,
                    token = t.Element("valorToken").Value,
                    fechaEmision = Convert.ToDateTime(t.Element("fechaEmision").Value),
                    fechaExpiracion = Convert.ToDateTime(t.Element("fechaExpiracion").Value),
                    estado = Convert.ToInt32(t.Element("estado").Value)
                }).ToList();

                if (listaTokens.Count > 0)
                    if (listaTokens.Where(t => t.correo == correo && t.fechaEmision <= DateTime.Now && t.fechaExpiracion >= DateTime.Now && t.estado == 0).FirstOrDefault() != null)
                        valido = true;
            }
            catch (Exception ex)
            {

            }

            return (valido);
        }

        /// <summary>
        /// Modifica el estado del token utilizado despues de cambiar la contraseña
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="token"></param>
        public static void ModificarToken(string correo, string token)
        {
            XDocument xml = XDocument.Load(ruta);
            var query = from t in xml.Elements("Tokens").Elements("Token") select t;

            try
            {
                foreach (XElement t in query)
                {
                    if (t.Element("correo").Value == correo && t.Element("valorToken").Value == token && t.Element("estado").Value == "0")
                        t.Element("estado").Value = "1";
                }

                xml.Save(ruta);
            }
            catch (Exception ex)
            {

            }
        }
    }
}