using UnityEngine;

public class main : MonoBehaviour
{
    public Transform crate;
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Instantiate(crate).transform.position = mousePos;
        }
    }
}
