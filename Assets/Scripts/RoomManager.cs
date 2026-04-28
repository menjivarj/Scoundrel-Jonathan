using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

    public int roomSize;
    public List<GameObject> cards;
    private RectTransform roomRect;
    public GameObject background;
    public Sprite[] backgrounds;
    public TMP_Text roomSizeText;

    void Awake()
    {
        cards = new List<GameObject>();
        roomRect = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Takes all cards and spreads them out evenly
    public void EqualizeDistance()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<CardScript>().defaultPosition = new Vector2(cards.Count > 1 ? ((roomRect.rect.width * i) / (cards.Count - 1)) - (roomRect.rect.width / 2) : 0.0f, 0.0f);
            cards[i].transform.localPosition = cards[i].GetComponent<CardScript>().defaultPosition;
        }
    }

    //Used to change the card rotation intensity of all cards in the room
    public void AngleMultiplierChange(float value)
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<CardScript>().angleMultiplier = value;
        }
    }


    public void ChangeBackground(int num)
    {
        background.GetComponent<Image>().sprite = backgrounds[num];
    }

    public void RandomBackground()
    {
        background.GetComponent<Image>().sprite = backgrounds[Random.Range(1, backgrounds.Length)];
    }

    public void Clear()
    {
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }

    //Updates the amount of cards able to be held in the room
    public void UpdateSize(float size)
    {
        roomSize = ((int)size);
        roomSizeText.SetText(roomSize.ToString());
    }

}
