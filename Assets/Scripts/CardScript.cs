using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardScript : MonoBehaviour
{
    private AudioSource source;
    public AudioClip hoverSound;
    public AudioClip pressedSound;

    public bool isHovering;
    public bool isDragging;
    public Vector2 currentVelocity;
    public float moveSpeed;
    public float angleMultiplier;
    public Vector2 defaultPosition;

    public int cardValue;
    public int effectiveValue;
    public int cardSuit;
    public string cardType;
    public bool isHeld;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        isHovering = false;
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Card Movement and Rotation based on mouse position
        if (isDragging)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 cardPos = Camera.main.WorldToScreenPoint(transform.position);
            float dist = Vector2.Distance(mousePos, cardPos);
            transform.eulerAngles = dist >= 1 ? new Vector3(0, 0, (-1 * (mousePos.x - cardPos.x) * (mousePos.y - cardPos.y) * moveSpeed * angleMultiplier) / dist) : Vector3.zero;
            float nDist = (dist / 1280);
            moveSpeed = Camera.main.scaledPixelHeight * easeOutCubic(Math.Clamp(nDist, 0.0f, 1.0f));
            Vector3 pos = Vector2.SmoothDamp(transform.position, Camera.main.ScreenToWorldPoint(mousePos),
                ref currentVelocity, Time.deltaTime, moveSpeed);
            transform.position = pos;
        } 
        else if (isHovering)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 cardPos = Camera.main.WorldToScreenPoint(transform.position);
            float dist = Vector2.Distance(mousePos, cardPos);
            transform.eulerAngles = new Vector3(0, 0, -angleMultiplier * (mousePos.x - cardPos.x) * (mousePos.y - cardPos.y) / dist);
        }

    }

    //Used when Cursor is detected to be hovering over the card
    public void IsHovering()
    {
        source.PlayOneShot(hoverSound, 1.0f);
        isHovering = true;
        transform.localScale *= 1.1f;
        //print("hi");
    }

    public void NotHovering()
    {
        isHovering = false;
        transform.localScale /= 1.1f;
        transform.eulerAngles = Vector3.zero;
    }

    //Used when player is detected to be dragging the card
    public void IsDragging() 
    {
        isDragging = true;
        source.PlayOneShot(pressedSound, 1.0f);
    }

    public void NotDragging()
    {
        isDragging = false;
        transform.localPosition = defaultPosition;
        source.PlayOneShot(pressedSound, 1.0f);
    }

    //Function for card movement calculations
    private float easeOutCubic(float number) {
        return (1.0f - (float) Math.Pow((1 - number), 3));
    }

    //Sets the values specified used for card instantiation
    public void GetSetUp(int suit, int value, string type, float angleMult)
    {
        cardSuit = suit;
        cardValue = value;
        cardType = type;
        angleMultiplier = angleMult;
        effectiveValue = value;
    }

    //Used to change the value of a card
    public void SetValues(int value)
    {
        cardValue = value;
        effectiveValue = value;
    }

}
