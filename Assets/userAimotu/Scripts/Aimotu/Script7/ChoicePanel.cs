using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    public static ChoicePanel Instance;

    [Header("根节点（默认隐藏）")]
    public GameObject panelRoot;

    [Header("提示语")]
    public TextMeshProUGUI promptText;

    [Header("选项 A")]
    public Button buttonA;
    public TextMeshProUGUI buttonALabel;

    [Header("选项 B（labelB 留空则隐藏，单按钮模式）")]
    public Button buttonB;
    public TextMeshProUGUI buttonBLabel;

    private Action<int> _onChosen;
    private IGameManager GameMgr => FindAnyObjectByType<SceneManagerBase>() as IGameManager;

    private void Awake()
    {
        Instance = this;
        if (panelRoot != null) panelRoot.SetActive(false);
        if (buttonA != null) buttonA.onClick.AddListener(() => Choose(0));
        if (buttonB != null) buttonB.onClick.AddListener(() => Choose(1));
    }

    public void Show(string prompt, string labelA, string labelB, Action<int> onChosen)
    {
        if (panelRoot == null) { Debug.LogWarning("[ChoicePanel] panelRoot 未配置"); return; }
        if (promptText != null) promptText.text = prompt;
        if (buttonALabel != null) buttonALabel.text = labelA;

        bool hasB = !string.IsNullOrEmpty(labelB);
        if (buttonB != null) buttonB.gameObject.SetActive(hasB);
        if (hasB && buttonBLabel != null) buttonBLabel.text = labelB;

        _onChosen = onChosen;
        GameMgr?.PushUIBlock("Choice");
        panelRoot.SetActive(true);
    }

    private void Choose(int index)
    {
        panelRoot.SetActive(false);
        GameMgr?.PopUIBlock("Choice");
        var cb = _onChosen;
        _onChosen = null;
        cb?.Invoke(index);
    }
}