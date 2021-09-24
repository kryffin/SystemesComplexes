using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    public float speed;

    private float bounceBound = 0.1f;
    private float yOffset = 1.5f;

    public void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, bounceBound) + yOffset;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
