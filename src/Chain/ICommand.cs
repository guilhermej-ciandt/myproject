using System;
using System.Linq;
using System.Text;
using AG.Framework.Log;

namespace AG.Framework.Chain
{
    public interface ICommand: ILoggable
    {
        bool Execute(Context context);
        string Name { get; }
    }

    public class Command : ICommand
    {
        public delegate bool CommandExecutor(Context context, LogEvent log);

        private CommandExecutor executor;
        private LogEvent logDeExecucao;

        public Command(CommandExecutor executor, string name)
        {
            if (executor == null) throw new ArgumentNullException("executor");
            if (name == null) throw new ArgumentNullException("name");
            this.executor = executor;
            Name = name;
            logDeExecucao = new LogEvent { Message = name };
        }

        public LogEvent GetLog()
        {
            return logDeExecucao;
        }

        public bool Execute(Context context)
        {
            logDeExecucao.Inicio();
            logDeExecucao.Status = StatusLog.Success;
            var result = executor(context, logDeExecucao);
            logDeExecucao.Fim();
            return result;
        }

        public string Name { get; private set; }
    }
}