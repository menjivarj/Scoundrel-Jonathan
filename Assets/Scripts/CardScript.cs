using System;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class CardScript : MonoBehaviour
{
    private AudioSource source;
    public AudioClip hoverSound;
    public AudioClip pressedSound;
    public bool isHovering;
    public bool isDragging;
    public Vector2 currentVelocity;
    public float moveSpeed;

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
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 cardPos = Camera.main.WorldToScreenPoint(transform.position);
        float dist = Vector2.Distance(mousePos, cardPos);
        if (isHovering) {
            transform.eulerAngles = new Vector3(0, 0, -1 * (mousePos.x - cardPos.x) * (mousePos.y - cardPos.y) / Camera.main.scaledPixelHeight);
        } else if(!isHovering)
        {
            transform.eulerAngles = Vector3.zero;
        }
        if (isDragging)
        {
            transform.eulerAngles = dist >= 1 ? new Vector3(0, 0, (Math.Clamp(-1 * (mousePos.x - cardPos.x) * (mousePos.y - cardPos.y), -1.0f, 1.0f) * moveSpeed) / dist) : Vector3.zero;
            float nDist = (dist / 1280);
                moveSpeed = Camera.main.scaledPixelHeight * easeOutCubic(Math.Clamp(nDist, 0.0f, 1.0f));
            Vector3 pos = Vector2.SmoothDamp(transform.position, Camera.main.ScreenToWorldPoint(mousePos), 
                ref currentVelocity, Time.deltaTime, moveSpeed);
            transform.position = pos;
        }
    }

    //Detect when Cursor is hovering over the card
    public void IsHovering()
    {
        //Plays sound while hovering over card
        source.PlayOneShot(hoverSound, 1.0f);
        isHovering = true;
        transform.localScale *= 1.1f;
        //print("hi");
    }

    public void NotHovering()
    {
        isHovering = false;
        transform.localScale /= 1.1f;

    }

    //Detect when player is dragging the card
    public void IsDragging() 
    {
        isDragging = true;
        source.PlayOneShot(pressedSound, 1.0f);
    }

    public void NotDragging()
    {
        isDragging = false;
        transform.localPosition = Vector3.zero;
        source.PlayOneShot(pressedSound, 1.0f);
    }

    private float easeOutCubic(float number) {
        return (1.0f - (float) Math.Pow((1 - number), 3));
    }
}
