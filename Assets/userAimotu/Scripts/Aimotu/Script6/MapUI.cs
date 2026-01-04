using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject canvasRoot;
    public static MapUI Instance; // 添加这一行

    private void Awake()
    {
        // 经典的单例初始化
        if (Instance == null) Instance = this;
    }
    public void Open()
    {
        Debug.Log("[MapUI] Open");
        canvasRoot.SetActive(true);
       // GameManager.Instance.PushUIBlock("Note");
    }

    public void Close()
    {
        Debug.Log("[MapUI] Close");
        canvasRoot.SetActive(false);
       // GameManager.Instance.PopUIBlock("Note");
    }// Start is called before the first frame update
    
}
