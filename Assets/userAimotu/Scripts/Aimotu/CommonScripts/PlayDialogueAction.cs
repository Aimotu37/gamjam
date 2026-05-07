using System.Collections;
using UnityEngine;

//Intro、ReadyToExit对话和旁白，状态切换提示

[CreateAssetMenu(fileName = "PlayDialogueAction", menuName = "Actions/Play Dialogue")]
public class PlayDialogueAction : StateAction
{
    public DialogueSession dialogue;
    
    private bool isRunning = false;
    // public bool changeStateAfterDialogue = false;
    //  public GameManager.RoomState nextState;
    public override IEnumerator Execute()
    {
        if (isRunning) yield break;
        if (DialogueManager.instance.IsDialogueActive) yield break;

        isRunning = true; // 上锁

        yield return new WaitForEndOfFrame();

        if (dialogue == null || DialogueManager.instance == null)
        {
            isRunning = false;
            yield break;
        }

        bool finished = false;

        DialogueManager.instance.StartDialogue(dialogue, () => { finished = true; });

        while (!finished)
            yield return null;

        isRunning = false;

    }
}
