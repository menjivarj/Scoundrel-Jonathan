using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    //public List<Card> deck = new List<Card>();
    public GameObject hoveringcard;
    public GameObject draggingcard;
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
        if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            print(draggingcard);
            mousePressed = false;
            draggingcard.GetComponent<CardScript>().NotDragging();
            isDragging = false;
            print("released");
        }
        else
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //print(mousepos);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            Debug.DrawRay(Mouse.current.position.ReadValue(), Vector2.down, Color.yellow);
            if (hit.collider)
            {
                //print("hi");
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
                else if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    mousePressed = true;
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
                if (hoveringcard != null)
                {
                    hoveringcard.GetComponent<CardScript>().NotHovering();
                    hoveringcard = null;
                }
            }

        }

    }

    public void DragTimeChange(float value)
    {
        holdToDragTime = value;
    }
}
