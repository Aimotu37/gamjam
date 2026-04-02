using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartUI : MonoBehaviour
{
    public GameObject canvasRoot;
    public static CartUI Instance; // 添加这一行

    private void Awake()
    {
        // 经典的单例初始化
        if (Instance == null) Instance = this;
    }
    public void Open()
    {
        Debug.Log("[CartUI] Open");
        canvasRoot.SetActive(true);
    }

    public void Close()
    {
        Debug.Log("[CartUI] Close");
        canvasRoot.SetActive(false);
    }// Start is called before the first frame update

}


