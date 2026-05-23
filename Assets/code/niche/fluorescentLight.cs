using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class fluorescentLight : MonoBehaviour
{

    public bool enabled = false;
    public float autoStartTime = 0f;

    public AudioClip onSound;
    private bool wasStarted = false;
    private float time = 0f;

    private bool wasDisabled = false;

    private Light2D light;
    private AudioSource audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(light == null) {
            light = transform.Find("bulb").transform.Find("light").GetComponent<Light2D>();
            audio = transform.Find("sound").GetComponent<AudioSource>();
        }
        if (!enabled && !wasDisabled) {
            light.enabled = false;
            audio.enabled = false;
            wasDisabled = true;
        }
        time += Time.deltaTime;
        if(!wasStarted) {
            if(autoStartTime == 0f) {
                wasStarted = true;
            } else {
                if(time >= autoStartTime) {
                    enabled = true;
                }
            }
        }
        if(enabled && !wasStarted) {
            print("i am ONNN");
            audio.enabled = true;
            light.enabled = true;
            wasStarted = true;
            audio.PlayOneShot(onSound);
        }
    }
}
