using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteUI : MonoBehaviour
{
    public GameObject canvasRoot;
    public static NoteUI Instance;
    private IGameManager _cachedManager;
    private IGameManager GetManager()
    {
        if (_cachedManager == null)
            _cachedManager = FindAnyObjectByType<SceneManagerBase>() as IGameManager;
        return _cachedManager;
    }

    private void Awake()
    {
       
        if (Instance == null) Instance = this;
    }
    public void Open()
    {
        Debug.Log("[StickyNoteBigUI] Open");
        canvasRoot.SetActive(true);
        GetManager()?.PushUIBlock("Note");
    }

    public void Close()
    {
        Debug.Log("[StickyNoteBigUI] Close");
        canvasRoot.SetActive(false);
        GetManager()?.PopUIBlock("Note");
    }
 
}
