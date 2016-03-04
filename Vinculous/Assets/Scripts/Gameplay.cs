using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Nyanulities;

public class Gameplay : MonoBehaviour, IDraggable
{
	// Editable-In-Inspector Fields
	[Header("Node Properties")]
	[Tooltip("The prefab of the node")]
	[SerializeField] private Object m_NodePrefab;
	[Tooltip("The parent of the nodes")]
	[SerializeField] private Transform m_NodeParent;

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
		m_arrnCurrentStep[0] = m_Level.StartCountToInt;

		// Node Initialisation
		m_arrGONode = new Node[m_arrnCurrentStep.Length];
		for (int i = 0; i < m_arrGONode.Length; i++)
		{
			m_arrGONode[i] = ((GameObject)Instantiate(m_NodePrefab, Camera.main.WorldToScreenPoint(Vector3.zero), Quaternion.identity)).GetComponent<Node>();
			m_arrGONode[i].transform.SetParent(m_NodeParent);
			m_arrGONode[i].transform.localScale = Vector3.one;

			switch (m_arrnCurrentStep[i])
			{
				case -1: m_arrGONode[i].DisableNode(); break;
				default: m_arrGONode[i].EnableNode(Level.Instance.StartCountToInt); break;
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

    /// <summary>
    /// Split the current node into two
    /// </summary>
    /// <param name="_node"> The node to be split </param>
    public void SplitNode(Node _node)
    {
        Node newNode = null;
        // for: Find the next available node
        for (int i = 0; i < m_arrGONode.Length; i++)
            if (!m_arrGONode[i].IsEnable)
            {
                newNode = m_arrGONode[i];
                break;
            }

        // if: Error check - technically, user cannot and should not reach here
        if (newNode == null)
        {
            Debug.LogWarning(this.name + ".SplitNode(): There is no new node, is array too small?");
            return;
        }

        _node.SetDigit(Mathf.FloorToInt((float)_node.Digit / 2f));
        newNode.EnableNode(Mathf.CeilToInt((float)_node.Digit / 2f));
        RecalculateNodeTargetPosition();
    }

	// Getter-Setter Functions
	/// <summary> Returns the level instance that this gameplay reference to </summary>
	public Level LevelInstance { get { return m_Level; } }
	public int NodeCount { get { return m_arrGONode.Length; } }

    // IDraggable Definition
    public void OnDragBegin(PointerEventData _pointerEventData) { return; }

    public void OnDrag(PointerEventData _pointerEventData) 
    { 
        Debug.Log(_pointerEventData.hovered[0].gameObject); 
    }

    public void OnDragExit(PointerEventData _pointerEventData) { return; }
}
