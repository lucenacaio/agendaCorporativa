using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Stefanini.Framework.Extensoes
{
    public static class ExtencoesDeString
    {
        /// <summary>
        /// Retorna apenas os digitos na string
        /// </summary>
        /// <param name="texto">Texto com digitos.</param>
        /// <returns>Apenas os digitos</returns>
        public static string ApenasDigitos(this string texto)
        {
            return Regex.Match(texto, @"\d+").Value;
        }
    }
}
