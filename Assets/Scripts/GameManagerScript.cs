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
        public string suit;
        public int value;

        public CardData(string suit, int value)
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
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };
            foreach (string s in suits)
            {
                for (int i = 1; i < 14; i++)
                {
                    deckData.Add(new CardData(s, i));
                }
            }
        }

    }

    public List<DeckData> cardDecks;
    public DeckData currentDeck;
    public GameObject room;


    void Start()
    {
        clickTime = Time.time;
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            print(draggingcard);
            mousePressed = false;
            draggingcard.GetComponent<CardScript>().NotDragging();
            isDragging = false;
            draggingcard = null;
            print("released");
        }
        else
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //print(mousepos);
            hits = Physics2D.RaycastAll(mousepos, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("CardLayer"));
            Debug.DrawRay(Mouse.current.position.ReadValue(), Vector2.down, Color.yellow);
            if (hits.Length != 0)
            {
                print(hits[0].collider.gameObject);
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

    

}
