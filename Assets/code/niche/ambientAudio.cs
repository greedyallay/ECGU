using NUnit.Framework;
using UnityEngine;

public class ambientAudio : MonoBehaviour
{
    private AudioSource source;
    public AudioClip sound1;

    public float maxTime;

    private float time;
    private float nextTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (source == null) {
            source = GetComponent<AudioSource>();
            nextTime = Random.Range(0f, maxTime);
        }
        time += Time.deltaTime;

        
        if(time > nextTime) {
            time = 0f;
            source.PlayOneShot(sound1);
            nextTime = Random.Range(0f, maxTime);
        }
    }
}
