using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : SceneManagerBase
{
    [SerializeField] public Button playBtn;
    [SerializeField] public Button continueBtn;
    [SerializeField] public Button setBtn;    
    [SerializeField] public Button detailsBtn;
    
    public GameObject settingsPanel;
    public GameObject detailsPanel;

    protected override RoomState InitialState => RoomState.None;
    public override GameObject TaskModuleObject => null;

    private void Start()
    {
        base.Start();
        playBtn.onClick.AddListener(OnClickPlay);
        continueBtn.onClick.AddListener(OnClickContinue);
        setBtn.onClick.AddListener(OnClickSetting);
        detailsBtn.onClick.AddListener(OnClickDetails);
        
    }

    private void OnClickPlay()
    {
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
        
    }

    private void OnClickSetting()
    {
        settingsPanel.SetActive(true);
    }

    private void OnClickDetails()
    {
        detailsPanel.SetActive(true);
    }
    
    
}
