using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Extensions
{

    public static class DeckCardExtensions
    {

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

        #endregion

    }

}
