using System.IO;
using UnityEditor;
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
    public Animator animator;



    private float walkingTime;

    private AudioSource audioSource;

    private bool playedStepSound;

    void Start()
    {
    }

    void Update()
    {
        int audioIndex = UnityEngine.Random.Range(0, 2);
        playedStepSound = false;
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }

        if(Input.GetKeyDown(KeyCode.P)) {
            audioSource.PlayOneShot(meowDeath);
        }
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        //print(state.normalizedTime + "/" + state.length);

        if(false) {
            float progress = state.normalizedTime % state.length;

            if(progress > 0.5f) {
                if(!playedStepSound) {
                    audioSource.PlayOneShot(step1);
                    playedStepSound = true;
                }
            } else if(progress < 0.1f) {
                playedStepSound = false;

            }
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
