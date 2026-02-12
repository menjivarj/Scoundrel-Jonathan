using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private AudioSource source;
    public AudioClip hoverSound;
    public AudioClip pressedSound;
    bool isHovering;
    bool isSelected;

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

    }

    //Detect when Cursor is hovering over the card
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Plays sound while hovering over card
        source.PlayOneShot(hoverSound, 10.0f);
        isHovering = true;
        print("hi");
    }

    //Detect when Cursor stops hovering over the card
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (isHovering)
        { if (!isSelected)
            {
                source.PlayOneShot(pressedSound, 10.0f);
                isSelected = true;
            }
            else if (isSelected)
            {
                isSelected = false;
            }
        }

    }

}
