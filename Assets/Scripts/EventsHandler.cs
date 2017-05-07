using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{

    public class EventsHandler
    {

        #region public events

        private static EventsHandler cEHrCurrent;
        public event EventHandler<StackPointClickedEventArgs> StackPointClicked;
        public event EventHandler<SpreadAreaCardClickedEventArgs> SpreadCardClicked;

        #endregion

        #region public properties

        public static EventsHandler Current
        {
            get
            {
                if (cEHrCurrent == null)
                {
                    cEHrCurrent = new EventsHandler();
                }
                return (cEHrCurrent);
            }
        }

        #endregion

        #region public methods

        public void CardOnMouseDown(DeckCard iCard)
        {
            DeckGroup pDGpGroup = iCard.Deck.Manager.GetDeckCardGroup(iCard);
            if (pDGpGroup != null)
            {
                if (pDGpGroup.Placement != null)
                {
                    if(pDGpGroup.Placement.GetType() == typeof(StackPoint))
                    {
                        if(StackPointClicked != null)
                        {
                            StackPointClicked(this, new StackPointClickedEventArgs((StackPoint)pDGpGroup.Placement, iCard));
                        }
                    }
                    else if(pDGpGroup.Placement.GetType() == typeof(SpreadArea))
                    {
                        if(SpreadCardClicked != null)
                        {
                            SpreadCardClicked(this, new SpreadAreaCardClickedEventArgs((SpreadArea)pDGpGroup.Placement, iCard));
                        }
                    }
                }
            }
        }

        #endregion

    }

}
