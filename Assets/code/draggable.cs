using UnityEngine;

public class draggable : MonoBehaviour
{
    private bool isDragging = false;
    private TargetJoint2D target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging) {
            target.target = new Vector2(0, 0);
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else
        {
            Destroy(target);
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        target = gameObject.AddComponent<TargetJoint2D>();
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
