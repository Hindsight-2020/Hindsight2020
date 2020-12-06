using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;

    void FixedUpdate()
    {
        this.transform.position =
            new Vector3(followTransform.position.x + 0.5f, followTransform.position.y - 0.5f, this.transform.position.z);
    }
}
