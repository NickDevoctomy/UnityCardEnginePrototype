using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{

    public class StackPointClickedEventArgs : EventArgs
    {

        #region public properties

        public StackPoint StackPoint { get; private set; }

        public DeckCard Card { get; private set; }

        #endregion

        #region constructor / destructor

        public StackPointClickedEventArgs(StackPoint iStackPoint,
            DeckCard iCard)
        {
            StackPoint = iStackPoint;
            Card = iCard;
        }

        #endregion

    }

}
