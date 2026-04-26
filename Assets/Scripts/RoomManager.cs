using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public int roomSize;
    public List<GameObject> cards = new List<GameObject>();
    private RectTransform roomRect;

    void Awake()
    {
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

    public void EqualizeDistance()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].GetComponent<CardScript>().defaultPosition = new Vector2(((roomRect.rect.width * i) / (cards.Count - 1)) - (roomRect.rect.width / 2), 0.0f);
            cards[i].transform.localPosition = cards[i].GetComponent<CardScript>().defaultPosition;
        }
    }

}
