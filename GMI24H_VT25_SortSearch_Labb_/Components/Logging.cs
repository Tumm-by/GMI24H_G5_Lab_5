namespace GMI24H_VT25_SortSearch_Labb_
{
    class Logging
    {
        public Logging() { }

        public void LoggingCSV (Dictionary<int, (int, TimeSpan, TimeSpan)> dataLog, string fileName)
        {
            using (StreamWriter writer = new StreamWriter($"../../../CSVs/{fileName}.csv"))
            {
                writer.WriteLine("Size,Iterations,MeanTime,StandardDeviation");
                foreach(KeyValuePair<int, (int, TimeSpan, TimeSpan)> data in dataLog)
                {
                    LoggData(data, writer);
                }
            }
        }
        public void LoggData (KeyValuePair<int,(int,TimeSpan,TimeSpan)> data, StreamWriter writer)
        {
            writer.WriteLine($"{data.Key},{data.Value.Item1},{data.Value.Item2.TotalMicroseconds},{data.Value.Item3.TotalMicroseconds}");
            //Console.WriteLine($"{data.Key};{data.Value.Item1};{data.Value.Item2.TotalMicroseconds};{data.Value.Item3.TotalMicroseconds};");
        }

    }
}
