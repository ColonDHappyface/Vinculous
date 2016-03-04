using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Node : MonoBehaviour
{
    // Editable-In-Inspector Fields
    [Header("Alignment Properties")]
    [Tooltip("The radius of all the nodes from the center")]
    [SerializeField] private float m_fRadius = 1.5f;
    [Tooltip("The speed multiplier for all nodes")]
    [SerializeField] private float m_fSpeed = 2f;

    // Uneditable-In-Inspector Fields
    private Text m_Text;                // m_Text: The text display of the current node
    private CanvasGroup m_CanvasGroup;  // m_CanvasGroup: The canvas group of the current node

    private int m_nDigit = 0;
    private float m_fCurrentAngle;
    private float m_fTargetAngle;
    private bool m_bIsEnabled = false;

    // Private Functions
    void Awake()
    {
        m_Text = transform.GetChild(0).GetComponent<Text>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_fCurrentAngle = 0;
        m_fTargetAngle = 0;
    }

    void Update()
    {
        float fDeltaAngle = Mathf.DeltaAngle(m_fCurrentAngle, m_fTargetAngle);

        if (Input.GetKeyDown(KeyCode.A) && m_bIsEnabled)
        {
            m_fCurrentAngle = UnityEngine.Random.value * 360f;
        }

        if (Mathf.Abs(fDeltaAngle) > 0.01f)
        {
            float fNewCurrentAngle = fDeltaAngle * Time.deltaTime;
            m_fCurrentAngle += fNewCurrentAngle;
            this.WorldPosition = Quaternion.Euler(0f, 0f, m_fCurrentAngle) * new Vector3(0f, m_fRadius, 0f);
        }
    }

    // Public Functions
    /// <summary>
    /// Re-enables the current node
    /// </summary>
    /// <param name="_nDigit"> The new digit of the node </param>
    public void EnableNode(int _nDigit)
    {
        m_CanvasGroup.alpha = 1f;
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
        m_nDigit = _nDigit;
        this.SetDigit(m_nDigit);
        m_bIsEnabled = true;
    }

    /// <summary>
    /// Disable the current node
    /// </summary>
    public void DisableNode()
    {
        m_CanvasGroup.alpha = 0f;
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
        m_bIsEnabled = false;
    }

    /// <summary>
    /// Set the digit of the current node
    /// </summary>
    /// <param name="_nDigit"> The digit to be used in the node </param>
    public void SetDigit(int _nDigit)
    {
        m_nDigit = _nDigit;
        m_Text.text = "" + m_nDigit;
    }

    // Getter-Setter Functions
    public Vector3 WorldPosition 
    { 
        get { return transform.position; }
        set { transform.position = Camera.main.WorldToScreenPoint(value); } 
    }

    public float Angle { get { return m_fCurrentAngle; } }
    public float TargetAngle { get { return m_fCurrentAngle; } set { m_fCurrentAngle = value; } }

    public int Digit { get { return m_nDigit; } }
    public bool IsEnable { get { return m_bIsEnabled; } }
}
