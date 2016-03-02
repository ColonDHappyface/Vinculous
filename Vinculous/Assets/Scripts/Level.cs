using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MergeSplitAlgorithm;

// Level.cs: Handles an instance of a level
public class Level : MonoBehaviour 
{
	// Editable-In-Inspector Fields
	[Header("Sequence References")]
	[Tooltip("The reference to the level's introduction sequence")]
	[SerializeField] private GameObject m_GOSequenceIntro;

	[Header("Level Properties")]
	[Tooltip("The level of the current level")][Range(3.0f, 8.0f)]
	[SerializeField] private uint m_unLevel = 3;
	[Tooltip("The sub-level of the current level")]
	[SerializeField] private uint m_unSubLevel = 1;

	[Header("Generator Properties")]
	[Tooltip("The intensity of the lower limit. The curve-ness of the the limit")]
	[SerializeField] private float m_fLowLimitIntensity = 100f;
	[Tooltip("The intensity of the upper limit. The curve-ness of the the limit")]
	[SerializeField] private float m_fUpLimitIntensity = 50f;
	[Tooltip("The tolerance between the lower limit and the upper limit. This is to prevent the lower limit to be the same graph as the upper limit")]
	// Note: This is to change the ending point of the lower limit, but both limits can have the same curve-ness
	// Another Note: If both limits is the same and the tolerance is set to 1, both the curves will be the same
	[SerializeField] private float m_fTolerance = 1.3f;
	[Tooltip("The minimum multiplier of the graph. For when the value is 0.0f")]
	[SerializeField] private float m_fMinimumMultiplier = 10f;
	[Tooltip("The maximum multiplier of the graph. For when the value is 1.0f")]
	[SerializeField] private float m_fMaximumMultiplier = 1000f;

	// Uneditable-In-Inspector Fields
	private MergeSplit_Algorithm m_Algorithm;   // m_Algorithm: The reference to the algorithm

	private uint m_unStart;     // m_unStart: The starting number of the current level
	private uint m_unTarget;    // m_unTarget: The target number of the current level

	private Text[] m_arrGOTextObject;   // m_arrGOTextObject: The array of Text type from m_GOSequenceIntro
	private float m_fCurrentTime;       // m_fCurrentTime: The current time of the current animation / pause
	private float m_fCurrentTotalTime;  // m_fCurrentTotalTime: The current total time of the current animation / pause
	private int m_nCurrentStep;         // m_nCurrentStep: The current step of the text array

	private Gameplay m_Gameplay;        // m_Gameplay: The gameplay instance that is linked to this level

	// Static Fields
	private static Level m_Instance = null; // m_Instance: The only instance of the current level

	// Private Functions
	// Awake(): is called at the start of the scene
	void Awake()
	{
		// Gameplay Initialisation
		m_Gameplay = null;
		m_Gameplay = this.GetComponent<Gameplay>();
		if (m_Gameplay == null)
			Debug.LogWarning(this.name + ".m_Gameplay: Initialisation returns null. Is Gameplay.cs missing from the current gameObject?");

		// Singleton
		if (m_Instance == null)
			m_Instance = this;
		else
			Destroy(this);

		// Intro-Sequence Initialisation Check
		if (m_GOSequenceIntro == null)
			Debug.LogWarning(name + ".m_GOSequenceIntro: is not assigned. Please assigned the appropriate intro sequence");

		// Calculations for the level
		// fLowerLimit: fUpperLimit: Calculate the lower and upper limit of the current sub level in terms of 0.0f to 1.0f
		float fLowerLimit = (-1.0f / Mathf.Pow(((float)m_unSubLevel + m_fLowLimitIntensity) / m_fLowLimitIntensity, 2.0f) + 1.0f) / m_fTolerance;
		float fUpperLimit = -1.0f / Mathf.Pow(((float)m_unSubLevel + m_fUpLimitIntensity) / m_fUpLimitIntensity, 2.0f) + 1.0f;

		// Calulates the starting number based on factors
		m_unStart = (uint)((float)(m_unLevel - 2) * (m_fMinimumMultiplier + (fLowerLimit + UnityEngine.Random.value * (fUpperLimit - fLowerLimit)) * (m_fMaximumMultiplier - m_fMinimumMultiplier)));

		m_Algorithm = new MergeSplit_Algorithm((int)m_unStart, (int)m_unLevel);
		m_Algorithm.Execute();
		int[] arrTarget = m_Algorithm.GetFinal();
		m_unTarget = (uint)arrTarget[(int)(arrTarget.Length - 1 * UnityEngine.Random.value)];
		m_Algorithm = null;

		// Variable Initialisation
		m_fCurrentTime = 0f;
		m_fCurrentTotalTime = 0;
		m_nCurrentStep = -1;
		int nObjectCount = m_GOSequenceIntro.transform.childCount;
		m_arrGOTextObject = new Text[nObjectCount];
		for (int i = 0; i < nObjectCount; i++)
		{
			m_arrGOTextObject[i] = m_GOSequenceIntro.transform.GetChild(i).GetComponent<Text>();
			m_arrGOTextObject[i].gameObject.SetActive(false);
		}

		Debug.Log("Execute Level " + m_unLevel + "-" + m_unSubLevel + ", Path " + m_unStart + " -> " + m_unTarget);

		InitIntro();
	}
	
