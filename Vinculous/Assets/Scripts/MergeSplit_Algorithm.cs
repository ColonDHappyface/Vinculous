using System;
using System.Collections.Generic;

namespace MergeSplitAlgorithm
{
    public enum MergeSplitType { Null, Merge, Split }

    /// <summary>
    /// Stores the current permutation of the algorithm
    /// </summary>
    struct MergeSplit_Node
    {
        /*  Formula to calculate the permutation of the next step from the current number of elements
         *  Let n be -> The number of elements in the current node
         *  
         *  Let s be -> The amount of splits it can perform in the current node
         *  Let m be -> The amount of (unique) merges it can perform in the current node
         *              Note: "Unique" means that a+b is the same as b+a and will NOT be cosidered as two different permutations
         *                    a.k.a NOT order-specific
         *                    However, "Unique" does NOT take into consideration that a+b and a+c where b=c to be "Unique"
         *                    
         *  Let t be -> The total amount of splits and merge it can perform, a.k.a m + s
         *  
         *  Formula:
         *  -> m + s = t
         *  -> where m = (n^2 - n) / 2
         *  -> where s = n
         *  -> therefore t = ((n^2 - 2) / 2) + n
         *                 = (n^2 + 2) / 2
         */

        // Structure Scope Variables - Definition
        private int m_nGeneration;                  // m_nGeneration: The current generation of the node (How deep is this node from the starting number)
        private MergeSplitType m_enumType;          // m_enumType: Determines the how the current node is derived from
        private int m_nCurrentNode;                 // m_nCurrentNode: The current index in the node array that is empty

        private int[] m_arrnElements;               // m_arrnElements: The array to store all the elements
        private MergeSplit_Node[] m_arrNodes;       // m_arrNodes: The array that stores all the nodes

        private MergeSplit_Algorithm m_Algorithm;    // m_Algorithm: A reference to the main algorithm

        /// <summary>
        /// Creates a new Merge-Split Node
        /// </summary>
        /// <param name="_nGeneration"> The current generation of the node </param>
        /// <param name="_arrnElements"> The elements of the current node </param>
        /// <param name="_enumType"> The enum type of the current node </param>
        /// <param name="_Algorithm"> The reference of the algorithm </param>
        public MergeSplit_Node(int _nGeneration, int[] _arrnElements, MergeSplitType _enumType, MergeSplit_Algorithm _Algorithm)
        {
            m_nGeneration = _nGeneration;
            m_enumType = _enumType;
            m_arrnElements = _arrnElements;
            m_Algorithm = _Algorithm;
            m_nCurrentNode = 0;

            m_arrNodes = new MergeSplit_Node[((int)Math.Pow(m_arrnElements.Length, 2.0f) + m_arrnElements.Length) / 2];

            // One Checker - Checks for number of elements with a 1.
            //               These elements will not be able to split further and will be removed from the Node array
            int nOneCount = 0;
            for (int i = 0; i < m_arrnElements.Length; i++)
            {
                if (m_arrnElements[i] == 1)
                    nOneCount++;
            }
            m_arrNodes = new MergeSplit_Node[m_arrNodes.Length - nOneCount];

            // Calculates its permutations - Recursion occurs here
            this.ComputeSplit();
            this.ComputeMerge();

            m_Algorithm.Permutations++;
        }
        
        #region Private Functions
        // ComputeSplit(): Calculates all the splitting possibilities
        private void ComputeSplit()
        {
            // if: The current generation of the current node is the final generation, stops creating permutations
            if (m_nGeneration >= m_Algorithm.Step)
                return;

            // for: Every element in the current node...
            for (int i = 0; i < m_arrnElements.Length; i++)
            {
                // if: The current element is not "1", since "1" cannot be split
                if (m_arrnElements[i] != 1)
                {
                    // fSplit: The result of the split, used float is to remain the floating point value
                    float fSplit = (float)m_arrnElements[i] / 2.0f;

                    // arrnNewElements: The new array in which is going to be used in the next node
                    int[] arrnNewElements = new int[m_arrnElements.Length + 1];

                    // for: Every element in the current node, push all the elements into the new array
                    for (int j = 0; j < m_arrnElements.Length; j++)
                    {
                        // if: Pushes all the elements that is NOT splitting into the new array
                        if (i != j)
                            arrnNewElements[j] = m_arrnElements[j];
                        else
                        {
                            // Place the first split into the array
                            arrnNewElements[j] = (int)Math.Floor(fSplit);
                        }
                    }

                    // Place the second split into the final element of the array
                    arrnNewElements[m_arrnElements.Length] = (int)Math.Ceiling(fSplit);

                    // Create new node
                    m_arrNodes[m_nCurrentNode] = new MergeSplit_Node(m_nGeneration + 1, arrnNewElements, MergeSplitType.Split, m_Algorithm);
                    m_nCurrentNode++;
                }
            }
        }

