using UnityEngine;
using System.Collections.Generic;    // 修复 Stack<> 报错
using System.Reflection;             // 修复 BindingFlags 和 FieldInfo 报错                    
using S4;

    public class S4StateController : MonoBehaviour
    {
        [Header("S4 调试快捷键 (大键盘数字键)")]        
        [Header("1: Intro | 2: NoteLocked | 3: Password | 4: AllTasksDone | 5: Exit")]
        public bool enableShortcuts = true;

        private void Update()
        {
            if (!enableShortcuts) return;

            if (Input.GetKeyDown(KeyCode.Alpha1)) Switch(RoomState.Intro, false);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Switch(RoomState.NoteLocked, false);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Switch(RoomState.PasswordCollecting, false);
            if (Input.GetKeyDown(KeyCode.Alpha4)) Switch(RoomState.AllTasksDone, true);
            if (Input.GetKeyDown(KeyCode.Alpha5)) Switch(RoomState.ReadyToExit, true);
    }

        private void Switch(RoomState target, bool fillTasks)
        {
            var gm = S4.GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("S4.GameManager.Instance 尚未初始化！");
                return;
            }

            Debug.Log($"<color=cyan>[Test Skip]</color> 触发键盘跳转 -> 目标状态: <color=yellow>{target}</color>");

            // 1. 暴力清理 UI 阻塞 (解决卡死的核心)
            gm.uiBlockCount = 0;

            // 2. 清理 Stack（反射）
            var stackField = typeof(S4.GameManager).GetField("uiBlockStack", BindingFlags.NonPublic | BindingFlags.Instance);
            if (stackField != null)
            {
                var stack = (Stack<string>)stackField.GetValue(gm);
                stack?.Clear();
            }

            //  强行终止并隐藏对话框
            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.StopAllCoroutines();
                DialogueManager.instance.dialoguePanel.SetActive(false);

                var f = typeof(DialogueManager).GetField("isDialogueActive",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                f?.SetValue(DialogueManager.instance, false);
            }

        if (fillTasks)
        {
            GlobalData.D1_Fish = true;
            GlobalData.D1_Doll = true;
            GlobalData.D1_Award = true;
            GlobalData.UnlockDiary(DiaryID.Diary1_FishAndBeads);
        }
        else
        {
            GlobalData.D1_Fish = false;
            GlobalData.D1_Doll = false;
            GlobalData.D1_Award = false;
        }

        // 刷新 Task UI
        var tm = FindAnyObjectByType<S4.Task_S4>();
        tm?.ShowTaskUI();
        tm?.UpdateUI();

        gm.EnterState(target);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gm);
        if (tm != null) UnityEditor.EditorUtility.SetDirty(tm);
#endif
    }

}
