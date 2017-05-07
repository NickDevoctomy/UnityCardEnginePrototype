using Assets.Scripts;
using UnityEngine;

public class Card : MonoBehaviour
{

    #region game object methods

    void Start()
    {

    }

    void Update()
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        pDCdCard.UpdateTween();
    }

    void OnMouseDown()
    {
        DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
        EventsHandler.Current.CardOnMouseDown(pDCdCard);
    }

    void OnDrawGizmos()
    {
        //DeckCard pDCdCard = Deck.CardsByGameObject[this.gameObject];
    }

    #endregion

}
