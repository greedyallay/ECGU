using UnityEngine;

public class doorHandler : MonoBehaviour {
    public bool isTriggered = false;

    public float speed = 1f;
    public float distance = 10f;

    public AudioClip doorSlide;
    public AudioClip doorStop;

    private AudioSource source;

    private float yPos;
    private float time;
    private float pos;
    private bool hasTriggered = false;
    private bool previousState = false;

    private bool internallyTriggered = false;

    void Start() {
        yPos = transform.position.y;
    }

    //it turns out that
    // MAKING DOORS IS A FUCKING NIGHTMARE NEVER DO IT PLEASE SAVE ME FROM THIS NIGHTMARE
    void Update() {
        if (source == null) {
            source = transform.Find("audio").GetComponent<AudioSource>();
        }
        if(isTriggered != previousState) {
            print("changing states");
            internallyTriggered = true;
            previousState = isTriggered;
        }
        print("ïstriggered " + isTriggered);
        print("internallytriggered " + internallyTriggered);
        if (!internallyTriggered) { return; }

        if (isTriggered) {
            if (!hasTriggered) {
                hasTriggered = true;
                yPos = transform.position.y;
                source.PlayOneShot(doorSlide);
                time = 0f;
            }

            time += Time.deltaTime;
            pos = Mathf.Min(distance, speed * time);
            transform.position = new Vector2(transform.position.x, pos + yPos);

            if (pos >= distance) {
                pos = distance;
                internallyTriggered = false;
                hasTriggered = false;
                source.PlayOneShot(doorStop);
            }
        }
        else {
            print("why");

            if (!hasTriggered) {
                hasTriggered = true;
                source.PlayOneShot(doorSlide);
                time = 0f;
            }

            time += Time.deltaTime;

            pos = Mathf.Max(-distance, -speed * time);
            transform.position = new Vector2(transform.position.x, pos + yPos + distance);

            if (pos <= -distance) {
                pos = -distance;
                hasTriggered = false;
                internallyTriggered = false;
                source.PlayOneShot(doorStop);
            }
        }
    }
}