﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterManagementAPI.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        T? GeT(Guid id);
        IList<T> All();

        void Delete(T entity);
        void SaveChanges();
        T? GeT(Guid id, params Expression<Func<T, object>>[] includes);
        IList<T> All(params Expression<Func<T, object>>[] includes);
    }
}
