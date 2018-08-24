using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service;
using Service.LabelQuery;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Execute().Wait();
            Console.ReadLine();
        }

        private static async Task Execute()
        {
            var labelQueryProvider = new TextResourceProvider();
            var stringSerializer = new StringSerializer();
            var stringDeserializer = new StringDeserializer();
            var mapper = new DataToOwnerMapper();

            var factory = new RepositoryQueryFactory(labelQueryProvider, stringSerializer);

            var queryExecutor = new RepositoryQueryExecutor(stringDeserializer, mapper);

            var query = factory.GetQuery("CHR");
            var response = await queryExecutor.ExecuteQuery(query, WriteProgress);

            var responseAsString = stringSerializer.Serialize(response);

            var filePath = System.IO.Path.GetTempPath() + $"LabelRetrievalOutput_{DateTime.UtcNow.ToBinary()}.json";

            System.IO.File.WriteAllText(filePath, responseAsString);

            Console.WriteLine(filePath);

            Console.WriteLine();

            var uniquelyNamedLabels = response.Repositories.SelectMany(repo => repo.Labels).Distinct((x, y) => x.Name == y.Name).ToList();

            Console.WriteLine($"Printing All Uniquely Named Labels (Total: {uniquelyNamedLabels.Count})");

            uniquelyNamedLabels.ForEach(label => Console.WriteLine(label.Name));

            "".ToString();
        }

        private static readonly IList<long> allMillisecondResponses = new List<long>();
        private static int totalPages = 0;

        private static void WriteProgress(int total, int current, long millisecondsTaken)
        {
            totalPages = total;

            allMillisecondResponses.Add(millisecondsTaken);

            var currentAverage = allMillisecondResponses.Average();

            Console.WriteLine(currentAverage);

            var expectedAmountOfTimeLeftInMilliseconds = currentAverage * (totalPages - current);

            var minutesSeconds = ConvertToMinutesSeconds(expectedAmountOfTimeLeftInMilliseconds);

            Console.WriteLine($"Currently retrieving page {current, 3} of {total}. Process expected to end in {minutesSeconds.Item1} minutes, {minutesSeconds.Item2} seconds.");
        }

        private static Tuple<int, int> ConvertToMinutesSeconds(double milliseconds)
        {
            int seconds = (int) milliseconds / 1000;
            int minutesLeft = seconds / 60;
            int secondsLeft = seconds - (minutesLeft * 60);

            return new Tuple<int, int>(minutesLeft, secondsLeft);
        }
    }
}
