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
    private float clickTime;
    public float holdToDragTime;
    private bool isDragging;
    private bool mousePressed;

    void Start()
    {
        clickTime = 0;
        isDragging = false;
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
                mousePressed = true;
                hoveringcard.GetComponent<CardScript>().IsSelected();
                clickTime = Time.time;
            } else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                mousePressed = false;
                hoveringcard.GetComponent <CardScript>().NotDragging();
                isDragging = false;
            } else if (mousePressed && !isDragging && Time.time - clickTime > holdToDragTime)
            {
                hoveringcard.GetComponent<CardScript>().IsDragging();
                isDragging = true;
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
