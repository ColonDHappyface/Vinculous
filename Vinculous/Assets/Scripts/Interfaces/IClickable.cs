using UnityEngine.EventSystems;

public interface IClickable
{
    /// <summary>
    /// This is called when the player taps the screen
    /// </summary>
    void OnClickBegin(PointerEventData _p);

    /// <summary>
    /// This is called when the player starts to drag, cancelling the click event
    /// </summary>
    void OnClickCancel(PointerEventData _p);

    /// <summary>
    /// This is called when the player starts to release the tap, without dragging on the screen
    /// </summary>
    void OnClickSuccessful(PointerEventData _p);
}
