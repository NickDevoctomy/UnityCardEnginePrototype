using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extensions
{

    public static class TransformExtensions
    {

        #region public methods

        public static Transform Add(this List<Transform> iTransformList,  
            Vector3 iPosition,
            Quaternion iRotation)
        {
            GameObject pGOtWaypoint = new GameObject();
            pGOtWaypoint.transform.SetPositionAndRotation(iPosition, iRotation);
            iTransformList.Add(pGOtWaypoint.transform);
            return (pGOtWaypoint.transform);
        }

        #endregion

    }

}
