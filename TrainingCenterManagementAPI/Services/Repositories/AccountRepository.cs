using Microsoft.EntityFrameworkCore;
using System.Linq;
using TrainingCenterManagement.Domain;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;

namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly TrainingCenterManagementDbContext _context;

        public AccountRepository(TrainingCenterManagementDbContext context)
        {
            _context = context;
        }

        // دالة لجلب حساب المستخدم بناءً على اسم المستخدم وكلمة المرور
        public Account GetAccount(string username, string password)
        {
            return _context.Accounts.Include(a => a.Administrator)
                                    .Include(a => a.Trainee)
                                    .Include(a => a.Trainer)
                                    .Include(a => a.TrainingOfficer)
                                    .Include(a => a.Receptionist)
                                    .FirstOrDefault(a => a.UserName == username && a.Password == password);
        }
    }
}
