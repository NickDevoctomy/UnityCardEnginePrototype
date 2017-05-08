using Assets.Scripts.Extensions;
using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{

    public static class MovementCreator
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

        public static List<Waypoint> CreateWaypoints(DeckCard iCard,
            Vector3 iStart,
            Vector3 iEnd,
            params PredefinedMovements[] iMovements)
        {
            String pStrGroupID = Guid.NewGuid().ToString();
            List<Waypoint> pLisMovementSets = new List<Waypoint>();
            foreach (PredefinedMovements curMovement in iMovements)
            {
                switch (curMovement)
                {
                    case PredefinedMovements.Start:
                        {
                            Waypoint pMStSet = new Waypoint(pStrGroupID,
                                "Start",
                                iStart,
                                null);

                            pLisMovementSets.Add(pMStSet);
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

                            Waypoint pMStSet = new Waypoint(pStrGroupID,
                                "MoveToCentre",
                                pVe3Waypoint,
                                null,
                                2.0f,
                                0.0f);

                            pLisMovementSets.Add(pMStSet);
                            break;
                        }
                    case PredefinedMovements.Flip:
                        {
                            Waypoint pMStSet = new Waypoint(pStrGroupID,
                                "Flip",
                                pLisMovementSets[pLisMovementSets.Count - 1].Position,
                                iCard.Facing == DeckCard.CardFacing.Up ? DeckCard.CardFacing.Down.ToVector3() : DeckCard.CardFacing.Up.ToVector3(),
                                2.0f,
                                0.5f);

                            pLisMovementSets.Add(pMStSet);
                            break;
                        }
                    case PredefinedMovements.End:
                        {
                            Waypoint pMStSet = new Waypoint(pStrGroupID,
                                "MoveToEnd",
                                iEnd,
                                null,
                                2.0f,
                                0.0f);

                            pLisMovementSets.Add(pMStSet);
                            break;
                        }
                }
            }
            return (pLisMovementSets);
        }

    }

}
