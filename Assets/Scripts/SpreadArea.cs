using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class SpreadArea : PlacementBase
    {

        #region public enums

        public enum SpreadAlignment
        {
            None = 0,
            Near = 1,
            Centre = 2,
            Far = 3
        }

        public enum SpreadOrientation
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2
        }

        #endregion

        #region private objects

        private SpreadAlignment cSAtAlignment = SpreadAlignment.None;
        private SpreadOrientation cSOnOrientation = SpreadOrientation.None;
        private float cFltLength;

        #endregion

        #region public properties

        public SpreadAlignment Alignment
        {
            get
            {
                return (cSAtAlignment);
            }
        }

        public SpreadOrientation Orientation
        {
            get
            {
                return (cSOnOrientation);
            }
        }

        public float Length
        {
            get
            {
                return (cFltLength);
            }
        }


        #endregion

        #region constructor / destructor

        public SpreadArea(CardManager iManager,
            GameObject iCardPrefab,
            String iName,
            Vector3 iPosition,
            SpreadAlignment iAlignment,
            SpreadOrientation iOrientation,
            float iLength)
            : base(iManager, iCardPrefab, iName, iPosition)
        {
            cSAtAlignment = iAlignment;
            cSOnOrientation = iOrientation;
            cFltLength = iLength;
        }

        #endregion

        #region private methods

        private Vector3 ShiftCardsLeft()
        {
            if(Group.Stack.Count > 0)
            {
                //Get total width and spacing
                Int32 pIntCardCount = Group.Stack.Count + 1;
                float pFltWidth = Length;
                float pFltCardSpace = pFltWidth / pIntCardCount;
                if (pFltCardSpace < 2.0f)
                {
                    pFltCardSpace = 2.0f;
                    pFltWidth = pFltCardSpace * pIntCardCount;
                }

                //Place the cards
                Vector3 pVecCurPosition = new Vector3(Position.x - (pFltWidth / 2), Position.y, Position.z);
                for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
                {
                    Group.Stack[curCard].GameObjectRef.transform.position = pVecCurPosition;
                    pVecCurPosition = new Vector3(pVecCurPosition.x + pFltCardSpace, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
                }
                return (pVecCurPosition);
            }
            else
            {
                return (new Vector3(Position.x - (Length / 2), 0.002f, Position.y));
            }
        }

        private Vector3 NextCardPos()
        {
            if (Alignment == SpreadAlignment.Centre) throw new InvalidOperationException("Must shift cards when centre aligned to get the next card position.");

            switch(Orientation)
            {
                case SpreadOrientation.Horizontal:
                    {
                        switch (Alignment)
                        {
                            case SpreadAlignment.Near:
                                {
                                    float pFltSpacing = 1.0f;
                                    Vector3 pVecCurPosition = new Vector3(Position.x, Position.y, Position.z);
                                    for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
                                    {
                                        pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z - pFltSpacing);
                                    }
                                    return (pVecCurPosition);
                                }
                            case SpreadAlignment.Far:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                case SpreadOrientation.Vertical:
                    {
                        switch(Alignment)
                        {
                            case SpreadAlignment.Near:
                                {
                                    break;
                                }
                            case SpreadAlignment.Far:
                                {
                                    break;
                                }
                        }
                        break;
                    }
            }

            throw new NotSupportedException(String.Format("The orientation '{0}' with alignment '{1}' is not supported when calling NextCardPos.",
                Orientation,
                Alignment));
        }

        #endregion

        #region protected methods

        protected override void OnPlaceGroup(GameObject iCardPrefab, Vector3 iPosition, DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down)
        {
            if (Group.Stack.Count > 0)
            {
                switch (Orientation)
                {
                    case SpreadOrientation.Horizontal:
                        {
                            switch (Alignment)
                            {
                                case SpreadAlignment.Centre:
                                    {
                                        //Calculate spread width
                                        Int32 pIntCardCount = Group.Stack.Count;
                                        float pFltWidth = Length;
                                        float pFltCardSpace = pFltWidth / pIntCardCount;
                                        if (pFltCardSpace < 2.0f)
                                        {
                                            pFltCardSpace = 2.0f;
                                            pFltWidth = pFltCardSpace * pIntCardCount;
                                        }

                                        //Place the cards
                                        Vector3 pVecCurPosition = new Vector3(iPosition.x - (pFltWidth / 2), iPosition.y, iPosition.z);
                                        for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
                                        {
                                            Group.Deck.CreateCard(Group.Stack[curCard], iCardPrefab, pVecCurPosition, iFacing);
                                            pVecCurPosition = new Vector3(pVecCurPosition.x + pFltCardSpace, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
                                        }

                                        break;
                                    }
                            }

                            break;
                        }
                    case SpreadOrientation.Vertical:
                        {
                            switch (Alignment)
                            {
                                case SpreadAlignment.Near:
                                    {
                                        float pFltSpacing = 1.0f;
                                        Vector3 pVecCurPosition = new Vector3(Position.x, Position.y, iPosition.z);
                                        for (Int32 curCard = 0; curCard < Group.Stack.Count; curCard++)
                                        {
                                            Group.Deck.CreateCard(Group.Stack[curCard], iCardPrefab, pVecCurPosition, iFacing);
                                            pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Group.Stack[curCard].Thickness + 0.03f, pVecCurPosition.z - pFltSpacing);
                                        }

                                        break;
                                    }
                                case SpreadAlignment.Centre:
                                    {
                                        break;
                                    }
                                case SpreadAlignment.Far:
                                    {
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }

        #endregion

        #region public methods

        public override Vector3 PrepareNextCardPos()
        {
            switch (Orientation)
            {
                case SpreadOrientation.Horizontal:
                    {
                        switch (Alignment)
                        {
                            case SpreadAlignment.Near:
                                {
                                    return NextCardPos();
                                }
                            case SpreadAlignment.Centre:
                                {
                                    return (ShiftCardsLeft()); // Right
                                }
                            case SpreadAlignment.Far:
                                {
                                    return NextCardPos();
                                }
                        }
                        break;
                    }
                case SpreadOrientation.Vertical:
                    {
                        switch(Alignment)
                        {
                            case SpreadAlignment.Near:
                                {
                                    return NextCardPos();
                                }
                            case SpreadAlignment.Centre:
                                {
                                    //ShuftCardsUp / DOwn
                                    break;
                                }
                            case SpreadAlignment.Far:
                                {
                                    return NextCardPos();
                                }
                        }
                        break;
                    }
            }
            throw new NotSupportedException(String.Format("The orientation '{0}' with alignment '{1}' is not supported when calling PrepareNextCardPos.",
                Orientation,
                Alignment));
        }

        #endregion

    }

}
