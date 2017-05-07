using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class ClickHandler
    {

        #region public events

        public event EventHandler<EventArgs> Clicked;
        public event EventHandler<EventArgs> DoubleClicked;

        #endregion

        #region public properties

        public float DoubleClickTime = 0.35f;

        #endregion

        #region private objects

        private Boolean cBlnClick = false;
        private float cFltClickTime;

        #endregion

        #region public methods

        public void Update()
        {
            if (cBlnClick && Time.time > (cFltClickTime + DoubleClickTime))
            {
                if(Clicked != null)
                {
                    Clicked(this, EventArgs.Empty);
                    cBlnClick = false;
                }
            }
        }

        public void OnMouseDown()
        {
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
        }

        #endregion

    }

}
