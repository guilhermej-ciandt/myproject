using System;
using System.Linq;
using NUnit.Framework;

namespace AG.Framework.Domain
{

    [TestFixture]
    public class PeriodoTests
    {

        [Test]
        public void EnumeratorDevePassarPorTodosOsMesesDoPeriodo()
        {
            var periodo = new Periodo(new AnoMes(201001), new AnoMes(201012));
            var counter = 0;
            foreach (var anoMes in periodo)
            {
                Console.WriteLine(anoMes);
                Assert.That(counter, Is.LessThanOrEqualTo(12));
                counter = counter + 1;
            }
            Assert.That(counter, Is.EqualTo(12));
        }


        [Test]
        public void AnosDevePassarPorTodosOsAnosDoPeriodo()
        {
            var periodo = new Periodo(new AnoMes(201001), new AnoMes(202112));
            var counter = 0;
            foreach (var ano in periodo.Anos())
            {
                Console.WriteLine(ano);
                Assert.That(counter, Is.LessThanOrEqualTo(12));
                counter = counter + 1;
            }
            Assert.That(counter, Is.EqualTo(12));
        }

        [Test]
        public void AnoInicioDeveSerAnteriorAnoFinal()
        {
            Assert.Throws<ArgumentException>(delegate
            {
                var periodo = new Periodo(new AnoMes(202112), new AnoMes(201001));
            }
            );
        }

        [Test]
        public void PeriodosPorAnoDeveRetornarPeriodosAnuais()
        {
            var periodo = new Periodo(new AnoMes(200012), new AnoMes(201001));
            Assert.That(periodo.PeriodosPorAno().Count(), Is.EqualTo(11));
        }

        [Test]
        public void T()
        {
            var periodo = new Periodo(2010, 2012);
            var periodos = periodo.PeriodosPorAno().ToList();
            periodos.AddRange(periodo.PeriodosPorMes().ToArray());

            //var periodos = periodo.PeriodosPorAno().Select(p => p.AnoMesInicio.ToInt() + ":" + p.AnoMesfim.ToInt()).ToList();
            //periodos.AddRange(periodo.PeriodosPorMes().Select(p => p.AnoMesInicio.ToInt() + ":" + p.AnoMesfim.ToInt()).ToArray());
            foreach (var list in periodos.OrderBy(p => p.AnoMesInicio))
            {
                Console.WriteLine(list);
            }
            Assert.That(periodos.Count, Is.EqualTo(39));
        }

        [Test]
        public void PedePeriodoAcumuladoAteJunho()
        {
            var acumulado = Periodo.PeriodoAcumulado(201006);
            Assert.AreEqual(201001, acumulado.AnoMesInicio.Value);
            Assert.AreEqual(201006, acumulado.AnoMesFim.Value);
        }

        [Test]
        public void PedePeriodoAcumuladoComMesInvalido()
        {
            Assert.Throws<ArgumentException>(() => Periodo.PeriodoAcumulado(201023));
        }

        [Test]
        public void VerificaProjecaoSemRealAteRetornaFalse()
        {
            Assert.Throws<ArgumentNullException>(() => Periodo.PeriodoAcumulado(201107).IsProjetado());

        }

        [Test]
        public void VerificaProjecaoComRealAteMaiorQueDataFinalRetornaFalse()
        {
            var umPeriodo = Periodo.PeriodoAcumulado(201107, 201111);
            Assert.IsFalse(umPeriodo.IsProjetado());
        }

        [Test]
        public void VerificaProjecaoComRealAteIgualADataFinalRetornaFalse()
        {
            var umPeriodo = Periodo.PeriodoAcumulado(201007, 201007);
            Assert.IsFalse(umPeriodo.IsProjetado());
        }

        [Test]
        public void VerificaProjecaoComRealAteMenorQueDataFinalRetornaTrue()
        {
            var umPeriodo = Periodo.PeriodoAcumulado(201107, 201003);
            Assert.IsTrue(umPeriodo.IsProjetado());
        }
    }
}