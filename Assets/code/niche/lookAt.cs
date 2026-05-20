using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class lookAt : MonoBehaviour
{
    public playerController player;
    public Vector3 target;
    public Transform body;
    public float faceAngle;
    Vector3 MousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (player.player.mirror) {
            MousePos.x = 0 - MousePos.x;
        }
        target = MousePos;
        Vector2 dir = target - body.position;
        faceAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 80;



        if (faceAngle > 120) {
            faceAngle = 120 + (faceAngle - 120) / 5;
        }
        if (faceAngle < 60) {
            faceAngle = 60 + (faceAngle - 60) / 5;
        }

        if (faceAngle > 150) { faceAngle = 150; }

        if (faceAngle < 32) { faceAngle = 32; }

        if (player.player.mirror) {
            faceAngle = 0 - faceAngle;
        }

        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, faceAngle);

    }
}
