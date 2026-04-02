// 临时挂在 EventSystem 上，确认点击事件打到了哪里
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ClickDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(
                new PointerEventData(EventSystem.current)
                { position = Input.mousePosition },
                results
            );
            foreach (var r in results)
                Debug.Log($"[ClickDebug] 命中：{r.gameObject.name}（深度{r.depth}）");
        }
    }
}