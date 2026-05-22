using AlgorithmLib;
using System.Diagnostics;
using System.Globalization; //Needed for Thread.CurrentThread.CurrentCulture
using System.Net;
using System.Security.Cryptography;



namespace GMI24H_VT25_SortSearch_Labb_
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); //Swedish's use of ';' as a separator and ',' as a decimal is problematic.

            //Testparametrar
            const int SEED = 123;

            //Parametrar funktionalitet
            const int NUMBER_OF_POSTS = 14;
            const int INT_TARGET = 450;
            const string STRING_TARGET = "172.16.0.1";

            //Parametrar tidstester
            const int ITERATIONS_SORT = 100;
            const int ITERATIONS_SEARCH = 1000;
            const int START_SIZE = 100;
            const int STEP_SIZE_SLOW_ALGORITHM = 150;
            const int MAX_SIZE_SLOW_ALGORITHM = 3000;
            const int STEP_SIZE_INSERTION = 250;
            const int MAX_SIZE_INSERTION_SORT = 5000;
            const int STEP_SIZE_FAST_SORT_ALGORITHM = 25000;
            const int MAX_SIZE_FAST_SORT_ALGORITHM = 500000;
            const int STEP_SIZE_FAST_SEARCH_ALGORITHM = 250000;
            const int MAX_SIZE_FAST_SEARCH_ALGORITHM = 5000000;
            const int EARLY_INT = 200;
            const int MIDDLE_INT = 401;
            const int LATE_INT = 500;
            const int MISSING_INT = 450;
            const string EARLY_STRING = "10.0.0.5";
            const string MIDDLE_STRING = "127.0.0.1";
            const string LATE_STRING = "192.168.1.10";
            int[] arraySizesInsertionSort = new int[] { START_SIZE }; //100+ 250 -> 5000
            arraySizesInsertionSort = arraySizesInsertionSort.Concat(Enumerable.Range(1, (MAX_SIZE_INSERTION_SORT - START_SIZE) / STEP_SIZE_INSERTION + 1).Select(i => i * STEP_SIZE_INSERTION).ToArray()).ToArray();
            int[] arraySizesSlowSortAlgs = new int[] { START_SIZE }; //100+ 150 -> 3000
            arraySizesSlowSortAlgs = arraySizesSlowSortAlgs.Concat(Enumerable.Range(1, (MAX_SIZE_SLOW_ALGORITHM - START_SIZE) / STEP_SIZE_SLOW_ALGORITHM + 1).Select(i =>  i * STEP_SIZE_SLOW_ALGORITHM).ToArray()).ToArray();
            int[] arraySizesFastSortAlgs = new int[] { START_SIZE }; //100+ 25 000 -> 500 000
            arraySizesFastSortAlgs = arraySizesFastSortAlgs.Concat(Enumerable.Range(1, (MAX_SIZE_FAST_SORT_ALGORITHM - START_SIZE) / STEP_SIZE_FAST_SORT_ALGORITHM + 1).Select(i =>  i * STEP_SIZE_FAST_SORT_ALGORITHM).ToArray()).ToArray();
            int[] arraySizesFastSearchAlgs = new int[] { START_SIZE }; //100 + 250 000 -> 5 000 000
            arraySizesFastSearchAlgs = arraySizesFastSearchAlgs.Concat(Enumerable.Range(1, (MAX_SIZE_FAST_SEARCH_ALGORITHM - START_SIZE) / STEP_SIZE_FAST_SEARCH_ALGORITHM + 1).Select(i => i * STEP_SIZE_FAST_SEARCH_ALGORITHM).ToArray()).ToArray();
            
            //Skapar data
            ILogGenerator generator = new RandomLogGenerator();

            //TestInstanser
            var stringSorter = new SortingManager<string>();
            var intSorter = new SortingManager<int>();
            var stringSearcher = new SearchingManager<string>();
            var intSearcher = new SearchingManager<int>();

            //Data för funktionalitetstester
            var logs = generator.GenerateLogs(NUMBER_OF_POSTS, SEED).ToList();
            LogEntry targetEntry = new LogEntry();
            targetEntry.IpAddress = "172.16.0.1";
            targetEntry.StatusCode = 450;
            logs.Add(targetEntry);

            //Kontrolldata
            IList<string> ipControl = logs.Select(entry => entry.IpAddress).ToList();
            IList<int> errCodeControl = logs.Select(entry => entry.StatusCode).ToList();

            //Strängdata
            IList<string> ipSelection = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipMerge = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipQuick = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipBubble = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipInsertion = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipHeap = logs.Select(entry => entry.IpAddress).ToList();

            //Intdata
            IList<int> errCodeSelection = logs.Select(entry => entry.StatusCode).ToList();
            IList<int> errCodeMerge = logs.Select(entry => entry.StatusCode).ToList();
            IList<int> errCodeQuick = logs.Select(entry => entry.StatusCode).ToList();
            IList<int> errCodeBubble = logs.Select(entry => entry.StatusCode).ToList();
            IList<int> errCodeInsertion = logs.Select(entry => entry.StatusCode).ToList();
            IList<int> errCodeHeap = logs.Select(entry => entry.StatusCode).ToList();


            Console.WriteLine("==== FunktionalitetsTest Sortering Strängar ====");
            //Sorting Controls
            ipControl = ipControl.Order().ToList();

            //Sorting with sortfunctions
            stringSorter.SelectionSort(ipSelection);
            stringSorter.MergeSort(ipMerge);
            stringSorter.QuickSort(ipQuick);
            stringSorter.BubbleSort(ipBubble);
            stringSorter.InsertionSort(ipInsertion);
            stringSorter.HeapSort(ipHeap);
            Console.Write("Control\t\t");
            foreach (var ipAddress in ipControl)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("Selection\t");
            foreach (var ipAddress in ipSelection)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("MergeString\t");
            foreach (var ipAddress in ipMerge)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("Quick\t\t");
            foreach (var ipAddress in ipQuick)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("Bubble\t\t");
            foreach (var ipAddress in ipBubble)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("Insertion\t");
            foreach (var ipAddress in ipInsertion)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine();
            Console.Write("Heap\t\t");
            foreach (var ipAddress in ipHeap)
                Console.Write($"{ipAddress}, ");
            Console.WriteLine("\n");

            Console.WriteLine("==== FunktionalitetsTest Sök Strängar ====");
            Console.Write($"Control: {ipControl.IndexOf(STRING_TARGET)}, ");
            Console.Write($"Binary: {stringSearcher.BinarySearch(ipControl, STRING_TARGET)}, ");
            Console.Write($"Exponential: {stringSearcher.ExponentialSearch(ipControl, STRING_TARGET)}, ");
            Console.Write($"Jump: {stringSearcher.JumpSearch(ipControl, STRING_TARGET)}, ");
            Console.Write($"Linear: {stringSearcher.LinearSearch(ipControl, STRING_TARGET)}, ");
            Console.WriteLine("\n");


            Console.WriteLine("==== FunktionalitetsTest Sortering Intar ====");
            //Sorting Control
            errCodeControl = errCodeControl.Order().ToList();

            //Sorting with sortfunctions
            intSorter.SelectionSort(errCodeSelection);
            intSorter.MergeSort(errCodeMerge);
            intSorter.QuickSort(errCodeQuick);
            intSorter.BubbleSort(errCodeBubble);
            intSorter.InsertionSort(errCodeInsertion);
            intSorter.HeapSort(errCodeHeap);
            Console.Write("Control\t\t");
            foreach (var errCode in errCodeControl)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("Selection\t");
            foreach (var errCode in errCodeSelection)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("MergeString\t");
            foreach (var errCode in errCodeMerge)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("Quick\t\t");
            foreach (var errCode in errCodeQuick)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("Bubble\t\t");
            foreach (var errCode in errCodeBubble)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("Insertion\t");
            foreach (var errCode in errCodeInsertion)
                Console.Write($"{errCode}, ");
            Console.WriteLine();
            Console.Write("Heap\t\t");
            foreach (var errCode in errCodeHeap)
                Console.Write($"{errCode}, ");
            Console.WriteLine("\n");

            Console.WriteLine("==== FunktionalitetsTest Sök Intar ====");
            Console.Write($"Control: {errCodeControl.IndexOf(INT_TARGET)}, ");
            Console.Write($"Binary: {intSearcher.BinarySearch(errCodeControl, INT_TARGET)}, ");
            Console.Write($"Interpolation: {intSearcher.InterpolationSearch(errCodeControl, INT_TARGET)}, ");
            Console.Write($"Exponential: {intSearcher.ExponentialSearch(errCodeControl, INT_TARGET)}, ");
            Console.Write($"Jump: {intSearcher.JumpSearch(errCodeControl, INT_TARGET)}, ");
            Console.Write($"Linear: {intSearcher.LinearSearch(errCodeControl, INT_TARGET)}, ");
            Console.WriteLine("\n");

            //TidsTester
            Logging logWriter = new Logging();
            Dictionary<int, (int, TimeSpan, TimeSpan)> timeData;

            Console.WriteLine("==== Tidstester Sortering Strängar Osorterad ====");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.QuickSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "QuickSortString");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.MergeSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "MergeSortString");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.HeapSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "HeapSortString");

            timeData = TimeTester.TimeTestSort<string>(stringSorter.InsertionSort, "IpAddress", ITERATIONS_SORT, arraySizesInsertionSort, SEED);
            logWriter.LoggingCSV(timeData, "InsertionSortString");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.SelectionSort, "IpAddress", ITERATIONS_SORT, arraySizesSlowSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "SelectionSortString");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.BubbleSort, "IpAddress", ITERATIONS_SORT, arraySizesSlowSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "BubbleSortString");

            Console.WriteLine("==== Tidstester Sortering Strängar Försorterad ====");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.QuickSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED,true);
            logWriter.LoggingCSV(timeData, "QuickSortStringFörSorterad");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.MergeSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED,true);
            logWriter.LoggingCSV(timeData, "MergeSortStringFörSorterad");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.HeapSort, "IpAddress", ITERATIONS_SORT, arraySizesFastSortAlgs, SEED,true);
            logWriter.LoggingCSV(timeData, "HeapSortStringFörSorterad");

            timeData = TimeTester.TimeTestSort<string>(stringSorter.InsertionSort, "IpAddress", ITERATIONS_SORT, arraySizesInsertionSort, SEED, true);
            logWriter.LoggingCSV(timeData, "InsertionSortStringFörsorterad");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.SelectionSort, "IpAddress", ITERATIONS_SORT, arraySizesSlowSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "SelectionSortStringFörsorterad");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.BubbleSort, "IpAddress", ITERATIONS_SORT, arraySizesSlowSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "BubbleSortStringFörsorterad");

            /*Console.WriteLine("==== Tidstester Sortering Intar ====");
            timeData = TimeTester.TimeTestSort<int>(intSorter.QuickSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "QuickSortInt");
            timeData = TimeTester.TimeTestSort<int>(intSorter.MergeSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "MergeSortInt");
            timeData = TimeTester.TimeTestSort<int>(intSorter.HeapSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "HeapSortInt");

            timeData = TimeTester.TimeTestSort<int>(intSorter.InsertionSort, "StatusCode", ITERATIONS, arraySizesInsertionSort, SEED);
            logWriter.LoggingCSV(timeData, "InsertionSortInt");
            timeData = TimeTester.TimeTestSort<int>(intSorter.SelectionSort, "StatusCode", ITERATIONS, arraySizesSlowSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "SelectionSortInt");
            timeData = TimeTester.TimeTestSort<int>(intSorter.BubbleSort, "StatusCode", ITERATIONS, arraySizesSlowSortAlgs, SEED);
            logWriter.LoggingCSV(timeData, "BubbleSortInt");*/

            /*Console.WriteLine("==== Tidstester Sortering Intar Försorterad ====");
            timeData = TimeTester.TimeTestSort<int>(intSorter.QuickSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "QuickSortIntFörsorterad");
            timeData = TimeTester.TimeTestSort<int>(intSorter.MergeSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "MergeSortIntFörsorterad");
            timeData = TimeTester.TimeTestSort<int>(intSorter.HeapSort, "StatusCode", ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "HeapSortIntFörsorterad");

            timeData = TimeTester.TimeTestSort<int>(intSorter.InsertionSort, "StatusCode", ITERATIONS, arraySizesInsertionSort, SEED, true);
            logWriter.LoggingCSV(timeData, "InsertionSortIntFörsorterad");
            timeData = TimeTester.TimeTestSort<int>(intSorter.SelectionSort, "StatusCode", ITERATIONS, arraySizesSlowSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "SelectionSortIntFörsorterad");
            timeData = TimeTester.TimeTestSort<int>(intSorter.BubbleSort, "StatusCode", ITERATIONS, arraySizesSlowSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "BubbleSortIntFörsorterad");*/

            /*Console.WriteLine("==== Tidstester Sökning Strängar Målvärde Tidigt  ====");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.BinarySearch, "IpAddress", EARLY_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED,true);
            logWriter.LoggingCSV(timeData, "BinarySearchStringEarly");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.ExponentialSearch, "IpAddress", EARLY_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED,true);
            logWriter.LoggingCSV(timeData, "ExponentialSearchStringEarly");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.JumpSearch, "IpAddress", EARLY_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "JumpSearchStringEarly");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.LinearSearch, "IpAddress", EARLY_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "LinearSearchStringEarly");*/

            Console.WriteLine("==== Tidstester Sökning Intar Målvärde Tidigt ====");
            int target = EARLY_INT;
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.BinarySearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"BinarySearchIntEarlyT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.ExponentialSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"ExponentialSearchIntEarlyT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.InterpolationSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"InterpolationSearchIntEarlyT{target}");
            
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.JumpSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"JumpSearchIntEarlyT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.LinearSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"LinearSearchIntEarlyT{target}");

            /*Console.WriteLine("==== Tidstester Sökning Strängar Målvärde Slutet ====");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.BinarySearch, "IpAddress", LATE_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "BinarySearchStringLate");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.ExponentialSearch, "IpAddress", LATE_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "ExponentialSearchStringLate");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.JumpSearch, "IpAddress", LATE_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "JumpSearchStringLate");
            timeData = TimeTester.TimeTestSearch<string>(stringSearcher.LinearSearch, "IpAddress", LATE_STRING, ITERATIONS, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, "LinearSearchStringLate");*/

            Console.WriteLine("==== Tidstester Sökning Intar Målvärde Slutet ====");
            target = LATE_INT;
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.BinarySearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"BinarySearchIntLateT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.ExponentialSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"ExponentialSearchIntLateT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.InterpolationSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"InterPolationSearchIntLateT{target}");

            timeData = TimeTester.TimeTestSearch<int>(intSearcher.JumpSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"JumpSearchIntLateT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.LinearSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"LinearSearchIntLateT{target}");

            Console.WriteLine("==== Tidstester Sökning Intar Målvärde SAKNAS ====");
            target = MISSING_INT;
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.BinarySearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"BinarySearchIntMissingT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.ExponentialSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"ExponentialSearchIntMissingT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.InterpolationSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSearchAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"InterpolationSearchIntMissingT{target}");
            
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.JumpSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"JumpSearchIntMissingT{target}");
            timeData = TimeTester.TimeTestSearch<int>(intSearcher.LinearSearch, "StatusCode", target, ITERATIONS_SEARCH, arraySizesFastSortAlgs, SEED, true);
            logWriter.LoggingCSV(timeData, $"LinearSearchIntMissingT{target}");
        }
    }
}
