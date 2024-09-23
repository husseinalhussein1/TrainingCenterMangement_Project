using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagementAPI.Services.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T? GeT(Guid id);
        IList<T> All();

        void Delete(T entity);
        void SaveChanges();
    }
}
