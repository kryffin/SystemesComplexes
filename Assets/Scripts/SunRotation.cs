using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public float rotationSpeed;

    // Rotates the DirectionalLight around its parent's pivot point
    void Update()
    {
        transform.Rotate(new Vector3(rotationSpeed * Time.deltaTime, 0f, 0f));
    }
}
