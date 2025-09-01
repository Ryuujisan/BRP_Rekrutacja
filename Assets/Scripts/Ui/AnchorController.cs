using System;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : MonoBehaviour
{
    public List<AnchorPosition> Anchors;

    private void Awake()
    {
        SetupObjectsPosition();
    }

    private void SetupObjectsPosition()
    {
        if (Anchors == null)
            return;

        for (var i = 0; i < Anchors.Count; i++)
        {
            Anchors[i].SetObjectPosition();
            Destroy(Anchors[i].Anchor.gameObject);
        }
    }
}

[Serializable]
public class AnchorPosition
{
    public RectTransform Anchor;
    public GameObject AnchoredObject;
    public bool IsActiveAtStart;

    public void SetObjectPosition()
    {
        AnchoredObject.SetActive(IsActiveAtStart);

        AnchoredObject.transform.position = Anchor.position;
    }
}