namespace Questao2
{
    public interface IFootballApiClient
    {
        List<Match> GetMatches(string team, int year, string teamPosition);
    }
}
