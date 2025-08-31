using System;
using UnityEngine;
using UnityEngine.UI;

public class SoulInformation : MonoBehaviour
{
    [SerializeField] private Image MainImage;
    [SerializeField] private Button SoulButton;
    [HideInInspector] public SoulItem soulItem;

    public void SetSoulItem(SoulItem _soulItem, Action OnSoulClick = null)
    {
        soulItem = _soulItem;
        MainImage.sprite = soulItem.Avatar;
       // SoulButton.interactable = soulItem.UsableInLocalization == GameControlller.Instance.CurrentGameLocalization;
        if (OnSoulClick != null)
        {
            SoulButton.onClick.AddListener(() =>
            {
                if (GameControlller.Instance.CurrentGameLocalization == soulItem.UsableInLocalization)
                {
                    OnSoulClick();
                }
            });
        }
    }
}