	// Update is called once per frame
	void Update() 
	{
		RunIntro();
	}

	// InitIntro(): Initialises the intro sequence of the current level
	private void InitIntro()
	{
		// for: Every element in the text array, initialise where appropreiate
		for (int i = 0; i < m_arrGOTextObject.Length; i++)
		{
			switch (i)
			{
				case 0: m_arrGOTextObject[i].text = "Level " + m_unLevel + "-" + m_unSubLevel; break;
				case 1: m_arrGOTextObject[i].text = "In " + m_unLevel + " steps..."; break;
				case 3: m_arrGOTextObject[i].text = "" + m_unStart; break;
				case 5: m_arrGOTextObject[i].text = m_unTarget.ToString(); break;
				default: break;
			}
		}
	}

	// RunIntro(): Execute the intro sequence of the current level
	private void RunIntro()
	{
		if (m_nCurrentStep > m_arrGOTextObject.Length) return;

		// switch: Initialisation of current step
		switch (m_nCurrentStep)
		{
			case 0:
			case 3:
			case 4:
			case 5:
				// if: The current time is 0, this is to determine that the current step is not initialised (Same for the case below)
				if (m_fCurrentTime == 0f)
					m_fCurrentTotalTime = 1f; 
				break;
			case 1:
			case 2:
			case 6:
				if (m_fCurrentTime == 0f)
					m_fCurrentTotalTime = 2f;
				break;
			default: break;
		}

		if (m_fCurrentTime == 0f && m_nCurrentStep < m_arrGOTextObject.Length && m_nCurrentStep >= 0)
			m_arrGOTextObject[m_nCurrentStep].gameObject.SetActive(true);

		m_fCurrentTime += Time.deltaTime;

		if (m_fCurrentTime > m_fCurrentTotalTime)
			m_fCurrentTime = 0f;

		if (m_fCurrentTime == 0f)
			m_nCurrentStep++;
	}

	// Getter-Setter Functions
	public static Level Instance { get { return m_Instance; } }

	/// <summary> Returns the level of the current level </summary>
	public uint LevelCount { get { return m_unLevel; } }
	/// <summary> Returns the level of the current level in type integer </summary>
	public int LevelCountToInt { get { return (int)m_unLevel; } }
	/// <summary> Returns the sub-level of the current level </summary>
	public uint SubLevel { get { return m_unSubLevel; } }
	/// <summary> Returns the sub-level of the current level in type integer </summary>
	public int SubLevelToInt { get { return (int)m_unSubLevel; } }
	/// <summary> Returns the starting number of the current level </summary>
	public uint Start { get { return m_unStart; } }
	/// <summary> Returns the starting number of the current level in type integer </summary>
	public int StartToInt {get { return (int)m_unStart; } }
	/// <summary> Returns the target number of the current level </summary>
	public uint Target { get { return m_unTarget; } }
	/// <summary> Returns the target number of the current level in type integer </summary>
	public int TargetToInt { get { return (int)m_unTarget; } }

	/// <summary> Returns the instance of the gameplay </summary>
	public Gameplay GameplayInstance { get { return m_Gameplay; } }
}
