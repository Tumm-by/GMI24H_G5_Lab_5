using System.Linq.Expressions;
using System.Reflection;

namespace GMI24H_VT25_SortSearch_Labb_
{
    /// <summary>
    /// Tests sort and search using functions provided to TimeTesterSort and TimeTesterSearch. Returns Time data Mean Execution Time, Standard Deviation, Iterations and Size of the collection
    /// </summary>
    class TimeTester
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortFunction">The sorting function to test</param>
        /// <param name="propertyName">The name of the property used as the value for sorting</param>
        /// <param name="iterations">Number of iterations each cycle should perform</param>
        /// <param name="arraySizes">Array containing the desired sizes of the test collections</param>
        /// <param name="seed">Seed given to the randomizer</param>
        /// <param name="sortCollection">Should the collection be sorted before it's used by the function?</param>
        /// <returns></returns>
        public static Dictionary<int, (int, TimeSpan, TimeSpan)> TimeTestSort<T>(Action<IList<T>> sortFunction,string propertyName, int iterations,int[] arraySizes,int seed, bool sortCollection=false)
        {
            
            Dictionary<int, (int, TimeSpan, TimeSpan)> testData = new Dictionary<int, (int, TimeSpan, TimeSpan)>();
            ILogGenerator generator = new RandomLogGenerator();

            //Warm up prep
            var logs = generator.GenerateLogs(1000, seed);
            var property = typeof(LogEntry).GetProperty(propertyName);
            IList<T> collection = logs.Select(x => (T)property?.GetValue(x)).ToList();
            List<T> sortWarmUp;
            for (int i = 0; i < 100; i++) //Warm up
            {
                sortWarmUp = collection.ToList();
                sortFunction(sortWarmUp);
            }

            foreach (int arraySize in arraySizes)
            {
                logs = generator.GenerateLogs(arraySize, seed);
                property = typeof(LogEntry).GetProperty(propertyName);
                collection = logs.Select(x => (T)property?.GetValue(x)).ToList();
                if (sortCollection)
                {
                    collection = collection.Order().ToList(); //Försorterar om så önskas
                }
                TimeSpan[] executionTimes = ExecutionTimes<T>(sortFunction, collection, iterations);
                TimeSpan averageExecutionTime = AverageExecutionTime(executionTimes);
                TimeSpan stdDeviation = StandardDeviation(executionTimes);
                testData.Add(arraySize,(iterations, averageExecutionTime, stdDeviation));
                Console.WriteLine($"Size: {arraySize}");
            }
            return testData;
        }

        /// <summary>
        /// Function that tests and returns time data for test performed by and on the function provided by the user. Mean Execution Time, Standard Deviaion, Size of collection for each datapoitn, and iterations performed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchFunction">The sorting function to test</param>
        /// <param name="propertyName">The name of the property used as the value for sorting</param>
        /// <param name="target">The value sought after by the search function</param>
        /// <param name="iterations">Number of iterations each cycle should perform</param>
        /// <param name="arraySizes">Array containing the desired sizes of the test collections</param>
        /// <param name="seed">Seed given to the randomizer</param>
        /// <param name="sortCollection">Should the collection be sorted before it's used by the function?</param>
        /// <param name="addTarget">Adds the target to the collection if true</param>
        public static Dictionary<int, (int, TimeSpan, TimeSpan)> TimeTestSearch<T>(Func<IList<T>, T, int> searchFunction, string propertyName, T target,int iterations,int[] arraySizes, int seed, bool sortCollection = false, bool addTarget = false)
        {

            Dictionary<int, (int, TimeSpan, TimeSpan)> testData = new Dictionary<int, (int, TimeSpan, TimeSpan)>();
            ILogGenerator generator = new RandomLogGenerator();
            
            //Warm up prep
            var logs = generator.GenerateLogs(1000, seed);
            var property = typeof(LogEntry).GetProperty(propertyName); //Används för att komma åt propertyn i T
            IList<T> collection = logs.Select(x => (T)property?.GetValue(x)).ToList();
            collection = collection.Order().ToList();
            for (int i = 0; i < 100; i++)
                searchFunction(collection, target); //Warm up

            foreach (int arraySize in arraySizes)
            {
                logs = generator.GenerateLogs(arraySize, seed);
                property = typeof(LogEntry).GetProperty(propertyName);
                collection = logs.Select(x => (T)property?.GetValue(x)).ToList();
                if (addTarget)
                {
                    collection[0] = target; //Lägger till target i listan om så önskas
                }
                if (sortCollection)
                {
                    collection = collection.Order().ToList(); //Försortera om så önskas.
                }
                TimeSpan[] executionTimes = ExecutionTimes<T>(searchFunction, collection, target, iterations);
                TimeSpan averageExecutionTime = AverageExecutionTime(executionTimes);
                TimeSpan stdDeviation = StandardDeviation(executionTimes);
                testData.Add(arraySize, (iterations, averageExecutionTime, stdDeviation));
                Console.WriteLine($"Size: {arraySize}");
            }
            return testData;
        }

