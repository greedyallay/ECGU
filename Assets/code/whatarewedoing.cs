using UnityEngine;

public class whatarewedoing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("idkwtfimdoing");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (false) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject obj = new GameObject("new");

            obj.AddComponent<Rigidbody2D>();
            obj.AddComponent<BoxCollider2D>();
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();

            sr.sprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0, 0, 2, 2),
                new Vector2(0, 0)
                );

            obj.transform.position = mousePos;
            }
        }
    }
}
