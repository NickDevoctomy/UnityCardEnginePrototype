using Assets.Scripts;
using Assets.Scripts.Meta;
using System;
using UnityEngine;

public class Card : MonoBehaviour
{

    #region private objects

    private ClickHandler cCHrClickHandler;
    private Vector3 cVe3StartDragPos;
    private Boolean cBlnDragging;
    private float cFltHighestCard = 0.0f;

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
        if (cBlnDragging)
        {
            RaycastHit pRHtHit;
            Ray pRayRayToTable = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pRayRayToTable, out pRHtHit))
            {
                //need to get the y coordinate of the highest card
                transform.position = new Vector3( pRHtHit.point.x, cFltHighestCard, pRHtHit.point.z);
            }
        }
        else
        {
            DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
            pDCdCard.UpdateTween();
        }

        cCHrClickHandler.Update();
    }

    void OnMouseDown()
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        cVe3StartDragPos = this.gameObject.transform.position;
        cFltHighestCard = pDCdCard.Deck.Manager.GetHighestCardPos() + 0.03f;            //This y co-ordinate will prevent the card from colliding with any other cards
        cCHrClickHandler.OnMouseDown();
        cBlnDragging = true;
    }

    void OnDrawGizmos()
    {
        //nothing to do
    }

    void OnMouseEnter()
    {
        //highlight the card and any cards above it
    }

    void OnMouseExit()
    {
        //unhighlight the card and any cards above it
    }

    void OnMouseUp()
    {
        this.gameObject.transform.position = cVe3StartDragPos;
        cBlnDragging = false;
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
