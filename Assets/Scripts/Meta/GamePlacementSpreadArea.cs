using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Meta
{

    public class GamePlacementSpreadArea
    {

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Position { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Alignment { get; set; }

        [JsonProperty(Required = Required.Always)]
        public String Orientation { get; set; }

        [JsonProperty(Required = Required.Always)]
        public float Length { get; set; }

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

        [JsonIgnore]
        public SpreadArea.SpreadAlignment AligmentAsSpreadAlignment
        {
            get
            {
                return((SpreadArea.SpreadAlignment)Enum.Parse(typeof(SpreadArea.SpreadAlignment), Alignment));
            }
        }

        [JsonIgnore]
        public SpreadArea.SpreadOrientation OrientationAsSpreadOrientation
        {
            get
            {
                return ((SpreadArea.SpreadOrientation)Enum.Parse(typeof(SpreadArea.SpreadOrientation), Orientation));
            }
        }

        #endregion

        #region constructor / destructor

        public GamePlacementSpreadArea()
        {

        }

        #endregion

    }

}
