using UnityEngine;

public class draggable : MonoBehaviour {
    private Vector3 mousePos;
    private bool mouseDown = false;
    private bool mouseHover = false;
    private bool dragging = false;
    private Rigidbody2D rigidBody;
    private TargetJoint2D targetJoint;
    private BoxCollider2D colliderThingie;
    private PolygonCollider2D backupThingie;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (rigidBody == null) {
            rigidBody = GetComponent<Rigidbody2D>();
            targetJoint = GetComponent<TargetJoint2D>();
            colliderThingie = GetComponent<BoxCollider2D>();
            if (colliderThingie == null) {
                backupThingie = GetComponent<PolygonCollider2D>();
            }
        }

        if (colliderThingie == null) {
            mouseHover = backupThingie.OverlapPoint(mousePos);
        } else {
            mouseHover = colliderThingie.OverlapPoint(mousePos);

        }

        if (mouseHover && mouseDown) {
            dragging = true;
        }

        if (!mouseDown) {
            dragging = false;
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDown = Input.GetMouseButton(0);

        targetJoint.enabled = dragging;

        if (dragging) {
            targetJoint.target = (Vector2)mousePos;
        }
    }
}
