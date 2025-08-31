using Input;
using UiInput;
using UnityEngine;

public class GUIController : MonoBehaviour
{

    #region singleton

    public static GUIController Instance;

    private void Awake()
    {
        DisableOnStartObject.SetActive(false);
        Instance = this;
    }

    #endregion

    [SerializeField] 
    private GameObject DisableOnStartObject;
    [SerializeField] 
    private RectTransform ViewsParent;
    [SerializeField] 
    private GameObject InGameGuiObject;
    [SerializeField] 
    private PopUpView PopUp;
    [SerializeField] 
    private PopUpScreenBlocker ScreenBlocker;

    [SerializeField]
    private UiView pauseView;

    [SerializeField]
    private UiView inventory;

    private void Start()
    {
        if (ScreenBlocker) ScreenBlocker.InitBlocker();
        InputManager.I.OnPause += ShowPauseMenu;
        InputManager.I.OnInventory += ShowInventory; 
    }
    
    private void ActiveInGameGUI(bool active)
    {
        InGameGuiObject.SetActive(active);
    }

    public void ShowPopUpMessage(PopUpInformation popUpInfo)
    {
        PopUpView newPopUp = Instantiate(PopUp, ViewsParent) as PopUpView;
        newPopUp.ActivePopUpView(popUpInfo);
    }

    public void ActiveScreenBlocker(bool active, PopUpView popUpView)
    {
        if (active) ScreenBlocker.AddPopUpView(popUpView);
        else ScreenBlocker.RemovePopUpView(popUpView);
    }


    #region IN GAME GUI Clicks

    public void InGameGUIButton_OnClick(UiView viewToActive)
    {
        TargetSelectedManager.I.SetWorldActive(false);
        viewToActive.ActiveView(() => ActiveInGameGUI(true));

        ActiveInGameGUI(false);
        GameControlller.Instance.IsPaused = true;
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    private void ShowPauseMenu()
    {
        InGameGUIButton_OnClick(pauseView);
    }

    private void ShowInventory()
    {
        InGameGUIButton_OnClick(inventory);
    }
    
    #endregion
}