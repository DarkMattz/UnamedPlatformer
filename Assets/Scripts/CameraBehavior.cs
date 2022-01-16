using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] private Transform targetObject;
    //[SerializeField] private int offset = -10;
    [SerializeField] private Vector2 screenSize;
    [SerializeField] private Vector2 screenDeadZonePercentage;
    [SerializeField] private float dampTime = 0.3F;
    private Vector3 velocityReference = Vector3.zero;
    private Camera mainCamera;


    private void Start()
    {
        targetObject = GameObject.Find("Player").transform;
        mainCamera = GetComponent<Camera>();
        screenSize = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }


    private void LateUpdate()
    {

        float screenDeadZoneX = screenSize.x * screenDeadZonePercentage.x / 100;
        if (targetObject.position.x > transform.position.x + screenDeadZoneX || targetObject.position.x < transform.position.x - screenDeadZoneX)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetObject.position.x, transform.position.y, -10), ref velocityReference, dampTime);
        }
    }
}
