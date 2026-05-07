using UnityEngine;

public class S7StateController : MonoBehaviour
{
    [Header("S7 调试快捷键 (仅编辑器)")]
    [Header("1:Intro 2:Reading 3:InterruptDone 4:BeforeSmash 5:AfterSmash 6:MemoryErase 7:End")]
    public bool enableShortcuts = true;

    private void Update()
    {
        if (!enableShortcuts) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) Trigger(RoomState.S7_Intro);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Trigger(RoomState.S7_DiaryReading);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Trigger(RoomState.S7_PhoneInterruptDone);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Trigger(RoomState.S7_BeforeSmash);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Trigger(RoomState.S7_AfterSmash);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Trigger(RoomState.S7_MemoryErase);
        if (Input.GetKeyDown(KeyCode.Alpha7)) Trigger(RoomState.S7_End);
    }

    private void Trigger(RoomState target)
    {
        var gm = S7.GameManager.Instance;
        if (gm == null)
        {
            Debug.LogError("[S7StateController] S7.GameManager.Instance 未初始化");
            return;
        }
        Debug.Log($"<color=cyan>[S7 Test]</color> 跳转 -> {target}");
        gm.EnterState(target);
    }
}
