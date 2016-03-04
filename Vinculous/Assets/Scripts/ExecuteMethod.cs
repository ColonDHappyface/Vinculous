using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ExecuteableMethod(): The only executable method type to work with ExecuteMethod()
public delegate void ExecutableMethod();

// ExecuteMethod.cs: Execution of a method in a certain way
public class ExecuteMethod : MonoBehaviour 
{
	// Static Fields
	private static List<ExecutableMethod> m_listEMUpdate;
	private static List<ExecutableMethod> m_listEMFixedUpdate;

	// Private Methods
	// Awake(): is called at the start of the program
	void Awake () 
	{
		m_listEMUpdate = new List<ExecutableMethod>();
		m_listEMFixedUpdate = new List<ExecutableMethod>();
	}
	
	// Update(): is called once per frame
	void Update () 
	{
		// while: Execute all the methods in the current update steps
		while (m_listEMUpdate.Count > 0)
		{
			m_listEMUpdate[0]();
			m_listEMUpdate.RemoveAt(0);
		}
	}

    // FixedUpdate(): is called 20 times every second
	void FixedUpdate()
	{
        // while: Execute all the methods in the current fixed-update step
		while (m_listEMFixedUpdate.Count > 0)
		{
			m_listEMFixedUpdate[0]();
			m_listEMFixedUpdate.RemoveAt(0);
		}
	}

	// Public Static Functions
	/// <summary>
	/// Pass in the current method so that it will be executed only once in an update step
	/// </summary>
	/// <param name="_executableMethod"> The method to be executed </param>
	/// <returns> Returns true if the method passed in is a complete new method, or false if it already exists in the list </returns>
	public static bool OnceInUpdate(ExecutableMethod _executableMethod)
	{
		// for: Checks for existing method in the list
		for (int i = 0; i < m_listEMUpdate.Count; i++)
			if (m_listEMUpdate[i].Equals(_executableMethod))
				return false;

		m_listEMUpdate.Add(_executableMethod);
		return true;
	}

	/// <summary>
	/// Pass in the current method so that it will be executed only once in an fixed-update step
	/// </summary>
	/// <param name="_executableMethod"> The method to be executed </param>
	/// <returns> Returns true if the method passed in is a complete new method, or false if it already exists in the list </returns>
	public static bool OnceInFixedUpdate(ExecutableMethod _executableMethod)
	{
		// for: Checks for exeisting method in the lists
		for (int i = 0; i < m_listEMFixedUpdate.Count; i++)
			if (m_listEMFixedUpdate[i].Equals(_executableMethod))
				return false;

		m_listEMFixedUpdate.Add(_executableMethod);
		return true;
	}
}
