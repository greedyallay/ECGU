using UnityEngine;
using UnityEngine.U2D;

public class followBone : MonoBehaviour
{

    public Transform bone;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(bone, false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
