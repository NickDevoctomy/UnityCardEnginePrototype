using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Debugging
{

    public class DisposableLogger : IDisposable
    {

        #region private objects

        private BaseLogger cBLrLogger;
        private Boolean cBlnDisposed;
        private DateTime cDTeStartedAt;
        private BaseLogger.MessageType cMTeMessageType;
        private String cStrOperation = String.Empty;

        #endregion

        #region public properties

        public BaseLogger Logger
        {
            get
            {
                return (cBLrLogger);
            }
        }

        public TimeSpan Elapsed
        {
            get
            {
                return (DateTime.UtcNow - cDTeStartedAt);
            }
        }

        public String Operation
        {
            get
            {
                return (cStrOperation);
            }
        }

        public BaseLogger.MessageType MessageType
        {
            get
            {
                return (cMTeMessageType);
            }
        }

        public Boolean? Success { get; set; }

        #endregion

        #region constructor / destructor

        public DisposableLogger(BaseLogger iLogger,
            BaseLogger.MessageType iMessageType,
            String iOperation)
        {
            cDTeStartedAt = DateTime.UtcNow;
            cBLrLogger = iLogger;
            cStrOperation = iOperation;
            cMTeMessageType = iMessageType;
            Logger.Log(MessageType, "Operation '{0}' started at '{1}'.", Operation, Elapsed);
        }

        ~DisposableLogger()
        {
            Dispose(false);
        }

        #endregion

        #region public methods

        public static DisposableLogger Create(String iName,
            BaseLogger.MessageType iMessageType,
            String iOperation)
        {
            if(Logman.Current.Loggers.ContainsKey(iName))
            {
                BaseLogger pBLogger = Logman.Current.Loggers[iName];
                return (new DisposableLogger(pBLogger, iMessageType, iOperation));
            }
            else
            {
                throw new KeyNotFoundException(String.Format("A debugger with the name '{0}' was not found.", iName));
            }
        }

        #endregion

        #region idisposable interface

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (cBlnDisposed) return;
            if (disposing)
            {
                if(Success.HasValue)
                {
                    Logger.Log(MessageType | (Success.Value ? BaseLogger.MessageType.Success : BaseLogger.MessageType.Fail), "Operation '{0}' finished in '{1}'.", Operation, Elapsed);
                }
                else
                {
                    Logger.Log(BaseLogger.MessageType.Information, "Operation '{0}' finished in '{1}'.", Operation, Elapsed);
                }
            }
            cBlnDisposed = true;
        }

        #endregion

    }

}
