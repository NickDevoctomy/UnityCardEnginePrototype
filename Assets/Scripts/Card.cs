using Assets.Scripts;
using Assets.Scripts.Meta;
using System;
using UnityEngine;

public class Card : MonoBehaviour
{

    #region private objects

    private MouseHandler cCHrClickHandler;

    #endregion

    #region game object methods

    void Start()
    {
        cCHrClickHandler = new MouseHandler();
        cCHrClickHandler.Clicked += CCHrClickHandler_Clicked;
        cCHrClickHandler.DoubleClicked += CCHrClickHandler_DoubleClicked;
        cCHrClickHandler.BeginDragging += CCHrClickHandler_BeginDragging;
    }

    void Update()
    {
        cCHrClickHandler.Update(this.gameObject);

        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        pDCdCard.UpdateTween();
    }

    void OnMouseDown()
    {
        cCHrClickHandler.OnMouseDown(this.gameObject);
    }

    void OnMouseEnter()
    {
        cCHrClickHandler.OnMouseEnter(this.gameObject);
    }

    void OnMouseExit()
    {
        cCHrClickHandler.OnMouseExit(this.gameObject);
    }

    void OnMouseUp()
    {
        cCHrClickHandler.OnMouseUp(this.gameObject);
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

    private void CCHrClickHandler_BeginDragging(object sender, BeginDraggingEventArgs e)
    {
        e.Allow = true;
    }

    #endregion

}
