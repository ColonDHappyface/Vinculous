using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// ScreenInteraction.cs: Handles the interactions on the screen
public class ScreenInteraction : MonoBehaviour 
{
	// Uneditable-In-Inspector Fields
	private List<IDraggable> m_listIDraggables;     // m_listIDraggables: A list of all the IDraggable type

	// Static Fields
	private static ScreenInteraction s_Instance = null;

	// Private Functions
	// Awake(): is called when the gameObject is enabled
	void Awake()
	{
		// Singleton
		if (s_Instance == null)
			s_Instance = this;
		else
			Destroy(this);

		// Variables Initialisation
		m_listIDraggables = new List<IDraggable>();
	}

	// Public Functions
	/// <summary>
	/// Initialises an IDraggable type. This is so that the functions in IDraggable type can be called
	/// </summary>
	/// <param name="_iDraggable"> T</param>
	public void InitialiseDraggable(IDraggable _iDraggable)
	{
		m_listIDraggables.Add(_iDraggable);
	}

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnBeginDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDragBegin(pointerEventData);
	}

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDrag(pointerEventData);
	}

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnExitDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDragExit(pointerEventData);
	}

	// Getter-Setter Functions
	public static ScreenInteraction Instance { get { return s_Instance; } }
}
