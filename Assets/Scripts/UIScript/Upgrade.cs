using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.DOScale(1.3f, 0.2f).SetUpdate(true);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.DOScale(1, 0.2f).SetUpdate(true);
    }
}
