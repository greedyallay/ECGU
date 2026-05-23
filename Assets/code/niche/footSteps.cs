using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class footSteps : MonoBehaviour
{

    public playerController player;


    public AudioClip[] clip;

    public AudioClip step;

    public AudioClip meowDeath;
    public AudioClip land;
    public Animator animator;

    public float maxAudioPitchChange = 0f;



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
            audioSource.volume = 1f;
            audioSource.PlayOneShot(meowDeath);
        }
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        //print(state.normalizedTime + "/" + state.length);

        if(false) {
            float progress = state.normalizedTime % state.length;

            if(progress > 0.5f) {
                if(!playedStepSound) {
                    audioSource.pitch = Random.Range(-maxAudioPitchChange, maxAudioPitchChange);
                    audioSource.PlayOneShot(step);
                    playedStepSound = true;
                }
            } else if(progress < 0.1f) {
                playedStepSound = false;

            }
        }


        if (player.player.walking && player.player.onFloor) {
            walkingTime += Time.deltaTime;
            if(walkingTime > 0.35f) {
                audioSource.volume = 0.15f;
                audioSource.PlayOneShot(step);
                walkingTime = 0f;
            }
        } else {
            walkingTime = 0f;
        }
    }

    public void landSound() {
        audioSource.PlayOneShot(land);
    }
}
