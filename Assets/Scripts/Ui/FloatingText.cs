using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI label;
    public CanvasGroup canvasGroup;

    [Header("Anim")]
    public float duration = 1.2f;

    public float riseDistance = 1.5f;
    public Ease riseEase = Ease.OutCubic;
    public float critPunch = 1.4f;
    public Color critColor = Color.yellow;

    private RectTransform rt;

    private void Awake()
    {
        rt = (RectTransform)transform;

        rt.pivot = rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);

        if (label) label.alignment = TextAlignmentOptions.Center;
    }

    public void Spawn(Vector3 worldPos, string text, Color color, bool isCrit = false)
    {
        transform.position = worldPos;

        if (label)
        {
            label.text = text;
            label.color = isCrit ? critColor : color;
        }

        if (canvasGroup) canvasGroup.alpha = 1f;

        transform.localScale = Vector3.one * (isCrit ? 1.3f : 1f);

        var seq = DOTween.Sequence();
        seq.Join(transform.DOMoveY(worldPos.y + riseDistance, duration).SetEase(riseEase));
        seq.Join(transform.DOPunchScale(Vector3.one * (isCrit ? critPunch - 1f : 0.2f),
            isCrit ? 0.35f : 0.25f, 8, 0.65f));
        if (canvasGroup) seq.Insert(0.3f, canvasGroup.DOFade(0f, duration - 0.3f));
        seq.OnComplete(() => Destroy(gameObject));
    }
}