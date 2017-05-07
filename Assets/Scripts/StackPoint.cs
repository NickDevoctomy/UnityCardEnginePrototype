using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class StackPoint : PlacementBase
    {

        #region constructor / destructor

        public StackPoint(CardManager iManager, 
            GameObject iCardPrefab,
            String iName,
            Vector3 iPosition)
            : base(iManager, iCardPrefab, iName, iPosition)
        {
        }

        #endregion

        #region private methods

        private Vector3 NextCardPos()
        {
            Vector3 pVecCurPosition = new Vector3(Position.x, Position.y, Position.z);
            if(Group.Stack.Count > 0)
            {
                for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
                {
                    pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
                }
            }
            return (pVecCurPosition);
        }

        #endregion

        #region protected methods

        protected override void OnPlaceGroup(GameObject iCardPrefab, Vector3 iPosition, DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down)
        {
            Vector3 pVecCurPosition = new Vector3(iPosition.x, iPosition.y, iPosition.z);
            for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
            {
                Group.Deck.CreateCard(Group.Stack[curCard], iCardPrefab, pVecCurPosition, iFacing);
                pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
            }
        }

        #endregion

        #region public methods

        public override Vector3 PrepareNextCardPos()
        {
            return (NextCardPos());
        }

        #endregion

    }

}
