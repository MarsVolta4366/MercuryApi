using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.NonEntityDtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.RequestModels;
using MercuryApi.Data.Upserts;
using MercuryApi.Helpers.Extensions;
using System.Security.Claims;

namespace MercuryApi.BLL
{
    public interface ITeamBusinessLogic
    {
        Task<IEnumerable<TeamDto>> GetUsersTeams(int userId);
        Task<TeamDto?> GetTeamById(int teamId);
        Task<ExistsDto> TeamNameExists(string teamName);
        Task<TeamDto?> CreateTeam(TeamUpsert request, int userId);
        Task<TeamDto?> AddUsersToTeam(TeamUpsert request);
        Task<TeamDto?> RemoveUserFromTeam(RemoveUserFromTeamRequest request);
        Task<TeamDto?> UserIsInTeam(int userId, int teamId);
        Task DeleteTeamById(int teamId, int userId);
    }

    public class TeamBusinessLogic : BusinessLogicBase, ITeamBusinessLogic
    {
        public TeamBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper) { }

        public async Task<IEnumerable<TeamDto>> GetUsersTeams(int userId)
        {
            IEnumerable<Team> teams = await _repositoryManager.Team.GetTeamsByUserId(userId);
            return _mapper.Map<IEnumerable<TeamDto>>(teams);
        }

        public async Task<TeamDto?> GetTeamById(int teamId)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(teamId);

            return _mapper.Map<TeamDto>(team);
        }

        public async Task<ExistsDto> TeamNameExists(string teamName)
        {
            if (await _repositoryManager.Team.GetTeamByName(teamName) != null)
            {
                return new() { Exists = true };
            }
            return new() { Exists = false };
        }

        public async Task<TeamDto?> CreateTeam(TeamUpsert request, int userId)
        {
            if ((await TeamNameExists(request.Name)).Exists)
            {
                return null;
            }
            List<int> userIds = request.Users.Select(user => user.Id).ToList();

            // Add the user who's creating the team to the new team.
            userIds.Add(userId);

            List<User> teamUsers = await _repositoryManager.User.GetUsersByIds(userIds, trackChanges: true);

            Team team = _mapper.Map<Team>(request);
            team.Users = teamUsers;
            await _repositoryManager.Team.CreateTeam(team);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto?> AddUsersToTeam(TeamUpsert request)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(request.Id, true);
            if (team == null) return null;

            IEnumerable<User> usersToAdd = await _repositoryManager.User.GetUsersByIds(request.Users.Select(user => user.Id), trackChanges: false);

            foreach (User user in usersToAdd)
            {
                team.Users.Add(user);
            }

            await _repositoryManager.SaveAsync();
            return _mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto?> RemoveUserFromTeam(RemoveUserFromTeamRequest request)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(request.TeamId, trackChanges: true);
            if (team == null) return null;

            User? userToRemove = team.Users.SingleOrDefault(x => x.Id == request.UserId);

            // Unassign user from any tasks under the given team.
            team.UnassignUser(request.UserId);

            if (userToRemove != null) team.Users.Remove(userToRemove);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<TeamDto>(team);
        }

        public async Task<TeamDto?> UserIsInTeam(int userId, int teamId)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(teamId);
            if (team == null) return null;

            // If the current user isn't in the given team, return unauthorized.
            if (!team.Users.Select(x => x.Id).ToList().Contains(userId))
            {
                return null;
            }

            return _mapper.Map<TeamDto>(team);
        }

        public async Task DeleteTeamById(int teamId, int userId)
        {
            Team? team = await _repositoryManager.Team.GetTeamById(teamId);
            if (team == null) return;

            // The user making the request to delete the team must be a member of the team.
            if (!team.Users.Select(x => x.Id).ToList().Contains(userId))
            {
                return;
            }

            _repositoryManager.Team.DeleteTeam(team);
            await _repositoryManager.SaveAsync();
        }
    }
}
