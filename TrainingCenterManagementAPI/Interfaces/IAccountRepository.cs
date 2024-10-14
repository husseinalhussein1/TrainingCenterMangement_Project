using TrainingCenterManagement.Domain;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface IAccountRepository
    {
        Account GetAccount(string username, string password); // للحصول على حساب المستخدم بناءً على اسم المستخدم وكلمة المرور
    }
}
