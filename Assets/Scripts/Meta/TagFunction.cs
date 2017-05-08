using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Meta
{

    public class TagFunction
    {

        #region private objects

        private String cStrName = String.Empty;
        private Dictionary<String, List<TagFunctionCondition>> cDicConditions;

        #endregion

        #region public properties

        public String Name
        {
            get
            {
                return (cStrName);
            }
        }

        public Dictionary<String, List<TagFunctionCondition>> Conditions
        {
            get
            {
                return (cDicConditions);
            }
        }

        #endregion

        #region constructor / destructor

        public TagFunction(String iName,
            Dictionary<String, List<TagFunctionCondition>> iConditions)
        {
            cStrName = iName;
            cDicConditions = iConditions;
        }

        #endregion

        #region public methods

        public static TagFunction FromJObject(JObject iJObject)
        {
            Dictionary<String, List<TagFunctionCondition>> pDicConditions = new Dictionary<String, List<TagFunctionCondition>>();
            JObject pJOtConditions = iJObject["Conditions"].Value<JObject>();
            foreach(JProperty curCondition in pJOtConditions.Children())
            {
                String pStrTag = curCondition.Name;
                List<TagFunctionCondition> pLisValues = new List<TagFunctionCondition>();
                JArray pJAyValues = (JArray)curCondition.Value;
                foreach(JObject curValue in pJAyValues)
                {
                    TagFunctionCondition pTFCCondition = TagFunctionCondition.FromJObject(pStrTag, curValue);
                    pLisValues.Add(pTFCCondition);
                }
                pDicConditions.Add(pStrTag, pLisValues);
            }

            TagFunction pTFnFunction = new TagFunction(iJObject["Name"].Value<String>(),
                pDicConditions);
            return (pTFnFunction);
        }

        public Boolean CheckFunction(DeckCard iCurrentCard,
            DeckCard iNextCard)
        {
            foreach (String curTag in Conditions.Keys)
            {
                foreach(TagFunctionCondition curCondition in Conditions[curTag])
                {
                    if(!curCondition.CheckFunction(iCurrentCard,
                        iNextCard))
                    {
                        return (false);
                    }
                }
            }
            return (true);
        }

        #endregion

    }

}
