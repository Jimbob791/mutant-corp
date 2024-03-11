using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LockChest : MonoBehaviour
{
    void Update()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x * 2) / 2, Mathf.Round(transform.localPosition.y * 2) / 2, 0);
        }
    }
}
