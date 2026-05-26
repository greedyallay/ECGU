using UnityEngine;

public class ak47 : MonoBehaviour
{
    public AudioClip shot;
    private AudioSource sound;

    private float time;

    public float firingDelay;

    public bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sound == null) {
            sound = transform.Find("audio").GetComponent<AudioSource>();
        }
        if(isActive) {
            time += Time.deltaTime;
            if(time > firingDelay) {
                time = 0f;
                sound.PlayOneShot(shot);
            }
        }
    }
}
