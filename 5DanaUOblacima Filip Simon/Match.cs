using _5DanaUOblacima_Filip_Simon;

namespace _5DanaUOblacima_Filip_Simon

{
    public class Match
    {
        public Guid Id { get; set; } 
        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }
        public Guid? WinningTeamId { get; set; }
        public int Duration { get; set; }
    }
}
