using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ChoiceUISystem : MonoBehaviour
{
    public static ChoiceUISystem Instance;
    private IGameManager GetManager()
    {
        // 1. 尝试直接通过单例获取（最推荐）
        if (S61.GameManager.Instance != null) return (IGameManager)S61.GameManager.Instance;

        // 2. 如果单例不可用，尝试在场景中寻找挂载了 GameManager 脚本的物体
        var gm = FindObjectOfType<S61.GameManager>();
        if (gm != null) return (IGameManager)gm;

        return null;
    }
    [Header("UI 引用")]
    public GameObject choicePanel;      // 整个选项面板
    public TextMeshProUGUI questionText; // 问题标题
    public Transform buttonContainer;   // 按钮的父物体（建议加 Vertical Layout Group）
    public GameObject buttonPrefab;     // 选项按钮预制体

    private Action<int> onSelectionDone;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 如果你希望在切关后保留，可以加上 DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // 确保不会执行后续逻辑
        }

        if (choicePanel) choicePanel.SetActive(false);
    }

    public void Show(string question, string[] options, Action<int> callback)
    {
        if (choicePanel == null) Debug.LogError("ChoiceUISystem: choicePanel 未在 Inspector 中赋值！");
        if (questionText == null) Debug.LogError("ChoiceUISystem: questionText 未在 Inspector 中赋值！");
        if (buttonContainer == null) Debug.LogError("ChoiceUISystem: buttonContainer 未在 Inspector 中赋值！");
        if (buttonPrefab == null) Debug.LogError("ChoiceUISystem: buttonPrefab 未在 Inspector 中赋值！");
        var manager = GetManager();
        if (manager == null) Debug.LogError("ChoiceUISystem: GetManager() 返回空，找不到 IGameManager！");
        // --- Debug 结束 ---
        onSelectionDone = callback;
        questionText.text = question;
        choicePanel.SetActive(true);
        // 增加 null 检查，防止第 48 行崩溃
        if (manager != null)
        {
            manager.PushUIBlock("ChoiceUI");
        }
        else
        {
            Debug.LogWarning("ChoiceUISystem: 找不到 GameManager，跳过 PushUIBlock。请检查场景配置。");
        }
        manager.PushUIBlock("ChoiceUI");

        // 清理旧按钮
        foreach (Transform child in buttonContainer) Destroy(child.gameObject);

        // 生成新按钮
        for (int i = 0; i < options.Length; i++)
        {
            int index = i; // 闭包捕获
            GameObject btnObj = Instantiate(buttonPrefab, buttonContainer);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
            btnObj.GetComponent<Button>().onClick.AddListener(() => SelectOption(index));
        }
    }

    private void SelectOption(int index)
    {
        Debug.Log($"<color=white>[ChoiceUI]</color> 用户点击了索引: {index}，准备触发回调");
        // 执行对应的 Action (即播放对话)
        onSelectionDone?.Invoke(index);
        StopAllCoroutines();
        StartCoroutine(HandlePostSelection(index));
    }

    // 协程：等待对话播完再显示按钮

    // 统一使用这个协程处理所有后续
    private IEnumerator HandlePostSelection(int index)
    {
        // A. 立即隐藏按钮，防止对话期间重复点击
        buttonContainer.gameObject.SetActive(false);

        // B. 稍微等待，确保 DialogueManager 已经接收到 Action 指令并启动
        yield return new WaitForSeconds(0.2f);

        // C. 等待对话结束
        if (DialogueManager.instance != null)
        {
            while (DialogueManager.instance.IsDialogueActive)
            {
                yield return null;
            }
        }

        // D. 判定是结束还是返回
        if (index == 0) // 炸串（结束项）
        {
            choicePanel.SetActive(false);
            GetManager()?.PopUIBlock("ChoiceUI");

            if (S61.Task_S61.Instance != null)
            {
                S61.Task_S61.Instance.OnFinalChoiceMade(0);
            }
        }
        else // 奶茶/蛋糕（返回项）
        {
            // 关键：对话播完了，重新显示按钮，让玩家可以继续选别的
            buttonContainer.gameObject.SetActive(true);
            // 重置 ChoiceAction 的等待状态 (见下方 ChoiceAction 修改)
        }
    }
}