using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class MovementSet
    {

        #region public properties

        public string Name { get; private set; }

        public Vector3? Position { get; private set; }

        public Vector3? Rotation { get; private set; }

        public Boolean Instant { get; private set; }

        public float PercentPerSecond { get; private set; }

        public float RotationTime { get; private set; }

        #endregion

        #region constructor / destructor

        public MovementSet(String iName,
            Vector3? iPosition,
            Vector3? iRotation,
            float iPercentPerSecond,
            float iRotationTime)
        {
            Name = iName;
            Position = iPosition;
            Rotation = iRotation;
            PercentPerSecond = iPercentPerSecond;
            RotationTime = iRotationTime;
        }

        public MovementSet(String iName,
            Vector3? iPosition,
            Vector3? iRotation)
        {
            Name = iName;
            Position = iPosition;
            Rotation = iRotation;
            Instant = true;
        }

        #endregion

    }

}
