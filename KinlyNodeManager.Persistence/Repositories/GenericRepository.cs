using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace KinlyNodeManager.Persistence.Repositories
{
    public class GenericRepository<TEntity, TContext>
        where TEntity : class
        where TContext : ApplicationDbContext, new()
    {
        protected TContext DataContext { get { return new TContext(); } }

        public TEntity Get(dynamic searchKey)
        {
            var Context = DataContext;
            return Context.Set<TEntity>().Find(searchKey);
        }

        public void Save(TEntity entity)
        {
            try
            {
                var context = DataContext;

                // Validator.ValidateObject will throw a ValidationException if validation fails, which
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);

                context.Entry<TEntity>(entity).State = EntityState.Added;
                context.SaveChanges();
            }
            catch (ValidationException dbex)
            {
                throw dbex;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                var context = DataContext;

                // Validator.ValidateObject will throw a ValidationException if validation fails, which
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);

                context.Entry<TEntity>(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (ValidationException dbex)
            {
                throw dbex;
            }
        }

        public void DetachAndUpdate(string id, TEntity modifiedEntity)
        {
            var entity = Get(id);

            var context = DataContext;
            try
            {
                // Validator.ValidateObject will throw a ValidationException if validation fails, which
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
                context.Entry(modifiedEntity).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (ValidationException dbex)
            {
                throw dbex;
            }

        }

        public IList<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            var context = DataContext;
            return context.Set<TEntity>().Where(predicate).ToList();
        }

        public void Remove(TEntity entity)
        {
            var context = DataContext;
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public IQueryable<TEntity> GetAll()
        {
            var context = DataContext;
            return context.Set<TEntity>();
        }
    }
}
