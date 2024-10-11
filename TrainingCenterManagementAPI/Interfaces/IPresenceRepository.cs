
using TrainingCenterManagement.Domain;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface IPresenceRepository : IRepository<Presence>
    {
        Task<Presence> AddPresenceAsync(Guid lectureId, Guid traineeId);
        Task<bool> GetPresenceAsync(Guid lectureId, Guid traineeId);

    }
}
