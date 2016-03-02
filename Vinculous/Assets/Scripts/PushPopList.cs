using System;
using System.Collections.Generic;

namespace Nyanulities
{
    class PushPopList <T>
    {
        private T[] marrGenericObject;      // marrGenericObject: The array to store all the objects

        private int mnCurrentIndex;          // mnCurrentIndex: The current index of the array
        private int mnSize;                  // mnSize: The size of the array

        // Constructor
        /// <summary>
        /// Declare a new Push and Pop List. Elements pushed into the list will be accessed in a First-In-Last-Out order.
        /// </summary>
        /// <param name="_mnSize"> The size of the push and pop list </param>
        public PushPopList(int _mnSize)
        {
            mnSize = _mnSize;
            mnCurrentIndex = 0;
            marrGenericObject = new T[mnSize];

            // Reset the array;
            for (int i = 0; i < mnSize; i++)
            {
                marrGenericObject[i] = default(T);
            }
        }

        // Public Functions
        /// <summary>
        /// Push an element into the list 
        /// </summary>
        /// <param name="_value"> The element to be pushed into the list </param>
        /// <returns> Returns if the element is pushed into the list </returns>
        public bool Push(T _value)
        {
            // if: The current list is full
            if (mnCurrentIndex == mnSize)
                return false;

            marrGenericObject[mnCurrentIndex] = _value;
            mnCurrentIndex++;
            return true;
        }

        /// <summary>
        /// Pop an element out of the list
        /// </summary>
        /// <param name="_value"> The element to be popped out of the list </param>
        /// <returns> Returns if the element is popped out of the list </returns>
        public bool Pop(out T _value)
        {
            // if: There is no element in the list
            if (mnCurrentIndex == 0)
            {
                _value = default(T);
                return false;
            }

            mnCurrentIndex--;
            _value = marrGenericObject[mnCurrentIndex];
            marrGenericObject[mnCurrentIndex] = default(T);
            return true;
        }

        /// <summary>
        /// Pop an element out f the list
        /// </summary>
        /// <returns> Returns the element that is popped out of the list </returns>
        public T Pop()
        {
            if (mnCurrentIndex == 0)
                return default(T);

            mnCurrentIndex--;
            T value = marrGenericObject[mnCurrentIndex];
            marrGenericObject[mnCurrentIndex] = default(T);
            return value;
        }

        /// <summary>
        /// Returns the value of the element at the top of the list
        /// </summary>
        public T Peek()
        {
            return marrGenericObject[mnCurrentIndex - 1];
        }

        /// <summary>
        /// Returns an array of elements, starting from the top of the stack
        /// </summary>
        /// <returns> Returns an array of elements, starting from the top of the stack </returns>
        public T[] ReadAll()
        {
            if (mnCurrentIndex == 0)
                return new T[0];

            T[] array_Return = new T[mnCurrentIndex];
            for (int i = 0; i < mnCurrentIndex; i++)
                array_Return[i] = marrGenericObject[mnCurrentIndex - i - 1];
            return array_Return;
        }

        /// <summary>
        /// Returns an array of elements, starting from the bottom of the stack
        /// </summary>
        /// <returns> Returns an array of elements, starting from the bottom of the stack </returns>
        public T[] ReadAllInverse()
        {
            if (mnCurrentIndex == 0)
                return new T[0];

            T[] array_Return = new T[mnCurrentIndex];
            for (int i = 0; i < mnCurrentIndex; i++)
                array_Return[i] = marrGenericObject[i];
            return array_Return;
        }

        /// <summary>
        /// Removes all elements from the list
        /// </summary>
        public void Clear()
        {
            marrGenericObject = new T[mnSize];
            mnCurrentIndex = 0;
        }

        /// <summary>
        /// Changes the size of the list. NOTE: If the new size of the list is smaller, it will remove all of the excessed elements
        /// </summary>
        /// <param name="_mnSize"> The new size of the list </param>
        /// <returns> Returns if the list is changed </returns>
        public bool ChangeSize(int _mnSize)
        {
            if (mnSize == _mnSize)
                return false;

            T[] array_newList = new T[_mnSize];

            // if: The current size is bigger or equal to the new size
            if (mnSize > _mnSize)
            {
                for (int i = 0; i < _mnSize; i++)
                    array_newList[i] = marrGenericObject[i];
                mnCurrentIndex = _mnSize;
                mnSize = _mnSize;
            }
            // else: The new size is bigger than the current size
            else
            {
                for (int i = 0; i < mnSize; i++)
                    array_newList[i] = marrGenericObject[i];
                mnSize = _mnSize;
            }

            marrGenericObject = array_newList;
            return true;
        }

        /// <summary>
        /// Returns if the current Push-Pop list is empty
        /// </summary>
        /// <returns> Returns if the current Push-pop list is empty </returns>
        public bool IsEmpty()
        {
            if (mnCurrentIndex == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns if the current Push-Pop list is full
        /// </summary>
        /// <returns> Returns if the current Push-Pop list is full </returns>
        public bool IsFull()
        {
            if (mnCurrentIndex == mnSize)
                return true;
            else
                return false;
        }

        // Getter-Setter Functions
        /// <summary>
        /// Returns the full size of the list
        /// </summary>
        public int Size { get { return mnSize; } }

        /// <summary>
        /// Returns the current number of elements in the list
        /// </summary>
        public int Count { get { return mnCurrentIndex; } }
    }
}
