using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [Header("玩家位置")]
    public GameObject Player;
    Transform target; // 角色的 Transform 組件

    [Header("地圖")]
    public Vector2 LeftDownminBounds; // 地圖左下角邊界
    public Vector2 RightUpmaxBounds; // 地圖右上角邊界

    private RectTransform canvasRectTransform;
    private Camera mainCamera;

    [Header("相機控制")]
    public float HighestCameraPosition;
    public float LowestCameraPosition;
    public float LeftestCameraPosition;
    public float RightestCameraPosition;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        canvasRectTransform = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
        target = Player.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        if (target != null && Player.activeSelf)
        {
            // 將攝影機的位置設置為與角色的位置相同，但限制在地圖邊界內
            float clampedX = Mathf.Clamp(target.position.x, LeftDownminBounds.x + mainCamera.orthographicSize * mainCamera.aspect, RightUpmaxBounds.x - mainCamera.orthographicSize * mainCamera.aspect);
            float clampedY = Mathf.Clamp(target.position.y, LeftDownminBounds.y + mainCamera.orthographicSize / mainCamera.aspect, RightUpmaxBounds.y - mainCamera.orthographicSize / mainCamera.aspect);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);

            if (mainCamera.transform.position.y > HighestCameraPosition)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, HighestCameraPosition, mainCamera.transform.position.z);
            }
            if (mainCamera.transform.position.y < LowestCameraPosition)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, LowestCameraPosition, mainCamera.transform.position.z);
            }
            if (mainCamera.transform.position.x < LeftestCameraPosition)
            {
                mainCamera.transform.position = new Vector3(LeftestCameraPosition, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            if (mainCamera.transform.position.x > RightestCameraPosition)
            {
                mainCamera.transform.position = new Vector3(RightestCameraPosition, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
        }
        // 如果是 Canvas，將其位置進行補正，使其始終在畫面內
        if (canvasRectTransform != null)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
            Vector2 anchoredPosition = canvasRectTransform.anchorMin;
            anchoredPosition.x = Mathf.Clamp01(viewportPosition.x);
            anchoredPosition.y = Mathf.Clamp01(viewportPosition.y);
            canvasRectTransform.anchorMin = anchoredPosition;
            canvasRectTransform.anchorMax = anchoredPosition;

            if (canvasRectTransform.transform.position.y > HighestCameraPosition)
            {
                canvasRectTransform.transform.position = new Vector3(canvasRectTransform.transform.position.x, HighestCameraPosition, canvasRectTransform.transform.position.z);
            }
            if (canvasRectTransform.transform.position.y < LowestCameraPosition)
            {
                canvasRectTransform.transform.position = new Vector3(canvasRectTransform.transform.position.x, LowestCameraPosition, canvasRectTransform.transform.position.z);
            }
            if (canvasRectTransform.transform.position.x < LeftestCameraPosition)
            {
                canvasRectTransform.transform.position = new Vector3(LeftestCameraPosition, canvasRectTransform.transform.position.y, canvasRectTransform.transform.position.z);
            }
            if (canvasRectTransform.transform.position.x > RightestCameraPosition)
            {
                canvasRectTransform.transform.position = new Vector3(RightestCameraPosition, canvasRectTransform.transform.position.y, canvasRectTransform.transform.position.z);
            }
        }
    }
}
