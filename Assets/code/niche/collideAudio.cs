using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Audio;

public class collideAudio : MonoBehaviour
{

    public AudioClip collisionSound;
    public new Collider2D collider;
    public AudioSource source;

    private float time;

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
        float impact = collision.relativeVelocity.magnitude;
        print(impact/10);
        source.volume = impact / 10;
        source.PlayOneShot(collisionSound);


        Collider2D collider = collision.collider;

    }
}
