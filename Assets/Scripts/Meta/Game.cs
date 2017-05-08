using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            String pStrConfigJSON = File.ReadAllText(String.Format("Assets\\Cards\\{0}.game", iName));
            Game pGamGame = JsonConvert.DeserializeObject<Game>(pStrConfigJSON);
            pGamGame.Initialise(iManager);
            return (pGamGame);
        }

        public void Initialise(CardManager iManager)
        {
            Manager = iManager;
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
                Manager.FlipTopNCards(curStackPoint.FlipCount, Manager.StackPoints[curStackPoint.Name]);
            }
            foreach (GamePlacementSpreadArea curSpreadArea in Placements.SpreadAreas)
            {
                Manager.FlipTopNCards(curSpreadArea.FlipCount, Manager.SpreadAreas[curSpreadArea.Name]);
            }
        }

        #endregion

    }

}
