using System;
using System.Collections.Generic;
using System.ComponentModel;
using AG.Framework.Utils;

namespace AG.Framework.Log
{
    /// <summary>
    /// Enum com os Status do Log
    /// </summary>
    public enum StatusLog
    {
        Success = 0,
        Warning = 1,
        Error = 2,
        Running = 3
    }

    /// <summary>
    /// Enum com os tipos de eventos
    /// </summary>
    public enum LogEventEnum
    {
        Evento = 0,
        Transformacao = 1,
        Publicacao = 3
    }

    /// <summary>
    /// Classe de eventos gerados que devem ser logados
    /// </summary>
    [Serializable]
    public class LogEvent
    {
        public DateTime DataInicio { set; get; }
        public DateTime DataFim { set; get; }

        private StatusLog _status;
        public StatusLog Status
        {
            get
            {
                return VerificaStatus(Events, _status);
            }
            set { _status = value; }
        }

        private StatusLog VerificaStatus(IEnumerable<LogEvent> events, StatusLog statusPai)
        {
            if (events != null)
            {
                foreach (var logEvent in events)
                {
                    statusPai = VerificaStatus(logEvent.Events, logEvent._status);
                }
            }
            if (statusPai == StatusLog.Error)
                _status = StatusLog.Error;
            else if (statusPai == StatusLog.Warning && _status == StatusLog.Success)
                _status = StatusLog.Warning;

            return _status;
        }

        public string Message { set; get; }
        public int? Id { set; get; }
        public LogEventEnum Tipo { set; get; }

        public string Duracao
        {
            get
            {
                return DateTimeUtils.CalculaTempoDecorrido(DataFim, DataInicio);
            }
        }

        public bool MostraDuracao { get; set; }

        [DefaultValue(true)]
        public bool ConsolidaErros { get; set; }

        public List<LogEvent> Events { set; get; }

        public LogEvent()
        {
            Id = null;
            Status = StatusLog.Success;
            Tipo = LogEventEnum.Evento;
            MostraDuracao = true;
        }

        public void AdicionaFilho(LogEvent log)
        {
            AtualizaStatus(log);
            if (Events == null)
            {
                Events = new List<LogEvent>();
            }
            Events.Add(PreencheDatas(log));
        }

        public void AtualizaStatus(LogEvent log)
        {
            if (!ConsolidaErros)
                return;

            switch (log.Status)
            {
                case StatusLog.Warning:
                case StatusLog.Error:
                    Status = StatusLog.Warning;
                    break;
            }
        }

        private LogEvent PreencheDatas(LogEvent logEvent)
        {
            logEvent.DataFim = DateTime.Now;
            logEvent.DataInicio = Events.Count > 0 ? Events[Events.Count - 1].DataFim : DataInicio;
            return logEvent;
        }

        public void Inicio()
        {
            DataInicio = DateTime.Now;
        }

        public void Fim()
        {
            DataFim = DateTime.Now;
        }

    }
}