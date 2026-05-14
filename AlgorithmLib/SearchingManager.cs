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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Utför exponential search i en sorterad lista.
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int ExponentialSearch(IList<T> collection, T target)
        {
            throw new NotImplementedException();
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
            T[] arrayOfCollection = collection.ToArray();
            int leftIndex = 0;
            int rightIndex = arrayOfCollection.Length - 1;
            int probeIndex = leftIndex;
            double targetAsDouble = Convert.ToDouble(target);
            double leftValue;
            double rightValue;

            //Determines if the array is ascending or descending
            bool isAscending = arrayOfCollection[0].CompareTo(arrayOfCollection[arrayOfCollection.Length - 1]) < 0;

            //Target below min or above max
            if (isAscending) //Ascending array
                if (target.CompareTo(arrayOfCollection[leftIndex]) < 0 || target.CompareTo(arrayOfCollection[rightIndex]) > 0)
                    return -1;
            else //Descending array
                if (target.CompareTo(arrayOfCollection[leftIndex]) > 0 || target.CompareTo(arrayOfCollection[rightIndex]) < 0)
                    return -1;

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

                //Out of bounds
                if (probeIndex < leftIndex || probeIndex > rightIndex)
                    return -1;
                else if (arrayOfCollection[probeIndex].Equals(target))
                    return probeIndex; //Match found
                    
                //Shrink search span
                if (isAscending) //Ascending array
                { 
                    if (arrayOfCollection[probeIndex].CompareTo(target) < 0)
                        leftIndex = probeIndex + 1;
                    else
                        rightIndex = probeIndex - 1;
                }
                else // Descending array
                {
                    if (arrayOfCollection[probeIndex].CompareTo(target) < 0)
                        rightIndex = probeIndex - 1;
                    else
                        leftIndex = probeIndex + 1;
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
            throw new NotImplementedException();
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
