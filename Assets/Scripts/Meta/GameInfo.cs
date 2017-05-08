using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Meta
{

    public class GameInfo
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Description { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Author { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Deck { get; set; }

        #endregion

        #region constructor / destructor

        public GameInfo()
        {

        }

        #endregion

    }

}
