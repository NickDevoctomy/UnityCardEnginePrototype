using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Meta.Extensions
{

    public static class CardFacingExtensions
    {

        #region public methods

        public static Vector3 ToVector3(this DeckCard.CardFacing iFacing)
        {
            return (new Vector3(0, 0, iFacing == DeckCard.CardFacing.Up ? 0.0f : -180.0f));
        }

        #endregion

    }

}
