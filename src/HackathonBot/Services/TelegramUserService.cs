using HackathonBot.Models;
using HackathonBot.Repository;
using Microsoft.Extensions.Logging;

namespace HackathonBot.Services;

internal class TelegramUserService(IBotUserRoleRepository roles, IParticipantRepository participants, ITeamRepository teams, ILogger<TelegramUserService> logger) : ITelegramUserService
{
    private readonly IBotUserRoleRepository _roles = roles;
    private readonly IParticipantRepository _participants = participants;
    private readonly ITeamRepository _teams = teams;
    private readonly ILogger<TelegramUserService> _logger = logger;

    public async Task DeleteUserAsync(string username, CancellationToken ct = default)
    {
        var participant = await _participants.FindByUsernameAsync(username, ct);
        if (participant != null)
        {
            await _participants.DeleteAsync(participant, ct);
            await _participants.SaveChangesAsync(ct);
        }
        var role = await _roles.FindByUsernameAsync(username, ct);
        if (role != null)
        {
            await _roles.DeleteAsync(role, ct);
            await _roles.SaveChangesAsync(ct);
        }
    }

    public async Task<BotUserRole?> EnsureRegisteredAsync(long userId, string username, CancellationToken cancellationToken = default)
    {
        try
        {
            // Best option: user is already registered in system.
            var role = await _roles.FindByTelegramIdAsync(userId, cancellationToken);
            if (role != null)
                return role;

            // Trying to find participant by telegram ID.
            var participant = await _participants.FindByTelegramIdAsync(userId, cancellationToken);
            if (participant != null)
            {
                // Create role record for quicker search
                role = await RegisterParticipantRoleAsync(userId, username, cancellationToken);
                await _roles.SaveChangesAsync(cancellationToken);
                return role;
            }

            // Trying to find participant by username.
            participant = await _participants.FindByUsernameAsync(username, cancellationToken);
            role = await _roles.FindByUsernameAsync(username, cancellationToken);
            if (participant != null)
            {
                return await UpdateParticipantTelegramIdAsync(userId, username, role, participant, cancellationToken);
            }
            else if (role != null)
            {
                // Update role Telegram ID.
                role.TelegramId = userId;
                await _roles.UpdateAsync(role, cancellationToken);
                await _roles.SaveChangesAsync(cancellationToken);
                return role;
            }
            // The worst case: just return an unknown role
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't get role for user {Username}", username);
            return null;
        }
    }

    private async Task<BotUserRole?> UpdateParticipantTelegramIdAsync(long userId,
                                                                      string username,
                                                                      BotUserRole? role,
                                                                      Participant participant,
                                                                      CancellationToken cancellationToken)
    {
        participant.TelegramId = userId;
        await _participants.UpdateAsync(participant, cancellationToken);
        // If somehow role has been already created but user is participant, update role.
        if (role != null)
        {
            role.TelegramId = userId;
            role.RoleId = RoleIndex.Participant;
            await _roles.UpdateAsync(role, cancellationToken);
        }
        else
        {
            role = await RegisterParticipantRoleAsync(userId, username, cancellationToken);
        }
        await _roles.SaveChangesAsync(cancellationToken);
        return role;
    }

    private async Task<BotUserRole?> RegisterParticipantRoleAsync(long userId, string username, CancellationToken cancellationToken)
    {
        BotUserRole? role = new()
        {
            RoleId = RoleIndex.Participant,
            TelegramId = userId,
            Username = username,
        };
        await _roles.AddAsync(role, cancellationToken);
        return role;
    }

    public async Task<long?> GetTelegramIdByUsernameAsync(string username, CancellationToken ct = default)
    {
        var role = await _roles.FindByUsernameAsync(username, ct);
        if (role != null)
            return role.TelegramId;
        var participant = await _participants.FindByUsernameAsync(username, ct);
        if (participant != null)
            return participant.TelegramId;
        return null;
    }

    public async Task<RoleIndex> CheckUserByNameAsync(string username, CancellationToken ct = default)
    {
        var role = await _roles.FindByUsernameAsync(username, ct);
        if (role != null)
            return role.RoleId;
        var participant = await _participants.FindByUsernameAsync(username, ct);
        if (participant != null)
            return RoleIndex.Participant;
        return RoleIndex.Unknown;
    }

    public async Task<Participant?> RegisterParticipantAsync(string teamName, string username, string fullName, CancellationToken cancellationToken = default)
    {
        var team = await _teams.FindByNameAsync(teamName, cancellationToken);
        bool isLeader = false;
        if (team == null)
        {
            isLeader = true;
            team = new()
            {
                Name = teamName,
            };
            await _teams.AddAsync(team, cancellationToken);
            await _teams.SaveChangesAsync(cancellationToken);
        }

        var participant = await _participants.FindByUsernameAsync(username, cancellationToken);
        if (participant != null)
        {
            participant.IsLeader = isLeader;
            participant.FullName = fullName;
            participant.TeamId = team.Id;
            await _participants.UpdateAsync(participant, cancellationToken);
        }
        else
        {
            participant = new Participant()
            {
                IsLeader = isLeader,
                FullName = fullName,
                Nickname = username,
                TeamId = team.Id,
            };
            await _participants.AddAsync(participant, cancellationToken);
        }
        await _participants.SaveChangesAsync(cancellationToken);
        return participant;
    }
}