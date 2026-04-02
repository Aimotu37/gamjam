using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
//UpdateTaskUI 直接读 GlobalData，不依赖本地 bool 重复维护

namespace S61
{
    public class Task_S61 : MonoBehaviour
    {
        public static Task_S61 Instance;

        [Header("UI 文本引用")]
        public TextMeshProUGUI diaryTaskText;
        public GameObject snackCartPanel;

        [Header("选项序列")]
        public ActionSequenceTrigger finalChoiceSequence;
        [Header("小吃车状态")]
        private HashSet<string> finishedSnacks = new();
        private int requiredSnackCount = 3; // 奶茶、炸串、蛋糕

        private bool SnackTaskDone => GlobalData.IsDiaryUnlocked(DiaryID.Diary2_SnackCart);
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }
        private void Start()
        {
            Invoke(nameof(UpdateTaskUI), 0.1f);
        }
        // 每次某个小吃被交互时调用
        public void OnSnackInteracted(string snackName)
        {
            if (SnackTaskDone) return; // 如果已经整体完成了，不再处理

            if (finishedSnacks.Add(snackName))
            {
                Debug.Log($"<color=yellow>[Snack]</color> {snackName} 已交互，进度：{finishedSnacks.Count}/{requiredSnackCount}");
                UpdateTaskUI();
            }


            if (finishedSnacks.Count >= requiredSnackCount)
            {
                CompleteSnackCartTask();
            }
        }
        public void CompleteSnackCartTask()
        {
            if (SnackTaskDone) return;

            // 写入全局进度
            GlobalData.UnlockDiary(DiaryID.Diary2_SnackCart);
            Debug.Log("<color=cyan>[Task S61]</color> 小吃车任务完成，日记2 已解锁");

            UpdateTaskUI();
            StartCoroutine(WaitAndTriggerFinalSequence());
        }
        private IEnumerator WaitAndTriggerFinalSequence()
        {
            // 1. 先等一帧，确保当前点击触发的对话已经完全启动
            yield return new WaitForSeconds(0.5f);

            // 2. 检查对话管理器是否正在显示对话
            if (DialogueManager.instance != null)
            {
                while (DialogueManager.instance.IsDialogueActive)
                {
                    yield return null;
                }
            }

            // 3. 对话结束了，多等 0.2 秒给 UI 动画一点缓冲时间，彻底避免“撞车”
            yield return new WaitForSeconds(0.3f);

            

            Debug.Log("<color=cyan>[Task S61]</color> 对话框已空闲，触发选择序列");
            finalChoiceSequence?.TriggerSequence();
        }

        // 玩家最终选择炸串（index == 0）时由外部调用
        public void OnFinalChoiceMade(int index)
        {
            if (index != 0) return;
            if (snackCartPanel != null) snackCartPanel.SetActive(false);
            UpdateTaskUI();
            StartCoroutine(PlayEndingSequence());
        }
        private IEnumerator PlayEndingSequence()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("<color=yellow>[Task S61]</color> 开始播放结束任务的旁白...");
           
        }
        private void UpdateTaskUI()
        {
            if (diaryTaskText == null) return;
            int current = SnackTaskDone ? 1 : 0;
            diaryTaskText.text = $"找回 {current} / 1 篇日记{(SnackTaskDone ? " ✓" : "")}";
        }

        public void UpdateUI() => UpdateTaskUI();
    }



}