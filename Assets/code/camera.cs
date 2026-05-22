using UnityEngine;

public class camera : MonoBehaviour
{


    float zoom = 1f;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public Vector2 MousePos;

    public RenderTexture renderTexture;

    public Transform toFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0) { zoom /= 1.1f; }
        if(scroll < 0) { zoom *= 1.1f; }

        renderTexture.Release();
        renderTexture.width = (int)(512 * zoom);
        renderTexture.height = (int)(288 * zoom);
        renderTexture.Create();

        Camera.main.orthographicSize = zoom * 5;

        Rigidbody2D playerBody = toFollow.GetComponent<Rigidbody2D>();

        Vector3 targetPos = toFollow.position + offset + (Vector3)(MousePos - (Vector2)toFollow.position) / 10f;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
