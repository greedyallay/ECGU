using UnityEngine;

public class followParent : MonoBehaviour
{

    public Transform bone;

    public Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start() {
        //offset = transform.position;
    }
    void LateUpdate() {
        transform.position = bone.position + (Vector3)offset;
        transform.rotation = bone.rotation * Quaternion.Euler(0f , 0f, -90f);
    }
}
  