using UnityEngine;
using UnityEngine.UI;

public class screenFade : MonoBehaviour
{
    private Image black;

    private float time;

    public bool state = false;

    private bool previousState = false;
    private bool isAnimating = false;

    public float speed = 1f;
    private float opacity = 0f;

    public bool unfadeAtStart = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (state) {
            opacity = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (black == null) {
            black = GetComponent<Image>();
            if (state) {
                opacity = 1f;
            }
            black.color = new Color(0f, 0f, 0f, opacity);
            if (unfadeAtStart) {
                state = false;
            }
        }
        time += Time.deltaTime;
        if (state != previousState) {
            time = 0f;
            isAnimating = true;

            previousState = state;

        }
        if (isAnimating) {
            if(state) {
                opacity = Mathf.Clamp(time / (1 / speed), 0, 1f);

            }
            else {
                opacity = Mathf.Clamp(1f - (time / (1 / speed)), 0, 1f);
            }
        }

        black.color = new Color(0f, 0f, 0f, opacity);
    }
}
