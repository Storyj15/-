using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerJobButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject JobText;
    private void Start()
    {
        JobText.SetActive(false);
    }
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        JobText.SetActive(true);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        JobText.SetActive(false);
    }
}
