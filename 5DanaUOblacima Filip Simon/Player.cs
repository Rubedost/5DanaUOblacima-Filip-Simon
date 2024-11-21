namespace _5DanaUOblacima_Filip_Simon
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nickname { get; set; } = string.Empty;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Elo { get; set; } = 0;
        public int HoursPlayed { get; set; } = 0;
        public string? Team { get; set; }
        public int? RatingAdjustment { get; set; }
        public Guid? TeamId { get; set; }  // New property for team reference
    }
}