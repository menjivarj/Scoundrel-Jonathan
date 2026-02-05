using UnityEngine;

public class CardSelection : MonoBehaviour
{
    bool isSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {
            Collider2D cardCol = GetComponent<Collider2D>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D mouseCol = Physics2D.OverlapPoint(worldPos);
            if (!isSelected) {
                if (cardCol = mouseCol) {
                    isSelected = true;
                }
            } else {
                if (!(cardCol = mouseCol)) {
                    isSelected = false;
                } else {

                }
            }
        }
    }
}
