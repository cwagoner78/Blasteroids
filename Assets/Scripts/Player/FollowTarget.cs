using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{

    public Transform target;
    public Vector3 targetOffset;
    public float moveSpeed = 5f;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, target.position + targetOffset, moveSpeed * Time.deltaTime);

    }
}
