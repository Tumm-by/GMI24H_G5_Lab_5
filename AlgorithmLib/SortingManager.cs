using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLib
{
    /// <summary>
    /// <summary>
    /// Implementation av olika sorteringsalgoritmer för generiska listor.
    /// </summary>
    /// <typeparam name="T">Typen på elementen som ska sorteras. Måste implementera IComparable<T>.</typeparam>

    public class SortingManager<T> : ISortingManager<T> where T : IComparable<T>
    {
        /// <summary>
        /// Sorterar listan med Bubble Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void BubbleSort(IList<T> collection)
        {
            //Edge cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2)
                return;

            T[] arr = collection.ToArray();
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;

                for (int j = 0; j < n - 1 - i; j++)
                {
                    // CompareTo returns > 0 if left is greater than right
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        // Swap using tuple syntax
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                        swapped = true;
                    }
                }

                if (!swapped) break;
            }
            for (int i = 0; i < n; i++)
            {
                collection[i] = arr[i];
            }
        }

        /// <summary>
        /// Sorterar listan med Merge Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void MergeSort(IList<T> collection)
        {
            //Edge cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2)
                return;

            T[] arrayOfCollection = collection.ToArray(); //To avoid stepping through the IList many times during sorting.Since the underlying type is unknown
            T[] scratchBuffer = new T[arrayOfCollection.Length]; //Buffer for the sorting.
            MergeSort(arrayOfCollection, scratchBuffer,0, arrayOfCollection.Length - 1);

            //Refilling the collection.
            for (int i = 0; i < arrayOfCollection.Length; i++)
                collection[i] = arrayOfCollection[i];             //How good or bad this is depends on the underlying instance type.
        }

        /// <summary>
        /// Recursive MergeSort using a scratchbuffer.
        /// </summary>
        /// <param name="arrayOfCollection">Håller värden för och efter sortering</param>
        /// <param name="scratchBuffer">Tar emot värden under sortering.</param>
        /// <param name="leftIndex">Vänster index av spannet som sorteras</param>
        /// <param name="rightIndex">Höger index av spannet som sorteras</param>
        private void MergeSort(T[] arrayOfCollection, T[] scratchBuffer, int leftIndex, int rightIndex)
        {
            //Exit criteria
            if (leftIndex >= rightIndex)
            {
                return;
            }
            //Recursion
            int middleIndex = (rightIndex + leftIndex) / 2;
            MergeSort(arrayOfCollection, scratchBuffer, leftIndex, middleIndex);
            MergeSort(arrayOfCollection, scratchBuffer, middleIndex + 1, rightIndex);
            
            //Reads and sorts the span [leftIndex..rightIndex] from arrayOfCollection and stores the sorted span in the scratchBuffer
            int l = leftIndex;      
            int r = middleIndex+1;
            int i = leftIndex;  
            while (l <= middleIndex && r <= rightIndex) 
            {
                if (arrayOfCollection[l].CompareTo(arrayOfCollection[r]) < 0) //Mindre elementet var i vänstra spannet.
                {
                    scratchBuffer[i] = arrayOfCollection[l];
                    l = l + 1; //Step forward in the left span
                }
                else //Mindre elementet var i högra spannet
                {
                    scratchBuffer[i] = arrayOfCollection[r];
                    r = r + 1; //Step forward in the right span
                }
                i++;
            }
            //Leftover elements in the left span
            for (; l <= middleIndex; l++)
            {
                scratchBuffer[i] = arrayOfCollection[l];
                i++;
            }
            //Leftover elements in the right span
            for (; r <= rightIndex; r++)
            {
                scratchBuffer[i] = arrayOfCollection[r];
                i++;
            }

            //Putting the sorted span into the array.
            for (int k = leftIndex; k <= rightIndex; k++)
            {
                arrayOfCollection[k] = scratchBuffer[k];
            }
        }

        /// <summary>
        /// Sorterar listan med Heap Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void HeapSort(IList<T> collection)
        {
            //Edge cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2)
                return;

            T[] arr = collection.ToArray();
            int n = arr.Length;
            // 1. Build a maxheap
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // 2. Extract elements from the heap one by one
            for (int i = n - 1; i > 0; i--)
            {
                // Move current root (largest) to the end
                (arr[0], arr[i]) = (arr[i], arr[0]);

                // Re-heapify the reduced heap
                Heapify(arr, i, 0);
            }
            for (int i = 0; i < n; i++)
            {
                collection[i] = arr[i];
            }
        }

        private static void Heapify(T[] collection, int heapSize, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < heapSize && collection[left].CompareTo(collection[largest]) > 0)
                largest = left;

            if (right < heapSize && collection[right].CompareTo(collection[largest]) > 0)
                largest = right;

            if (largest != i)
            {
                (collection[i], collection[largest]) = (collection[largest], collection[i]);
                Heapify(collection, heapSize, largest);
            }
        }

        /// <summary>
        /// Sorterar listan med Insertion Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void InsertionSort(IList<T> collection)
        {
            //Edge cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2)
                return;

            T[] arr = collection.ToArray();

            LinkedList<T> lLOfCollection = new LinkedList<T>();
            LinkedListNode<T>? nodeInCollection;
            lLOfCollection.AddFirst(arr[0]);
            T current;

            for (int i = 1; i < arr.Length; i++)
            {
                current = arr[i];
                nodeInCollection = lLOfCollection.First;
                if (current.CompareTo(lLOfCollection.Last.Value) >= 0)
                {
                    lLOfCollection.AddLast(current);
                    continue;
                }
                else if (current.CompareTo(lLOfCollection.First.Value) <= 0)
                {
                    lLOfCollection.AddFirst(current);
                    continue;
                }

                while (current.CompareTo(nodeInCollection.Value) > 0)
                {
                    nodeInCollection = nodeInCollection.Next;
                }

                lLOfCollection.AddBefore(nodeInCollection, current);
            }
            int j = 0;
            foreach (T item in lLOfCollection)
            {
                collection[j] = item;
                j++;
            }
        }

        /// Learned from https://www.geeksforgeeks.org/dsa/quick-sort-algorithm/
        /// <summary>
        /// Sorterar listan med Quick Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void QuickSort(IList<T> collection)
        {
            //Edge Cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2)
                return;

            T[] arrayOfList = collection.ToArray(); //To avoid stepping through the IList many times during sorting.Since the underlying type is unknown
            QuickSort(arrayOfList, 0, arrayOfList.Length - 1);

            for (int i = 0; i < arrayOfList.Length; i++)
                collection[i] = arrayOfList[i];

        }

        ///Code inspired by https://www.geeksforgeeks.org/dsa/hoare-s-partition-algorithm/
        ///Code inspired by https://en.wikipedia.org/wiki/Quicksort#Hoare_partition_scheme
        /// <summary>
        /// Recursive Hoare-Partition Version of Quick-Sort
        /// </summary>
        /// <param name="arrayOfList">The array to be sorted</param>
        /// <param name="leftIndex">Leftmost index of the partition</param>
        /// <param name="rightIndex">Rightmost index of the partition</param>)
        private void QuickSort(T[] arrayOfList, int leftIndex, int rightIndex)
        {
            //Exit Criteria
            if (leftIndex >= rightIndex)
                return;

            int middleIndex = (leftIndex + rightIndex) / 2;
            T pivot = arrayOfList[middleIndex];
            int l = leftIndex;
            int r = rightIndex;
            T tempVar;

            while (true)
            {
                //Step rightward until element bigger than pivot
                while (arrayOfList[l].CompareTo(pivot) < 0)
                {
                    l++;
                } 
                //Step leftward until element smaller than pivot
                while (arrayOfList[r].CompareTo(pivot) > 0)
                {
                    r--;
                } 
                
                //Exit
                if (l >= r)
                    break;

                //Swap 
                tempVar = arrayOfList[l];
                arrayOfList[l] = arrayOfList[r];
                arrayOfList[r] = tempVar;
                r--; //Step leftward
                l++; //Step rightward
            }

            QuickSort(arrayOfList, leftIndex, r);
            QuickSort(arrayOfList, r + 1, rightIndex);
        }

        /// <summary>
        /// Sorterar listan med Selection Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void SelectionSort(IList<T> collection)
        {
            //Edge Cases
            if (collection == null)
                throw new ArgumentNullException($"nameof{collection} was null");
            else if (collection.Count < 2) 
                return;

            T[] arrayOfList = collection.ToArray(); //To avoid stepping through the IList many times during sorting.Since the underlying type is unknown
            T tempVar;
            int smallestIndex;

            //Sorting
            for (int i = 0; i < arrayOfList.Length - 1; i++)
            {
                smallestIndex = i;
                for (int j = i + 1; j < arrayOfList.Length; j++)
                {
                    if (arrayOfList[j].CompareTo(arrayOfList[smallestIndex]) < 0)
                        smallestIndex = j;
                }
                if (smallestIndex != i) 
                { 
                    tempVar = arrayOfList[i];
                    arrayOfList[i] = arrayOfList[smallestIndex];
                    arrayOfList[smallestIndex] = tempVar;
                }
            }

            //Refilling the collection.
            for (int i = 0; i < arrayOfList.Length; i++)
                collection[i] = arrayOfList[i];             //How good or bad this is depends on the underlying instance type.
        }
    }
}
