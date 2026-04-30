using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlUIAction", menuName = "Actions/Control UI")]
public class ControlUIAction : StateAction
{
    public enum UIType { Notebook, NoteBigImage, Map, Cart, OpenNoteBook, OpenComputer, ComputerMessage, NextComputerMessage }
    public UIType targetUI;
    public bool isClose = false; // 新增：是否是关闭操作
    [Header("音效设置")]
    public AudioClip customSFX;
    [Range(0f, 1f)]
    public float volume = 1.0f; // 新增：音量控制
    public override IEnumerator Execute()
    {
        var manager = GetManager();
        Debug.Log($"<color=cyan>[UI Debug]</color> 当前 Action 的目标是: {targetUI}， 资源名称: {name}");

        manager?.PlayGlobalSFX(customSFX, volume);
        switch (targetUI)
        {
            case UIType.Notebook:
                HandleNotebook(manager);
                break;
            case UIType.NoteBigImage:
                HandleNoteBigImage(manager);
                break;
            case UIType.Map:
                HandleMap(manager);
                break;
            case UIType.Cart:
                HandleCart(manager);
                break;
            case UIType.OpenNoteBook:
                HandleOpenNoteBook(manager);
                break;
            case UIType.OpenComputer:
                HandleOpenComputer(manager);
                break;
            case UIType.ComputerMessage:
                HandleComputerMessage(manager);
                break;
            case UIType.NextComputerMessage:
                HandleNextComputerMessage(manager);
                break;
        }

        yield return null;
    }
    private void HandleNotebook(IGameManager manager)
    {
        if (isClose)
            NotebookUI.Instance.Close();
        else
            NotebookUI.Instance.Open();
    }
    private void HandleNoteBigImage(IGameManager manager)
    {
        if (isClose)
            NoteUI.Instance.Close();
        else
            NoteUI.Instance.Open();
    }
    private void HandleMap(IGameManager manager)
    {
        if (isClose)
        {
            // 假设你的地图 UI 类名叫 MapUI
            MapUI.Instance.Close();
            manager?.PopUIBlock("Map");
        }
        else
        {
            manager?.PushUIBlock("Map");
            MapUI.Instance.Open();
        }
    }
    // 增加对应的处理方法
    private void HandleCart(IGameManager manager)
    {
        Debug.Log($"CartUI Instance 是否为空: {CartUI.Instance == null}");
        if (isClose)
        {
            CartUI.Instance.Close(); // 调用小吃车UI的关闭
            manager?.PopUIBlock("Cart");
        }
        else
        {
            manager?.PushUIBlock("Cart");
            CartUI.Instance.Open(); // 调用小吃车UI的开启
        }
    }

    //打开日记本内页
    private void HandleOpenNoteBook(IGameManager manager)
    {
        Debug.Log($"NoteUI Instance 是否为空: {NoteUI.Instance == null}");
        if (isClose)
        {
            NotebookUI.Instance.ClosePages();
            manager?.PopUIBlock("DiaryPages");
        }
        else
        {
            manager?.PushUIBlock("DiaryPages");
            NotebookUI.Instance.OpenPages();
            NotebookUI.Instance.GetPageContent();
        }
    }

    //打开电脑
    private void HandleOpenComputer(IGameManager manager)
    {
        Debug.Log($"ComputerUI Instance 是否为空: {ComputerUI.Instance == null}");
        if (isClose)
            ComputerUI.Instance.Close();
        else
            ComputerUI.Instance.Open();

    }

    //打开消息弹窗
    private void HandleComputerMessage(IGameManager manager)
    {
        Debug.Log($"ComputerUI Instance 是否为空: {ComputerUI.Instance == null}");
        ComputerUI.Instance.OpenMessageWindow();
        ComputerUI.Instance.GetMessageContent();
    }

    //下一条消息
    private void HandleNextComputerMessage(IGameManager manager)
    {
        Debug.Log($"ComputerUI Instance 是否为空: {ComputerUI.Instance == null}");
        ComputerUI.Instance.NextMessage();
    }


}
