using System;
using System.Linq;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

namespace AG.Framework.Utils
{
    /// <summary>
    /// Classe utilitária com métodos para manipulação de strings
    /// </summary>
    public class StringUtils
    {

        /// <summary>
        /// Remove os acentos de uma string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String RemoveDiacritics(String s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < normalizedString.Length; i++)
            {
                var c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Retorna a collection como uma string separada pelo delimitador fornecido
        /// </summary>
        /// <param name="c"></param>
        /// <param name="delimiter"></param>
        /// <param name="delimiterAtEnd"></param>
        /// <returns></returns>
        public static String CollectionToDelimitedString(ICollection<object> c, string delimiter, bool delimiterAtEnd)
        {
            if (c == null || c.Count == 0) return null;
            
            var str = Spring.Util.StringUtils.CollectionToDelimitedString(c, delimiter);

            if (delimiterAtEnd)
            {
                str += delimiter;
            }

            return str;
        }

        /// <summary>
        /// Função usada para transformar caminhos de organogramas (hierárquias) em string para ser usada pelo componente
        /// de arvore de checkbox do extJs
        /// </summary>
        /// <param name="caminhos"></param>
        /// <returns>
        /// </returns>
        public static List<string> TransformaCaminhosDeOrganogramaParaString(List<List<int>> caminhos)
        {
            var caminhosString = caminhos
                .Select(caminho => string.Join("/", caminho.ConvertAll(item => item.ToString()).ToArray()))
                .Select(idsOrganogramas => string.Format("/-1/{0}", idsOrganogramas))
                .ToList();
            return caminhosString;
        }
    }
}