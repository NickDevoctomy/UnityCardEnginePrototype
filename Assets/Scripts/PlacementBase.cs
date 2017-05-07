using System;
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
            //Do nothing
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

        #endregion

    }
}
