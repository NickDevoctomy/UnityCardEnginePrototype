using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Extensions
{

    public static class DeckCardTagExtensions
    {

        #region public methods

        public static String ToTagString(this DeckCardTag[] iTags)
        {
            StringBuilder pSBrString = new StringBuilder();
            foreach(DeckCardTag curTag in iTags)
            {
                pSBrString.Append(curTag.ToString());
                pSBrString.Append(",");
            }
            if(pSBrString.Length > 0) pSBrString.Length -= 1;
            return (pSBrString.ToString());
        }

        #endregion

    }

}
