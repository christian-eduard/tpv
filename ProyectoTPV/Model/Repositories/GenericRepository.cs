using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Validation;

namespace OpenPOS.Model
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        //Repositorio generico que permite el acceso generico a cualquier tabla
        protected OpenPOSEntities context;
        DbSet<TEntity> dbSet;
        public GenericRepository(OpenPOSEntities context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public void Update(TEntity entity)
        {

            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(TEntity entityToDelete)
        {
            dbSet = context.Set<TEntity>();
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);

            context.Entry(entityToDelete).State = EntityState.Deleted;
            context.SaveChanges();

        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            dbSet = context.Set<TEntity>();
            var entities = dbSet.Where(predicate).ToList();
            entities.ForEach(x => context.Entry(x).State = EntityState.Deleted);
            context.SaveChanges();

        }

        public void Create(TEntity entity)
        {
            try
            {
                dbSet = context.Set<TEntity>();
                dbSet.Add(entity);
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

              // throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



        }

        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();

        }
        public List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            dbSet = context.Set<TEntity>();
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }
        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            dbSet = context.Set<TEntity>();
            return dbSet.FirstOrDefault(predicate);
        }
    }
}

