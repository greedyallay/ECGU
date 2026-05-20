using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class walkSound : MonoBehaviour
{
    private class AudioClips {
        public AudioClip step1;
        public AudioClip step2;
        public AudioClip step3;
    }

    public playerController player;


    public AudioClip[] clip;

    public AudioClip step1;
    public AudioClip step2;
    public AudioClip step3;



    private float walkingTime;

    private AudioSource audioSource;

    void Start()
    {
    }

    void Update()
    {
        int audioIndex = UnityEngine.Random.Range(0, 2);
        if(audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
        walkingTime += Time.deltaTime;

        if(walkingTime > 1f ) {
            audioSource.PlayOneShot(step1);
            walkingTime = 0f;
        }
    }
}
