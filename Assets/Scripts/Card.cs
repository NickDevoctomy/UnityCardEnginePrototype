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
    private float cFltDistance;

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

        if (cBlnDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(cFltDistance);
            transform.position = rayPoint;
        }
        else
        {
            pDCdCard.UpdateTween();
        }


        cCHrClickHandler.Update();
    }

    void OnMouseDown()
    {
        cVe3StartDragPos = this.gameObject.transform.position;
        cFltDistance = Vector3.Distance(transform.position, Camera.main.transform.position) - 3.0f;
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
