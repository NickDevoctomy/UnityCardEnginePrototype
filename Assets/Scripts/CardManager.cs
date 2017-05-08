using Assets.Scripts;
using Assets.Scripts.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager
{

    #region private objects

    private GameObject cGOtCardPrefab;
    private Dictionary<String, Deck> cDicDecks;
    private Dictionary<String, DeckGroup> cDicDeckGroups;
    private Dictionary<String, StackPoint> cDicStackPoints;
    private Dictionary<String, SpreadArea> cDicSpreadAreas;
    private Dictionary<String, PlacementBase> cDicPlacements;

    #endregion

    #region public properties

    public Dictionary<String, Deck> Decks
    {
        get
        {
            return (cDicDecks);
        }
    }

    public Dictionary<String, DeckGroup> Groups
    {
        get
        {
            return (cDicDeckGroups);
        }
    }

    public Dictionary<String, StackPoint> StackPoints
    {
        get
        {
            return (cDicStackPoints);
        }
    }

    public Dictionary<String, SpreadArea> SpreadAreas
    {
        get
        {
            return (cDicSpreadAreas);
        }
    }

    public Dictionary<String, PlacementBase> Placements
    {
        get
        {
            return (cDicPlacements);
        }
    }

    public List<PlacementBase> AllPlacements
    {
        get
        {
            return (cDicPlacements.Values.ToList());
        }
    }

    #endregion

    #region constructor / destructor

    public CardManager(GameObject iCardPrefab)
    {
        cGOtCardPrefab = iCardPrefab;
        cDicDecks = new Dictionary<String, Deck>();
        cDicDeckGroups = new Dictionary<String, DeckGroup>();
        cDicStackPoints = new Dictionary<String, StackPoint>();
        cDicSpreadAreas = new Dictionary<String, SpreadArea>();
        cDicPlacements = new Dictionary<String, PlacementBase>();
    }

    #endregion

    #region public methods

    public Deck LoadDeck(String iName)
    {
        Deck pDekDeck = Deck.LoadFromAssets(this, iName);
        cDicDecks.Add(iName, pDekDeck);
        return (pDekDeck);
    }

    public Game LoadGame(String iName)
    {
        Game pGamGame = Game.LoadFromAssets(this, iName);
        return (pGamGame);
    }

    /// <summary>
    /// Create a deck group from a deck.  This method will take cards from a deck stack and add it to a deck group.
    /// </summary>
    /// <param name="iDeckName">Name of the deck to create the group from.</param>
    /// <param name="iGroupName">Name of the new group</param>
    /// <param name="iCount">Count of cards to put into the group, -1 for the remainder of the stack</param>
    public DeckGroup CreateDeckGroup(String iDeckName,
        String iGroupName,
        Int32 iCount = -1)
    {
        String pStrKey = String.Format("{0}.{1}", iDeckName, iGroupName);
        if (cDicDecks.ContainsKey(iDeckName))
        {
            if (!cDicDeckGroups.ContainsKey(pStrKey))
            {
                Deck pDekDeck = cDicDecks[iDeckName];
                DeckGroup pDGpGroup = new DeckGroup(pDekDeck, iDeckName, iGroupName);
                Int32 pIntCount = iCount == -1 ? pDekDeck.Stack.Count : iCount;
                if(pIntCount > 0)
                {
                    while (pDGpGroup.Stack.Count < pIntCount)
                    {
                        if (pDekDeck.Stack.Count > 0)
                        {
                            DeckCard pDCdTopOfStack = pDekDeck.Stack[pDekDeck.Stack.Count - 1];
                            pDekDeck.Stack.RemoveAt(pDekDeck.Stack.Count - 1);
                            pDGpGroup.Stack.Add(pDCdTopOfStack);
                        }
                        else
                        {
                            throw new Exception("No cards left on the stack.");
                        }
                    }
                }
                cDicDeckGroups.Add(pStrKey, pDGpGroup);
                return (pDGpGroup);
            }
            else
            {
                throw new KeyNotFoundException(String.Format("There is already a deck group the key '{0}'.", pStrKey));
            }
        }
        else
        {
            throw new KeyNotFoundException(String.Format("There are deck with the name '{0}'.", iDeckName));
        }
    }

    public StackPoint CreateStackPoint(String iName,
        Vector2 iPosition)
    {
        if(!cDicStackPoints.ContainsKey(iName))
        {
            StackPoint pSPtPoint = new StackPoint(this, 
                cGOtCardPrefab, 
                iName, 
                new Vector3(iPosition.x, 0.01f, iPosition.y));
            cDicStackPoints.Add(iName, pSPtPoint);
            cDicPlacements.Add(iName, pSPtPoint);

            ////let's create a plane so that we can see our stack point
            //GameObject pGOtSpreadArea = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //pGOtSpreadArea.transform.position = new Vector3(iPosition.x, 0.001f, iPosition.y);
            //pGOtSpreadArea.transform.localScale = new Vector3(cGOtCardPrefab.transform.localScale.x, 0.001f, cGOtCardPrefab.transform.localScale.z);

            return (pSPtPoint);
        }
        else
        {
            throw new Exception(String.Format("A spread area already exists with the name '{0}'.", iName));
        }
    }

    public SpreadArea CreateSpreadArea(String iName,
        Vector2 iPosition,
        SpreadArea.SpreadAlignment iAlignment,
        SpreadArea.SpreadOrientation iOrientation,
        float iLength)
    {
        if (!cDicSpreadAreas.ContainsKey(iName))
        {
            SpreadArea pSAeArea = new SpreadArea(this, 
                cGOtCardPrefab, 
                iName, 
                new Vector3(iPosition.x, 0.01f, iPosition.y),
                iAlignment,
                iOrientation, 
                iLength);
            cDicSpreadAreas.Add(iName, pSAeArea);
            cDicPlacements.Add(iName, pSAeArea);

            ////let's create a plane so that we can see our spread area
            //GameObject pGOtSpreadArea = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //pGOtSpreadArea.transform.position = new Vector3(iPosition.x, 0.001f, iPosition.y);
            //pGOtSpreadArea.transform.localScale = new Vector3(cGOtCardPrefab.transform.localScale.x, 0.001f, cGOtCardPrefab.transform.localScale.z);

            return (pSAeArea);
        }
        else
        {
            throw new Exception(String.Format("A spread area already exists with the name '{0}'.", iName));
        }
    }

    public DeckGroup GetDeckCardGroup(DeckCard iCard)
    {
        foreach(DeckGroup curGroup in Groups.Values)
        {
            if(curGroup.Stack.Contains(iCard))
            {
                return (curGroup);
            }
        }
        return (null);
    }

    public void FlipTopNCards(Int32 iCount,
        params PlacementBase[] iPlacements)
    {
        foreach(PlacementBase curPlacement in iPlacements)
        {
            curPlacement.FlipTopNCards(iCount);
        }
    }

    #endregion

}
