using System;
using System.Collections.Generic;
using System.Globalization;
using AG.Framework.Domain;

namespace AG.Framework.Utils
{
    public class DateTimeUtils
    {
        public static readonly string yyyyMM = "yyyyMM";
        public static readonly string ddMMyyyy = "dd/MM/yyyy";
        public static readonly string MMyyyy = "MM/yyyy";
        public static readonly string[] shortMonth = { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };

        /// <summary>
        /// Verifica se o inteiro está no formato yyyyMM
        /// </summary>
        /// <param name="anoMes"></param>
        /// <returns></returns>
        public static bool IsAnoMesValido(int anoMes)
        {
            var valid = true;
            var anoMesStr = anoMes.ToString();
            if (anoMesStr.Length != 6)
            {
                valid = false;
            }
            else
            {
                //var ano = ExtraiAnoDeString(anoMesStr);
                var mes = ExtraiMesDeString(anoMesStr);
                if (mes < 1 || mes > 12)
                {
                    valid = false;
                }
            }
            return valid;
        }


        public static int ExtraiAno(int anoMes)
        {
            return ExtraiAnoDeString(anoMes.ToString());
        }

        public static int ExtraiAnoDeString(string anoMesStr)
        {
            ValidaStringDeData(anoMesStr);

            return Convert.ToInt32(anoMesStr.Substring(0, 4));
        }

        private static void ValidaStringDeData(string anoMesStr)
        {
            if (String.IsNullOrEmpty(anoMesStr))
            {
                throw new ArgumentException("Parametro nao pode ser vazio ou nulo.");
            }

            if (anoMesStr.Length != 6)
            {
                throw new ArgumentException("Parametro deve ter pelo 6 caracteres.");
            }
        }

        public static int ExtraiMes(int anoMes)
        {
            return ExtraiMesDeString(anoMes.ToString());
        }

        public static int ExtraiMesDeString(string anoMesStr)
        {
            ValidaStringDeData(anoMesStr);

            return Convert.ToInt32(anoMesStr.Substring(4, 2));
        }

