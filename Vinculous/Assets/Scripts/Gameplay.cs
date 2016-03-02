using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Nyanulities;

public class Gameplay : MonoBehaviour, IDraggable
{
	// Editable-In-Inspector Fields
	[Header("Playing Node Properties")]
	[Tooltip("The prefab of the node")]
	[SerializeField] private Object m_NodePrefab;
	[Tooltip("The parent of the nodes")]
	[SerializeField] private Transform m_NodeParent;
	[Tooltip("The radius of the circle in which the nodes will move in")]
	[SerializeField] private float m_fRadius;

	// Uneditable-In-Inspector Fields
	private PushPopList<int[]> m_PlayerPreviousStep;    // m_lPlayerPreviousStep: The steps made by the player
	private int[] m_arrnCurrentStep;                    // m_arrnCurrentStep: The current array of all the elements in the current step
														//                    Note: The null value for the elements in the array will be -1
	private Node[] m_arrGONode;                    // m_arrGONode: The array of transform of the nodes

	private uint m_unCurrentStepLeft;   // m_unCurrentStep: The current step of the gameplay

	private Level m_Level;  // m_Level: The current level that this gameplay is reference to

	// Private Functions
	void Awake()
	{
		// Level Initialisation
		m_Level = null;
		m_Level = this.GetComponent<Level>();
		if (m_Level == null)
			Debug.LogWarning(this.name + ".m_Level: Initialisation returns null. Is Level.cs missing from the current gameObject?");

		// Variables Initialisation
		m_unCurrentStepLeft = m_Level.LevelCount;
		m_PlayerPreviousStep = new PushPopList<int[]>(m_Level.LevelCountToInt);
		m_arrnCurrentStep = new int[m_Level.LevelCountToInt + 1];
		ScreenInteraction.Instance.InitialiseDraggable(this);

		// Current-Step Array Initialisation
		for (int i = 0; i < m_arrnCurrentStep.Length; i++)
			m_arrnCurrentStep[i] = -1;
		m_arrnCurrentStep[0] = m_Level.StartToInt;

		// Node Initialisation
		m_arrGONode = new Node[m_arrnCurrentStep.Length];
		for (int i = 0; i < m_arrGONode.Length; i++)
		{
			m_arrGONode[i] = ((GameObject)Instantiate(m_NodePrefab, Camera.main.WorldToScreenPoint(new Vector3(0f, m_fRadius, 0f)), Quaternion.identity)).GetComponent<Node>();
			m_arrGONode[i].transform.parent = m_NodeParent;
			m_arrGONode[i].transform.localScale = Vector3.one;

			switch (m_arrnCurrentStep[i])
			{
				case -1: m_arrGONode[i].DisableNode(); Debug.Log(m_arrGONode[i].IsEnable); break;
				default: m_arrGONode[i].EnableNode(Level.Instance.StartToInt); break;
			}
		}
	}

	// Public Functions
	/// <summary>
	/// Recalculates all Node's target position
	/// </summary>
	public void RecalculateNodeTargetPosition()
	{
		int nCount = 0;
		for (int i = 0; i < m_arrGONode.Length; i++)
		{
			if (m_arrGONode[i].IsEnable)
				nCount++;
		}
		float nAngle = 360f / (float)nCount;
		nCount = 1;
		for (int i = 0; i < m_arrGONode.Length; i++)
			if (m_arrGONode[i].IsEnable)
			{
				m_arrGONode[i].TargetAngle = nAngle * nCount;
				nCount++;
			}
	}

	public void OnDragBegin(PointerEventData _pointerEventData)
	{
		return;
	}

	public void OnDrag(PointerEventData _pointerEventData)
	{
		Debug.Log(_pointerEventData.pointerCurrentRaycast.screenPosition + " of " + new Vector2(Screen.width, Screen.height));
	}

	public void OnDragExit(PointerEventData _pointerEventData)
	{
		return;
	}

	// Getter-Setter Functions
	/// <summary> Returns the level instance that this gameplay reference to </summary>
	public Level LevelInstance { get { return m_Level; } }
	public int NodeCount { get { return m_arrGONode.Length; } }
	public float NodeRadius { get { return m_fRadius; } }
}
