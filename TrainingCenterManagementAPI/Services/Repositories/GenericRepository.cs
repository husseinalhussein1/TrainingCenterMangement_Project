using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var NewEntity = context.Add(entity);
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

        public T? GeT(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            // Apply each include expression
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Find the key property that ends with 'Id'
            var keyProperty = typeof(T).GetProperties()
                                       .FirstOrDefault(p => p.Name.EndsWith("Id",
                                                                            StringComparison.OrdinalIgnoreCase)
                                                        && p.PropertyType == typeof(Guid));

            if (keyProperty == null)
            {
                throw new InvalidOperationException($"Entity {typeof(T).Name} does not have an 'Id' property.");
            }

            var keyName = keyProperty.Name;

            // Build a dynamic condition for the key
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, keyName);
            var condition = Expression.Equal(property, Expression.Constant(id));

            var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

            return query.FirstOrDefault(lambda);
        }

        public IList<T> All(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            // Apply each include expression
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }



    }
}
