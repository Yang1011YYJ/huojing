using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;

public class SquareControll : MonoBehaviour
{
    [Header("滑方塊")]
    public bool IsDragging = false;
    public Vector3 offset;

    [Header("文本UI")]
    public GameObject TextPanel;

    public EventSystem eventSystem;
    private void Awake()
    {
        IsDragging = false;
    }
    private void Update()
    {
        if (IsDragging)
        {
            Debug.Log("Is Dragging");
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z; // 確保物件的 z 座標不變

            if(Mathf.Abs(transform.rotation.eulerAngles.z + 90f) < 1f || Mathf.Abs(transform.rotation.eulerAngles.z - 270f) < 1f)
            {//直向擺放
                Debug.Log("-90");
                if (Mathf.Abs(Input.GetAxis("Mouse X")) < Mathf.Abs(Input.GetAxis("Mouse Y")))
                {
                    Debug.Log("Move Y");
                    // 上下移動，只改變 Y 軸數值
                    transform.position = new Vector3(transform.position.x, mousePosition.y + offset.y, transform.position.z);
                }
            }
            else if(Mathf.Abs(transform.rotation.eulerAngles.z) < 1f)
            {//橫向擺放
                Debug.Log("0");
                if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
                {
                    Debug.Log("Move X");
                    // 左右移動，只改變 X 軸數值
                    transform.position = new Vector3(mousePosition.x + offset.x, transform.position.y, transform.position.z);
                }
            }
        }
    }
    private void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        if (IsMouseOverObject() && !TextPanel.activeSelf)
        {
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Is Dragging");
            IsDragging = true;
        }
    }
    private void OnMouseUp()
    {
        Debug.Log("UP");
        IsDragging = false;
    }
    private bool IsMouseOverObject()//確認滑鼠點到物件
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            Debug.Log("On Objects");
            return collider.bounds.Contains(mouseWorldPos);
        }
        else
        {
            Debug.Log("Error");
        }

        return false;
    }
}
