using _5DanaUOblacima_Filip_Simon;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly AppDbContext _context;

    public PlayerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public ActionResult<object> GetPlayerById(Guid id)
    {
        var player = _context.Players.Find(id);
        if (player == null)
        {
            return NotFound();
        }

        var response = new
        {
            player.Id,
            player.Nickname,
            player.Wins,
            player.Losses,
            player.Elo,
            player.HoursPlayed,
            player.TeamId,
            player.RatingAdjustment
        };

        return Ok(response);
    }


    public class CreatePlayerRequest
    {
        public string Nickname { get; set; } = string.Empty;
    }

    [HttpPost("create")]
    public ActionResult<Player> CreatePlayer([FromBody] CreatePlayerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nickname))
        {
            return BadRequest("Nickname is required.");
        }

        var nicknameExists = _context.Players.Any(p => p.Nickname == request.Nickname);
        if (nicknameExists)
        {
            return BadRequest("This nickname is already taken.");
        }

        var player = new Player
        {
            Nickname = request.Nickname
        };

        _context.Players.Add(player);
        _context.SaveChanges();

        return Ok(player);
    }

}
