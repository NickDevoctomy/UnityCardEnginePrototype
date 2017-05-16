using Assets.Scripts.Meta;
using System;
using UnityEngine;

namespace Assets.Scripts
{

    public class MouseHandler
    {

        #region public enums

        public enum DraggingStage
        {
            None = 1,
            BeginDragging = 2,
            Dragging = 3,
            Dropping = 4
        }

        #endregion

        #region public events

        public event EventHandler<EventArgs> Clicked;
        public event EventHandler<EventArgs> DoubleClicked;
        public event EventHandler<BeginDraggingEventArgs> BeginDragging;

        #endregion

        #region public properties

        public float DoubleClickTime = 0.35f;

        #endregion

        #region private objects

        private Boolean cBlnClick = false;
        private float cFltClickTime;
        private DraggingStage cDSeDraggingStage = DraggingStage.None;
        private GameObject cGOtDragging = null;
        private Vector3 cVe3StartDragPos;
        private float cFltDragHeight = 0.0f;

        #endregion

        #region public methods

        public void Update(GameObject iObject)
        {
            switch(cDSeDraggingStage)
            {
                case DraggingStage.None:
                    {
                        if (cBlnClick && Time.time > (cFltClickTime + DoubleClickTime))
                        {
                            if (Clicked != null)
                            {
                                Clicked(this, EventArgs.Empty);
                                cBlnClick = false;
                            }
                        }
                        break;
                    }
                case DraggingStage.BeginDragging:
                    {
                        BeginDraggingEventArgs pEAsArgs = new BeginDraggingEventArgs();
                        BeginDragging(this, pEAsArgs);
                        if (pEAsArgs.Allow)
                        {
                            cDSeDraggingStage = DraggingStage.Dragging;
                            cVe3StartDragPos = iObject.transform.position;
                            cGOtDragging = iObject;
                        }
                        break;
                    }
                case DraggingStage.Dragging:
                    {
                        RaycastHit pRHtHit;
                        Ray pRayRayToTable = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(pRayRayToTable, out pRHtHit))
                        {
                            iObject.transform.position = new Vector3(pRHtHit.point.x, cFltDragHeight, pRHtHit.point.z);
                        }
                        break;
                    }
            }
        }

        public void OnMouseDown(GameObject iObject)
        {
            DeckCard pDCdCard = Deck.CardsByGameObject[iObject];

            if (cBlnClick && Time.time <= (cFltClickTime + DoubleClickTime))
            {
                if (DoubleClicked != null)
                {
                    DoubleClicked(this, EventArgs.Empty);
                    cBlnClick = false;
                }
            }
            else
            {
                cBlnClick = true;
                cFltClickTime = Time.time;
            }

            cFltDragHeight = pDCdCard.Deck.Manager.GetHighestCardPos() + 0.03f;
            cVe3StartDragPos = iObject.transform.position;
            cDSeDraggingStage = DraggingStage.BeginDragging;
        }

        public void OnDrawGizmos(GameObject iObject)
        {
            //nothing to do
        }

        public void OnMouseEnter(GameObject iObject)
        {
            //highlight the card and any cards above it
        }

        public void OnMouseExit(GameObject iObject)
        {
            //unhighlight the card and any cards above it
        }

        public void OnMouseUp(GameObject iObject)
        {
            if(cDSeDraggingStage== DraggingStage.Dragging)
            {
                //end dragging here
                cGOtDragging.transform.position = cVe3StartDragPos;
            }
            cDSeDraggingStage = DraggingStage.None;
        }

        #endregion

    }

}
