using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject hudObject;

    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Camera.main.orthographicSize == 50)
            {
                hudObject.SetActive(true);
                Camera.main.orthographicSize = 5;
            }
            else
            {
                hudObject.SetActive(false);
                Camera.main.orthographicSize = 50;
            }

        }

    }
}
