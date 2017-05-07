using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extensions
{

    public static class DeckCardExtensions
    {

        #region public enums

        public enum PredefinedMovements
        {
            None = 0,
            Start = 1,
            CeilingCentrePoint = 2,
            Flip = 3,
            End = 4,
        }

        #endregion

        #region public methods

        public static Stack<DeckCard> ToStack(this List<DeckCard> iList)
        {
            Stack<DeckCard> pStaStack = new Stack<DeckCard>();
            foreach(DeckCard curCard in iList)
            {
                pStaStack.Push(curCard);
            }
            return (pStaStack);
        }

        public static List<Transform> CreatePath(this DeckCard iCard,
            Vector3 iStart,
            Vector3 iEnd,
            params PredefinedMovements[] iMovements)
        {
            List<Transform> pLisTransforms = new List<Transform>();
            foreach (PredefinedMovements curMovement in iMovements)
            {
                switch (curMovement)
                {
                    case PredefinedMovements.Start:
                        {
                            pLisTransforms.Add(iStart, Quaternion.identity);
                            break;
                        }
                    case PredefinedMovements.CeilingCentrePoint:
                        {
                            float pFltCentrePointX = 0.0f;
                            if (iStart.x < iEnd.x)              //Travelling right
                            {
                                float pFltDistance = iEnd.x - iStart.x;
                                pFltCentrePointX = iStart.x + (pFltDistance / 2);
                            }
                            else if (iStart.x > iEnd.x)         //Travelling left
                            {
                                float pFltDistance = iStart.x - iEnd.x;
                                pFltCentrePointX = iStart.x - (pFltDistance / 2);
                            }
                            else
                            {
                                pFltCentrePointX = iEnd.x;
                            }

                            float pFltCentrePointZ = 0.0f;
                            if (iStart.z < iEnd.z)              //Travelling up
                            {
                                float pFltDistance = iEnd.z - iStart.z;
                                pFltCentrePointZ = iStart.x + (pFltDistance / 2);
                            }
                            else if (iStart.z > iEnd.z)         //Travelling down
                            {
                                float pFltDistance = iStart.z - iEnd.z;
                                pFltCentrePointZ = iStart.z - (pFltDistance / 2);
                            }
                            else
                            {
                                pFltCentrePointZ = iEnd.z;
                            }

                            Vector3 pVe3Waypoint = new Vector3(pFltCentrePointX, 10.0f, pFltCentrePointZ);
                            pLisTransforms.Add(pVe3Waypoint, Quaternion.identity);
                            break;
                        }
                    case PredefinedMovements.Flip:
                        {
                            pLisTransforms.Add(pLisTransforms[pLisTransforms.Count - 1].position, 
                                Quaternion.AngleAxis(iCard.Facing == DeckCard.CardFacing.Up ? 0.0f : 180.0f, new Vector3(0.0f, 0.0f, 1.0f)));
                            break;
                        }
                    case PredefinedMovements.End:
                        {
                            pLisTransforms.Add(iEnd, Quaternion.identity);
                            break;
                        }
                }
            }
            return (pLisTransforms);
        }

        #endregion

    }

}
