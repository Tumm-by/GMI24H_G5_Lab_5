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
            const int seed = 123;

            //Parametrar funktionalitet
            const int numberOfPosts = 14;
            const int intTarget = 450;
            const string stringTarget = "172.16.0.1";

            //Parametrar tidstester
            const int iterations = 100;
            const int startSize = 100;
            const int stepSizeSlowSortAlgs = 100;
            const int maxSizeSlowSortAlgs = 1000;
            const int stepSizeFastSortAlgs = 10000;
            const int maxSizeFastSortAlgs = 100000;
            // 100 -> 10 0000 Step 100
            int[] arraySizesSlowSortAlgs = Enumerable.Range(0, (maxSizeSlowSortAlgs - startSize) / stepSizeSlowSortAlgs + 1).Select(i => startSize + i * stepSizeSlowSortAlgs).ToArray();
            // 100 -> 1000 0000 Step 10000
            int[] arraySizesFastSortAlgs = Enumerable.Range(0, (maxSizeFastSortAlgs - startSize) / stepSizeFastSortAlgs + 1).Select(i => startSize + i * stepSizeSlowSortAlgs).ToArray();

            //Skapar data
            ILogGenerator generator = new RandomLogGenerator();

            //TestInstanser
            var stringSorter = new SortingManager<string>();
            var intSorter = new SortingManager<int>();
            var stringSearcher = new SearchingManager<string>();
            var intSearcher = new SearchingManager<int>();

            //Data för funktionalitetstester
            var logs = generator.GenerateLogs(numberOfPosts, seed).ToList();
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
            Console.Write($"Control: {ipControl.IndexOf(stringTarget)}, ");
            Console.Write($"Binary: {stringSearcher.BinarySearch(ipControl, stringTarget)}, ");
            Console.Write($"Exponential: {stringSearcher.ExponentialSearch(ipControl, stringTarget)}, ");
            Console.Write($"Jump: {stringSearcher.JumpSearch(ipControl, stringTarget)}, ");
            Console.Write($"Linear: {stringSearcher.LinearSearch(ipControl, stringTarget)}, ");
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
            Console.Write($"Control: {errCodeControl.IndexOf(intTarget)}, ");
            Console.Write($"Binary: {intSearcher.BinarySearch(errCodeControl, intTarget)}, ");
            Console.Write($"Interpolation: {intSearcher.InterpolationSearch(errCodeControl, intTarget)}, ");
            Console.Write($"Exponential: {intSearcher.ExponentialSearch(errCodeControl, intTarget)}, ");
            Console.Write($"Jump: {intSearcher.JumpSearch(errCodeControl, intTarget)}, ");
            Console.Write($"Linear: {intSearcher.LinearSearch(errCodeControl, intTarget)}, ");
            Console.WriteLine("\n");

            Console.WriteLine("==== Tidstester Sortering Strängar ====");
            Logging logWriter = new Logging();
            Dictionary<int, (int, TimeSpan, TimeSpan)> timeData;
            timeData = TimeTester.TimeTestSort<string>(stringSorter.QuickSort, "IpAddress", iterations, arraySizesFastSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "QuickSort");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.MergeSort, "IpAddress", iterations, arraySizesFastSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "MergeSort");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.HeapSort, "IpAddress", iterations, arraySizesFastSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "HeapSort");

            timeData = TimeTester.TimeTestSort<string>(stringSorter.InsertionSort, "IpAddress", iterations, arraySizesSlowSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "InsertionSort");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.SelectionSort, "IpAddress", iterations, arraySizesSlowSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "SelectionSort");
            timeData = TimeTester.TimeTestSort<string>(stringSorter.BubbleSort, "IpAddress", iterations, arraySizesSlowSortAlgs, seed);
            logWriter.LoggingCSV(timeData, "BubbleSort");

            /*timeData = TimeTester.TimeTestSearch(stringSearcher.BinarySearch, "IpAddress", "127.0.0.1", 200, [100, 1000, 10000], 123, true);
            logWriter.LoggingCSV(timeData, "BinarySearchMiddle");

            //Från våra objekt, sorter och searcher, kan vi sedan anropa olika metoder där vi skickar in vår data som parametrar.
            //Det finns ingen implementation av bubblesort i SortingManager just nu. Det här metodanropet är
            //enbart en referens för att visa hur ni kan anropa en metod och skicka er sampledata som ni hämtar 
            //med LogParsern från textfilen. 
            //sorter.BubbleSort(ipAddresses); // <-- implementerar metod från SortingManager-classen som jag vill använda...

            //För att
            //vi ska kunna mäta hur lång tid det tar att köra algoritmen kan vi använda
            //stopwatch och timespan 
            Stopwatch sw = new Stopwatch();
            //TIPS1: det här är ett lämpligt ställe att placera körningen/anropet av din algoritm.
            /*sw.Restart();
            stringSorter.SelectionSort(ipSelection);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tSelectionSort\tTid:{sw.Elapsed.ToString()}");*/
            /*int target = 401;
            for (int i = 0; i < 20; i++)
            {
                logs = generator.GenerateLogs(numberOfPosts, RandomNumberGenerator.GetInt32(999)).ToList();
                errCodeMerge = logs.Select(entry => entry.StatusCode).ToList();
                sw.Restart();
                intSorter.MergeSort(errCodeMerge);
                sw.Stop();
                //Console.WriteLine($"Element:{numberOfPosts}\tMergeSortInt\tTid:{sw.Elapsed.ToString()}");
                int index = intSearcher.ExponentialSearch(errCodeMerge, target);
                Console.WriteLine($"{string.Join(",",errCodeMerge)}");
                Console.WriteLine($"MergeSortInt\tTarget:{target}\t At index:{index}");
                if (index == -1)
                    Console.WriteLine($"Value at index:{index} = Not Found");
                else
                    Console.WriteLine($"Value at index:{index} = {errCodeMerge[index]}");

                Console.WriteLine($"=================================================");

            }
            /*sw.Restart();
            stringSorter.MergeSort(ipMerge);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tMergeSortString\tTid:{sw.Elapsed.ToString()}");
            sw.Restart();
            stringSorter.QuickSort(ipQuick);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tQuickSort\tTid:{sw.Elapsed.ToString()}");
            sw.Restart();
            stringSorter.BubbleSort(ipBubble);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tBubbleSort\tTid:{sw.Elapsed.ToString()}")
            sw.Restart();
            stringSorter.InsertionSort(ipInsertion);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tInsertionSort\tTid:{sw.Elapsed.ToString()}");
            sw.Restart();
            stringSorter.HeapSort(ipHeap);
            sw.Stop();
            Console.WriteLine($"Element:{numberOfPosts}\tHeapSort\tTid:{sw.Elapsed.ToString()}");*/
            //TimeSpan elapsedTime = sw.Elapsed; //TIPS2: här är det kanske en bra idé att göra någonting med data som sparats i elapsedTime... 
            //Man kan ju till exempel tänka sig att det kan vara lämpligt att gå tillbaka till deluppgift 1 i labb 1
            //och kolla hur ni gjorde med er data där...

            Console.WriteLine($"Totalt antal rader inlästa: {logs.Count}");
        }
    }
}
