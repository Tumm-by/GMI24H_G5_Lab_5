using AlgorithmLib;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;


namespace GMI24H_VT25_SortSearch_Labb_
{

    internal class Program
    {
        static void Main(string[] args)
        {
            //Här är kod som kan användas om man vill jobba med dataströmmar (som ligger i Generator-katalogen och skapas som ström utifrån en given seed). 
            const int numberOfPosts = 5;
            //const int numberOfPosts = 20;
            //const int seed = 123;
            const int seed = 992;

            ILogGenerator generator = new RandomLogGenerator();
            var logs = generator.GenerateLogs(numberOfPosts, seed).ToList();
            //var logs2 = generator.GenerateLogs(numberOfPosts, seed).ToList();


            //Skriver ut de fem första posterna i listan med LogEntry-typer. 
            Console.WriteLine("förhandsvisning av loggdata:");
            foreach (var entry in logs.Take(5))
            {
                Console.WriteLine(entry);
            }

            //Eftersom metoderna i SortingManager och SearchingManager-klasserna inte är statiska så behöver vi instansiera objekt av dessa klasser.
            //Eftersom vi gjort våra Sorting- och SearchingManager-klasserna generiska (<T>) behöver vi även ange vilken typ av data det
            //är som vi vill sortera eller söka efter. Vi anger datatyp i "diamanten" <>.
            var stringSorter = new SortingManager<string>();
            var intSorter = new SortingManager<int>();
            var stringSearcher = new SearchingManager<string>();
            var intSearcher = new SearchingManager<int>();


            //Välj vilka data som ska plockas ut ur loggarna och jämföras. T.ex. Int eller strängar. Här behöver
            //vi tänka på att välja samma datatyp som vi vill köra våra algoritmer på, dvs. de vi bestämde oss för
            //när vi instansierade SortingManager och SearchingManager. I det här exemplet är det strängar.
            //Därför skapar vi en lista av strängar dit vi kan spara våra ip-adresser.
            //Vi använder LINQ för att selektera ut ip-adress-propertyn från varje enskilt logentry-post i logs-listan. 

            IList<string> ipSelection = logs.Select(entry => entry.IpAddress).ToList();
            IList<int> errCodeMerge = logs.Select(entry => entry.StatusCode).ToList();
            IList<string> ipMerge = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipQuick = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipBubble = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipInsertion = logs.Select(entry => entry.IpAddress).ToList();
            IList<string> ipHeap = logs.Select(entry => entry.IpAddress).ToList();
            Console.WriteLine();
            /*Console.WriteLine("Selection");
            stringSorter.SelectionSort(ipSelection);
            intSorter.MergeSort(errCodeMerge);
            stringSorter.MergeSort(ipMerge);
            stringSorter.QuickSort(ipQuick);
            stringSorter.BubbleSort(ipBubble);
            stringSorter.InsertionSort(ipInsertion);
            stringSorter.HeapSort(ipHeap);*/
            /*Console.WriteLine("Selection");
            foreach (var ipAddress in ipSelection.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("MergeInt");
            foreach (var ipAddress in errCodeMerge.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("MergeString");
            foreach (var ipAddress in ipMerge.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("Quick");
            foreach (var ipAddress in ipQuick.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("Bubble");
            foreach (var ipAddress in ipBubble.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("Insertion");
            foreach (var ipAddress in ipInsertion.Take(10))
                Console.WriteLine(ipAddress);
            Console.WriteLine("Heap");
            foreach (var ipAddress in ipHeap.Take(10))
                Console.WriteLine(ipAddress);*/

            /*foreach (var entry in logs.Take(5))
            {
                Console.WriteLine(entry);
            }*/
            Logging logWriter = new Logging();
            Dictionary<int, (int, TimeSpan, TimeSpan)> timeData = TimeTester.TimeTest<string>(stringSorter.QuickSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "QuickSort");
            timeData = TimeTester.TimeTest<string>(stringSorter.MergeSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "MergeSort");
            timeData = TimeTester.TimeTest<string>(stringSorter.HeapSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "HeapSort");
            /*timeData = TimeTester.TimeTest<string>(stringSorter.InsertionSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "InsertionSort");
            timeData = TimeTester.TimeTest<string>(stringSorter.SelectionSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "SelectionSort");
            timeData = TimeTester.TimeTest<string>(stringSorter.BubbleSort, "IpAddress", 200, [1000, 2000, 3000, 4000, 5000, 30000], 123);
            logWriter.LoggingCSV(timeData, "BubbleSort");*/
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
                int index = intSearcher.InterpolationSearch(errCodeMerge, target);
                Console.WriteLine($"{string.Join(",",errCodeMerge)}");
                Console.WriteLine($"MergeSortInt\tTarget:{target}\t At index:{index}");
                if (index == -1)
                    Console.WriteLine($"Value at index:{index} = Not Found");
                else
                    Console.WriteLine($"Value at index:{index} = {errCodeMerge[index]}");
            }*/
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
