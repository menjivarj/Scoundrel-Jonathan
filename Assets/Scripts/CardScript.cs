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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();
        isHovering = false;
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering) {
            transform.eulerAngles = new Vector3(0, 0, (Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - transform.position.x) * 4);
        } else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

    //Detect when Cursor is hovering over the card
    public void IsHovering()
    {
        //Plays sound while hovering over card
        source.PlayOneShot(hoverSound, 10.0f);
        isHovering = true;
        transform.localScale = new Vector3(4.1f, 4.1f, 4.1f);
        print("hi");
    }

    public void NotHovering()
    {
        isHovering = false;
        transform.localScale = new Vector3(4f, 4f, 4f);

    }

    //Detect when Cursor stops hovering over the card
    public void IsSelected()
    {
        if (!isSelected)
        {
            isSelected = true;
            transform.localPosition = Vector3.up;
        } else
        {
            isSelected = false;
            transform.localPosition = Vector3.zero;
        }
    }


}
