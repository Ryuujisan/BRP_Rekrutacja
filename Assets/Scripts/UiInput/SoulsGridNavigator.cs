using System.Collections;
using System.Collections.Generic;
using UiInput;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulsGridNavigator : MonoBehaviour
{
    [Header("Refs")]
    public ScrollRect scrollRect;

    public GridLayoutGroup grid;
    public Selectable defaultFirst;

    [Header("Grid")]
    public int columns = 3;

    [Header("Scrolling")]
    public float scrollSpeed = 10f;

    public int waitFrames = 1;

    private GameObject _lastSel;
    private Coroutine _scrollCo;

    private void Update()
    {
        var es = EventSystem.current;
        if (!es) return;

        var cur = es.currentSelectedGameObject;
        if (cur == null || !cur.activeInHierarchy)
        {
            if (_lastSel && _lastSel.activeInHierarchy) es.SetSelectedGameObject(_lastSel);
            else FocusFirstAlive();
            cur = es.currentSelectedGameObject;
        }

        if (cur != _lastSel)
        {
            _lastSel = cur;
            if (_scrollCo != null) StopCoroutine(_scrollCo);
            _scrollCo = StartCoroutine(SmoothEnsureVisible(cur.GetComponent<RectTransform>()));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SetupNextFrame());
    }

    public void ReFocus()
    {
        StartCoroutine(SetupNextFrame());
    }

    private IEnumerator SetupNextFrame()
    {
        yield return null;
        RebuildNavigation();
        FocusFirstAlive();
    }

    public void RebuildNavigation()
    {
        var items = new List<Selectable>();
        foreach (var s in GetComponentsInChildren<NavButton>(true))
            if (s.IsActive() && s.interactable)
                items.Add(s);

        for (var i = 0; i < items.Count; i++)
        {
            var n = new Navigation { mode = Navigation.Mode.Explicit };
            int row = i / columns, col = i % columns;

            n.selectOnRight = FindNeighbor(items, row, col + 1, 0, +1);
            n.selectOnLeft = FindNeighbor(items, row, col - 1, 0, -1);
            n.selectOnDown = FindNeighbor(items, row + 1, col, +1, 0);
            n.selectOnUp = FindNeighbor(items, row - 1, col, -1, 0);

            items[i].navigation = n;
        }
    }

    private Selectable FindNeighbor(List<Selectable> items, int r, int c, int dr, int dc)
    {
        var rows = Mathf.CeilToInt(items.Count / (float)columns);
        while (r >= 0 && r < rows && c >= 0 && c < columns)
        {
            var idx = r * columns + c;
            if (idx >= 0 && idx < items.Count) return items[idx];
            r += dr;
            c += dc;
        }

        return null;
    }

    private void FocusFirstAlive()
    {
        var es = EventSystem.current;
        if (!es) return;

        Selectable first = null;
        if (defaultFirst && defaultFirst.IsActive() && defaultFirst.interactable) first = defaultFirst;
        else
            foreach (var s in GetComponentsInChildren<Selectable>(true))
                if (s.IsActive() && s.interactable)
                {
                    first = s;
                    break;
                }

        if (first)
        {
            es.SetSelectedGameObject(null);
            first.Select();
            es.SetSelectedGameObject(first.gameObject);
            _lastSel = first.gameObject;
            if (_scrollCo != null) StopCoroutine(_scrollCo);
            _scrollCo = StartCoroutine(SmoothEnsureVisible(first.GetComponent<RectTransform>()));
        }
    }

    private IEnumerator SmoothEnsureVisible(RectTransform item)
    {
        if (!scrollRect || !item) yield break;

        yield return null;

        var content = scrollRect.content;
        var viewport = scrollRect.viewport ? scrollRect.viewport : (RectTransform)scrollRect.transform;

        var itemB = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, item);
        var viewB = viewport.rect;


        var overTop = itemB.max.y - viewB.yMax;
        var overBottom = viewB.yMin - itemB.min.y;


        if (overTop <= 0f && overBottom <= 0f) yield break;


        var contentH = content.rect.height;
        var viewH = viewport.rect.height;
        var range = Mathf.Max(1e-3f, contentH - viewH);

        var cur = scrollRect.verticalNormalizedPosition;
        var target = cur;

        if (overTop > 0f)
            target = Mathf.Clamp01(cur + overTop / range);

        if (overBottom > 0f)
            target = Mathf.Clamp01(cur - overBottom / range);

        scrollRect.velocity = Vector2.zero;

        float t = 0f, speed = 10f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * speed;
            scrollRect.verticalNormalizedPosition = Mathf.SmoothStep(cur, target, t);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = target;
    }
}