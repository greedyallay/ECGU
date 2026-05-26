using UnityEngine;
using UnityEngine.Audio;

public class collideAudio : MonoBehaviour
{

    public AudioClip collisionSound;
    public AudioSource source;
    public float volume = 1f;

    private float time;

    private float velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 0.1f) {
            time = 0f;
        }

    }

    void OnCollisionEnter2D(Collision2D collision) {
        //make it so that only if this object has a higher speed than the other one it plays the audio to prevent doublt shit
        float impact = collision.relativeVelocity.magnitude;
        source.volume = (impact / 20) * volume;
        source.PlayOneShot(collisionSound);


        Collider2D collider = collision.collider;

    }
}
