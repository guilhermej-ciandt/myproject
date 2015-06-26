using System;
using System.Collections.Generic;
using System.Diagnostics;
using AG.Framework.Log;
using Common.Logging;

namespace AG.Framework.Chain
{
    public abstract class ChainBase : IChain
    {
        protected LogEvent Log = new LogEvent();

        #region Logging

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ChainBase));

        #endregion

        /// <summary>
        /// The list of Commands configured for this Chain, in
        /// the order in which they may delegate processing to the remainder of
        /// the Chain.
        /// </summary>
        protected ICommand[] Commands = new ICommand[0];

        /// <summary>
        /// Flag indicating whether the configuration of our commands list
        /// has been frozen by a call to the <code>Execute()</code> method.
        /// </summary>
        protected bool Frozen;

        protected Exception SaveException;

        /// <summary>
        /// Flag indicating whether the Chain execution stops or continues after exceptions
        /// </summary>
        protected bool PauseOnException { get; set; }

        public bool StopExecution { get; set; }

        /// <summary>
        /// Construct a Chain configured with the specified Commands
        /// </summary>
        /// <param name="commands">commands The Commands to be configured</param>
        /// <exception cref="ArgumentNullException">if <code>commands</code>, 
        /// or one of the individual Command elements, is <code>null</code>
        /// </exception>
        public ChainBase(ICommand[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException();
            }
            for (int i = 0; i < commands.Length; i++)
            {
                AddCommand(commands[i]);
            }
        }

        /// <summary>
        /// Construct a Chain configured with the specified Commands
        /// </summary>
        /// <param name="commands">commands The Commands to be configured</param>
        /// <exception cref="ArgumentNullException">if <code>commands</code>, 
        /// or one of the individual Command elements, is <code>null</code>
        /// </exception>
        public ChainBase(IEnumerable<ICommand> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException();
            }
            foreach (var command in commands)
            {
                AddCommand(command);
            }
        }

        /// <summary>
        /// Resets the command list
        /// </summary>
        protected void Reset()
        {
            Commands = new ICommand[0];
            Log = new LogEvent{Message = Name};
        }

        protected ChainBase()
        {
            Log.Message = "Transformações";
            Log.DataInicio = DateTime.Now;
        }

        public virtual bool Execute(Context context)
        {
            // Verify our parameters
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(String.Format("{0} command(s) in chain.", Commands.Length));
            }

            Frozen = true;
            bool saveResult = false;
            SaveException = null;
            Log = new LogEvent {Message = Name, DataInicio = DateTime.Now};
            var stopWatch = new Stopwatch();
            for (var i = 0; i < Commands.Length; i++)
            {

                if (StopExecution)
                {
                    Logger.Debug("Stopping command...");
                    StopExecution = false;
                    break;
                }

                var command = Commands[i];

                if (Logger.IsInfoEnabled)
                {
                    Logger.Info(String.Format("{0}. Starting \"{1}\" execution.", i, command.Name));
                }

                try
                {
                    stopWatch.Start();
                   
                    saveResult = command.Execute(context);
                    if (saveResult)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Log.Status = StatusLog.Error;
                    Log.Message = command.Name + ": " + e.Message;
                    Logger.Error(String.Format("Exception during \"{0}\" execution.", command.Name), e);
                    SaveException = e;
                    if (PauseOnException)
                    {
                        throw;
                        // break;
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    Log.AdicionaFilho(command.GetLog());
                    
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.Info(String.Format("Finishing \"{0}\" execution in {1} seg.", command.Name, stopWatch.ElapsedMilliseconds / 1000.00));
                    }
                }
            }

            Log.DataFim = DateTime.Now;
            return (saveResult);
        }

        public abstract string Name { get; }

        public void AddCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("");
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(String.Format("Adding \"{0}\" to chain.", command.Name));
            }

            var results = new ICommand[Commands.Length + 1];
            Array.Copy(Commands, 0, results, 0, Commands.Length);
            results[Commands.Length] = command;
            Commands = results;
        }

        public LogEvent GetLog()
        {
            return Log;
        }
    }
}