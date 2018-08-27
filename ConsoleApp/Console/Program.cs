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
            string authorization = string.Empty;
            string hostName = string.Empty;
            string organization = string.Empty;

            authorization = AskUserForValue("Please enter a valid GitHub Authorization Key");
            hostName = AskUserForValue("Please enter a valid GitHub hostname");
            organization = AskUserForValue("Please enter a valid GitHub organization");

            Execute(authorization, hostName, organization).Wait();
            Console.ReadLine();
        }

        private static string AskUserForValue(string message)
        {
            string value = string.Empty;

            while (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine(message);
                value = Console.ReadLine();
            }

            return value;
        }

        private static async Task Execute(string authorization, string hostname, string organization)
        {
            var labelQueryProvider = new TextResourceProvider();
            var stringSerializer = new StringSerializer();
            var stringDeserializer = new StringDeserializer();
            var mapper = new DataToOwnerMapper();
            var configurationProvider = new ConfigurationProvider();
            configurationProvider.SetConfigurationValue("authorization", authorization);

            var webClient = new WebClient(stringSerializer, stringDeserializer, configurationProvider);

            var factory = new RepositoryQueryFactory(labelQueryProvider, stringSerializer);

            var queryExecutor = new RepositoryQueryExecutor(stringDeserializer, mapper, webClient);

            var query = factory.GetQuery(organization);
            var response = await queryExecutor.ExecuteQuery(hostname, query, WriteProgress);

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
