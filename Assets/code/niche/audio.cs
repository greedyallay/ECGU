using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
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

    public AudioClip meowDeath;



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

        if(Input.GetKeyDown(KeyCode.P)) {

                        audioSource.PlayOneShot(meowDeath);
        }


        if (player.player.walking && player.player.onFloor) {
            walkingTime += Time.deltaTime;
            if(walkingTime > 0.35f) {
                switch (audioIndex) {
                    case 0: {
                        audioSource.PlayOneShot(step1);
                        break;
                    }
                    case 1: {
                        audioSource.PlayOneShot(step2);
                        break;
                    }
                    case 2: {
                        audioSource.PlayOneShot(step3);
                        break;
                    }
                }
            audioSource.PlayOneShot(step1);
            walkingTime = 0f;
            }
        } else {
            walkingTime = 0f;
        }
    }
}
