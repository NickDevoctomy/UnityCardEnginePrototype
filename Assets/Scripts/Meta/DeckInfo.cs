using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Meta
{

    public class DeckInfo
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Description { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String BackImageFile { get; set; }

        #endregion

    }

}
