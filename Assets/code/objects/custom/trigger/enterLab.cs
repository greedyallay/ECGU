using UnityEngine;

public class enterLab : MonoBehaviour
{
    public doorHandler door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) {
            door.isTriggered = false;
        }
    }
}
