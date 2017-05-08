using Newtonsoft.Json.Linq;
using System;

namespace Assets.Scripts.Meta
{

    public class TagFunctionCondition
    {

        #region private objects

        private String cStrTag = String.Empty;
        private String cStrCurrentValue = String.Empty;
        private String cStrNextValue = String.Empty;

        #endregion

        #region public properties

        public String Tag
        {
            get
            {
                return (cStrTag);
            }
        }

        public String CurrentValue
        {
            get
            {
                return (cStrCurrentValue);
            }
        }

        public String NextValue
        {
            get
            {
                return (cStrNextValue);
            }
        }

        #endregion

        #region constructor / destructor

        private TagFunctionCondition(String iTag,
            String iCurrentValue,
            String iNextValue)
        {
            cStrTag = iTag;
            cStrCurrentValue = iCurrentValue;
            cStrNextValue = iNextValue;
        }

        #endregion

        #region private methods

        private String ReplaceVariables(String iCurrentValue,
            String iNextValue)
        {
            String pStrReplaced = iNextValue;
            pStrReplaced = pStrReplaced.Replace("{CurrentValue}", iCurrentValue);
            return (pStrReplaced);
        }

        #endregion

        #region public methods

        public static TagFunctionCondition FromJObject(String iTag,
            JObject iJObject)
        {
            TagFunctionCondition pTFCCondition = new TagFunctionCondition(iTag,
                iJObject["CurrentValue"].Value<String>(),
                iJObject["NextValue"].Value<String>());
            return (pTFCCondition);
        }

        public Boolean CheckFunction(DeckCard iCurrentCard,
            DeckCard iNextCard)
        {
            String pStrCurrentValue = iCurrentCard.TagsByName[Tag].Value;
            String pStrNextValue = iNextCard.TagsByName[Tag].Value;

            ReplaceVariables(pStrCurrentValue, pStrNextValue);

            //call any internal functions


            //Determine how to check values here
            return (pStrCurrentValue.Equals(pStrNextValue));
        }

        #endregion

    }

}
