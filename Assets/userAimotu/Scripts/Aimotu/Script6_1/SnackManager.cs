using UnityEngine;

public class SnackGameManager : MonoBehaviour
{
    public static SnackGameManager Instance;

    public int totalSnacks = 3;
    private int currentCount = 0;
    private System.Collections.Generic.HashSet<string> finishedList = new System.Collections.Generic.HashSet<string>();

    public ActionSequenceTrigger finalSequence;

    private void Awake()
    {
        Instance = this;
    }

    public void OnSnackCompleted(string id)
    {
        if (finishedList.Contains(id)) return;

        finishedList.Add(id);
        currentCount++;

        Debug.Log($"<color=green>[Snack]</color> {id} 收集成功！当前进度: {currentCount}/{totalSnacks}");

        if (currentCount >= totalSnacks)
        {
            Invoke("StartFinalSequence", 0.5f); // 延迟半秒，等当前的交互 UI 关干净
        }
    }

    private void StartFinalSequence()
    {
        if (finalSequence != null)
        {
            finalSequence.TriggerSequence();
        }
    }
}