using Assets.Scripts.Debugging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Config
{

    public class LoggerConfig
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String MessageTypes { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String NewLine { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Boolean BaseLog { get; set; }

        #endregion

        #region constructor / destructor

        public LoggerConfig()
        {
        }

        #endregion

        #region public methods

        public void Create()
        {
            Dictionary<String, String> pDicParams = new Dictionary<String, String>();
            pDicParams.Add("MessageTypes", MessageTypes );
            pDicParams.Add("NewLine", NewLine);
            pDicParams.Add("BaseLog", BaseLog.ToString());      //This causes massive performance issues when true
            Logman.CreateLog<FileLogger>(Name, pDicParams);
        }

        #endregion

    }

}
