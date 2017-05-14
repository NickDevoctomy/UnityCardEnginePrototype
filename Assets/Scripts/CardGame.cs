using Assets.Scripts;
using Assets.Scripts.Config;
using Assets.Scripts.Debugging;
using Assets.Scripts.Meta;
using Assets.Scripts.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardGame : MonoBehaviour
{

    #region public objects

    public GameObject CardPrefab;
    public String Game;

    #endregion

    #region private objects

    private CardManager cCMrManager;
    private DeckCard cDCdSelectedCard;

    #endregion

    #region game object methods

    void Start ()
    {
        AppConfig.Current.Initialise();

        EventsHandler.Current.StackPointClicked += Current_StackPointClicked;
        EventsHandler.Current.SpreadCardClicked += Current_SpreadCardClicked;

        try
        {
            Game pGamGame = null;

            using (var log = Logman.Time("Loading game"))
            {
                cCMrManager = new CardManager(CardPrefab);
                pGamGame = cCMrManager.LoadGame(Game);
            }

            using (Logman.Time("Setting up game."))
            {
                pGamGame.Setup();
            }
        }
        catch(Exception ex)
        {
            Logman.LogException(ex);
        }
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
            foreach(PlacementBase curPlacement in cCMrManager.AllPlacements)
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
                foreach (PlacementBase curPlacement in cCMrManager.AllPlacements)
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
                e.Card.Flip(false);
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
