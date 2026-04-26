using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;
using NUnit.Framework.Internal.Builders;
using Unity.VisualScripting;

public class GameManagerScript : MonoBehaviour
{
    //public List<Card> deck = new List<Card>();
    public GameObject hoveringcard;
    public GameObject draggingcard;
    RaycastHit2D[] hits;
    private float clickTime;
    public float holdToDragTime;
    private bool isDragging;
    private bool mousePressed;
    public float angleMultiplier;

    [System.Serializable]
    public class CardData
    {
        public string name;
        public int suit;
        public int value;

        public CardData(int suit, int value)
        {
            this.suit = suit;
            this.value = value;
            name = suit + " " + value;
        }
    }

    [System.Serializable]
    public class DeckData
    {
        public string deckName;
        public List<CardData> deckData;

        public DeckData(string name, List<CardData> deck)
        {
            deckName = name;
            deckData = deck;
        }

        public void AddStandardDeck()
        {
            for (int s = 0; s < 4; s++)
            {
                for (int v = 1; v < 14; v++)
                {
                    deckData.Add(new CardData(s, v));
                }
            }
        }
    }

    public GameObject card;
    public List<DeckData> cardDecks;
    public DeckData currentDeck;
    public int currentDecknum;
    public GameObject room;
    public List<Sprite> cardSprites;

    void Start()
    {
        clickTime = Time.time;
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            mousePressed = false;
            if (isDragging)
            {
                //print(draggingcard);
                if (hoveringcard != null)
                {
                    //if (draggingcard.GetComponent<CardScript>().cardValue == 0)
                    {
                        room.GetComponent<RoomManager>().EqualizeDistance();
                    }
                }
                mousePressed = false;
                draggingcard.GetComponent<CardScript>().NotDragging();
                isDragging = false;
                draggingcard = null;
                //print("released");
            }
        }
        else
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //print(mousepos);
            hits = Physics2D.RaycastAll(mousepos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("CardLayer"));
            if (hits.Length != 0)
            {
                //print(hits[0].collider.gameObject);
                if (!isDragging)
                {
                    HoveringOverCard(hits[0]);
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        mousePressed = true;
                        clickTime = Time.time;
                    }
                    else if (mousePressed && !isDragging && Time.time - clickTime > holdToDragTime)
                    {
                        hoveringcard.GetComponent<CardScript>().IsDragging();
                        draggingcard = hoveringcard;
                        isDragging = true;
                    }
                }
                else
                {
                    if (hits.Length > 1)
                    {
                        HoveringOverCard(hits[1]);
                    }
                    else
                    {
                        NotHovering();
                    }
                }
            }
            else
            {
                NotHovering();
            }

        }

    }

    public void DragTimeChange(float value)
    {
        holdToDragTime = value;
    }

    public void AngleMultiplierChange(float value)
    {
        angleMultiplier = value;
    }

    private void HoveringOverCard(RaycastHit2D card)
    {
        if (hoveringcard == null)
        {
            hoveringcard = card.transform.gameObject;
            hoveringcard.GetComponent<CardScript>().IsHovering();
        }
        else if (hoveringcard != card.transform.gameObject)
        {
            hoveringcard.GetComponent<CardScript>().NotHovering();
            hoveringcard = card.transform.gameObject;
            hoveringcard.GetComponent<CardScript>().IsHovering();
        }
    }

    private void NotHovering()
    {
        if (hoveringcard != null)
        {
            hoveringcard.GetComponent<CardScript>().NotHovering();
            hoveringcard = null;
        }
    }

    public void StartGame()
    {
        print("Starting Game");
        currentDeck.AddStandardDeck();
        print("Added Deck");
        DrawRoom();
    }

    public void DrawRoom()
    {
        RoomManager roomManager = room.GetComponent<RoomManager>();
        for (int i = roomManager.cards.Count; i < roomManager.roomSize; i++)
        {
            CardData cardDraw = currentDeck.deckData[Random.Range(0, currentDeck.deckData.Count)];
            roomManager.cards.Add(Instantiate(card, room.transform));
            roomManager.cards[i].GetComponent<CardScript>().GetSetUp(cardDraw.suit, cardDraw.value, angleMultiplier);
            roomManager.cards[i].GetComponent<SpriteRenderer>().sprite = cardSprites[((cardDraw.suit * 13) + cardDraw.value) - 1];
            print("Card Added");
        }
        roomManager.EqualizeDistance();
        print("Room Drawn");
    }

}
