using System.Threading;
using UnityEngine;

public class handleFace : MonoBehaviour {

    private float blinkTime = 0f;

    public Sprite angry;
    public Sprite neutral;
    public Sprite blink;

    public playerController player;

    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        blinkTime += Time.deltaTime;


        if (blinkTime > 5f) {
            sprite.sprite = blink;
            blinkTime = 0f;
        } else if(blinkTime > 0.2f) {
            //if(player.player.sneaking) {
            //s//prite.sprite = neutral;
            //
            //}
            sprite.sprite = angry;
        }
    }
}
