using UnityEditor;
using UnityEngine;

public class camera : MonoBehaviour
{


    public float zoom = 1f;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public Vector2 MousePos;

    public bool doFollow = true;

    private Rigidbody2D playerBody;

    private bool pixelate = true;

    public Vector2 gridSize;

    public RenderTexture renderTexture;

    public Transform toFollow;

    private Camera cam;

    private bool updateZoom = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {
        if (cam == null) {
            cam = GetComponent<Camera>();
            Rigidbody2D playerBody = toFollow.GetComponent<Rigidbody2D>();
        }
        Vector3 mScreen = Input.mousePosition;
        mScreen.z = -cam.transform.position.z;
        MousePos = cam.ScreenToWorldPoint(mScreen);
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0) { zoom /= 1.1f; updateZoom = true; }
        if (scroll < 0) { zoom *= 1.1f; updateZoom = true; }



        if(updateZoom) {
            gridSize.x = 512 * zoom;
            gridSize.y = 288 * zoom;

            renderTexture.Release();
            renderTexture.width = (int)gridSize.x;
            renderTexture.height = (int)gridSize.x;
            renderTexture.Create();

            cam.orthographicSize = zoom * 5;
        }



        if (Input.GetKeyDown(KeyCode.L)) {
            pixelate = !pixelate;
        }


        Vector3 targetPos = toFollow.position + offset + (Vector3)(MousePos - (Vector2)toFollow.position) / 10f;

        Vector3 finalTarget = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        //finalTarget.x = Mathf.Round(finalTarget.x / gridSize.x) * gridSize.x;
        // finalTarget.y = Mathf.Round(finalTarget.y / gridSize.y) * gridSize.y;

        if(doFollow) {
            transform.position = finalTarget;
        }

        updateZoom = false;
    }
}