        /// <summary>
        /// Converte para DateTime inteiros no formato: yyyyMM
        /// </summary>
        /// <param name="anoMes"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(int anoMes)
        {
            if (IsAnoMesValido(anoMes))
            {
                var anoMesStr = anoMes.ToString();
                var ano = ExtraiAnoDeString(anoMesStr);
                var mes = ExtraiMesDeString(anoMesStr);
                return new DateTime(ano, mes, 1);
            }
            throw new ArgumentException(string.Format("Inteiro {0} deve estar no formato: yyyyMM", anoMes.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anoMes"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTimeComUltimoDiaDoMes(int anoMes)
        {
            return PreencheComUltimoDiaDoMes(ConvertToDateTime(anoMes));
        }

        /// <summary>
        /// Retorna uma data com o mesmo mês e ano da data de entrada 
        /// mas com o dia atualizado para o último dia do mês 
        /// e as informações de hora zeradas.
        /// </summary>
        /// <param name="data">data de entrada</param>
        /// <returns>nova data alterada</returns>
        public static DateTime PreencheComUltimoDiaDoMes(DateTime data)
        {
            var daysInMonth = DateTime.DaysInMonth(data.Year, data.Month);
            return new DateTime(data.Year, data.Month, daysInMonth);
        }

        public static DateTime? PreencheComUltimoDiaDoMes(DateTime? data)
        {
            if (data == null)
            {
                return null;
            }
            return PreencheComUltimoDiaDoMes((DateTime)data);
        }

        public static string Formata(int anoMes, string formato)
        {
            return ConvertToDateTime(anoMes).ToString(formato);
        }

        public static int Formata(DateTime anoMes)
        {
            return Convert.ToInt32((anoMes.Year * 100) + anoMes.Month);
        }

        public static string FormataParaMMyyyy(int anoMes)
        {
            return ConvertToDateTime(anoMes).ToString(MMyyyy);
        }

        // ReSharper disable InconsistentNaming
        public static string ConvertePara_ddMMYYYY(DateTime data)
        // ReSharper restore InconsistentNaming
        {
            return data.ToString(ddMMyyyy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        public static string PreencheComUltimoDiaDoMes(string dataString)
        {
            var d = DateTime.ParseExact(dataString, ddMMyyyy, CultureInfo.InvariantCulture);
            return PreencheComUltimoDiaDoMes(d).ToString(ddMMyyyy);
        }

        public static int ConverteDateTimeToAnoMes(DateTime data)
        {
            return Int32.Parse(data.ToString(yyyyMM));
        }

        /// <summary>
        /// Retorna a maior data
        /// </summary>
        /// <param name="second"></param>
        /// <param name="first"></param>
        /// <returns></returns>
        public static DateTime? MaxDateTime(DateTime? first, DateTime? second)
        {
            return first.Value.CompareTo(second) >= 0 ? first : second;
        }

        /// <summary>
        /// Retorna a menor data
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime? MinDateTime(DateTime? first, DateTime? second)
        {
            return first.Value.CompareTo(second) >= 0 ? second : first;
        }

        /// <summary>
        /// Gera uma lista de inteiros de AnoMes (yyyymm) apartir de uma data
        /// usando o total de meses(param) sempre incrementando
        /// </summary>
        /// <param name="anoMes">Marco inicial para gerar a lista de AnoMes</param>
        /// <param name="meses">Total de meses a serem gerados</param>
        /// <returns></returns>
        public static IList<int> GeraAnoMesInc(int anoMes, int meses)
        {
            var anoMesStr = anoMes.ToString();
            var ano = (ExtraiAnoDeString(anoMesStr) * 100);
            var mes = ExtraiMesDeString(anoMesStr);

            var result = new List<int>();

            for (int i = 0; i < meses; i++, mes++)
            {
                result.Add(ano + mes);

                if (mes == 12)
                {
                    mes = 0;
                    ano += 100;
                }
            }

            return result;
        }


        /// <summary>
        /// Gera uma lista de inteiros de AnoMes (yyyymm) apartir de uma data
        /// usando o total de meses(param) sempre incrementando
        /// </summary>
        /// <param name="anoMes">Marco inicial para gerar a lista de AnoMes</param>
        /// <param name="meses">Total de meses a serem gerados</param>
        /// <returns></returns>
        public static int AdicionaMesesAData(int anoMes, int meses)
        {
            int ano = anoMes / 100;
            int novoMes = Convert.ToInt32(anoMes.ToString().Substring(4, 2));

            if (meses >= 0)
            {
                int mes = meses + Convert.ToInt32(anoMes.ToString().Substring(4, 2)) + 1;
                while (novoMes < mes)
                {
                    if (novoMes == 12)
                    {
                        ano = ano + 1;
                        novoMes = 0;
                        mes = mes - 12;
                    }
                    else novoMes++;
                }
            }
            if (meses < 0)
            {
                int contador = meses + 1;
                while (contador < 0)
                {
                    if (novoMes == 1)
                    {
                        ano = ano - 1;
                        novoMes = 12;
                    }
                    else novoMes--;

                    contador++;
                }
            }

            return (novoMes >= 10)
                       ? Convert.ToInt32(ano.ToString() + novoMes.ToString())
                       : Convert.ToInt32(ano.ToString() + "0" + novoMes.ToString());
        }




        /// <summary>
        /// Gera uma lista de inteiros de AnoMes (yyyymm) apartir de uma data
        /// usando o total de meses(param) sempre decrementando
        /// </summary>
        /// <param name="anoMes">Marco inicial para gerar a lista de AnoMes</param>
        /// <param name="meses">Total de meses a serem gerados</param>
        /// <returns></returns>
        public static IList<int> GeraAnoMesDec(int anoMes, int meses)
        {
            var anoMesStr = anoMes.ToString();
            var ano = (ExtraiAnoDeString(anoMesStr) * 100);
            var mes = ExtraiMesDeString(anoMesStr);

            var result = new List<int>();

            for (int i = 0; i < meses; i++, mes--)
            {
                result.Add(ano + mes);

                if (mes == 1)
                {
                    mes = 13;
                    ano -= 100;
                }
            }

            return result;
        }

        /// <summary>
        /// Retorna o nome abreviado de um mês
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static string GetShortMes(int mes)
        {
            if (mes > 0 && mes < 13)
                return shortMonth[mes - 1];
            else
                throw new Exception("Parametro mês deve estar entre 1 e 12.");
        }

        /// <summary>
        /// Calcula o tempo decorrido entre dois DateTimes e retorna uma string amigavel
        /// separando seg, h  e min.
        /// </summary>
        /// <param name="dataHoraFim"></param>
        /// <param name="dataHoraInicio"></param>
        /// <returns></returns>
        public static string CalculaTempoDecorrido(DateTime? dataHoraFim, DateTime? dataHoraInicio)
        {
            string duracao = "";
            if (dataHoraFim != null && dataHoraInicio != null)
            {
                const string seg = " seg ";
                const string hora = " h ";
                const string min = " min ";
                var timeSpan = dataHoraFim.Value.Subtract(dataHoraInicio.Value);

                if (timeSpan.Hours > 0)
                {
                    duracao = timeSpan.Hours + hora;
                }

                if (timeSpan.Minutes > 0)
                {
                    duracao += timeSpan.Minutes + min;
                }

                if (timeSpan.Seconds > 0)
                {
                    duracao += timeSpan.Seconds + seg;
                }
                else
                {
                    if (timeSpan.Milliseconds > 0)
                    {
                        duracao += Decimal.Round(((decimal)timeSpan.Milliseconds / (decimal)1000), 2) + seg;
                    }
                    else
                    {
                        duracao += 0 + seg;
                    }
                }
            }
            return duracao;
        }

        public static AnoMes GetInicioDosTempos()
        {
            return new AnoMes(180001);
        }

        public static DateTime? AdicionaMeioDia(DateTime? data)
        {
            if (data.HasValue && data.Value.Hour == 0)
            {
                return data.Value.AddHours(12);
            }
            return data;
        }


    }
}
