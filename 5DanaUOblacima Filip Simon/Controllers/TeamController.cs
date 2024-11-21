using _5DanaUOblacima_Filip_Simon;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeamController(AppDbContext context)
    {
        _context = context;
    }

    public class CreateTeamRequest
    {
        public string TeamName { get; set; } = string.Empty;
        public List<Guid> players { get; set; } = new List<Guid>();
    }


    [HttpGet("{id}")]
    public ActionResult<Team> GetTeamById(Guid id)
    {
        var team = _context.Teams.Find(id);

        if (team == null)
        {
            return NotFound();
        }

        return Ok(team);
    }


    [HttpPost]
    public ActionResult<Team> CreateTeam([FromBody] CreateTeamRequest request)
    {
        if (request.players.Count != 5)
        {
            return BadRequest("Team must have exactly 5 players");
        }

        var team = new Team
        {
            TeamName = request.TeamName,
            PlayerIds = request.players
        };

        var players = _context.Players
            .Where(p => request.players.Contains(p.Id))
            .ToList();

        if (players.Count != 5)
        {
            return BadRequest("One or more player IDs are invalid");
        }

        foreach (var player in players)
        {
            player.Team = team.TeamName;   
            player.TeamId = team.Id;       
        }

        _context.Teams.Add(team);
        _context.SaveChanges();

        var response = new
        {
            team.Id,
            team.TeamName,
            Players = players.Select(p => new
            {
                p.Id,
                p.Nickname,
                p.Wins,
                p.Losses,
                p.Elo,
                p.HoursPlayed,
                p.TeamId,
                p.RatingAdjustment
            }).ToList()
        };

        return CreatedAtAction(nameof(CreateTeam), response);
    }

}
