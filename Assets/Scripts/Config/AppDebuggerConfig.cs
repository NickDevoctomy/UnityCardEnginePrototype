using Assets.Scripts.Debugging;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Config
{

    public class AppDebuggerConfig
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String DefaultLogName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public LoggerConfig[] Loggers { get; set; }

        #endregion

        #region constructor / destructor

        public AppDebuggerConfig()
        {

        }

        #endregion

        #region public methods

        public void Initialise()
        {
            Logman.Initialise(IOUtility.GetDataPath(), DefaultLogName);
            foreach (LoggerConfig curLogger in Loggers)
            {
                curLogger.Create();
            }
        }

        #endregion

    }

}
