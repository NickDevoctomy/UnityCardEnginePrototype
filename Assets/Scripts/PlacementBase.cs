using Assets.Scripts.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlacementBase
    {

        #region private objects

        private CardManager cCMrManager;
        private String cStrName = String.Empty;
        private GameObject cGOtCardPrefab;
        private Vector3 cVe3Position;
        private DeckGroup cDGpGroup;

        #endregion

        #region public properties

        public CardManager Manager
        {
            get
            {
                return (cCMrManager);
            }
        }

        public String Name
        {
            get
            {
                return (cStrName);
            }
        }

        public GameObject CardPrefab
        {
            get
            {
                return (cGOtCardPrefab);
            }
        }

        public Vector3 Position
        {
            get
            {
                return (cVe3Position);
            }
        }

        public DeckGroup Group
        {
            get
            {
                return (cDGpGroup);
            }
        }

        public DeckCard TopCard
        {
            get
            {
                if(Group.Stack.Count > 0)
                {
                    return (Group.Stack[Group.Stack.Count - 1]);
                }
                else
                {
                    return (null);
                }
            }
        }

        #endregion

        #region constructor / destructor

        public PlacementBase(CardManager iManager,
            GameObject iCardPrefab,
            String iName,
            Vector3 iPosition)
        {
            cCMrManager = iManager;
            cStrName = iName;
            cGOtCardPrefab = iCardPrefab;
            cVe3Position = iPosition;
        }

        #endregion

        #region protected methods

        protected virtual void OnPlaceGroup(GameObject iCardPrefab,
            Vector3 iPosition,
            DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region public methods

        public void PlaceGroup(DeckGroup iGroup,
            DeckCard.CardFacing iFacing)
        {
            cDGpGroup = iGroup;
            cDGpGroup.Placement = this;
            OnPlaceGroup(CardPrefab, Position, iFacing);
        }

        public void FlipTopNCards(Int32 iCount)
        {
            for (Int32 curCard = 1; curCard <= iCount; curCard++)
            {
                Int32 pIntIndex = Group.Stack.Count - curCard;
                if (pIntIndex >= 0)
                {
                    Group.Stack[pIntIndex].Flip();
                }
            }
        }

        public virtual Vector3 PrepareNextCardPos()
        {
            throw new NotImplementedException();
        }

        public void MoveTopCardToPlacement(String iStackPoint,
            Boolean iFlip)
        {
            if (Manager.StackPoints.ContainsKey(iStackPoint))
            {
                MoveTopCardToPlacement(Manager.StackPoints[iStackPoint], iFlip);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void MoveTopCardToPlacement(PlacementBase iPlacement,
            Boolean iFlip)
        {
            if (Group != null && Group.Stack.Count > 0)
            {
                //Get top card and remove it from source stack
                DeckCard pDCdCard = Group.Stack[Group.Stack.Count - 1];
                Group.Stack.RemoveAt(Group.Stack.Count - 1);

                Debug.Log(String.Format("Moving card '{0}' from '{1}' to '{2}'.", pDCdCard.Tags.ToTagString(), Name, iPlacement.Name));

                //Get start and end pos and add it to the destination stack
                Vector3 pVe3StartPos = pDCdCard.GameObjectRef.transform.position;
                Vector3 pVe3EndPos = iPlacement.PrepareNextCardPos();
                iPlacement.Group.Stack.Add(pDCdCard);

                //Now let's animate from start placement to end placement
                //First we need to create a list of predefined movements
                List<MovementCreator.PredefinedMovements> pLisMovements = new List<MovementCreator.PredefinedMovements>();
                pLisMovements.Add(MovementCreator.PredefinedMovements.Start);
                pLisMovements.Add(MovementCreator.PredefinedMovements.CeilingCentrePoint);
                if(iFlip) pLisMovements.Add(MovementCreator.PredefinedMovements.Flip);
                pLisMovements.Add(MovementCreator.PredefinedMovements.End);

                //Create our waypoints
                List<Waypoint> pLisMovementSets = MovementCreator.CreateWaypoints(pDCdCard,
                    pVe3StartPos,
                    pVe3EndPos,
                    pLisMovements.ToArray());

                //Add waypoints to the card
                pDCdCard.AddWaypoints(pLisMovementSets);

                //Start animating the card
                pDCdCard.StartTween(true);
            }
        }

        #endregion

    }
}
