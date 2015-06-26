using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AG.Framework.Log;

namespace AG.Framework.Domain
{
    [TestFixture]
    public class LogEventTests
    {

        [Test]
        public void LogSemFilhosDeveTerOProprioStatus()
        {
            var logEvent = new LogEvent { Status = StatusLog.Success };
            Assert.AreEqual(StatusLog.Success, logEvent.Status);
        }

        [Test]
        public void FilhoComWarningPaiDeveSerWarning()
        {
            var logEventPai = new LogEvent { Status = StatusLog.Success };
            logEventPai.AdicionaFilho(new LogEvent { Status = StatusLog.Warning });
            Assert.AreEqual(StatusLog.Warning, logEventPai.Status, "Pai deve ser Warning");
        }

        [Test]
        public void FilhoComErrorPaiDeveSerError()
        {
            var logEventPai = new LogEvent { Status = StatusLog.Success };
            logEventPai.AdicionaFilho(new LogEvent { Status = StatusLog.Error });
            Assert.AreEqual(StatusLog.Error, logEventPai.Status, "Pai deve ser Error");
        }

        [Test]
        public void LogComFilhosComStatusErroOPaiTambemDeveSerErro()
        {
            //prescedência: Erro, Warning, Sucesso
            
            var logWarning = new LogEvent {Status = StatusLog.Warning};
            var logError = new LogEvent {Status = StatusLog.Error};
            
            var logEventPai = new LogEvent { Status = StatusLog.Success };
            logEventPai.AdicionaFilho(logError);
            logEventPai.AdicionaFilho(logWarning);

            Assert.AreEqual(StatusLog.Error, logEventPai.Status);

        }

        [Test]
        public void CasoGrande()
        {
            var logEventPai = new LogEvent { Status = StatusLog.Success };
            logEventPai.AdicionaFilho(new LogEvent
                                          {
                                              Status = StatusLog.Success,
                                              Events = new List<LogEvent>
                                                           {
                                                               new LogEvent
                                                                   {
                                                                       Status = StatusLog.Warning
                                                                   },
                                                               new LogEvent
                                                                   {
                                                                       Status = StatusLog.Success
                                                                   }
                                                           }

                                          });
            logEventPai.AdicionaFilho(new LogEvent
                                          {
                                              Status = StatusLog.Success,
                                              Events = new List<LogEvent>
                                                           {
                                                               new LogEvent
                                                                   {
                                                                       Status = StatusLog.Error
                                                                   }
                                                           }
                                          });

            logEventPai.AdicionaFilho(new LogEvent
                                          {
                                              Status = StatusLog.Success,
                                              Events = new List<LogEvent>
                                                           {
                                                               new LogEvent
                                                                   {
                                                                       Status = StatusLog.Success
                                                                   }
                                                           }
                                          });

            
            Assert.AreEqual(StatusLog.Warning, logEventPai.Events[0].Status, "Primeiro filho deve ser Warning");
            Assert.AreEqual(StatusLog.Error, logEventPai.Events[1].Status, "Segundo filho deve ser Error");
            Assert.AreEqual(StatusLog.Success, logEventPai.Events[2].Status, "Terceiro filho deve ser Success");
            Assert.AreEqual(StatusLog.Success, logEventPai.Events[2].Events[0].Status, "Filho do terceiro filho deve ser Success");
            Assert.AreEqual(StatusLog.Error, logEventPai.Status, "Pai deve ser Error");

        }

        
    }
}
