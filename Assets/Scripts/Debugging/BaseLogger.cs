using System;

namespace Assets.Scripts.Debugging
{

    public class BaseLogger
    {

        #region public enums

        [Flags]
        public enum MessageType
        {
            None = 0,
            Information = 1,
            Warning = 2,
            Error = 4,
            Exception = 8,
            Success = 16,
            Fail = 32,
            Verbose = 64            //This should be used on messages being written during a frame update as it will greatly impact performance
        }

        #endregion

        #region private objects

        private String cStrName = String.Empty;
        private MessageType cMTeMessageTypes;

        #endregion

        #region public properties

        public String Name
        {
            get
            {
                return (cStrName);
            }
        }

        public MessageType MessageTypes
        {
            get
            {
                return (cMTeMessageTypes);
            }
        }

        #endregion

        #region constructor / destructor

        protected BaseLogger(String iName,
            MessageType iMessageTypes)
        {
            cStrName = iName;
            cMTeMessageTypes = iMessageTypes;
        }

        #endregion

        #region protected methods

        protected virtual String PrependMessage(MessageType iMessageType, 
            String iMessage,
            params Object[] iParams)
        {
            String pStrPlainMessage = String.Format(iMessage, iParams);
            String pStrPrependedMessage = String.Format("{0} : {1} : {2}", DateTime.UtcNow.ToString(), iMessageType.ToString(), pStrPlainMessage);
            return (pStrPrependedMessage);
        }

        protected virtual void OnLog(MessageType iMessageType,
            String iMessage)
        {
            UnityEngine.Debug.Log(iMessage);
        }

        #endregion

        #region private methods

        private void DoLog(MessageType iMessageType,
            String iMessage,
            params Object[] iParams)
        {
            String pStrPrependedMessage = PrependMessage(iMessageType,
                iMessage,
                iParams);
            OnLog(iMessageType, 
                String.Format("{0}", pStrPrependedMessage));
        }

        #endregion

        #region public methods

        public void Log(MessageType iMessageType,
            String iMessageFormat, 
            params Object[] iParams)
        {
            if((MessageTypes & iMessageType) == iMessageType)
            {
                DoLog(iMessageType,
                    iMessageFormat,
                    iParams);
            }
        }

        public void LogException(Exception iException)
        {
            if ((MessageTypes & MessageType.Exception) == MessageType.Exception)
            {
                DoLog(MessageType.Exception,
                    "{0}",
                    iException.ToString());
            }
        }

        #endregion

    }

}
