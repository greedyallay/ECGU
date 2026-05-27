using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class enterLab : MonoBehaviour
{
    public doorHandler door;
    public stasisChamberHandler chamber;
    public playerController player;
    public camera camera;
    public dialog dialog;

    public Sprite siaHead;
    public Sprite siaNeutral;
    public Sprite siaScared;
    public Sprite siaCry;
    public Sprite siaSmirk;

    public AudioClip lightOn;

    public Transform flashlight;

    private Animator anim;

    public AudioSource music;

    public AudioClip glassCrack1;
    public AudioClip glassCrack2;
    public AudioClip glassCrack3;

    public AudioClip glassBreak;

    private int dialogProgress = 0;
    private bool dialogStarted  = false;

    private float time = 0f;

    private AudioSource sound;

    public Transform evilCat;
    public Transform siamie;

    //bullshit vairables
    private bool equipFlashlight;
    private bool animateScared;
    private float lastTime = 0f;

    private float netTime = 0f;

    private bool wasTriggered = false;

    public bool canSkip = true;
    public bool waitForFinish = false;

    private bool fadeOutMusic = false;

    private bool dropStasisChamber = false;

    public bool fuckLore = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialog.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(anim == null) {
            anim = player.transform.Find("rig").GetComponent<Animator>();
            sound = transform.Find("audio").GetComponent<AudioSource>() ;
        }
        if(dialogStarted) {
            handleDialog();
        }


        //bullshit code
        if (equipFlashlight) {
            if (lastTime == 0f) {
                lastTime = time;
                anim.CrossFade("equip", .5f);
            }
            netTime = time - lastTime;
            if (netTime > 0.5f) {
                flashlight.gameObject.SetActive(true);
                flashlight.transform.Find("Spot Light 2D").GetComponent<Light2D>().enabled = false;
            }
            if (netTime > 1f) {
                flashlight.transform.Find("Spot Light 2D").GetComponent<Light2D>().enabled = true;
            }
            if (netTime > 1.3f) {
                anim.CrossFade("idle", .5f);
                lastTime = 0f;
                equipFlashlight = false;
                
                //dialogProgress++;
               //showDialog(dialogProgress);
            }
        }
        if (animateScared) {
            if (lastTime == 0f) {
                print("som,ething very weird just happened");
                lastTime = time;
                anim.speed = 1f;
                anim.CrossFade("scared", .5f);
                flashlight.gameObject.GetComponent<Rigidbody2D>().simulated = true;
                flashlight.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                flashlight.SetParent(null);
            }
            netTime = time - lastTime;
            if (netTime > 2f) {
                anim.speed = 1.3f;
                anim.CrossFade("idle", .5f);
                animateScared = false;
                lastTime = 0f;
            }
        }

        if(waitForFinish) {
            return;
            if(dialog.finishedTyping) {
                dialogProgress++;
                showDialog(dialogProgress);
                waitForFinish = false;
            }
        }

        if (dropStasisChamber) {
            return;
            print("fast"); 
            print(lastTime); 
            if (lastTime == 0f) {
                lastTime = time;
                Rigidbody2D body = chamber.gameObject.GetComponent<Rigidbody2D>();
                body.simulated = true;
                chamber.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                body.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            netTime = time - lastTime;
            if (netTime > 0.5f) {
                sound.PlayOneShot(glassBreak);
                lastTime = 0f;
            }
            if (netTime > 1.3f) {
                anim.CrossFade("idle", .5f);
                lastTime = 0f;
                dropStasisChamber = false;

                //dialogProgress++;
                //showDialog(dialogProgress);
            }
        }


    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!wasTriggered) {
            door.isTriggered = false;
            player.allowMove = false;
            camera.zoom = 0.6f;
            dialogStarted = true;
            wasTriggered = true;
        }

    }

    void handleDialog() {
        if(fuckLore) {
            showDialog(22);
        }
        else {
            if (Input.GetMouseButtonDown(0) && canSkip) {
                lastTime = 0f;
                showDialog(dialogProgress);
                dialogProgress++;
            }
        }

    }

    void showDialog(int progress) {
        dialog.gameObject.SetActive(true);
        switch (progress) {
            case 0: {
                    dialog.talk("hello", "siamie", "siamie-neutral", false);
                    music.Play();
                    break;
                }
            case 1: {
                    dialog.talk("thanks for showing up so quick", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 2: {
                    dialog.talk("what happened to the lights?", "siamie", "siamie-neutral", false);
                    break;
                }
            case 3: {
                    dialog.talk("i like working in the dark.", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 4: {
                    dialog.talk("...", "siamie", "siamie-neutral", false);
                    //canSkip = false;
                    equipFlashlight = true;
                    break;
                }
            case 5: {
                    dialog.talk("you know how I've been experimenting with different serums lately?", "gray tabby", "siamie-neutral", true);
                    //anim.CrossFade("idle", .5f);

                    break;
                }
            case 6: {
                    dialog.talk("you could call it an obsession, haven't heard you talk about anything else for the past 8 months.", "siamie", "siamie-neutral", false);

                    break;
                }
            case 7: {
                    dialog.talk("well, this might be my breakthrough!", "gray tabby", "siamie-neutral", true);

                    break;
                }
            case 8: {
                    dialog.talk("lo and behold...", "gray tabby", "siamie-neutral", true);
                    animateScared = true;
                    chamber.isEnabled = true;
                    break;
                }
            case 9: {
                    dialog.talk("W- WHATS THAT", "siamie", "siamie-neutral", false);

                    break;
                }
            case 10: {
                    dialog.talk("it's...", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 11: {
                    dialog.talk("evil.", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 12: {
                    dialog.talk("what does that mean", "siamie", "siamie-neutral", false);
                    break;
                }
            case 13: {
                    dialog.talk("it means that this is a highly unstable and experimental test subject", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 14: {
                    dialog.talk("is it... dangerous?", "siamie", "siamie-neutral", false);
                    break;
                }
            case 15: {
                    dialog.talk("hahaha...", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 16: {
                    dialog.talk("i actually haven't conducted any experiments on it yet, so i don't know.", "gray tabby", "siamie-neutral", true);
                    break;
                }
            case 17: {
                    dialog.talk("who doesn't like surprises, am i right?", "gray tabby", "siamie-neutral", true);
                    sound.PlayOneShot(glassCrack1);
                    break;
                }
                //make the music fade out
                //then the glass starts to crack...
            case 18: {
                    dialog.talk("what's that?", "siamie", "siamie-neutral", false);
                    break;
                }
            case 19: {
                    dialog.talk("it appears as if something's breaking", "gray tabby", "siamie-neutral", true);
                    sound.PlayOneShot(glassCrack2);
                    break;
                }
            case 20: {
                    dialog.talk("...", "siamie", "siamie-neutral", false);
                    break;
                }
            case 21: {
                    dialog.talk("OH WAIT", "siamie", "siamie-neutral", false);
                    break;
                }
            case 22: {
                    dialog.talk("IT'S THE STASIS CHAMBE~", "siamie", "siamie-neutral", false);
                    sound.PlayOneShot(glassCrack3);
                    lastTime = 0f;
                    dropStasisChamber = true;

                    Rigidbody2D body = chamber.gameObject.GetComponent<Rigidbody2D>();
                    body.simulated = true;
                    chamber.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    body.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    break;
                }
            case 23: {
                    dialog.talk("------", "siamie", "siamie-neutral", false);
                    evilCat.gameObject.SetActive(true);
                    camera.toFollow = evilCat;
                    siamie.GetComponent<playerController>().enabled = false;
                    siamie.transform.Find("rig").transform.Find("body-rig").transform.Find("head").GetComponent<lookAt>().enabled = false;
                    Destroy(chamber.transform.Find("cat").gameObject);

                    siamie.transform.Find("rig")
                        .transform.Find("body-rig")
                        .transform.Find("rightArmA")
                        .transform.Find("rightArmB")
                        .transform.Find("rightHand")
                        .transform.Find("ak47").gameObject.SetActive(true);

                    Animator animatorA = siamie.transform.Find("rig").GetComponent<Animator>();
                    animatorA.CrossFade("riflefire", 0.5f);
                    break;
                }
            default: {
                    dialog.gameObject.SetActive(false);
                    player.allowMove = true;
                    dialogStarted = false;
                    fadeOutMusic = true;
                    break;
                }
        }
    }
}
