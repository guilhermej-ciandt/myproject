using NUnit.Framework;
using System;

namespace AG.Framework.Domain
{
    
    [TestFixture]
    public class AnoMesTests
    {

        [Test]
        public void ConstrutorComAnoMesSeparados()
        {
            var anoMes = new AnoMes(2010 , 01);
            Assert.That(anoMes.Value, Is.EqualTo(201001));
        }

        [Test]
        public void SobrecargaOperadores()
        {
            var menor = new AnoMes(201001);
            var maior = new AnoMes(201101);
            var refMenor = menor;
            var refMaior = maior;
            
            Assert.That(maior > menor, Is.True);
            Assert.That(maior < menor, Is.False);
            Assert.That(maior < menor, Is.False);
            Assert.That(menor > maior, Is.False);
            Assert.That(maior == menor, Is.False);
            Assert.IsTrue(maior == refMaior);
            Assert.IsTrue(maior != menor);
            Assert.IsFalse(menor != refMenor);
            Assert.IsTrue(menor != null);
            Assert.IsTrue(maior != null);
            Assert.IsFalse(menor == null);
            Assert.IsFalse(maior == null);
        }

        [Test]
        public void SobrecargaOperadorIgualComUmDosArgumentosNuloRetornaFalse()
        {
            var anoMes = new AnoMes(201103);
            Assert.IsFalse(anoMes == null);
            Assert.IsTrue(anoMes != null);

            anoMes = null;
            Assert.IsTrue(anoMes == null);
            Assert.IsFalse(anoMes != null);
        }

        [Test]
        public void ComparandoNumerosIguais()
        {
            var anoMes1 = new AnoMes(201103);
            var anoMes2 = new AnoMes(201103);

            Assert.That(anoMes1 == anoMes2, Is.True);
            Assert.That(anoMes1 > anoMes2, Is.False);
            Assert.That(anoMes1 < anoMes2, Is.False);
        }

        [Test]
        public void SobrecargaOperadorInc()
        {
            var anoMes = new AnoMes(201001);
            anoMes++;
            Assert.That(anoMes.Value, Is.EqualTo(201002));
        }

        [Test]
        public void SobrecargaOperadorIncCruzandoAnos()
        {
            var anoMes = new AnoMes(201012);
            anoMes++;
            Assert.That(anoMes.Value, Is.EqualTo(201101));
        }

        [Test]
        public void SobrecargaOperadorDec()
        {
            var anoMes = new AnoMes(201002);
            anoMes--;
            Assert.That(anoMes.Value, Is.EqualTo(201001));
        }

        [Test]
        public void SobrecargaOperadorDecCruzandoAnos()
        {
            var anoMes = new AnoMes(201001);
            anoMes--;
            Assert.That(anoMes.Value, Is.EqualTo(200912));
        }

        [Test]
        public void Now()
        {
            var anoMes = AnoMes.Now;
            var now = DateTime.Now;
            Assert.That(anoMes.Ano(), Is.EqualTo(now.Year));
            Assert.That(anoMes.Mes(), Is.EqualTo(now.Month));
        }
    }
}
