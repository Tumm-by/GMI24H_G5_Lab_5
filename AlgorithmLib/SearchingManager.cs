using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLib
{
    /// <summary>
    /// Implementation av olika sökalgoritmer för generiska listor.
    /// </summary>
    /// <typeparam name="T">Typen på elementen som ska sökas i. Måste implementera IComparable<T>.</typeparam>

    public class SearchingManager<T> : ISearchingManager<T> where T : IComparable<T>
    {
        /// <summary>
        /// Utför binär sökning i en sorterad lista.
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int BinarySearch(IList<T> collection, T target)
        {
            T[] arr = collection.ToArray();
            int n = arr.Length;

            int low = 0;
            int high = n - 1;


            while (low <= high)
            {
                int mid = (low + high) / 2;
                int cmp = arr[mid].CompareTo(target);

                if (cmp == 0)
                {
                    return mid;
                }
                if (cmp < 0)
                {
                    low = mid + 1;
                }
                else 
                { 
                    high = mid - 1;
                }
            }
            return -1;
        }

        /// Inspired by: https://www.geeksforgeeks.org/dsa/exponential-search/
        /// <summary>
        /// Utför exponential search i en sorterad lista.
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int ExponentialSearch(IList<T> collection, T target)
        {
            T[] arr = collection.ToArray();
            int n = arr.Length;

            if (arr[0].Equals(target))
            {
                return 0;
            }

            int i = 1;
            while(i < n && arr[i].CompareTo(target) <= 0)
            {
                i *= 2;
            }
            int low = i / 2;
            T[] newArr = new T[Math.Min(i, n) - low];
            for (int j = low; j < Math.Min(i, n); j++)
            {
                newArr[j - low] = arr[j];
            }

            return low + BinarySearch(newArr.ToList(), target);
        }

        /// Inspired by: https://www.geeksforgeeks.org/dsa/interpolation-search/
        /// <summary>
        /// Utför interpolationssökning. Endast för typer som är int-kompatibla.
        /// </summary>
        /// <param name="collection">Sorterad lista av heltal.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int InterpolationSearch(IList<T> collection, T target)
        {
            //if (collection == null) throw new ArgumentNullException($"{nameof(collection)} was null");
            //if (target == null) throw new ArgumentNullException($"{nameof(target)} was null");
            T[] arrayOfCollection = collection.ToArray();
            int leftIndex = 0;
            int rightIndex = arrayOfCollection.Length - 1;
            int probeIndex = leftIndex;
            double targetAsDouble;
            try
            {
                targetAsDouble = Convert.ToDouble(target);
            }
            catch (InvalidCastException) 
            {
                throw new ArgumentException($"Argument {nameof(target)}={target} is non-numeric");
            }
            double leftValue;
            double rightValue;

            //Determines if the array is ascending or descending
            bool isAscending = arrayOfCollection[0].CompareTo(arrayOfCollection[arrayOfCollection.Length - 1]) < 0;

            //Target below min or above max
            if (isAscending) //Ascending array
            {
                if (target.CompareTo(arrayOfCollection[leftIndex]) < 0 || target.CompareTo(arrayOfCollection[rightIndex]) > 0)
                    return -1;
            }
            else //Descending array
            {
                if (target.CompareTo(arrayOfCollection[leftIndex]) > 0 || target.CompareTo(arrayOfCollection[rightIndex]) < 0)
                    return -1;
            }

            while (leftIndex <= rightIndex)
            {
                leftValue = Convert.ToDouble(arrayOfCollection[leftIndex]);
                rightValue = Convert.ToDouble(arrayOfCollection[rightIndex]);
                if (rightValue.Equals(leftValue)) //Avoid division by zero
                {   //Is target
                    if (arrayOfCollection[leftIndex].Equals(target))
                        return leftIndex;
                    return -1;
                }

                probeIndex = (int)((targetAsDouble - leftValue) * (rightIndex - leftIndex) / (rightValue - leftValue));
                probeIndex = Math.Max(leftIndex, Math.Min(probeIndex, rightIndex));
                //Out of bounds
                    /*if (probeIndex < leftIndex || probeIndex > rightIndex)
                    {
                        return -1;
                    }*/
                if (arrayOfCollection[probeIndex].Equals(target))
                {
                    return probeIndex; //Match found
                }
                    
                //Shrink search span
                if (isAscending) //Ascending array
                {
                    if (arrayOfCollection[probeIndex].CompareTo(target) < 0)
                    {
                        leftIndex = probeIndex + 1;
                    }
                    else
                    {
                        rightIndex = probeIndex - 1;
                    }
                }
                else // Descending array
                {
                    if (arrayOfCollection[probeIndex].CompareTo(target) < 0)
                    {
                        rightIndex = probeIndex - 1;
                    }
                    else
                    {
                        leftIndex = probeIndex + 1;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Utför jump search i en sorterad lista.
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int JumpSearch(IList<T> collection, T target)
        {
            T[] arr = collection.ToArray();
            int n = arr.Length;

            int step = (int)Math.Sqrt(n);
            int prev = 0;

            for(int minStep = Math.Min(step, n) - 1; arr[minStep].CompareTo(target) < 0; minStep = Math.Min(step, n) - 1)
            {
                prev = step;
                step += (int)Math.Sqrt(n);
                if (prev >= n)
                {
                    return -1;
                }
            }

            while (prev < Math.Min(step, n))
            {
                if (arr[prev].CompareTo(target) == 0)
                {
                    return prev;
                }
                prev++;
            }

            if (arr[prev].CompareTo(target) == 0)
            {
                return prev;
            }
            return -1;
        }

        /// <summary>
        /// Utför linjär sökning i en lista.
        /// </summary>
        /// <param name="collection">Listan att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int LinearSearch(IList<T> collection, T target)
        {
            T[] arrayOfCollection = collection.ToArray();

            for (int i = 0; i < arrayOfCollection.Length; i++) 
            {
                if (arrayOfCollection[i].Equals(target))
                    return i;
            }
            return -1;
        }
    }
}
