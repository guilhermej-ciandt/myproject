using System;
using AG.Framework.Utils;

namespace AG.Framework.Domain
{
    [Serializable]
    public class AnoMes : IComparable
    {
        private int _value;
        private int _ano;
        private int _mes;

        #region construtores

        // para serialização
        public AnoMes()
        {
        }

        public AnoMes(int anoMes)
        {
            InicializaValor(anoMes);
        }

        private void InicializaValor(int anoMes)
        {
            if (!DateTimeUtils.IsAnoMesValido(anoMes))
            {
                throw new ArgumentException("AnoMes deve estar no formato: " + DateTimeUtils.yyyyMM);
            }
            _value = anoMes;
            _ano = DateTimeUtils.ExtraiAno(Value);
            _mes = DateTimeUtils.ExtraiMes(Value);
        }

        public AnoMes(DateTime anoMes)
        {
            if (anoMes==null)
            {
                throw new ArgumentNullException("anoMes");
            }
            _value = DateTimeUtils.ConverteDateTimeToAnoMes(anoMes);
            _ano = DateTimeUtils.ExtraiAno(Value);
            _mes = DateTimeUtils.ExtraiMes(Value);
        }

        public AnoMes(int ano, int mes)
        {
            if (!(ano >= DateTime.MinValue.Year && ano <= DateTime.MaxValue.Year))
            {
              throw  new Exception(String.Format("Ano deve estar entre {0} e {1}", DateTime.MinValue.Year, DateTime.MaxValue.Year));
            }

            if (!(mes > 0 && mes <= 12))
            {
                throw new Exception("Mês deve estar entre 1 e 12.");
            }

            var mesStr = mes < 10 ? "0" + mes : mes.ToString();
            _value = Convert.ToInt32(ano + mesStr);
            _ano = ano;
            _mes = mes;
        }

        #endregion

        public int Value
        {
            get { return _value; }
            set { InicializaValor(value); }
        }

        public static AnoMes Now
        {
            get
            {
                var now = DateTime.Now;
                return new AnoMes( now.Year, now.Month );
            }
        }

        public int Ano()
        {
            return _ano;
        }

        public int Mes()
        {
            return _mes;
        }

        public override string ToString()
        {
            return DateTimeUtils.FormataParaMMyyyy(Value);
        }

        public int CompareTo(object obj)
        {
            var anoMes = (AnoMes)obj;
            if (this > anoMes)
            {
                return 1;
            }

            if (this < anoMes)
            {
                return -1;
            }
            return 0;
        }

        public DateTime ToDateTime()
        {
            return DateTimeUtils.ConvertToDateTimeComUltimoDiaDoMes(Value);
        }

        public static bool operator >(AnoMes c1, AnoMes c2)
        {
            return c1.Value > c2.Value;
        }

        public static bool operator <(AnoMes c1, AnoMes c2)
        {
            return c1.Value < c2.Value;
        }

        public static bool operator >=(AnoMes c1, AnoMes c2)
        {
            return c1.Value >= c2.Value;
        }

        public static bool operator <=(AnoMes c1, AnoMes c2)
        {
            return c1.Value <= c2.Value;
        }

        public static bool operator ==(AnoMes c1, AnoMes c2)
        {
            if (Equals(c1, c2)) return true;

            return (object) c1 != null && (object) c2 != null && c1.Value == c2.Value;
        }

        public static bool operator !=(AnoMes c1, AnoMes c2)
        {
            if (!Equals(c1, c2)) return true;

            return (object)c1 != null && (object) c2 != null && c1.Value != c2.Value;
        }

        public static AnoMes operator ++(AnoMes anoMes)
        {
            var ano = anoMes.Ano();
            var mes = anoMes.Mes();
            if (mes == 12)
            {
                mes = 1;
                ano = ano + 1;
            }
            else
            {
                mes = mes + 1;
            }
            return new AnoMes(ano, mes);
        }

        public static AnoMes operator --(AnoMes anoMes)
        {
            var ano = anoMes.Ano();
            var mes = anoMes.Mes();
            if (mes == 1)
            {
                mes = 12;
                ano = ano - 1;
            }
            else
            {
                mes = mes - 1;
            }
            return new AnoMes(ano, mes);
        }

        public bool Equals(AnoMes other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Value == Value && other._ano == _ano && other._mes == _mes;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AnoMes)) return false;
            return Equals((AnoMes) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Value;
                result = (result*397) ^ _ano;
                result = (result*397) ^ _mes;
                return result;
            }
        }
    }
}
