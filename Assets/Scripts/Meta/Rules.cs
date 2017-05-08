using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Meta
{

    public class Rules
    {

        #region private objects

        private Dictionary<String, TagFunction> cDicTagFunctions;

        #endregion

        #region public properties

        public Dictionary<String, TagFunction> TagFunctions
        {
            get
            {
                return (cDicTagFunctions);
            }
        }

        #endregion

        #region construtor / destructor

        public Rules()
        {
            cDicTagFunctions = new Dictionary<String, TagFunction>();
        }

        #endregion

        #region public methods

        public static  Rules FromJObject(JObject iJObject)
        {
            Rules pRulRules = new Rules();

            if (iJObject["TagFunctions"] != null)
            {
                JArray pJAyTagFunctions = iJObject["TagFunctions"].Value<JArray>();
                foreach(JObject curFunction in pJAyTagFunctions)
                {
                    TagFunction pTFnFunction = TagFunction.FromJObject(curFunction);
                    pRulRules.cDicTagFunctions.Add(pTFnFunction.Name, pTFnFunction);
                }

            }

            return (pRulRules);
        }

        public Boolean CheckFunction(String iName,
            DeckCard iCurrentCard,
            DeckCard iNextCard)
        {
            if(TagFunctions.ContainsKey(iName))
            {
                TagFunction pTFnFunction = TagFunctions[iName];
                if(pTFnFunction.CheckFunction(iCurrentCard,
                    iNextCard))
                {
                    return (true);
                }
            }
            return (false);
        }

        #endregion

    }

}
