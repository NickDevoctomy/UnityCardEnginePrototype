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
            for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
            {
                pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
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

        public void MoveTopCardToSpreadArea(String iSpreadArea)
        {
            if(Manager.SpreadAreas.ContainsKey(iSpreadArea))
            {
                MoveTopCardToSpreadArea(Manager.SpreadAreas[iSpreadArea]);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void MoveTopCardToSpreadArea(SpreadArea iSpreadArea)
        {
            if(Group != null && Group.Stack.Count > 0)
            {
                DeckCard pDCdCard = Group.Stack[Group.Stack.Count - 1];
                Group.Stack.RemoveAt(Group.Stack.Count - 1);
                Vector3 pVe3Position = iSpreadArea.PrepareNextCardPos();
                iSpreadArea.Group.Stack.Add(pDCdCard);
                iTween.MoveTo(pDCdCard.GameObjectRef, pVe3Position, 1.0f);
            }
        }

        public void MoveTopCardToStackPoint(String iStackPoint,
            Boolean iFlip)
        {
            if (Manager.StackPoints.ContainsKey(iStackPoint))
            {
                MoveTopCardToStackPoint(Manager.StackPoints[iStackPoint], iFlip);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void MoveTopCardToStackPoint(StackPoint iStackPoint,
            Boolean iFlip)
        {
            if (Group != null && Group.Stack.Count > 0)
            {
                DeckCard pDCdCard = Group.Stack[Group.Stack.Count - 1];
                Vector3 pVe3StartPos = pDCdCard.GameObjectRef.transform.position;
                Group.Stack.RemoveAt(Group.Stack.Count - 1);
                Vector3 pVe3EndPos = iStackPoint.PrepareNextCardPos();
                iStackPoint.Group.Stack.Add(pDCdCard);

                List<MovementSet> pLisMovementSets = MovementCreator.CreateMovementSets(pDCdCard,
                    pVe3StartPos,
                    pVe3EndPos,
                    MovementCreator.PredefinedMovements.Start,
                    MovementCreator.PredefinedMovements.CeilingCentrePoint,
                    MovementCreator.PredefinedMovements.Flip,
                    MovementCreator.PredefinedMovements.End);
                pDCdCard.AddMovementSets(pLisMovementSets);

                pDCdCard.StartTween(true);
            }
        }

        public Vector3 PrepareNextCardPos()
        {
            return (NextCardPos());
        }

        #endregion

    }

}
