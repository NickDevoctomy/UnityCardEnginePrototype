using Assets.Scripts.Debugging;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Config
{

    public class AppConfig
    {

        #region private objects

        private static AppConfig cACgCurrent;

        #endregion

        #region public properties

        [JsonIgnore]
        public static AppConfig Current
        {
            get
            {
                if (cACgCurrent == null)
                {
                    cACgCurrent = LoadDefault();
                }
                return (cACgCurrent);
            }
        }

        [JsonProperty(Required = Required.Always)]
        public AppDebuggerConfig Debugger { get; set; }

        #endregion

        #region constructor / destructor

        public AppConfig()
        {
        }

        #endregion

        #region private methods

        private static AppConfig LoadDefault()
        {
            String pStrAppName = Application.productName;
            String pStrConfigJSON = IOUtility.LoadStreamingAssestsFileAsString(String.Format("{0}.config", pStrAppName));
            AppConfig pACgConfig = JsonConvert.DeserializeObject<AppConfig>(pStrConfigJSON);
            return (pACgConfig);
        }

        #endregion

        #region public methods

        public void Initialise()
        {
            Debugger.Initialise();
        }

        #endregion

    }

}
