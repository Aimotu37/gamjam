// 调试用：键盘快捷键跳过 Script3 各状态
// 1: S3_Intro | 2: S3_Exploring | 3: S3_AllItemsViewed
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace S3
{
    public class S3StateController : MonoBehaviour
    {
        [Header("S3 调试快捷键（编辑器/开发模式）")]
        public bool enableShortcuts = true;

        private void Update()
        {
            if (!enableShortcuts) return;
            if (Input.GetKeyDown(KeyCode.Alpha1)) Switch(RoomState.S3_Intro);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Switch(RoomState.S3_Exploring);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Switch(RoomState.S3_ReadyToSleep);
        }

        private void Switch(RoomState target)
        {
            var gm = GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("[S3StateController] S3.GameManager.Instance 未初始化");
                return;
            }

            Debug.Log($"<color=cyan>[S3 Test Skip]</color> 强制跳转 -> <color=yellow>{target}</color>");

            gm.uiBlockCount = 0;

            var stackField = typeof(SceneManagerBase).GetField("uiBlockStack",
                BindingFlags.NonPublic | BindingFlags.Instance);
            (stackField?.GetValue(gm) as Stack<string>)?.Clear();

            if (DialogueManager.instance != null)
            {
                DialogueManager.instance.StopAllCoroutines();
                DialogueManager.instance.dialoguePanel.SetActive(false);
                typeof(DialogueManager)
                    .GetField("isDialogueActive", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(DialogueManager.instance, false);
            }

            gm.EnterState(target);
        }
    }
}
