using TMPro;
using UnityEditor;
using UnityEngine;

public class labsMain : MonoBehaviour
{
    public GameObject base1;
    public Transform player;

    private float lastPos = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(lastPos == 0f) {
            lastPos = player.transform.position.x;
        }
        //print(player.transform.position.x);
        print("last pos = " + lastPos);
        if (player.transform.position.x > lastPos + 20) {
            print("WHAT THE FUCK?");
            addStructure(player.transform.position);
            lastPos = player.transform.position.x;
        }

    }

    

    void addStructure(Vector2 pos) {
        Instantiate(base1);
        base1.transform.position = pos;
    }
}
