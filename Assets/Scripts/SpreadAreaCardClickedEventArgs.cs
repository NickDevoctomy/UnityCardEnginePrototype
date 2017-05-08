using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{

    public class SpreadAreaCardClickedEventArgs : EventArgs
    {

        #region public properties

        public SpreadArea SpreadArea { get; private set; }

        public DeckCard Card { get; private set; }

        public Boolean IsDouble { get; private set; }

        #endregion

        #region constructor / destructor

        public SpreadAreaCardClickedEventArgs(SpreadArea iSpreadArea,
            DeckCard iCard,
            Boolean iIsDouble)
        {
            SpreadArea = iSpreadArea;
            Card = iCard;
            IsDouble = iIsDouble;
        }

        #endregion

    }

}
