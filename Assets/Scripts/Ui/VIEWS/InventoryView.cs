using Input;
using UiInput;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : UiView
{
    [Header("Inventory Elements")] [SerializeField]
    private SoulInformation SoulItemPlaceHolder;

    [SerializeField] private Text Description;
    [SerializeField] private Text Name;
    [SerializeField] private Image Avatar;
    [SerializeField] private Button UseButton;
    [SerializeField] private Button DestroyButton;

    [SerializeField]
    private SoulsGridNavigator _gridNavigator;

    private RectTransform _contentParent;
    private GameObject _currentSelectedGameObject;
    private SoulInformation _currentSoulInformation;

    public override void Awake()
    {
        base.Awake();
        _contentParent = (RectTransform)SoulItemPlaceHolder.transform.parent;
        InitializeInventoryItems();
    }
    
    protected override void ActiveView_OnClick(UiView viewToActive)
    {
        base.ActiveView_OnClick(viewToActive);
        //StartCoroutine(UISelectHelper.GiveFocus(BackButon));
        _gridNavigator.RebuildNavigation();

    }
    
    private void InitializeInventoryItems()
    {
        for (int i = 0, j = SoulController.Instance.Souls.Count; i < j; i++)
        {
            SoulInformation newSoul = Instantiate(SoulItemPlaceHolder.gameObject, _contentParent).GetComponent<SoulInformation>();
            newSoul.SetSoulItem(SoulController.Instance.Souls[i], () => SoulItem_OnClick(newSoul));
        }

        SoulItemPlaceHolder.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        ClearSoulInformation();
        InputManager.I.OnInventory += OnInventory;

        void OnInventory()
        {
            InputManager.I.OnInventory -= OnInventory;
            BackButon.onClick.Invoke();
        }
    }

    private void ClearSoulInformation()
    {
        Description.text = "";
        Name.text = "";
        Avatar.sprite = null;
        SetupUseButton(false);
        SetupDestroyButton(false);
        _currentSelectedGameObject = null;
        _currentSoulInformation = null;
    }

    public void SoulItem_OnClick(SoulInformation soulInformation)
    {
        _currentSoulInformation = soulInformation;
        _currentSelectedGameObject = soulInformation.gameObject;
        GameEvents.Points.Value += soulInformation.soulItem.points;
        SetupSoulInformation(soulInformation.soulItem);
    }

    private void SetupSoulInformation(SoulItem soulItem)
    {
        Description.text = $"Use soul give: {soulItem.points} points\n\n{soulItem.Description}";
        Name.text = soulItem.Name;
        Avatar.sprite = soulItem.Avatar;
        StartCoroutine(UISelectHelper.GiveFocus(UseButton));
        SetupUseButton(soulItem.CanBeUsed);
        SetupDestroyButton(soulItem.CanBeDestroyed);
        InputManager.I.OnCancel += OnCancel;

        void OnCancel()
        {
            InputManager.I.OnCancel -= OnCancel;
            
            _gridNavigator.ReFocus();
        }
    }

    private void SelectElement(int index)
    {

    }

    private void CantUseCurrentSoul()
    {
        PopUpInformation popUpInfo = new PopUpInformation { DisableOnConfirm = true, UseOneButton = true, Header = "CAN'T USE", Message = "THIS SOUL CANNOT BE USED IN THIS LOCALIZATION" };
        GUIController.Instance.ShowPopUpMessage(popUpInfo);
    }

    private void UseCurrentSoul(bool canUse)
    {
        if (!canUse)
        {
            CantUseCurrentSoul();
        }
        else
        {
            //USE SOUL
            Destroy(_currentSelectedGameObject);
            ClearSoulInformation();
        }
        _gridNavigator.ReFocus();
    }

    private void DestroyCurrentSoul()
    {
        Destroy(_currentSelectedGameObject);
        ClearSoulInformation();
    }

    private void SetupUseButton(bool active)
    {
        UseButton.onClick.RemoveAllListeners();
        if (active)
        {
            bool isInCorrectLocalization = GameControlller.Instance.IsCurrentLocalization(_currentSoulInformation.soulItem.UsableInLocalization);
            UseButton.interactable = isInCorrectLocalization;
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = isInCorrectLocalization,
                UseOneButton = false,
                Header = "USE ITEM",
                Message = "Are you sure you want to USE: " + _currentSoulInformation.soulItem.Name + " ?",
                Confirm_OnClick = () => UseCurrentSoul(isInCorrectLocalization)
            };
            UseButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
        }
        UseButton.gameObject.SetActive(active);
    }

    private void SetupDestroyButton(bool active)
    {
        DestroyButton.onClick.RemoveAllListeners();
        if (active)
        {
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = true,
                UseOneButton = false,
                Header = "DESTROY ITEM",
                Message = "Are you sure you want to DESTROY: " + Name.text + " ?",
                Confirm_OnClick = () => DestroyCurrentSoul()
            };
            DestroyButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
        }

        DestroyButton.gameObject.SetActive(active);
    }
}