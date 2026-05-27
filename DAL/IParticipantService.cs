using Entities;

namespace DAL;

public interface IParticipantService
{
    Task SaveAsync(DriveParticipant participant);
    Task<List<DriveParticipant>> GetByDriveIdAsync(string driveId);
    Task<List<DriveParticipant>> GetAllAsync();
}