using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic; // 必须引用

namespace S61
{
    public class Task_S61 : MonoBehaviour
    {
        public static Task_S61 Instance;

        [Header("UI 文本引用")]
        public TextMeshProUGUI diaryTaskText;
        public GameObject snackCartPanel;

        [Header("小吃任务配置")]
        // 在 Inspector 赋值你的“旁白+选项”序列
        public ActionSequenceTrigger finalChoiceSequence;
        private HashSet<string> finishedSnacks = new HashSet<string>();
        private int requiredSnackCount = 3; // 奶茶、炸串、蛋糕

        [Header("任务状态")]
        public bool hasFinishedSnackTask = false;
        private int totalDiaries = 1;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
        }

        void Update()
        {
            // 测试快捷键：按下大键盘或小键盘的 6
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                Debug.Log("<color=orange>[Debug]</color> 按下快捷键 6，强制触发最终剧情");

                // 如果任务还没完成，强制调用完成逻辑
                if (!hasFinishedSnackTask)
                {
                    CompleteSnackCartTask();
                }
            }
        }
        // --- 新增：小吃打卡方法 ---
        public void OnSnackInteracted(string snackName)
        {
            if (hasFinishedSnackTask) return; // 如果已经整体完成了，不再处理

            if (!finishedSnacks.Contains(snackName))
            {
                finishedSnacks.Add(snackName);
                Debug.Log($"<color=yellow>[Snack]</color> {snackName} 已交互。进度: {finishedSnacks.Count}/{requiredSnackCount}");
                UpdateTaskUI();
            }


            if (finishedSnacks.Count >= requiredSnackCount)
            {
                CompleteSnackCartTask();
            }
        }
        public void CompleteSnackCartTask()
        {
            if (hasFinishedSnackTask) return;

            hasFinishedSnackTask = true;

            Debug.Log("<color=cyan>[Task S6]</color> 任务完成：触发最终抉择旁白");
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

            // 3. 对话结束了，再等一小下（给 UI 消失的动画一点时间）
            //yield return new WaitForSeconds(0.1f);

            Debug.Log("<color=cyan>[Task S61]</color> 对话框已空闲，触发最终旁白");
            if (finalChoiceSequence != null)
            {
                finalChoiceSequence.TriggerSequence(); // 触发旁白和选项
            }

            
        }

        // ... 保持你原有的 UpdateTaskUI, Start, OnDestroy 等方法不变 ...
        private void Start()
        {
            Invoke(nameof(UpdateTaskUI), 0.1f);
        }

        private void UpdateTaskUI()
        {
            if (diaryTaskText != null)
            {
                int currentCount = hasFinishedSnackTask ? 1 : 0;
                string colorTag = hasFinishedSnackTask ? "<color=green>" : "";
                string endTag = hasFinishedSnackTask ? "</color>" : "";
                diaryTaskText.text = $"找回 {currentCount}/ {totalDiaries} 篇日记";
            }
            if (finishedSnacks.Count >= requiredSnackCount && !hasFinishedSnackTask)
            {
                CompleteSnackCartTask();
            }
        }
        // 在 Task_S61 中，监听最终选择的结果
        public void OnFinalChoiceMade(int index)
        {
            if (index == 0) // 选中炸串，任务完成
            {
                hasFinishedSnackTask = true;

                // 1. 关闭摊位面板
                if (snackCartPanel != null)
                {
                    snackCartPanel.SetActive(false);
                }

                // 2. 状态切换（这里要确保状态切换后 Task_S61 不会被 Destroy）
                if (diaryTaskText != null)
                {
                    // A. 强制激活物体（防止它随父物体一起变灰）
                    diaryTaskText.gameObject.SetActive(true);

                    // B. 强制检查父物体是否被关掉（如果父物体关了，子物体点不亮）
                    if (diaryTaskText.transform.parent != null)
                    {
                        diaryTaskText.transform.parent.gameObject.SetActive(true);
                    }

                    // C. 立即执行刷新逻辑
                    UpdateTaskUI();
                    Debug.Log("<color=green>[Task S61]</color> TaskUI 已更新，当前数值应为 1/1");
                }

                StartCoroutine(PlayEndingSequence());
            }
        }
        private IEnumerator PlayEndingSequence()
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("<color=yellow>[Task S61]</color> 开始播放结束任务的旁白...");
            // 这里可以触发 finalChoiceSequence 之外的另一个结局 Sequence
        }
    }
}