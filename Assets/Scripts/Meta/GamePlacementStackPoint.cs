using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Meta
{

    public class GamePlacementStackPoint
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Position { get; set; }

        [JsonProperty(Required = Required.Always)]
        public Int32 FlipCount { get; set; }

        [JsonIgnore]
        public Vector2 PositionAsVector2
        {
            get
            {
                String[] pStrCoOrds = Position.Split(',');
                return (new Vector2(float.Parse(pStrCoOrds[0]), float.Parse(pStrCoOrds[1])));
            }
        }

        #endregion

        #region constructor / destructor

        public GamePlacementStackPoint()
        {

        }

        #endregion

    }

}
