using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Meta
{

    public class GameGroup
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Placement { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Int32 CardCount { get; set; }

        #endregion

        #region constructor / destructor

        public GameGroup()
        {

        }

        #endregion

    }

}
