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

    #endregion

    #region game object methods

    void Start ()
    {
        EventsHandler.Current.StackPointClicked += Current_StackPointClicked;
        EventsHandler.Current.SpreadCardClicked += Current_SpreadCardClicked;

        cCMrManager = new CardManager(CardPrefab);
        cCMrManager.LoadDeck("Standard");

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

        //Centred
        //cCMrManager.CreateStackPoint("StartStack", new Vector2(-30.45f, 20));
        //cCMrManager.CreateStackPoint("FlippedStack", new Vector2(-20.45f, 20));

        //cCMrManager.CreateStackPoint("SuitStack1", new Vector2(-0.45f, 20));
        //cCMrManager.CreateStackPoint("SuitStack2", new Vector2(9.55f, 20));
        //cCMrManager.CreateStackPoint("SuitStack3", new Vector2(19.55f, 20));
        //cCMrManager.CreateStackPoint("SuitStack4", new Vector2(29.55f, 20));

        //cCMrManager.CreateSpreadArea("Spread1", new Vector2(-30.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread2", new Vector2(-20.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread3", new Vector2(-10.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread4", new Vector2(-0.45f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread5", new Vector2(9.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread6", new Vector2(19.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);
        //cCMrManager.CreateSpreadArea("Spread7", new Vector2(29.55f, 5), SpreadArea.SpreadAlignment.Near, SpreadArea.SpreadOrientation.Vertical, 1.0F);

        cCMrManager.CreateDeckGroup("Standard", "Spread1", 1);
        cCMrManager.CreateDeckGroup("Standard", "Spread2", 2);
        cCMrManager.CreateDeckGroup("Standard", "Spread3", 3);
        cCMrManager.CreateDeckGroup("Standard", "Spread4", 4);
        cCMrManager.CreateDeckGroup("Standard", "Spread5", 5);
        cCMrManager.CreateDeckGroup("Standard", "Spread6", 6);
        cCMrManager.CreateDeckGroup("Standard", "Spread7", 7);
        cCMrManager.CreateDeckGroup("Standard", "StartStack");
        cCMrManager.CreateDeckGroup("Standard", "FlippedStack");

        cCMrManager.StackPoints["StartStack"].PlaceGroup(cCMrManager.Groups["Standard.StartStack"], DeckCard.CardFacing.Down);
        cCMrManager.StackPoints["FlippedStack"].PlaceGroup(cCMrManager.Groups["Standard.FlippedStack"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread1"].PlaceGroup(cCMrManager.Groups["Standard.Spread1"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread2"].PlaceGroup(cCMrManager.Groups["Standard.Spread2"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread3"].PlaceGroup(cCMrManager.Groups["Standard.Spread3"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread4"].PlaceGroup(cCMrManager.Groups["Standard.Spread4"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread5"].PlaceGroup(cCMrManager.Groups["Standard.Spread5"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread6"].PlaceGroup(cCMrManager.Groups["Standard.Spread6"], DeckCard.CardFacing.Down);
        cCMrManager.SpreadAreas["Spread7"].PlaceGroup(cCMrManager.Groups["Standard.Spread7"], DeckCard.CardFacing.Down);

        //flip cards that need flipping
        cCMrManager.FlipToNCards(1, cCMrManager.SpreadAreas["Spread1"],
            cCMrManager.SpreadAreas["Spread2"],
            cCMrManager.SpreadAreas["Spread3"],
            cCMrManager.SpreadAreas["Spread4"],
            cCMrManager.SpreadAreas["Spread5"],
            cCMrManager.SpreadAreas["Spread6"],
            cCMrManager.SpreadAreas["Spread7"]);
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
            SelectCard(e.Card);
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
                        //e.Card.Flip();
                        e.StackPoint.MoveTopCardToStackPoint("FlippedStack", true);
                        SelectCard(e.Card);
                    }
                    break;
                }
            case "FlippedStack":
                {
                    break;
                }
        }
    }


    #endregion

}
