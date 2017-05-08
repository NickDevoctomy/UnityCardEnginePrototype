using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Meta
{

    public class GamePlacements
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public GamePlacementStackPoint[] StackPoints { get; set; }

        [JsonProperty(Required = Required.Always)]
        public GamePlacementSpreadArea[] SpreadAreas { get; set; }

        #endregion

        #region constructor / destructor

        public GamePlacements()
        {

        }

        #endregion

    }

}
