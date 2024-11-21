public class Team
{
    // Unique identifier for the team
    public Guid Id { get; set; } = Guid.NewGuid();

    // Name of the team
    public string TeamName { get; set; } = string.Empty;

    // Collection of player IDs that make up the team (limited to 5 players)
    public List<Guid> PlayerIds { get; set; } = new List<Guid>(5);
}
