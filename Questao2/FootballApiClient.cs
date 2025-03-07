using System.Text.Json;

namespace Questao2
{
    public class FootballApiClient : IFootballApiClient
    {
        private readonly HttpClient _httpClient;

        public FootballApiClient()
        {
            _httpClient = new HttpClient();
        }

        public List<Match> GetMatches(string team, int year, string teamPosition)
        {
            var matches = new List<Match>();
            int page = 1;
            int totalPages = 1;

            do
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamPosition}={team}&page={page}";
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);

                    matches.AddRange(result.Data);
                    totalPages = result.TotalPages;
                }

                page++;
            } while (page <= totalPages);

            return matches;
        }
    }
}
