using UnityEngine;

public class buttonHandler : MonoBehaviour
{
    public doorHandler trigger;
    public AudioClip triggerSound;
    public AudioClip secondaryTriggerSound;
    private AudioSource source;

    private bool isTriggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(source == null) {
            source = transform.Find("audio").GetComponent<AudioSource>();
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (GetComponent<SpriteRenderer>().bounds.Contains(mousePos)) {
            if (Input.GetMouseButtonDown(0)) { 
                if(!isTriggered) {
                    trigger.isTriggered = true;
                    isTriggered = true;
                    source.PlayOneShot(secondaryTriggerSound);
                    source.PlayOneShot(triggerSound);
                }            
            }


        }

    }
}
