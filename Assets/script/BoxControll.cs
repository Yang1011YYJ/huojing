using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControll : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        
    }
    private void Update()
    {
        Vector2 cameraPosition = cam.transform.position;
        Vector2 screenCenter = new Vector3(Screen.width / 2 - 394.5f, Screen.height / 2 -287, 0 );
        Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);
        worldCenter.z = 0; // 確保物件在相機的平面上
        gameObject.transform.position = worldCenter;
    }
}