        /// <summary>
        /// Exekverar en given sorteringsfunktion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeTestable">testobjectet</param>
        /// <param name="collection">testdatan som objektet ska ha</param>
        /// <param name="iterations">antal iterationer</param>
        /// <returns>TimeSpan[] innehåller individuella exekveringstider</returns>
        static TimeSpan[] ExecutionTimes<T>(Action<IList<T>> timeTestable, IList<T> collection, int iterations)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            TimeSpan[] executionTimes = new TimeSpan[iterations];

            for (int i = 0; i<iterations; i++)
            {
                IList<T> logsToSort = collection.ToList();
                stopWatch.Restart();
                timeTestable(logsToSort);
                stopWatch.Stop();
                executionTimes[i] = stopWatch.Elapsed;
            }
            return executionTimes;
        }

        /// <summary>
        /// Exekverar en given sökfunktion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="timeTestable">testobjectet</param>
        /// <param name="collection">testdatan som objektet ska ha</param>
        /// /// <param name="target">tesdata för funktionen</param>
        /// <param name="iterations">antal iterationer</param>
        /// <returns>TimeSpan[] innehåller individuella exekveringstider</returns>
        static TimeSpan[] ExecutionTimes<T>(Func<IList<T>, T, int> timeTestable, IList<T> collection, T target,int iterations)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            TimeSpan[] executionTimes = new TimeSpan[iterations];

            for (int i = 0; i < iterations; i++)
            {
                stopWatch.Restart();
                timeTestable(collection, target);
                stopWatch.Stop();
                executionTimes[i] = stopWatch.Elapsed;
            }
            return executionTimes;
        }

        /// <summary>
        /// Beräknar och återger den genomsnittliga exekveringstiden.
        /// </summary>
        /// <param name="executionTimes"></param>
        /// <returns>genomsnittlig exekveringstid</returns>
        static TimeSpan AverageExecutionTime(TimeSpan[] executionTimes)
        {
            TimeSpan totalTime = new TimeSpan();
            for (int i = 0; i<executionTimes.Length; i++)
            {
                totalTime = totalTime.Add(executionTimes[i]);
            }
            TimeSpan averageTime = totalTime.Divide(Convert.ToDouble(executionTimes.Length));
            return averageTime;
        }

        /// <summary>
        /// Beräknar standardavvikelsen för en samling exekveringstider
        /// </summary>
        /// <param name="executionTimes">Exekveringstiderna</param>
        /// <returns>Standardavvikelsen</returns>
        static TimeSpan StandardDeviation(TimeSpan[] executionTimes)
        {
            TimeSpan mean = AverageExecutionTime(executionTimes);
            double stdSum = 0;
            foreach (TimeSpan e in executionTimes)
            {
                stdSum += Math.Pow((e.TotalMicroseconds - mean.TotalMicroseconds), 2);
            }
            double stdDev = Math.Sqrt((stdSum / Convert.ToDouble(executionTimes.Length)));
            return TimeSpan.FromMicroseconds(stdDev);
        }
        
        public static string GetPropertyName<T>(Expression<Func<T, object>> selector)
        {
            var body = selector.Body as MemberExpression
                    ?? (selector.Body as UnaryExpression)?.Operand as MemberExpression;

            return body.Member.Name;
        }
    }

}