using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour
{

    #region public objects

    public GameObject CardPrefab;

    #endregion

    #region private objects

    private CardManager cCMrManager;
    private DeckCard cDCdSelectedCard;
    private List<PlacementBase> cLisAllPlacements;

    #endregion

    #region game object methods

    void Start ()
    {
        EventsHandler.Current.StackPointClicked += Current_StackPointClicked;
        EventsHandler.Current.SpreadCardClicked += Current_SpreadCardClicked;

        cCMrManager = new CardManager(CardPrefab);
        cCMrManager.LoadDeck("Standard");

        //we should get all of this startup code into some game file
        //hopefully along with the game rules
        cCMrManager.CreateStackPoint("StartStack", new Vector2(-30.45f, 20));
        cCMrManager.CreateStackPoint("FlippedStack", new Vector2(-20.45f, 20));

        cCMrManager.CreateStackPoint("SuitStack1", new Vector2(-0.45f, 20));
        cCMrManager.CreateStackPoint("SuitStack2", new Vector2(9.55f, 20));
        cCMrManager.CreateStackPoint("SuitStack3", new Vector2(19.55f, 20));
        cCMrManager.CreateStackPoint("SuitStack4", new Vector2(29.55f, 20));

        cCMrManager.CreateSpreadArea("Spread1", new Vector2(-30.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread2", new Vector2(-20.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread3", new Vector2(-10.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread4", new Vector2(-0.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread5", new Vector2(9.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread6", new Vector2(19.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        cCMrManager.CreateSpreadArea("Spread7", new Vector2(29.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);

        cCMrManager.CreateDeckGroup("Standard", "Spread1", 1);
        cCMrManager.CreateDeckGroup("Standard", "Spread2", 2);
        cCMrManager.CreateDeckGroup("Standard", "Spread3", 3);
        cCMrManager.CreateDeckGroup("Standard", "Spread4", 4);
        cCMrManager.CreateDeckGroup("Standard", "Spread5", 5);
        cCMrManager.CreateDeckGroup("Standard", "Spread6", 6);
        cCMrManager.CreateDeckGroup("Standard", "Spread7", 7);
        cCMrManager.CreateDeckGroup("Standard", "StartStack", -1);
        cCMrManager.CreateDeckGroup("Standard", "FlippedStack", 0);
        cCMrManager.CreateDeckGroup("Standard", "SuitStack1", 0);
        cCMrManager.CreateDeckGroup("Standard", "SuitStack2", 0);
        cCMrManager.CreateDeckGroup("Standard", "SuitStack3", 0);
        cCMrManager.CreateDeckGroup("Standard", "SuitStack4", 0);

        cCMrManager.StackPoints["StartStack"].PlaceGroup(cCMrManager.Groups["Standard.StartStack"], DeckCard.CardFacing.Down);
        cCMrManager.StackPoints["FlippedStack"].PlaceGroup(cCMrManager.Groups["Standard.FlippedStack"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread1"].PlaceGroup(cCMrManager.Groups["Standard.Spread1"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread2"].PlaceGroup(cCMrManager.Groups["Standard.Spread2"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread3"].PlaceGroup(cCMrManager.Groups["Standard.Spread3"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread4"].PlaceGroup(cCMrManager.Groups["Standard.Spread4"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread5"].PlaceGroup(cCMrManager.Groups["Standard.Spread5"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread6"].PlaceGroup(cCMrManager.Groups["Standard.Spread6"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread7"].PlaceGroup(cCMrManager.Groups["Standard.Spread7"], DeckCard.CardFacing.Down);
        cCMrManager.StackPoints["SuitStack1"].PlaceGroup(cCMrManager.Groups["Standard.SuitStack1"], DeckCard.CardFacing.Up);
        cCMrManager.StackPoints["SuitStack2"].PlaceGroup(cCMrManager.Groups["Standard.SuitStack2"], DeckCard.CardFacing.Up);
        cCMrManager.StackPoints["SuitStack3"].PlaceGroup(cCMrManager.Groups["Standard.SuitStack3"], DeckCard.CardFacing.Up);
        cCMrManager.StackPoints["SuitStack4"].PlaceGroup(cCMrManager.Groups["Standard.SuitStack4"], DeckCard.CardFacing.Up);

        //flip cards that need flipping
        cCMrManager.FlipTopNCards(1, cCMrManager.SpreadAreas["Spread1"],
            cCMrManager.SpreadAreas["Spread2"],
            cCMrManager.SpreadAreas["Spread3"],
            cCMrManager.SpreadAreas["Spread4"],
            cCMrManager.SpreadAreas["Spread5"],
            cCMrManager.SpreadAreas["Spread6"],
            cCMrManager.SpreadAreas["Spread7"]);

        cLisAllPlacements = new List<PlacementBase>();
        cLisAllPlacements.Add(cCMrManager.StackPoints["SuitStack1"]);
        cLisAllPlacements.Add(cCMrManager.StackPoints["SuitStack2"]);
        cLisAllPlacements.Add(cCMrManager.StackPoints["SuitStack3"]);
        cLisAllPlacements.Add(cCMrManager.StackPoints["SuitStack4"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread1"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread2"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread3"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread4"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread5"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread6"]);
        cLisAllPlacements.Add(cCMrManager.SpreadAreas["Spread7"]);
    }

    void Update()
    {

    }

    #endregion

    #region private methods

    private void SelectCard(DeckCard iCard)
    {
        if(cDCdSelectedCard != iCard)
        {
            if (cDCdSelectedCard != null)
            {
                cDCdSelectedCard.Highlight = false;
            }
            cDCdSelectedCard = iCard;
            cDCdSelectedCard.Highlight = true;
        }
    }

    private void AutoPlayCard(PlacementBase iOrigin,
        DeckCard iCard)
    {
        if(iOrigin.Name == "FlippedStack")
        {
            foreach(PlacementBase curPlacement in cLisAllPlacements)
            {
                DeckCard pDCdPlacementTopCard = curPlacement.TopCard;
                if(pDCdPlacementTopCard != null)
                {
                    //Only move to suit stack
                    if (curPlacement.Name.StartsWith("SuitStack"))
                    {
                        //Only move if next card up and the same suite
                        if (IsNextUp(pDCdPlacementTopCard, iCard) &&
                            iCard.TagsByName["Suit"].Value.Equals(pDCdPlacementTopCard.TagsByName["Suit"].Value))
                        {
                            iOrigin.MoveTopCardToPlacement(curPlacement, false);
                            break;
                        }
                    }
                }
                else
                {
                    //This placement is currently empty
                    if (curPlacement.Name.StartsWith("SuitStack"))
                    {
                        //Only move to stack if it's an ace
                        if(Int32.Parse(iCard.TagsByName["Value"].Value) == 1)
                        {
                            iOrigin.MoveTopCardToPlacement(curPlacement, false);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if(iOrigin.Name.StartsWith("SuitStack"))
            {
                //Let's not allow autoplay back again
            }
            else if(iOrigin.Name.StartsWith("Spread"))
            {
                foreach (PlacementBase curPlacement in cLisAllPlacements)
                {
                    DeckCard pDCdPlacementTopCard = curPlacement.TopCard;
                    if (pDCdPlacementTopCard != null)
                    {
                        //Only move to suit stack
                        if (curPlacement.Name.StartsWith("SuitStack"))
                        {
                            //Only move if next card up and the same suite
                            if (IsNextUp(pDCdPlacementTopCard, iCard) && 
                                iCard.TagsByName["Suit"].Value.Equals(pDCdPlacementTopCard.TagsByName["Suit"].Value))
                            {
                                iOrigin.MoveTopCardToPlacement(curPlacement, false);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //This placement is currently empty
                        if (curPlacement.Name.StartsWith("SuitStack"))
                        {
                            //Only move to stack if it's an ace
                            if (Int32.Parse(iCard.TagsByName["Value"].Value) == 1)
                            {
                                iOrigin.MoveTopCardToPlacement(curPlacement, false);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public Boolean IsNextUp(DeckCard iCard, 
        DeckCard iNextCard)
    {
        Int32 pIntCurCardValue = Int32.Parse(iCard.TagsByName["Value"].Value);
        Int32 pIntNextCardValue = Int32.Parse(iNextCard.TagsByName["Value"].Value);
        return (pIntNextCardValue == (pIntCurCardValue + 1));
    }

    public Boolean IsNextDown(DeckCard iCard,
        DeckCard iNextCard)
    {
        Int32 pIntCurCardValue = Int32.Parse(iCard.TagsByName["Value"].Value);
        Int32 pIntNextCardValue = Int32.Parse(iNextCard.TagsByName["Value"].Value);
        return (pIntNextCardValue == (pIntCurCardValue - 1));
    }

    #endregion

    #region object events

    private void Current_SpreadCardClicked(object sender, SpreadAreaCardClickedEventArgs e)
    {
        //This will flip the top card if it's not already flipped
        if(e.Card == e.SpreadArea.Group.Stack[e.SpreadArea.Group.Stack.Count - 1])
        {
            if(e.Card.Facing == DeckCard.CardFacing.Down)
            {
                e.Card.Flip();
            }
            else
            {
                if(e.IsDouble)
                {
                    AutoPlayCard(e.SpreadArea, e.Card);
                }
            }
        }
    }

    private void Current_StackPointClicked(object sender, StackPointClickedEventArgs e)
    {
        switch(e.StackPoint.Name)
        {
            case "StartStack":
                {
                    if (e.Card.Facing == DeckCard.CardFacing.Down)
                    {
                        e.StackPoint.MoveTopCardToPlacement("FlippedStack", true);
                    }
                    break;
                }
            case "FlippedStack":
                {
                    if (e.IsDouble)
                    {
                        AutoPlayCard(e.StackPoint, e.Card);
                    }
                    break;
                }
        }
    }


    #endregion

}
