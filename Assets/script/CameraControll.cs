using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [Header("���a��m")]
    public GameObject Player;
    Transform target; // ���⪺ Transform �ե�

    [Header("�a��")]
    public Vector2 LeftDownminBounds; // �a�ϥ��U�����
    public Vector2 RightUpmaxBounds; // �a�ϥk�W�����

    private RectTransform canvasRectTransform;
    private Camera mainCamera;

    [Header("�۾�����")]
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
            // �N��v������m�]�m���P���⪺��m�ۦP�A������b�a����ɤ�
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
        // �p�G�O Canvas�A�N���m�i��ɥ��A�Ϩ�l�צb�e����
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
