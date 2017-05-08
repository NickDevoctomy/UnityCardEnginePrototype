using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{

    public class DeckGroup
    {

        #region private objects

        private System.Random cRndRandom;
        private Deck cDekDeck;
        private String cStrDeckName = String.Empty;
        private String cStrName = String.Empty;
        private List<DeckCard> cLisStack;
        private PlacementBase cPBePlacement;

        #endregion

        #region public properties

        public Deck Deck
        {
            get
            {
                return (cDekDeck);
            }
        }

        public String DeckName
        {
            get
            {
                return (cStrDeckName);
            }
        }

        public String Name
        {
            get
            {
                return (cStrName);
            }
        }

        public List<DeckCard> Stack
        {
            get
            {
                return (cLisStack);
            }
        }

        public PlacementBase Placement
        {
            get
            {
                return (cPBePlacement);
            }
            set
            {
                cPBePlacement = value;
            }
        }

        #endregion

        #region constructor / destructor

        public DeckGroup(Deck iDeck,
            String iDeckName,
            String iName)
        {
            cRndRandom = new System.Random(Environment.TickCount);
            cDekDeck = iDeck;
            cStrDeckName = iDeckName;
            cStrName = iName;
            cLisStack = new List<DeckCard>();
        }

        #endregion

        #region public methods

        public void ShuffleStack(Int32 iCount = 500)
        {
            List<DeckCard> pLisStack = Stack.ToList();
            for (Int32 curShuffle = 0; curShuffle < iCount; curShuffle++)
            {
                Int32 pIntRandomIndex = cRndRandom.Next(0, pLisStack.Count);
                DeckCard pDCdShuffleCard = pLisStack[pIntRandomIndex];
                pLisStack.RemoveAt(pIntRandomIndex);
                Int32 pIntRandomInsertIndex = cRndRandom.Next(0, pLisStack.Count);
                pLisStack.Insert(pIntRandomInsertIndex, pDCdShuffleCard);
            }
            cLisStack = pLisStack;
        }

        #endregion

    }

}
