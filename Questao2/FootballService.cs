namespace Questao2
{
    public class FootballService : IFootballService
    {
        private readonly IFootballApiClient _apiClient;

        public FootballService(IFootballApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public int GetTotalScoredGoals(string team, int year)
        {
            int totalGoals = 0;
            totalGoals += GetGoalsByTeam(team, year, "team1");
            totalGoals += GetGoalsByTeam(team, year, "team2");
            return totalGoals;
        }

        private int GetGoalsByTeam(string team, int year, string teamPosition)
        {
            int goals = 0;
            var matches = _apiClient.GetMatches(team, year, teamPosition) ?? new List<Match>();

            foreach (var match in matches)
            {
                goals += int.Parse(teamPosition == "team1" ? match.Team1Goals : match.Team2Goals);
            }

            return goals;
        }
    }

}
