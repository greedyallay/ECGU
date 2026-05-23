using UnityEngine;

public class buttonHandler : MonoBehaviour
{
    public doorHandler trigger;

    private bool isTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (GetComponent<SpriteRenderer>().bounds.Contains(mousePos)) {
            if(!isTriggered) {
                trigger.isTriggered = true;
                isTriggered = true;
            }

        }

    }
}
