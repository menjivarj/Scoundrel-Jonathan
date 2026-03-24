using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardScript : MonoBehaviour
{
    private AudioSource source;
    public AudioClip hoverSound;
    public AudioClip pressedSound;
    public bool isHovering;
    public bool isSelected;
    public bool isDragging;
    private Vector2 currentVelocity;
    public float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        isHovering = false;
        isSelected = false;
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering) {
            transform.eulerAngles = new Vector3(0, 0, (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - transform.position.x) * 4);
        } else if(!isHovering)
        {
            transform.eulerAngles = Vector3.zero;
        }
        if (isDragging)
        {
            transform.position = Vector2.SmoothDamp(transform.position, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), ref currentVelocity, Time.deltaTime, moveSpeed);
            transform.eulerAngles = new Vector3(0, 0, -1 * currentVelocity.x * currentVelocity.y);
        }
    }

    //Detect when Cursor is hovering over the card
    public void IsHovering()
    {
        //Plays sound while hovering over card
        source.PlayOneShot(hoverSound, 1.0f);
        isHovering = true;
        transform.localScale *= 1.1f;
        print("hi");
    }

    public void NotHovering()
    {
        isHovering = false;
        transform.localScale /= 1.1f;

    }

    //Detect when Cursor stops hovering over the card
    public void IsSelected()
    {
        if (!isSelected)
        {
            isSelected = true;
            transform.localPosition = Vector3.up;
            source.PlayOneShot(pressedSound, 1.0f);
        } else
        {
            isSelected = false;
            transform.localPosition = Vector3.zero;
            source.PlayOneShot(pressedSound, 1.0f);
        }
    }

    public void IsDragging() 
    {
        isDragging = true;
    }

    public void NotDragging()
    {
        isDragging = false;
        transform.localPosition = Vector3.zero;
    }
}
