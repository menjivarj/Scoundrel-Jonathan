using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    //public List<Card> deck = new List<Card>();
    public GameObject hoveringcard;
    public GameObject selectedcard;
    RaycastHit2D hit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(Mouse.current.position.ReadValue(), Vector2.down);
        if (hit.collider)
        {
            print("hi");
            hoveringcard = hit.transform.gameObject;
            hoveringcard.GetComponent<CardScript>().IsHovering();
        }

    }
}
