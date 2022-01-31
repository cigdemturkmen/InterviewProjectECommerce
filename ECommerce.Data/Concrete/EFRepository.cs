using ECommerce.Data.Abstract;
using ECommerce.Entities;
using ECommerce.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ECommerce.Data.Concrete
{
    public class EFRepository<T> : IRepository<T> where T : class, IBaseEntity
    {
        private readonly ECommerceDbContext _context;
        public EFRepository(ECommerceDbContext context)
        {
            _context = context;
        }
        public bool Add(T entity)
        {
            _context.Set<T>().Add(entity);

            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var entity = Get(x => x.Id == id && x.IsActive);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;

            return _context.SaveChanges() > 0;
        }

        public bool Edit(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;

            _context.Set<T>().Update(entity);

            return _context.SaveChanges() > 0;
        }

        public T Get(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.FirstOrDefault();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            return query.ToList();
        }
    }
}
