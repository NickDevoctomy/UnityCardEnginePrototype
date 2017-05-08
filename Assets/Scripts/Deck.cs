using Assets.Scripts.Extensions;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Deck
{

    #region private objects

    private System.Random cRndRandom;
    private Boolean cBlnInitialised;
    private Texture2D cTexBackTexture;

    #endregion

    #region public properties

    [JsonProperty(Required = Required.Always)]
    public DeckInfo Info;

    [JsonProperty(Required = Required.Always)]
    public DeckCard[] Cards;

    [JsonIgnore]
    public List<DeckCard> Stack;

    [JsonIgnore]
    public CardManager Manager { get; private set; }

    [JsonIgnore]
    public static Dictionary<GameObject, DeckCard> CardsByGameObject = new Dictionary<GameObject, DeckCard>();

    public Texture2D CardBackTexture
    {
        get
        {
            if(cTexBackTexture == null)
            {
                cTexBackTexture = TextureUtility.LoadPNG("Assets\\Cards\\" + Info.BackImageFile);
            }
            return (cTexBackTexture);
        }
    }

    #endregion

    #region constructor / destructor

    public Deck()
    {
        cRndRandom = new System.Random(Environment.TickCount);
        Stack = new List<DeckCard>();
    }

    #endregion

    #region public methods

    public static Deck LoadFromAssets(CardManager iManager,
        String iName)
    {
        String pStrConfigJSON = File.ReadAllText(String.Format("Assets\\Cards\\{0}.deck", iName));
        Deck pDekDeck = JsonConvert.DeserializeObject<Deck>(pStrConfigJSON);
        pDekDeck.Initialise(iManager);
        return (pDekDeck);
    }

    public void Initialise(CardManager iManager)
    {
        if(!cBlnInitialised)
        {
            Manager = iManager;
            foreach (DeckCard curCard in Cards)
            {
                curCard.Initialise(this, Info.BackImageFile);
                Stack.Add(curCard);
            }
            ShuffleStack(Stack.Count * Stack.Count);
            cBlnInitialised = true;
        }
    }

    public void ShuffleStack(Int32 iCount = 500)
    {
        List<DeckCard> pLisStack = Stack.ToList();
        for(Int32 curShuffle = 0; curShuffle < iCount; curShuffle++)
        {
            Int32 pIntRandomIndex = cRndRandom.Next(0, pLisStack.Count);
            DeckCard pDCdShuffleCard = pLisStack[pIntRandomIndex];
            pLisStack.RemoveAt(pIntRandomIndex);
            Int32 pIntRandomInsertIndex = cRndRandom.Next(0, pLisStack.Count);
            pLisStack.Insert(pIntRandomInsertIndex, pDCdShuffleCard);
        }
        Stack = pLisStack;
    }

    public void CreateStack(GameObject iCardPrefab,
        Vector2 iPosition,
        DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down)
    {
        Vector3 pVecCurPosition = new Vector3(iPosition.x, 0, iPosition.y);
        for (Int32 curCard = 0; curCard < Stack.Count; curCard++)
        {
            CreateCard(Stack[curCard], iCardPrefab, pVecCurPosition, iFacing);
            pVecCurPosition = new Vector3(pVecCurPosition.x, pVecCurPosition.y + Stack[curCard].Thickness + 0.03f, pVecCurPosition.z);
        }
    }

    public void CreateCard(GameObject iCardPrefab,
        Vector3 iPosition,
        DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down,
        params DeckCardTag[] iTags)
    {
        List<DeckCard> pLisCards = GetCards(iTags);
        pLisCards[0].Create(iCardPrefab, iPosition, iFacing);
        CardsByGameObject.Add(pLisCards[0].GameObjectRef, pLisCards[0]);
    }

    public void CreateCard(DeckCard iCard,
        GameObject iCardPrefab,
        Vector3 iPosition,
        DeckCard.CardFacing iFacing = DeckCard.CardFacing.Down)
    {
        iCard.Create(iCardPrefab, iPosition, iFacing);
        CardsByGameObject.Add(iCard.GameObjectRef, iCard);
    }

    public List<DeckCard> GetCards(params DeckCardTag[] iTags)
    {
        List<DeckCard> pLisCards = new List<DeckCard>();
        foreach(DeckCard curCard in Cards)
        {
            Int32 pIntMatches = 0;
            foreach(DeckCardTag curTag in iTags)
            {
                if(curCard.TagsByName.ContainsKey(curTag.Name) && 
                    (curCard.TagsByName[curTag.Name].Value == curTag.Value))
                {
                    pIntMatches += 1;
                }
            }
            Boolean pBlnMatch = (pIntMatches == iTags.Length);
            if (pBlnMatch)
            {
                pLisCards.Add(curCard);
            }
        }
        return (pLisCards);
    }

    #endregion

}
