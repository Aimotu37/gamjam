using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : SceneManagerBase
{
    [SerializeField] public Button playBtn;
    [SerializeField] public Button continueBtn;
    [SerializeField] public Button setBtn;    
    [SerializeField] public Button detailsBtn;
    [SerializeField] public Button returnBtn;
    
    public GameObject settingsPanel;
    public GameObject detailsPanel;

    [Header("Audio")] 
    public AudioClip openSound;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    protected override RoomState InitialState => RoomState.None;
    public override GameObject TaskModuleObject => null;

    private void Start()
    {
        base.Start();

        if (continueBtn != null)
        {
            continueBtn.interactable = SaveSystem.HasSaveFile();
        }
        playBtn.onClick.AddListener(() => { PlayClickSound(); OnClickPlay(); });
        continueBtn.onClick.AddListener(() => { PlayClickSound(); OnClickContinue(); });
        detailsBtn.onClick.AddListener(() => { PlayClickSound(); OnClickDetails(); });
        setBtn.onClick.AddListener(OnClickSetting);
        returnBtn.onClick.AddListener(OnCloseDetail);
        
        AddHoverEffect(playBtn);
        AddHoverEffect(continueBtn);
        AddHoverEffect(detailsBtn);
    }

    private void OnClickPlay()
    {
        SceneManagerBase.PendingStateIndex = null;
        StartCoroutine(FadeToScene("Script2"));
    }
    
    private IEnumerator FadeToScene(string sceneName)
    {
        if (transitionMaskGroup != null)
        {
            PushUIBlock("Main MenuTransition");
            transitionMaskGroup.blocksRaycasts = true;
            float elasped = 0;
            while (elasped < 1f)
            {
                elasped += Time.deltaTime;
                transitionMaskGroup.alpha = elasped / 1f;
                yield return null;
            }
        }

        
        SceneManager.LoadScene(sceneName);
    }

    private void OnClickContinue()
    {
        GameSaveData data = SaveSystem.Load();

        if (data != null)
        {
            SceneManagerBase.PendingStateIndex = data.roomStateIndex;
            
            StartCoroutine(FadeToScene(data.sceneName));
        }
    }

    private void OnClickSetting()
    {
        if (AudioManager.Instance != null && openSound != null)
        {
            AudioManager.Instance.PlaySFX(openSound);
        }
        settingsPanel.SetActive(true);
    }

    private void OnClickDetails()
    {
        detailsPanel.SetActive(true);
    }

    private void OnCloseDetail()
    {
        detailsPanel.SetActive(false);
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null && clickSound != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
    }

    private void AddHoverEffect(Button btn)
    {
        if (btn == null) return;
        EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
        if(trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => {
            if (AudioManager.Instance != null && hoverSound != null && btn.interactable)
            {
                AudioManager.Instance.PlaySFX(hoverSound);
            }
        });

        trigger.triggers.Add(entry);
    }
}
