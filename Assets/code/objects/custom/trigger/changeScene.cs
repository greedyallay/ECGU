using UnityEngine;
using UnityEngine.SceneManagement;
using static playerController;

public class changeScene : MonoBehaviour
{
    private bool wasTriggered = false;

    public string scene;

    private float timer = 0f;
    private bool changingScene = false;

    public screenFade fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(changingScene) {
            if (timer == 0f) {
                fade.state = true;
                fade.speed = 3f;
            }
            timer += Time.deltaTime;
            if (timer > 1f) {
                SceneManager.LoadScene(scene);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!wasTriggered) {
            changingScene = true;
            wasTriggered = true;
        }

    }
}
