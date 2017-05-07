using Assets.Scripts;
using System;
using UnityEngine;

public class Card : MonoBehaviour
{

    #region private objects

    private ClickHandler cCHrClickHandler;

    #endregion

    #region game object methods

    void Start()
    {
        cCHrClickHandler = new ClickHandler();
        cCHrClickHandler.Clicked += CCHrClickHandler_Clicked;
        cCHrClickHandler.DoubleClicked += CCHrClickHandler_DoubleClicked;
    }

    void Update()
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        pDCdCard.UpdateTween();

        cCHrClickHandler.Update();
    }

    void OnMouseDown()
    {
        cCHrClickHandler.OnMouseDown();
    }

    void OnDrawGizmos()
    {
        //DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
    }

    #endregion

    #region object events

    private void CCHrClickHandler_DoubleClicked(object sender, EventArgs e)
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        EventsHandler.Current.DoubleClick(pDCdCard);
    }

    private void CCHrClickHandler_Clicked(object sender, EventArgs e)
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        EventsHandler.Current.SingleClick(pDCdCard);
    }

    #endregion

}
