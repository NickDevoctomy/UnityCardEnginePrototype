using System;
using UnityEngine;

namespace Assets.Scripts
{

    public class Waypoint
    {

        #region public properties

        public String GroupID { get; private set; }

        public String Name { get; private set; }

        public Vector3? Position { get; private set; }

        public Vector3? Rotation { get; private set; }

        public Boolean Instant { get; private set; }

        public float PercentPerSecond { get; private set; }

        public float RotationTime { get; private set; }

        #endregion

        #region constructor / destructor

        public Waypoint(String iGroupID,
            String iName,
            Vector3? iPosition,
            Vector3? iRotation,
            float iPercentPerSecond,
            float iRotationTime)
        {
            GroupID = iGroupID;
            Name = iName;
            Position = iPosition;
            Rotation = iRotation;
            PercentPerSecond = iPercentPerSecond;
            RotationTime = iRotationTime;
        }

        public Waypoint(String iGroupID, 
            String iName,
            Vector3? iPosition,
            Vector3? iRotation)
        {
            GroupID = iGroupID;
            Name = iName;
            Position = iPosition;
            Rotation = iRotation;
            Instant = true;
        }

        #endregion

    }

}
