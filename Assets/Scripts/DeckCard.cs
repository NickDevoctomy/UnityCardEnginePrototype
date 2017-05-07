using Assets.Scripts.Extensions;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckCard
{

    #region public enums

    public enum CardFacing
    {
        None = 0,
        Up,
        Down
    }

    #endregion

    #region private objects

    private Deck cDekDeck;
    private List<Transform> cLisWaypoints;
    private Stack<List<Transform>> cStaWaypointHistory;
    private float cFltCurrentPathPercent = 0.0f;
    private float cFltPathPercentPerSecond = 0.5f;
    private bool cBlnIsTweening = false;

    #endregion

    #region public properties

    [JsonProperty(Required = Required.Always)]
    public String FrontImageFile { get; set; }

    [JsonIgnore]
    public String BackImageFile { get; set; }

    [JsonProperty(Required = Required.Always)]
    public DeckCardTag[] Tags { get; set; }

    [JsonIgnore]
    public Deck Deck
    {
        get
        {
            return (cDekDeck);
        }
    }

    [JsonIgnore]
    public Dictionary<String, DeckCardTag> TagsByName { get; private set; }

    [JsonIgnore]
    public CardFacing Facing { get; private set; }

    [JsonIgnore]
    public GameObject GameObjectRef { get; private set; }

    [JsonIgnore]
    public float Thickness { get; private set; }

    [JsonIgnore]
    public Boolean Highlight
    {
        get
        {
            if(GameObjectRef != null)
            {
                ParticleSystem pPSmParticles = GameObjectRef.GetComponent<ParticleSystem>();
                return (pPSmParticles.isPlaying);
            }
            else
            {
                return (false);
            }
        }
        set
        {
            if (GameObjectRef != null)
            {
                ParticleSystem pPSmParticles = GameObjectRef.GetComponent<ParticleSystem>();
                if (value)
                {
                    pPSmParticles.Play();
                }
                else
                {
                    pPSmParticles.Stop();
                }
            }
        }
    }

    [JsonIgnore]
    public List<Transform> Waypoints
    {
        get
        {
            return (cLisWaypoints);
        }
    }

    [JsonIgnore]
    public Boolean IsTweening
    {
        get
        {
            return (cBlnIsTweening);
        }
    }

    #endregion

    #region constructor / destructor

    public DeckCard()
    {
        cLisWaypoints = new List<Transform>();
        cStaWaypointHistory = new Stack<List<Transform>>();
    }

    #endregion

    #region public methods

    public void Initialise(Deck iDeck,
        String iBackImageFile)
    {
        cDekDeck = iDeck;
        BackImageFile = iBackImageFile;
        Dictionary<String, DeckCardTag> pDicTags = new Dictionary<String, DeckCardTag>();
        foreach(DeckCardTag curTag in Tags)
        {
            pDicTags.Add(curTag.Name, curTag);
        }
        TagsByName = pDicTags;
    }

    public void Create(GameObject iCardPrefab,
        Vector3 iPosition,
        CardFacing iFacing)
    {
        //only create our object once, otherwise just move it to the new location
        if (GameObjectRef == null)
        {
            //create our card and map the textures
            GameObjectRef = GameObject.Instantiate(iCardPrefab, new Vector3(iPosition.x, iPosition.y, iPosition.z), Quaternion.identity);
            Highlight = false;
            GameObject pGOtFront = GameObjectRef.GetChildGameObject("Front");
            GameObject pGOtBack = GameObjectRef.GetChildGameObject("Back");

            Texture2D textureFront = TextureUtility.LoadPNG("Assets\\Cards\\" + FrontImageFile);
            Renderer cardRenderer = pGOtFront.GetComponent<Renderer>();
            cardRenderer.material.mainTexture = textureFront;

            Texture2D textureBack = cDekDeck.CardBackTexture;
            cardRenderer = pGOtBack.GetComponent<Renderer>();
            cardRenderer.material.mainTexture = textureBack;

            Thickness = pGOtFront.transform.position.y - pGOtBack.transform.position.y;
        }
        else
        {
            GameObjectRef.transform.position = iPosition;
        }

        if (iFacing == CardFacing.Down)
        {
            GameObjectRef.transform.rotation = Quaternion.AngleAxis(180.0f, new Vector3(0.0f, 0.0f, 1.0f));
        }
        Facing = iFacing;    
    }

    public void Flip()
    {
        Facing = Facing == CardFacing.Up ? CardFacing.Down : CardFacing.Up;
        GameObjectRef.transform.rotation = Quaternion.AngleAxis(Facing == CardFacing.Up ? 0.0f : 180.0f, new Vector3(0.0f, 0.0f, 1.0f));
    }

    public void AddPathWaypoints(params Transform[] iWaypoints)
    {
        if (!IsTweening)
        {
            cLisWaypoints.AddRange(iWaypoints);
        }
    }

    public void StartTween(Boolean iRestart)
    {
        if(iRestart) cFltCurrentPathPercent = 0;
        cBlnIsTweening = true;
    }

    public void PauseTween()
    {
        cBlnIsTweening = false;
    }

    public void StopTween()
    {
        cFltCurrentPathPercent = 0;
        cBlnIsTweening = false;
    }

    public Boolean UpdateTween()
    {
        if(IsTweening)
        {
            cFltCurrentPathPercent += cFltPathPercentPerSecond * Time.deltaTime;
            if (cFltCurrentPathPercent > 1.0f) cFltCurrentPathPercent = 1.0f;

            Vector3 pVe3CurrentPos = iTween.PointOnPath(Waypoints.ToArray(), cFltCurrentPathPercent);
            GameObjectRef.transform.position = pVe3CurrentPos;
            //we need rotation here too, how?

            //iTween.PutOnPath(GameObjectRef, Waypoints.ToArray(), cFltCurrentPathPercent);
            //iTween.MoveUpdate(GameObjectRef, iTween.Hash("path", Waypoints.ToArray(), "time", 10, "islocal", true, "orientToPath", true));

            if (cFltCurrentPathPercent == 1.0f)                                     //We have finished tweening
            {
                cStaWaypointHistory.Push(new List<Transform>(cLisWaypoints));       //Add to our history
                cLisWaypoints.Clear();                                              //Clear our waypoints
                cBlnIsTweening = false;                                             //Drop tweening flag
            }
        }
        return (IsTweening);
    }

    public void UndoLastTween(Boolean iStart)
    {
        List<Transform> pLisWaypoints = cStaWaypointHistory.Pop();
        pLisWaypoints.Reverse();
        AddPathWaypoints(pLisWaypoints.ToArray());
        if (iStart) StartTween(true);
    }

    public void DrawPath()
    {
        iTween.DrawPath(Waypoints.ToArray());
    }

    #endregion

}
