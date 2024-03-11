using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    public Camera cam;
    public Transform subject;
    public float factor;

    Vector2 startPos;

    Vector2 travel => (Vector2)cam.transform.position - startPos;

    Vector2 parallaxFactor;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + travel * factor;
    }
}
