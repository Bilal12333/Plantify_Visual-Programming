using Entities;
using MongoDB.Driver;

namespace DAL;

public class ParticipantService : IParticipantService
{
    private readonly IMongoCollection<DriveParticipant> _collection;

    public ParticipantService()
    {
        _collection = MongoHelper.GetParticipantCollection;
    }

    public async Task SaveAsync(DriveParticipant participant)
    {
        await _collection.InsertOneAsync(participant);
    }

    public async Task<List<DriveParticipant>> GetByDriveIdAsync(string driveId)
    {
        return await _collection
            .Find(p => p.DriveId == driveId)
            .SortByDescending(p => p.JoinedAt)
            .ToListAsync();
    }

    public async Task<List<DriveParticipant>> GetAllAsync()
    {
        return await _collection
            .Find(_ => true)
            .SortByDescending(p => p.JoinedAt)
            .ToListAsync();
    }
}