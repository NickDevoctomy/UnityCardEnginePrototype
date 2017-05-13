using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Debugging
{

    public class FileLogger : BaseLogger
    {

        #region private objects

        private static String[] cStrProperties = { "MessageTypes", "NewLine" };
        private Logman cLMnOwner;
        private String cStrNewLine = String.Empty;

        #endregion


        #region public properties

        public static String[] Properties
        {
            get
            {
                return (cStrProperties);
            }
        }

        public Logman Owner
        {
            get
            {
                return (cLMnOwner);
            }
        }

        public String FullPath
        {
            get
            {
                return(String.Format("{0}{1}_{2}.log", Owner.LogRoot, Name, DateTime.UtcNow.ToString("ddMMyyyy")));
            }
        }

        public String NewLine
        {
            get
            {
                return (cStrNewLine);
            }
        }

        #endregion

        #region constructor / destructor

        public FileLogger(Logman iOwner,
            String iName,
            MessageType iMessageTypes,
            String iNewLine) :
            base(iName, iMessageTypes)
        {
            cLMnOwner = iOwner;
            cStrNewLine = iNewLine;
        }

        #endregion

        #region protected methods

        protected override void OnLog(MessageType iMessageType, 
            String iMessage)
        {
            base.OnLog(iMessageType,
                iMessage);
            File.AppendAllText(FullPath, 
                String.Format("{0}{1}", iMessage, NewLine));
        }

        #endregion

        #region public methods

        public static BaseLogger Create(Logman iOwner,
            String iName,
            Dictionary<String, String> iParams)
        {
            String pStrMessageTypes = iParams["MessageTypes"];
            String pStrNewLine = iParams["NewLine"];
            FileLogger pFLrLogger = new FileLogger(iOwner,
                iName,
                (MessageType)Enum.Parse(typeof(MessageType), pStrMessageTypes),
                pStrNewLine);
            return (pFLrLogger);
        }

        #endregion

    }

}
