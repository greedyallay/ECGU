using UnityEngine;

public class tutorialMain : MonoBehaviour
{
    public Transform crate;
    public dialog dialog;

    public screenFade fade;
    private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Instantiate(crate).transform.position = mousePos;
        }

    }

    void Awake() {
        if (false) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 0;
        }

    }
}