        // ComputeMerge(): Calculates all the merging possibilities
        private void ComputeMerge()
        {
            // if: The current generation of the current node is the final generation, stops creating permutations
            if (m_nGeneration >= m_Algorithm.Step)
                return;

            // if: There is only one element in the array
            if (m_arrnElements.Length < 1)
                return;

            // for: Every element in the the current node...
            for (int i = 0; i < m_arrnElements.Length - 1; i++)
            {
                // for: Every next element accessible...
                for (int j = i + 1; j < m_arrnElements.Length; j++)
                {
                    int nResult = m_arrnElements[i] + m_arrnElements[j];

                    // if: The element a and element b DOES NOT adds up to the start number. Remove this loop
                    if (nResult != m_Algorithm.Start)
                    {
                        int[] arrNewElements = new int[m_arrnElements.Length - 1];

                        // for: Push all of the existing elements into a new array for the new node
                        for (int k = 0; k < arrNewElements.Length; k++)
                        {
                            // if: The current element is the first merged number, fill in with the result
                            if (k == i)
                                arrNewElements[k] = nResult;
                            // else if: The current element is the second merged number, fill in with the last element
                            else if (k == j)
                                arrNewElements[k] = m_arrnElements[arrNewElements.Length];
                            else
                                arrNewElements[k] = m_arrnElements[k];
                        }

                        m_arrNodes[m_nCurrentNode] = new MergeSplit_Node(m_nGeneration + 1, arrNewElements, MergeSplitType.Merge, m_Algorithm);
                        m_nCurrentNode++;
                    }
                }
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Returns a nicely formatted string of the hierachy of the current node. Note: This is very intensive
        /// </summary>
        /// <returns></returns>
        public string GetHierachyInString()
        {
            string strReturn = "";
            // if: It is the first element of the algorithm
            if (m_nGeneration == 0)
                strReturn += "Step 0: { " + m_arrnElements[0] + " }\n";
            else
            {
                //for: Used for Indentations
                for (int i = 0; i < m_nGeneration; i++)
                    strReturn += "|   ";

                strReturn += "Step " + m_nGeneration + ": ";

                switch (m_enumType)
                {
                    case MergeSplitType.Merge: strReturn += "<M> { "; break;
                    case MergeSplitType.Split: strReturn += "<S> { "; break;
                    default: strReturn += "<MergeSplitType: Reached default> { "; break;
                }

                // for: Display all values of the current node
                for (int i = 0; i < m_arrnElements.Length; i++)
                {
                    if (i != 0)
                        strReturn += ", ";
                    strReturn += m_arrnElements[i];
                }

                strReturn += " }\n";
            }

            // for: All the nodes that is linked to this node...
            for (int i = 0; i < m_arrNodes.Length; i++)
            {
                if (m_arrNodes[i].m_enumType != MergeSplitType.Null)
                {
                    strReturn += m_arrNodes[i].GetHierachyInString();
                }
            }

            return strReturn;
        }

        // GetAllElementsInHierachyInBoolArray(): Calculates which element is in the algorithm using recursion
        public bool[] GetAllElementsInHierachyInBoolArray(bool[] _arrbArray)
        {
            if (m_arrNodes != null)
            {
                for (int i = 0; i < m_arrNodes.Length; i++)
                    _arrbArray = m_arrNodes[i].GetAllElementsInHierachyInBoolArray(_arrbArray);

                for (int i = 0; i < m_arrnElements.Length; i++)
                {
                    if (m_arrnElements[i] == m_Algorithm.Start)
                        _arrbArray[0] = true;
                    else
                        _arrbArray[m_arrnElements[i]] = true;
                }
            }

            return _arrbArray;
        }

        // GetAllNonFinalElementsInHierachyInBoolArray(): Calculates which element is not in the finals using recursion
        public bool[] GetAllNonFinalElementsInHierachyInBoolArray(bool[] _arrbArray)
        {
            if (m_arrNodes != null)
            {
                for (int i = 0; i < m_arrNodes.Length; i++)
                    _arrbArray = m_arrNodes[i].GetAllNonFinalElementsInHierachyInBoolArray(_arrbArray);

                if (m_nGeneration < m_Algorithm.Step)
                    for (int i = 0; i < m_arrnElements.Length; i++)
                    {
                        if (m_arrnElements[i] == m_Algorithm.Start)
                            _arrbArray[0] = true;
                        else
                            _arrbArray[m_arrnElements[i]] = true;
                    }
            }

            return _arrbArray;
        }
        #endregion
    }


    /// <summary>
    /// Generates a Merge-Split algorithm
    /// </summary>
    class MergeSplit_Algorithm
    {
        // Class Scope Variables - Definition
        private int m_nStart;                           // m_nStart: The starting number of the algorithm
        private int m_nStep;                            // m_nStep: The number of steps the algorithm will calculate
        private Nullable<MergeSplit_Node> m_StartNode;  // m_StartNode: The starting node of the algorithm
        private int m_nPermutations;                    // m_nPermutations: The number of instances of elements it runs throughout the algorithm

        private bool[] m_arrbElementInAlgorithm;    // m_arrbElementInAlgorithm: Stores if the current element is reachable in the algorithm
        private bool[] m_arrbElementInFinal;        // m_arrbElementInFinal: Stores if the current element is reachable in the LAST generation of the algorithm

        // Constructor
        /// <summary>
        /// Generates a Merge-Split Algorithm
        /// </summary>
        /// <param name="_nStart"> The starting number of the algorithm </param>
        /// <param name="_nStep"> The steps it can take before reaching the ending number </param>
        public MergeSplit_Algorithm(int _nStart, int _nStep)
        {
            m_nStart = _nStart;
            m_nStep = _nStep;
            m_StartNode = null;
            m_nPermutations = 0;

            m_arrbElementInAlgorithm = null;
            m_arrbElementInFinal = null;
        }

        #region Private Functions
        private void UpdateAllElementsArray()
        {
            m_arrbElementInAlgorithm = new bool[m_nStart];
            for (int i = 0; i < m_arrbElementInAlgorithm.Length; i++)
                m_arrbElementInAlgorithm[i] = false;

            m_arrbElementInAlgorithm = m_StartNode.Value.GetAllElementsInHierachyInBoolArray(m_arrbElementInAlgorithm);
        }

        private void UpdateAllFinalArray()
        {
            // if: The boolean array is not checked yet
            if (m_arrbElementInAlgorithm == null)
            {
                this.UpdateAllElementsArray();
            }

            // Reset "Final" array
            m_arrbElementInFinal = new bool[m_nStart];
            for (int i = 0; i < m_arrbElementInFinal.Length; i++)
                m_arrbElementInFinal[i] = false;

            // Create "Non-Final" array
            bool[] arrbElementNotInFinal = new bool[m_nStart];
            for (int i = 0; i < arrbElementNotInFinal.Length; i++)
                arrbElementNotInFinal[i] = false;

            arrbElementNotInFinal = m_StartNode.Value.GetAllNonFinalElementsInHierachyInBoolArray(arrbElementNotInFinal);

            // for: Compares every element between "All-element" array and "All Not Final Element" array
            for (int i = 0; i < m_nStart; i++)
            {
                // if: The current index does not exist in the "non-final" array but in the "all element" array
                if (!arrbElementNotInFinal[i] && m_arrbElementInAlgorithm[i])
                    m_arrbElementInFinal[i] = true;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Execute the algorithm
        /// </summary>
        public void Execute()
        {
            m_StartNode = new MergeSplit_Node(0, new int[1]{ m_nStart }, MergeSplitType.Null, this);

            // The starting element is already registered, do not need to register
            //this.SetElementInAlgorithm(0);
        }

        /// <summary>
        /// Returns a nicely printed string of all the numbers that exists in the algorithm
        /// </summary>
        /// <returns></returns>
        public string GetAllElementsInString()
        {
            // if: The boolean array is not checked yet
            if (m_arrbElementInAlgorithm == null)
            {
                this.UpdateAllElementsArray();
            }

            // String Printing
            string strReturn = "{ ";
            for (int i = 1; i < m_arrbElementInAlgorithm.Length; i++)
                if (m_arrbElementInAlgorithm[i])
                {
                    if (strReturn == "{ ")
                        strReturn += i;
                    else
                        strReturn += ", " + i;
                }
            strReturn += ", " + m_nStart + " }";
            return strReturn;
        }

        /// <summary>
        /// Returns a nicely printed string of all the numbers that exists ONLY in the final generation
        /// </summary>
        /// <returns></returns>
        public string GetAllFinalElementsInString()
        {
            // if: The boolean array is not checked yet
            if (m_arrbElementInFinal == null)
            {
                this.UpdateAllFinalArray();
            }

            // String Printing
            string strReturn = "{ ";
            for (int i = 1; i < m_arrbElementInFinal.Length; i++)
                if (m_arrbElementInFinal[i])
                {
                    if (strReturn == "{ ")
                        strReturn += i;
                    else
                        strReturn += ", " + i;
                }
            strReturn += " }";
            return strReturn;
        }

        /// <summary>
        /// Get the entire hierachy of the algorithm in a nicely formatted string. Note: This can be intensive
        /// </summary>
        /// <returns> Returns a nicely formatted string of the entire hierachy of the algorithm </returns>
        public string GetHierachyInString()
        {
            return m_StartNode.Value.GetHierachyInString();
        }

        /// <summary>
        /// Returns an array of elements (integers) in which can be reached within this algorithm
        /// </summary>
        /// <returns> Returns the array </returns>
        public int[] GetResult()
        {
            // if: The boolean array is not checked yet
            if (m_arrbElementInAlgorithm == null)
            {
                this.UpdateAllElementsArray();
            }

            // nArrayCount: Determines the number of elemnts in the array
            int nArrayCount = 0;
            for (int i = 0; i < m_arrbElementInAlgorithm.Length; i++)
                if (m_arrbElementInAlgorithm[i])
                    nArrayCount++;

            int[] arrnReturn = new int[nArrayCount];
            for (int i = 0; i < arrnReturn.Length; i++)
                arrnReturn[i] = 0;

            for (int i = 0; i < m_arrbElementInAlgorithm.Length; i++)
                if (m_arrbElementInAlgorithm[i])
                    for (int j = 0; j < arrnReturn.Length; j++)
                        if (arrnReturn[j] == 0)
                        {
                            arrnReturn[j] = i;
                            break;
                        }

            return arrnReturn;
        }

        /// <summary>
        /// Returns an array of elements (integers) in which can only be reached after the final step
        /// </summary>
        /// <returns> Returns the array </returns>
        public int[] GetFinal()
        {
            // if: The boolean array is not checked yet
            if (m_arrbElementInFinal == null)
            {
                this.UpdateAllFinalArray();
            }

            // nArrayCount: Determines the number of elemnts in the array
            int nArrayCount = 0;
            for (int i = 0; i < m_arrbElementInFinal.Length; i++)
                if (m_arrbElementInFinal[i])
                    nArrayCount++;

            int[] arrnReturn = new int[nArrayCount];
            for (int i = 0; i < arrnReturn.Length; i++)
                arrnReturn[i] = 0;

            for (int i = 0; i < m_arrbElementInFinal.Length; i++)
                if (m_arrbElementInFinal[i])
                    for (int j = 0; j < arrnReturn.Length; j++)
                        if (arrnReturn[j] == 0)
                        {
                            arrnReturn[j] = i;
                            break;
                        }

            return arrnReturn;
        }
        #endregion

        #region Getter-Setter Functions
        /// <summary>
        /// The starting number for the algorithm
        /// </summary>
        public int Start { get { return m_nStart; } }

        /// <summary>
        /// The number of steps the algorithm is going to execute
        /// </summary>
        public int Step { get { return m_nStep; } }

        /// <summary>
        /// Returns the number of possible outcome it generated
        /// </summary>
        public int Permutations { get { return m_nPermutations; } set { m_nPermutations = value; } }
        #endregion
    }
}