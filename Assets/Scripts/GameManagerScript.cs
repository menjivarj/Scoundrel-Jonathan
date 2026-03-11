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
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        print(mousepos);
        hit = Physics2D.Raycast(mousepos, Vector2.down);
        Debug.DrawRay(Mouse.current.position.ReadValue(), Vector2.down, Color.yellow);
        if (hit.collider)
        {
            print("hi");
            if (hoveringcard == null)
            {
                hoveringcard = hit.transform.gameObject;
                hoveringcard.GetComponent<CardScript>().IsHovering();
            }
            else if (hoveringcard != hit.transform.gameObject)
            {
                hoveringcard.GetComponent<CardScript>().NotHovering();
                hoveringcard = hit.transform.gameObject;
                hoveringcard.GetComponent<CardScript>().IsHovering();
            }
                
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                hoveringcard.GetComponent<CardScript>().IsSelected();
            }
        }
        else
        {
            if (hoveringcard != null)
            {
                hoveringcard.GetComponent<CardScript>().NotHovering();
                hoveringcard = null;
            }
        }

    }
}
