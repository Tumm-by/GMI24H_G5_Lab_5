using System.Linq.Expressions;
using System.Reflection;

namespace GMI24H_VT25_SortSearch_Labb_
{
    class TimeTester
    {
        public static Dictionary<int, (int, TimeSpan, TimeSpan)> TimeTest<T>(Action<IList<T>> sortFunction,string propertyName, int iterations,int[] arraySizes,int seed, bool showProgress=false)
        {
            
            Dictionary<int, (int, TimeSpan, TimeSpan)> testData = new Dictionary<int, (int, TimeSpan, TimeSpan)>();
            foreach (int arraySize in arraySizes)
            {
                ILogGenerator generator = new RandomLogGenerator();
                var logs = generator.GenerateLogs(arraySize, seed);
                var property = typeof(LogEntry).GetProperty(propertyName);
                IList<T> collection = logs.Select(x => (T)property?.GetValue(x)).ToList();
                if (showProgress)
                {
                    Console.WriteLine($"Current ArraySize:{arraySize}");
                }
                TimeSpan[] executionTimes = ExecutionTimes<T>(sortFunction, collection, iterations);
                TimeSpan averageExecutionTime = AverageExecutionTime(executionTimes);
                TimeSpan stdDeviation = StandardDeviation(executionTimes);
                testData.Add(arraySize,(iterations, averageExecutionTime, stdDeviation));
            }
            return testData;
        }

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