using TMPro;
using UnityEngine;

public class updateFPS : MonoBehaviour
{
    private TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text == null) {
            text = GetComponent<TMP_Text>();
        }
        float fps = 1f / Time.unscaledDeltaTime;
        text.text = Mathf.Round(fps) + " fps";
    }

    void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;
    }
}
