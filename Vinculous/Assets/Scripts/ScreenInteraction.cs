using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// ScreenInteraction.cs: Handles the interactions on the screen
public class ScreenInteraction : MonoBehaviour 
{
    // Editable-In-Inspector Fields
    [Header("Screen Interaction")]
    [Tooltip("The barrier to stop (most) interaction on the screen")]
    [SerializeField] private GameObject m_GOScreen;    // m_GOScreen: The object that controls the interaction

	// Uneditable-In-Inspector Fields
	private List<IDraggable> m_listIDraggables;     // m_listIDraggables: A list of all the IDraggable type
    private List<IClickable> m_listIClickables;     // m_listIClickables: A list of all the IClickable type

    private bool isClickResponded = true;          // isClickResponded: Returns if a certain click is responded

    private CanvasGroup m_CGBarrier;

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
        m_listIClickables = new List<IClickable>();
	}

	// Public Functions
	/// <summary>
	/// Initialises an IDraggable type. This is so that the functions in IDraggable type can be called
	/// </summary>
	/// <param name="_iDraggable"> The IDraggable type that is going to be added to the list </param>
	public void InitialiseDraggable(IDraggable _iDraggable)
	{
		m_listIDraggables.Add(_iDraggable);
	}

    /// <summary>
    /// Initialises an IClickable type. This is so that the functions in IDraggable type can be called
    /// </summary>
    /// <param name="_iDraggable"> The IClickable type that is going to be added to the list </param>
    public void InitialiseClickable(IClickable _iClickable)
    {
        m_listIClickables.Add(_iClickable);
    }

    /// <summary>
    /// Determine if the screen is interactable
    /// </summary>
    /// <param name="_bIsActive"> Set if the screen is interactable by the player </param>
    public void Screen(bool _bIsActive)
    {
        m_GOScreen.SetActive(_bIsActive);
    }

    public void OnBeginClick(BaseEventData _baseEventData)
    {
        PointerEventData pointerEventData = (PointerEventData)_baseEventData;

        // for: Runs all OnClickBegin() in type IClickable
        for (int i = 0; i < m_listIClickables.Count; i++)
            m_listIClickables[i].OnClickBegin(pointerEventData);

        isClickResponded = false;
    }

    public void OnExitClick(BaseEventData _baseEventData)
    {
        PointerEventData pointerEventData = (PointerEventData)_baseEventData;

        if (!isClickResponded)
        {
            for (int i = 0; i < m_listIClickables.Count; i++)
                m_listIClickables[i].OnClickSuccessful(pointerEventData);
            isClickResponded = true;
        }
    }

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnBeginDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;

        // for: Runs all OnDragBegin() in type IDraggable
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDragBegin(pointerEventData);

        if (!isClickResponded)
        {
            for (int i = 0; i < m_listIClickables.Count; i++)
                m_listIClickables[i].OnClickCancel(pointerEventData);
            isClickResponded = true;
        }
	}

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;

        // for: Runs all OnDrag() in type IDraggable
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDrag(pointerEventData);
	}

	/// <summary>
	/// Called by the event trigger. It will provide no use if called by the user
	/// </summary>
	public void OnExitDrag(BaseEventData _baseEventData)
	{
		PointerEventData pointerEventData = (PointerEventData)_baseEventData;

        // for: Runs all OnDragExit() in type IDraggable
		for (int i = 0; i < m_listIDraggables.Count; i++)
			m_listIDraggables[i].OnDragExit(pointerEventData);
	}

	// Getter-Setter Functions
	public static ScreenInteraction Instance { get { return s_Instance; } }
}
