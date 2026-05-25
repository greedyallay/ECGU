using UnityEngine;

public class doorHandler : MonoBehaviour
{
    public bool isTriggered = false;

    public float speed = 1f;
    public float maxHeight = 10f;

    public AudioClip doorSlide;
    public AudioClip doorStop;

    private AudioSource source;

    private float yPos;
    private float time;
    private float distance;
    private float pos;
    private bool hasTriggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(source == null) {
            source = transform.Find("audio").GetComponent<AudioSource>();
        }
        if(isTriggered) {
            if(!hasTriggered) {
                distance = maxHeight - yPos;
                source.PlayOneShot(doorSlide);
            }
            hasTriggered = true;
            time += Time.deltaTime;
            pos = (distance / (1 / speed)) * time;
            transform.position = new Vector2(transform.position.x, pos + yPos);
            if(pos > maxHeight) {
                isTriggered = false;
                source.PlayOneShot(doorStop);
            }
        }
        
    }
}
