using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace _5DanaUOblacima_Filip_Simon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchController(AppDbContext context)
        {
            _context = context;
        }

        public class CreateMatchRequest
        {
            public Guid Team1Id { get; set; }
            public Guid Team2Id { get; set; }
            public Guid? WinningTeamId { get; set; }
            public int Duration { get; set; }
        }

        [HttpPost]
        public ActionResult<Match> CreateMatch([FromBody] CreateMatchRequest request)
        {
            if (request.Duration < 1)
            {
                return BadRequest("Duration must be at least 1 hour");
            }

            var team1 = _context.Teams.Find(request.Team1Id);
            var team2 = _context.Teams.Find(request.Team2Id);

            if (team1 == null || team2 == null)
            {
                return NotFound("One or both teams not found");
            }

            if (request.WinningTeamId != null &&
                request.WinningTeamId != request.Team1Id &&
                request.WinningTeamId != request.Team2Id)
            {
                return BadRequest("Winner must be one of the participating teams");
            }

            var match = new Match
            {
                Id = Guid.NewGuid(),
                Team1Id = request.Team1Id,
                Team2Id = request.Team2Id,
                WinningTeamId = request.WinningTeamId,
                Duration = request.Duration
            };

            var team1Players = _context.Players.Where(p => p.Team == team1.TeamName).ToList();
            var team2Players = _context.Players.Where(p => p.Team == team2.TeamName).ToList();

            double team1AverageElo = team1Players.Average(p => p.Elo);
            double team2AverageElo = team2Players.Average(p => p.Elo);

            foreach (var player in team1Players.Concat(team2Players))
            {
                player.HoursPlayed += request.Duration;

                int k = player.HoursPlayed switch
                {
                    < 500 => 50,
                    < 1000 => 40,
                    < 3000 => 30,
                    < 5000 => 20,
                    _ => 10
                };

                double s = 0.5;
                if (request.WinningTeamId != null)
                {
                    bool isTeam1Player = team1Players.Contains(player);
                    bool isWinner = (isTeam1Player && request.WinningTeamId == team1.Id) ||
                                  (!isTeam1Player && request.WinningTeamId == team2.Id);
                    s = isWinner ? 1.0 : 0.0;

                    if (isWinner)
                        player.Wins++;
                    else
                        player.Losses++;
                }

                double opponentAverageElo = team1Players.Contains(player) ? team2AverageElo : team1AverageElo;
                double e = 1.0 / (1.0 + Math.Pow(10, (opponentAverageElo - player.Elo) / 400.0));

                player.Elo = (int)Math.Round(player.Elo + k * (s - e));
            }

            foreach (var player in team1Players.Concat(team2Players))
            {
                player.HoursPlayed += request.Duration;

                int k = player.HoursPlayed switch
                {
                    < 500 => 50,
                    < 1000 => 40,
                    < 3000 => 30,
                    < 5000 => 20,
                    _ => 10
                };

                double s = 0.5;
                if (request.WinningTeamId != null)
                {
                    bool isTeam1Player = team1Players.Contains(player);
                    bool isWinner = (isTeam1Player && request.WinningTeamId == team1.Id) ||
                                  (!isTeam1Player && request.WinningTeamId == team2.Id);
                    s = isWinner ? 1.0 : 0.0;

                    if (isWinner)
                        player.Wins++;
                    else
                        player.Losses++;
                }

                double opponentAverageElo = team1Players.Contains(player) ? team2AverageElo : team1AverageElo;
                double e = 1.0 / (1.0 + Math.Pow(10, (opponentAverageElo - player.Elo) / 400.0));

                player.Elo = (int)Math.Round(player.Elo + k * (s - e));
            }
            foreach (var player in team1Players.Concat(team2Players))
            {
                player.HoursPlayed += request.Duration;

                int k = player.HoursPlayed switch
                {
                    < 500 => 50,
                    < 1000 => 40,
                    < 3000 => 30,
                    < 5000 => 20,
                    _ => 10
                };

                double s = 0.5;
                if (request.WinningTeamId != null)
                {
                    bool isTeam1Player = team1Players.Contains(player);
                    bool isWinner = (isTeam1Player && request.WinningTeamId == team1.Id) ||
                                  (!isTeam1Player && request.WinningTeamId == team2.Id);
                    s = isWinner ? 1.0 : 0.0;

                    if (isWinner)
                        player.Wins++;
                    else
                        player.Losses++;
                }

                double opponentAverageElo = team1Players.Contains(player) ? team2AverageElo : team1AverageElo;
                double e = 1.0 / (1.0 + Math.Pow(10, (opponentAverageElo - player.Elo) / 400.0));

                player.Elo = (int)Math.Round(player.Elo + k * (s - e));
            }

            _context.Matches.Add(match);
            _context.SaveChanges();

            return Ok(match);
        }
    }
}