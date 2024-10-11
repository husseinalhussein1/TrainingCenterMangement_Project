


using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;


namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class PresenceRepository : GenericRepository<Presence>, IPresenceRepository
    {


        public PresenceRepository(TrainingCenterManagementDbContext context) : base(context)
        {
        }

        public async Task<Presence> AddPresenceAsync(Guid lectureId, Guid traineeId)
        {
            var presence = new Presence()
            {
                IsPresence = true,
                LectureId = lectureId,
                TraineeId = traineeId
            };
            Add(presence);
            return presence;
        }

        public async Task<bool> GetPresenceAsync(Guid lectureId, Guid traineeId)
        {
            var presence = All().Where(pr => pr.LectureId == lectureId)
                                .FirstOrDefault(pr => pr.TraineeId == traineeId);
            if (presence == null) return false;//لا يمكن ان تكون قيمة البوليان null 
                                               // لذلك بما انو واحد من الاي ديات غلط فهو غير حاضر 
            return presence.IsPresence;
        }


        // توابع اضافية غير الاساسية
    }
}
