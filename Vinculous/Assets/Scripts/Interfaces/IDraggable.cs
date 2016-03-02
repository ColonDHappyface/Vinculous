using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface IDraggable 
{
    /// <summary>
    /// This is called when the player starts dragging
    /// </summary>
    void OnDragBegin(PointerEventData _p);

    /// <summary>
    /// This is called when the is dragging
    /// </summary>
    void OnDrag(PointerEventData _p);

    /// <summary>
    /// This is called when the player stops dragging
    /// </summary>
    void OnDragExit(PointerEventData _p);
}
