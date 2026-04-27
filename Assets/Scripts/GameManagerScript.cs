using NUnit.Framework.Internal.Builders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
        public string type;

        public CardData(int suit, int value, string type)
        {
            this.suit = suit;
            this.value = value;
            name = suit + " " + value;
            this.type = type;
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
                    deckData.Add(new CardData(s, v, s < 2 ? "Enemy" : s == 2 ? "Weapon" : "Health"));
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
    public List<Sprite> specialCardSprites;

    public GameObject hand;
    public int health;
    public int startingHealth;
    public TMP_Text healthText;
    public TMP_Text deckText;
    public GameObject winlossPanel;
    public TMP_Text winlossText;
    public GameObject winParticle;
    public Sprite[] winParticles;

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
                    CardScript hoveringScript = hoveringcard.GetComponent<CardScript>();
                    CardScript draggingScript = draggingcard.GetComponent<CardScript>();
                    RoomManager handManager = hand.GetComponent<RoomManager>();
                    RoomManager roomManager = room.GetComponent<RoomManager>();
                    if (hoveringScript.isHeld)
                    {
                        
                        if (draggingScript.cardType == "Weapon")
                        {
                            if (!draggingScript.isHeld)
                            {
                                roomManager.cards.Remove(draggingcard);
                                handManager.cards.Add(draggingcard);
                                handManager.cards.RemoveSwapBack(hoveringcard);
                                draggingcard.transform.SetParent(hand.transform);
                                draggingScript.isHeld = true;
                                draggingScript.effectiveValue = 20;
                                Destroy(hoveringcard);
                            }
                            else
                            {
                                handManager.cards.Remove(draggingcard);
                                FillHand();
                                Destroy(draggingcard);
                            }
                        }
                        else if (draggingScript.cardType == "Health")
                        {
                            health += draggingScript.cardValue;
                            roomManager.cards.Remove(draggingcard);
                            Destroy(draggingcard);
                        }
                        else if (draggingScript.cardType == "Enemy")
                        {
                            if (draggingScript.cardValue < hoveringScript.effectiveValue)
                            {
                                roomManager.cards.Remove(draggingcard);
                                draggingcard.transform.SetParent(hoveringcard.transform);
                                draggingScript.defaultPosition = new Vector2(0.1f * hoveringcard.transform.childCount, -0.1f * hoveringcard.transform.childCount);
                                draggingcard.GetComponent<Collider2D>().enabled = false;
                                health -= Mathf.Max(0, draggingScript.cardValue - hoveringScript.cardValue);
                                hoveringScript.effectiveValue = draggingScript.cardValue;
                            } 
                            else
                            {
                                health -= draggingScript.cardValue;
                                roomManager.cards.Remove(draggingcard);
                                Destroy(draggingcard);
                            }
                            if (health <= 0)
                            {
                                WinLoss(false);
                            }
                        }
                        if (roomManager.cards.Count == 0)
                        {
                            DrawRoom();
                        }
                    }
                    healthText.text = health.ToString();
                    roomManager.EqualizeDistance();
                    handManager.EqualizeDistance();
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
        room.GetComponent<RoomManager>().AngleMultiplierChange(value);
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
        health = startingHealth;
        healthText.text = health.ToString();
        currentDeck.deckData.Clear();
        currentDeck.AddStandardDeck();
        print("Added Deck");
        room.GetComponent<RoomManager>().Clear();
        hand.GetComponent<RoomManager>().Clear();
        DrawRoom();
        FillHand();
    }

    public void DrawRoom()
    {
        if (currentDeck.deckData.Count == 0)
        {
            WinLoss(true);
        }
        RoomManager roomManager = room.GetComponent<RoomManager>();
        for (int i = roomManager.cards.Count; (i < roomManager.roomSize && currentDeck.deckData.Count > 0); i++)
        {
            int j = Random.Range(0, currentDeck.deckData.Count);
            CardData cardDraw = currentDeck.deckData[j];
            roomManager.cards.Add(Instantiate(card, room.transform));
            roomManager.cards[i].GetComponent<CardScript>().GetSetUp(cardDraw.suit, cardDraw.value, cardDraw.type, angleMultiplier);
            roomManager.cards[i].GetComponent<SpriteRenderer>().sprite = cardSprites[((cardDraw.suit * 13) + cardDraw.value) - 1];
            currentDeck.deckData.RemoveAt(j);
            print("Card Added");
        }
        roomManager.EqualizeDistance();
        roomManager.RandomBackground();
        deckText.text = currentDeck.deckData.Count.ToString();
        print("Room Drawn");
    }

    public void FillHand()
    {
        RoomManager handManager = hand.GetComponent<RoomManager>();
        for (int i = handManager.cards.Count; i < handManager.roomSize; i++)
        {
            handManager.cards.Add(Instantiate(card, hand.transform));
            handManager.cards[i].GetComponent<CardScript>().GetSetUp(-1, -1, "Blank", angleMultiplier);
            handManager.cards[i].GetComponent<CardScript>().isHeld = true;
            handManager.cards[i].GetComponent<SpriteRenderer>().sprite = specialCardSprites[0];
        }
        handManager.EqualizeDistance();
        print("Hand Filled");
    }

    public void WinLoss(bool winloss)
    {
        if (winloss)
        {
            winlossText.text = "YOU WIN";
            winParticle.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            winlossText.text = "YOU LOSE";
        }
        hand.SetActive(false);
        room.GetComponent<RoomManager>().ChangeBackground(0);
        winlossPanel.SetActive(true);
    }

}
