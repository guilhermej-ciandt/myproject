using System;
using System.Collections;
using System.Collections.Generic;
using Spring.Util;

namespace AG.Framework.Domain
{
    [Serializable]
    public class Periodo : IEnumerable<AnoMes>
    {
        public const int MesJaneiro = 1;
        public const int MesDezembro = 12;

        public AnoMes AnoMesInicio { get; private set; }
        public AnoMes AnoMesFim { get; private set; }
        public AnoMes RealAte { get; set; }

        public Periodo(AnoMes inicio, AnoMes fim, AnoMes realAte) : this(inicio, fim)
        {
            RealAte = realAte;
        }

        public Periodo(AnoMes inicio, AnoMes fim)
        {
            AssertUtils.IsTrue( inicio <= fim, "anoMes inicio dever ser menor ou igual ao anoMes fim" );
            AnoMesInicio = inicio;
            AnoMesFim = fim;
        }

        public Periodo(int anoInicial, int anoFinal) 
            : this(new AnoMes( anoInicial, MesJaneiro ), new AnoMes( anoFinal, MesDezembro) )
        {}

        public Periodo(int anoMesInicial, int anoMesFinal, int anoMesRealAte) : 
            this(new AnoMes(anoMesInicial), new AnoMes(anoMesFinal), new AnoMes(anoMesRealAte))
        {}

        public static Periodo PeriodoExercicio(int ano)
        {
            return new Periodo(ano, ano);
        }

        public static Periodo PeriodoAcumulado(int anoMesFinal)
        {
            var ano = anoMesFinal/100; //elimina mes
            var mes = anoMesFinal%100; //obtem o mes
            AssertUtils.IsTrue(mes <= MesDezembro, "mes deve ser no máximo 12, equivalente a dezembro");
            return new Periodo(new AnoMes(ano, MesJaneiro), new AnoMes(anoMesFinal));
        }

        public static Periodo PeriodoAcumulado(int anoMesFinal, int anoMesRealAte)
        {
            var periodoAcumulado = PeriodoAcumulado(anoMesFinal);
            periodoAcumulado.RealAte = new AnoMes(anoMesRealAte);
            return periodoAcumulado;
        }

        public static Periodo PeriodoMensal(int anoMes)
        {
            return new Periodo(new AnoMes(anoMes), new AnoMes(anoMes));
        }

        public bool IsProjetado()
        {
            if ( RealAte == null ) 
                throw new ArgumentNullException("RealAte deve ter valor para saber se o periodo é projetado"); 
            return AnoMesFim > RealAte;
        }

        public IEnumerable<Periodo> PeriodosPorAno()
        {
            foreach (var ano in Anos())
            {
                yield return new Periodo(
                                 new AnoMes(ano, MesJaneiro),
                                 new AnoMes(ano, MesDezembro)
                         );
                
            }
        }

        public IEnumerable<Periodo> PeriodosPorMes()
        {
            foreach (var mes in this)
            {
                yield return new Periodo(mes,mes);
            }
        }


        public IEnumerable<int> Anos()
        {
            var anoCorrente = AnoMesInicio.Ano();
            while (anoCorrente <= AnoMesFim.Ano())
            {
                yield return anoCorrente;
                anoCorrente++;
            }
        }

        public IEnumerator<AnoMes> GetEnumerator()
        {
            var anoMesCorrente = new AnoMes(AnoMesInicio.Value);
            while (anoMesCorrente.Value <= AnoMesFim.Value)
            {
                yield return anoMesCorrente;
                anoMesCorrente++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return String.Format("De {0} até {1}", AnoMesInicio, AnoMesFim);
        }

    }
}
