using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;

public class dialog : MonoBehaviour
{
    private TextMeshProUGUI dialogText;
    private TextMeshProUGUI talkerName;
    public Sprite face;

    private AudioSource sound;

    private float dialogWriteProgress = 0;

    public float dialogSpeed = 10f;

    private string fullText = "";
    private string fullName = "";
    private bool textSide = false;

    private int oldLength;

    public bool finishedTyping = false;

    public AudioClip tick;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //dot forget to add the sliding in animation for the dialog thingy thank you

    // Update is called once per frame
    void Update()
    {
        if(dialogText == null) {
            dialogText = transform.Find("text").GetComponent<TextMeshProUGUI>();
            talkerName = transform.Find("name").GetComponent<TextMeshProUGUI>();
            sound = transform.Find("sound").GetComponent<AudioSource>();
        }

        if(!finishedTyping) {
            display(fullName, fullText.Substring(0, (int)dialogWriteProgress), textSide);
            dialogWriteProgress += Time.deltaTime * 10 * dialogSpeed;
        }


        if ((int)dialogWriteProgress > oldLength) {
            oldLength = (int)dialogWriteProgress;

            sound.pitch = Random.Range(0.8f, 1.2f);

            sound.PlayOneShot(tick);
        }

        if ((int)dialogWriteProgress > fullText.Length) {
            finishedTyping = true;
        }
;
        //dialogs should slide in from the bottom rather quickly

        //dialogs should change the players face in game and show a from facing version of the character

    }

    public void talk(string text, string name, string face, bool side) {
        finishedTyping = false;
        print("we are SO TALKING RIGHT NOW");
        fullName = name;
        fullText = text;
        textSide = side;
        dialogWriteProgress = 0f;
        oldLength = 0;
    }

    private void display(string name, string text, bool side) {
        talkerName.text = name;
        dialogText.text = text;
        if(side) {
            talkerName.alignment = TextAlignmentOptions.Right;
        }
        else {
            talkerName.alignment = TextAlignmentOptions.Left;

        }
    }
}
