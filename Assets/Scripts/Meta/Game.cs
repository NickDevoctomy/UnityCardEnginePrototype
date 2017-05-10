using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Meta
{

    public class Game
    {

        #region public properties

        [JsonIgnore]
        public CardManager Manager { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public GameInfo Info { get; set; }

        [JsonProperty(Required = Required.Always)]
        public GamePlacements Placements { get; set; }

        [JsonProperty(Required = Required.Always)]
        public GameGroup[] Groups { get; set; }

        [JsonIgnore]
        public Rules Rules { get; private set; }

        #endregion

        #region constructor / destructor

        public Game()
        {

        }

        #endregion

        #region public methods

        public static Game LoadFromAssets(CardManager iManager,
            String iName)
        {
            String pStrConfigJSON = File.ReadAllText(String.Format("{0}/Cards/{1}.game", Application.streamingAssetsPath, iName));
            Game pGamGame = JsonConvert.DeserializeObject<Game>(pStrConfigJSON);

            //Let's parse the rules manually as it's going to be quite complex
            JObject pJOtJSON = JObject.Parse(pStrConfigJSON);
            if(pJOtJSON["Rules"] != null)
            {
                Rules pRulRules = Rules.FromJObject(pJOtJSON["Rules"].Value<JObject>());
                pGamGame.Rules = pRulRules;

                DeckCard pDCnCard1 = new DeckCard(new DeckCardTag() { Name = "Suit", Value = "Hearts" });
                DeckCard pDCnCard2 = new DeckCard(new DeckCardTag() { Name = "Suit", Value = "Diamonds" });
                DeckCard pDCnCard3 = new DeckCard(new DeckCardTag() { Name = "Suit", Value = "Clubs" });
                DeckCard pDCnCard4 = new DeckCard(new DeckCardTag() { Name = "Suit", Value = "Spades" });

                Boolean pBlnChecked = pRulRules.CheckFunction("OppositeColour", pDCnCard1, pDCnCard1);
                if(pBlnChecked)
                {

                }

            }

            pGamGame.Initialise(iManager);
            return (pGamGame);
        }

        public void Initialise(CardManager iManager)
        {
            Manager = iManager;
            Manager.LoadDeck(Info.Deck);
        }

        public void Setup()
        {
            //Create our stack points
            foreach(GamePlacementStackPoint curStackPoint in Placements.StackPoints)
            {
                Manager.CreateStackPoint(curStackPoint.Name, curStackPoint.PositionAsVector2);
            }

            //Create spread areas
            foreach(GamePlacementSpreadArea curSpreadArea in Placements.SpreadAreas)
            {
                Manager.CreateSpreadArea(curSpreadArea.Name,
                    curSpreadArea.PositionAsVector2,
                    curSpreadArea.AligmentAsSpreadAlignment,
                    curSpreadArea.OrientationAsSpreadOrientation,
                    curSpreadArea.Length);
            }

            //Create groups
            foreach(GameGroup curGroup in Groups)
            {
                Manager.CreateDeckGroup(Info.Deck, curGroup.Placement, curGroup.CardCount);
            }

            //Place all groups
            foreach (GameGroup curGroup in Groups)
            {
                Manager.Placements[curGroup.Placement].PlaceGroup(Manager.Groups[String.Format("{0}.{1}", Info.Deck, curGroup.Placement)], DeckCard.CardFacing.Down);
            }

            //Flip cards that need flipping
            foreach(GamePlacementStackPoint curStackPoint in Placements.StackPoints)
            {
                Manager.FlipTopNCards(curStackPoint.FlipCount, true, Manager.StackPoints[curStackPoint.Name]);
            }
            foreach (GamePlacementSpreadArea curSpreadArea in Placements.SpreadAreas)
            {
                Manager.FlipTopNCards(curSpreadArea.FlipCount, true, Manager.SpreadAreas[curSpreadArea.Name]);
            }
        }

        #endregion

    }

}
