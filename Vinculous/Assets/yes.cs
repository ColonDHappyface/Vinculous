using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class yes : MonoBehaviour 
{
    public void OnDragBegin(BaseEventData _Data)
    {
        PointerEventData pointerData = _Data as PointerEventData;
        //Debug.Log(pointerData.pointerCurrentRaycast.screenPosition);

        pointerData.pointerCurrentRaycast.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnDrag(BaseEventData _Data)
    {
        PointerEventData pointerData = _Data as PointerEventData;
        Debug.Log(Camera.main.ScreenToWorldPoint(pointerData.pointerCurrentRaycast.screenPosition));
    }

    public void OnDragExit(BaseEventData _Data)
    {
        PointerEventData pointerData = _Data as PointerEventData;
        //Debug.Log(pointerData.pointerCurrentRaycast.screenPosition);

        pointerData.pointerPressRaycast.gameObject.GetComponent<Image>().color = Color.black;
        Debug.Log(pointerData.position);
    }
}
