using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extensions
{

    public static class GameObjectExtensions
    {

        #region public methods

        public static GameObject GetChildGameObject(this GameObject iParent,
            String iName)
        {
            Transform[] pTraChildren = iParent.transform.GetComponentsInChildren<Transform>();
            foreach (Transform curTransform in pTraChildren)
            {
                if (curTransform.gameObject.name == iName)
                {
                    return (curTransform.gameObject);
                }
            }
            return (null);
        }

        #endregion

    }

}
