using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterManagement.Infrastructure;
using TrainingCenterManagementAPI.Interfaces;

namespace TrainingCenterManagementAPI.Services.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly TrainingCenterManagementDbContext context;

        public GenericRepository(TrainingCenterManagementDbContext context)
        {
            this.context = context;
        }

        public T Add(T entity)
        {
            var NewEntity=context.Add(entity);
            return NewEntity.Entity;
        }

        public IList<T> All()
        {
            var All = context.Set<T>().ToList();
            return All;
        }

        public T? GeT(Guid id)
        {
            return context.Find<T>(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public T Update(T entity)
        {
            return context.Update(entity).Entity;
        }


        public void Delete(T entity)
        {
 
            context.Entry(entity).Property("IsDelete").CurrentValue = true;
            context.Update(entity);
        }
    }
}
