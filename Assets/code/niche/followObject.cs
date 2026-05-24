using UnityEngine;

public class followObject : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + (Vector3)offset;
    }
}
