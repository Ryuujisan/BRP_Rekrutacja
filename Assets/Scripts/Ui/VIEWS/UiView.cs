using System;
using UiInput;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{

    [Header("UI VIEW elements")] [SerializeField]
    private bool UnpauseOnClose = false;

    [SerializeField] private bool CloseOnNewView = true;
    [SerializeField] protected Button BackButon;

    private UiView _parentView;

    public virtual void Awake()
    {
        BackButon.onClick.AddListener(() => DisableView_OnClick(this));
    }

    protected virtual void ActiveView_OnClick(UiView viewToActive)
    {
        viewToActive.SetParentView(this);
        viewToActive.ActiveView();
        this.ActiveView(!CloseOnNewView);
        
    }

    protected void DisableView_OnClick(UiView viewToDisable)
    {
        viewToDisable.DisableView();
        
    }

    public void DestroyView_OnClick(UiView viewToDisable)
    {
        viewToDisable.DestroyView();
    }

    public void SetParentView(UiView parentView)
    {
        _parentView = parentView;
    }

    public void ActiveView(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void ActiveView(Action onBackButtonAction = null)
    {
        if (onBackButtonAction != null) BackButon.onClick.AddListener(() => onBackButtonAction());

        if (!gameObject.activeSelf) this.ActiveView(true);
    }

    public void DisableView()
    {
        Debug.Log("Rise disbale on Inventory");
        if (_parentView != null)
        {
            _parentView.ActiveView();
        }

        if (UnpauseOnClose)
        {
            GameControlller.Instance.IsPaused = false;
            TargetSelectedManager.I.SetWorldActive(true);
        }

        this.ActiveView(false);
        
    }

    public void DestroyView()
    {
        if (_parentView != null)
        {
            _parentView.ActiveView();
            TargetSelectedManager.I.SetWorldActive(false);
        }

        Destroy(this.gameObject);
    }

    public void DisableBackButton()
    {
        BackButon.gameObject.SetActive(false);
    }

    public Button GetBackButton()
    {
        return BackButon;
    }
}