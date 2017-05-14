using Assets.Scripts.IO;
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

        private static String[] cStrProperties = { "MessageTypes", "NewLine", "BaseLog" };
        private Logman cLMnOwner;
        private String cStrNewLine = String.Empty;
        private String cStrLastLogFile = String.Empty;
        private Int32 cIntBufferSizeMB;
        private FileStream cFSmOutput;
        private Boolean cBlnBaseLog;

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

        public FileStream OutputStream
        {
            get
            {
                String pStrFullPath = FullPath;
                if(cStrLastLogFile != pStrFullPath)
                {
                    cFSmOutput = new FileStream(pStrFullPath, 
                        FileMode.Append, 
                        FileAccess.Write, 
                        FileShare.Read, 
                        (Int32)FileSize.Convert(BufferSizeMB, FileSize.UnitSize.Megabytes, FileSize.UnitSize.Bytes), 
                        FileOptions.None);
                    cStrLastLogFile = pStrFullPath;
                }
                return (cFSmOutput);
            }
        }

        public String NewLine
        {
            get
            {
                return (cStrNewLine);
            }
        }

        public Boolean BaseLog
        {
            get
            {
                return (cBlnBaseLog);
            }
        }

        public Int32 BufferSizeMB
        {
            get
            {
                return (cIntBufferSizeMB);
            }
        }

        #endregion

        #region constructor / destructor

        public FileLogger(Logman iOwner,
            String iName,
            MessageType iMessageTypes,
            String iNewLine,
            Boolean iBaseLog,
            Int32 iBufferSizeMB) :
            base(iName, iMessageTypes)
        {
            cLMnOwner = iOwner;
            cStrNewLine = iNewLine;
            cBlnBaseLog = iBaseLog;
            cIntBufferSizeMB = iBufferSizeMB;
        }

        #endregion

        #region protected methods

        protected override void OnLog(MessageType iMessageType, 
            String iMessage)
        {
            if(BaseLog)
            {
                base.OnLog(iMessageType,
                    iMessage);
            }

            String pStrOutput = String.Format("{0}{1}", iMessage, NewLine);
            Byte[] pBytOutput = Encoding.UTF8.GetBytes(pStrOutput);
            OutputStream.Write(pBytOutput, 0, pBytOutput.Length);
        }

        #endregion

        #region public methods

        public static BaseLogger Create(Logman iOwner,
            String iName,
            Dictionary<String, String> iParams)
        {
            String pStrMessageTypes = iParams["MessageTypes"];
            String pStrNewLine = iParams["NewLine"];
            Boolean pBlnBaseLog = Boolean.Parse(iParams["BaseLog"]);
            Int32 pIntBufferSizeMB = Int32.Parse(iParams["BufferSizeMB"]);
            FileLogger pFLrLogger = new FileLogger(iOwner,
                iName,
                (MessageType)Enum.Parse(typeof(MessageType), pStrMessageTypes),
                pStrNewLine,
                pBlnBaseLog,
                pIntBufferSizeMB);
            return (pFLrLogger);
        }

        #endregion

    }

}
