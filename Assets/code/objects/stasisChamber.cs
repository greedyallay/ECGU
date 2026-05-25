using UnityEngine;
using UnityEngine.Rendering.Universal;

public class stasisChamberHandler : MonoBehaviour
{
    public bool isEnabled = true;

    private Light2D light1;
    private Light2D light2;

    public AudioClip onSound;
    private AudioSource sound;

    private bool previousState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(light1 == null) {
            light1  = transform.Find("light1").GetComponent<Light2D>();
            light2  = transform.Find("light2").GetComponent<Light2D>();
            sound  = transform.Find("sound").GetComponent<AudioSource>();
            previousState = false;
        }

        if(isEnabled != previousState) {
            light1.enabled = isEnabled;
            light2.enabled = isEnabled;
            sound.PlayOneShot(onSound);
            previousState = isEnabled;
        }

    }
}
