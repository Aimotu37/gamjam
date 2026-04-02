using UnityEngine;
public class FindDuplicate : MonoBehaviour
{
    void Awake()
    {
        var all = FindObjectsByType<PopupSystem>(FindObjectsSortMode.None);
        foreach (var p in all)
            Debug.Log($"[FindDuplicate] PopupSystem ÔÚ£º{p.gameObject.name}£¬Â·¾¶£º{GetPath(p.gameObject)}", p.gameObject);
    }
    string GetPath(GameObject go)
    {
        string path = go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = go.name + "/" + path;
        }
        return path;
    }
}