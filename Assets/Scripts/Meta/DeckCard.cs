﻿using Assets.Scripts.Debugging;
using Assets.Scripts.Extensions;
using Assets.Scripts.Meta.Extensions;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
//using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Meta
{

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
        private Queue<Waypoint> cQueWaypoints;
        private DateTime cDteCurrentStartedAt;
        private Waypoint cWayCurrentWaypoint;
        private Vector3 pVe3LastPosition;
        private float cFltCurrentPathPercent = 0.0f;
        private bool cBlnIsTweening = false;

        #endregion

        #region public properties

        [JsonProperty(Required = Required.Always)]
        public String FrontImageFile { get; set; }

        [JsonIgnore]
        public String BackImageFile { get; private set; }

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
                if (GameObjectRef != null)
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
        public Queue<Waypoint> MovementSets
        {
            get
            {
                return (cQueWaypoints);
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
            cQueWaypoints = new Queue<Waypoint>();
        }

        /// <summary>
        /// Constructor used primarily for testing
        /// </summary>
        /// <param name="iTags"></param>
        public DeckCard(params DeckCardTag[] iTags)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.Constructor({0})", iTags.ToTagString());

            cQueWaypoints = new Queue<Waypoint>();
            Tags = iTags;
            EnumerateTagsByName();
        }

        #endregion

        #region private methods

        private Boolean SelectNextWaypoint()
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.SelectNextWaypoint()");

            try
            {
                Waypoint pMStNext = cQueWaypoints.Dequeue();
                cFltCurrentPathPercent = 0.0f;
                cDteCurrentStartedAt = DateTime.UtcNow;
                Logman.Log(BaseLogger.MessageType.Information, "Got next movement set '{0}'.", pMStNext.Name);
                cWayCurrentWaypoint = pMStNext;
                pVe3LastPosition = GameObjectRef.transform.position;
                return (true);
            }
            catch(InvalidOperationException)    //Empty queue
            {
                cWayCurrentWaypoint = null;
                return (false);
            }
        }

        private void EnumerateTagsByName()
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.EnumerateTagsByName()");

            Dictionary<String, DeckCardTag> pDicTags = new Dictionary<String, DeckCardTag>();
            foreach (DeckCardTag curTag in Tags)
            {
                pDicTags.Add(curTag.Name, curTag);
            }
            TagsByName = pDicTags;
        }

        #endregion

        #region public methods

        public void Initialise(Deck iDeck,
            String iBackImageFile)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.Initialise({0}, {1})", iDeck.Info.Name, iBackImageFile);

            cDekDeck = iDeck;
            BackImageFile = iBackImageFile;
            EnumerateTagsByName();
        }

        public void Create(GameObject iCardPrefab,
            Vector3 iPosition,
            CardFacing iFacing)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.Create({0}, {1}, {2})", iCardPrefab.name, iPosition.ToString(), iFacing.ToString());

            //only create our object once, otherwise just move it to the new location
            if (GameObjectRef == null)
            {
                //create our card and map the textures
                GameObjectRef = GameObject.Instantiate(iCardPrefab, new Vector3(iPosition.x, iPosition.y, iPosition.z), Quaternion.identity);
                Highlight = false;
                GameObject pGOtFront = GameObjectRef.GetChildGameObject("Front");
                GameObject pGOtBack = GameObjectRef.GetChildGameObject("Back");

                Logman.Log(BaseLogger.MessageType.Information, "Loading texture file '{0}'.", FrontImageFile);

                Texture2D textureFront = IOUtility.LoadStreamingAssestsFileAsTexture2D(String.Format("Cards/{0}", FrontImageFile));
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
                GameObjectRef.transform.rotation = Quaternion.Euler(iFacing.ToVector3());
            }
            Facing = iFacing;
        }

        public void Flip(Boolean iInstant)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.Flip({0})", iInstant);

            Facing = Facing == CardFacing.Up ? CardFacing.Down : CardFacing.Up;
            if (iInstant)
            {
                GameObjectRef.transform.rotation = Quaternion.Euler(Facing.ToVector3());
            }
            else
            {
                iTween.RotateTo(GameObjectRef, Facing.ToVector3(), 0.5f);
            }
        }

        public void AddWaypoints(List<Waypoint> iWaypoints)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.AddWaypoints({0})", iWaypoints.Count);

            if (!IsTweening)
            {
                foreach (Waypoint curWaypoint in iWaypoints)
                {
                    cQueWaypoints.Enqueue(curWaypoint);
                }
            }
        }

        public void StartTween(Boolean iRestart)
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.StartTween({0})", iRestart);

            if (iRestart)
            {
                cFltCurrentPathPercent = 0;
            }
            if (cWayCurrentWaypoint == null)
            {
                Logman.Log(BaseLogger.MessageType.Information, "Selecting next waypoint.");
                cBlnIsTweening = SelectNextWaypoint();
            }
            cDteCurrentStartedAt = DateTime.UtcNow;
        }

        public void PauseTween()
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.PauseTween()");

            cBlnIsTweening = false;
        }

        public void StopTween()
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.StopTween()");

            cFltCurrentPathPercent = 0;
            cBlnIsTweening = false;
        }

        public Boolean UpdateTween()
        {
            Logman.Log(BaseLogger.MessageType.Verbose, "DeckCard.UpdateTween()");

            if (cWayCurrentWaypoint == null)
            {
                cBlnIsTweening = SelectNextWaypoint();
            }

            if (IsTweening)
            {
                if (cWayCurrentWaypoint.Instant)
                {
                    //This set is done instantly with no tweening
                    if (cWayCurrentWaypoint.Position.HasValue)
                    {
                        GameObjectRef.transform.position = cWayCurrentWaypoint.Position.Value;
                    }
                    if (cWayCurrentWaypoint.Rotation.HasValue)
                    {
                        GameObjectRef.transform.rotation = Quaternion.Euler(cWayCurrentWaypoint.Rotation.Value);
                    }
                    cFltCurrentPathPercent = 1.0f;                                      //This has finished
                }
                else
                {
                    //Increment the position
                    cFltCurrentPathPercent += cWayCurrentWaypoint.PercentPerSecond * Time.deltaTime;
                    if (cFltCurrentPathPercent > 1.0f) cFltCurrentPathPercent = 1.0f;

                    //Change position if it's been set
                    if (cWayCurrentWaypoint.Position.HasValue)
                    {
                        //Tween from last position (when movement set was selected) to next position
                        Vector3 pVe3CurrentPos = iTween.PointOnPath(new Vector3[] { pVe3LastPosition, cWayCurrentWaypoint.Position.Value }, cFltCurrentPathPercent);
                        GameObjectRef.transform.position = pVe3CurrentPos;
                    }

                    //Change rotation if it's been set
                    if (cWayCurrentWaypoint.Rotation.HasValue)
                    {
                        iTween.RotateUpdate(GameObjectRef, cWayCurrentWaypoint.Rotation.Value, cWayCurrentWaypoint.RotationTime);
                    }
                }

                if (cFltCurrentPathPercent == 1.0f)                                     //We have finished tweening
                {
                    Logman.Log(BaseLogger.MessageType.Verbose, "Previous movement set took '{0}' to complete.", DateTime.UtcNow - cDteCurrentStartedAt);
                    cBlnIsTweening = SelectNextWaypoint();
                }
            }
            return (IsTweening);
        }

        #endregion

    }

}
