using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

//This is an old repository design I came up with long ago for EF 4.3.1 I think. It worked well at the time.
namespace Repository
{
    /// <summary>
    /// Base class for handling all of the common tasks invovled with using entity framwork Entities
    /// </summary>
    /// <typeparam name="D">This is your target Data Transfer Object (D = DTO)</typeparam>
    /// <typeparam name="E">This is your target Entity Framework Entity (E = Entity)</typeparam>
    public abstract class EntityBaseRepository<D, E>
        where D : class
        where E : class
    {
        #region Regular ADO, for later if needed
        //protected DataTable ExecuteDataTable(DbContext context, string sqlString)
        //{
        //    using (SqlConnection con = new SqlConnection(GetConnectionString(context)))
        //    {
        //        return con.ExecuteDataTable(sqlString);
        //    }
        //}

        //protected DataSet ExecuteDataSet(DbContext context, string sqlString)
        //{
        //    using (SqlConnection con = new SqlConnection(GetConnectionString(context)))
        //    {
        //        return con.ExecuteDataSet(sqlString);
        //    }
        //}

        //protected void ExecuteNonQuery(DbContext context, string sqlString)
        //{
        //    using (SqlConnection con = new SqlConnection(GetConnectionString(context)))
        //    {
        //        con.ExecuteNonQuery(sqlString);
        //    }
        //}
        #endregion

        /// <summary>
        /// Get the connection string from the provided DbContext
        /// </summary>
        /// <param name="context">the target context</param>
        /// <returns>the connection string this context is using</returns>
        protected string GetConnectionString(DbContext context)
        {
            return context.Database.Connection.ConnectionString;
        }

        /// <summary>
        /// Insert the entity of type T into the appropriate entity collection of type T
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        protected void Insert(DbContext context, E[] entityObject)
        {
            foreach (E entity in entityObject)
                context.Set<E>().Add(entity);
            
            context.SaveChanges();
        }

        protected void UpdateEntity(DbContext context, params E[] entityObject)
        {
            //http://www.mattburkedev.com/saving-changes-with-entity-framework-6-in-asp-dot-net-mvc-5/
            foreach (E entity in entityObject)
                context.Entry(entity).State = EntityState.Modified;
            
            //context.Set<E>().Attach(entityObject); //Attach to the context
            
            context.SaveChanges();
        }

        /// <summary>
        /// Update an entity of type T and only update the specified properties
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        /// <param name="properties">a strict list of properties to update</param>
        protected void Update(DbContext context, E entityObject, params string[] properties)
        {
            context.Entry(entityObject).State = EntityState.Modified;
            //context.Set<E>().Attach(entityObject); //Attach to the context

            var entry = context.Entry(entityObject); //Get the Entry for this entity

            //Mark each property provided as modified. All properties are initially 
            //assumed to be false - further more properties cannot be marked as false, 
            //this is sadly a limitation of EF 4.3.1
            foreach (string name in properties)
                entry.Property(name).IsModified = true;

            context.SaveChanges();
        }

        protected void SoftDelete(DbContext context, E entityObject)
        {
            SoftDelete(context, entityObject, null);
        }

        /// <summary>
        /// Mark an entity of type T as deleted. This is lazy deletion aka soft deletion. This method should only be used
        /// if the entity has a Boolean Property named "Deleted".
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        /// <param name="properties">A list of other properties to update during the soft delete</param>
        protected void SoftDelete(DbContext context, E entityObject, params string[] properties)
        {
            //This method is the equivalent of setting the "Deleted" property to true and saving changes. 
            //Essentially this is a single property update.
            context.Set<E>().Attach(entityObject);

            var entry = context.Entry(entityObject);

            DbPropertyEntry p = entry.Property("Deleted");

            p.CurrentValue = true; //Mark this as deleted
            p.IsModified = true; //Mark this as modified

            if (properties != null)
            {
                foreach (string name in properties)
                    entry.Property(name).IsModified = true;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Permanently delete/remove an entity of Type T from it's corresponding entity collection.
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        protected void HardDelete(DbContext context, E[] entityObject)
        {
            foreach (E entity in entityObject)
            {
                context.Set<E>().Attach(entity);
                context.Set<E>().Remove(entity);
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Detach an entity of type T from the change collection
        /// </summary>
        /// <typeparam name="T">an entity of type T</typeparam>
        /// <param name="context"></param>
        /// <param name="entityObject">an entity of type T</param>
        protected void Detach(DbContext context, E entityObject)
        {
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.Detach(entityObject);
        }

        protected abstract E TranslateToEntity(D dto);

        protected abstract D TranslateToDto(E entity);

        protected List<E> ToListOfEntity(List<D> list)
        {
            if (list == null)
                return new List<E>();
            
            List<E> lst = new List<E>(list.Count);

            foreach (D obj in list)
                lst.Add(TranslateToEntity(obj));

            return lst;
        }

        protected List<D> ToListOfDto(IEnumerable<E> list)
        {
            if (list == null)
                return new List<D>();

            //Unfortunately the size cannot be initilized here because 
            //the list of IEnumerable isn't enumerated until called 
            //since it is using delayed execution
            List<D> lst = new List<D>();

            foreach (E obj in list)
                lst.Add(TranslateToDto(obj));

            return lst;
        }

        //protected Result UsingContext<Result>(Func<Result> method)
        //{
        //    using (IntranetApplicationsEntities context = new IntranetApplicationsEntities())
        //    {
        //        return method();
        //    }
        //}

        protected void UsingContext(Action<IntranetApplicationsEntities, E[]> method, params E[] entity)
        {
            using (IntranetApplicationsEntities context = new IntranetApplicationsEntities())
            {
                method(context, entity);
            }
        }

        protected virtual E Insert(D dto)
        {
            E obj = TranslateToEntity(dto);

            UsingContext(Insert, obj);

            return obj;
        }

        protected virtual void Update(List<D> listOfDto)
        {
            List<E> lst = ToListOfEntity(listOfDto);

            UsingContext(UpdateEntity, lst.ToArray());
        }

        protected virtual void Update(List<E> listOfEntity)
        {
            UsingContext(UpdateEntity, listOfEntity.ToArray());
        }

        protected virtual void Update(D dto)
        {
            E obj = TranslateToEntity(dto);

            UsingContext(UpdateEntity, obj);
        }

        protected virtual void Delete(D dto)
        {
            E obj = TranslateToEntity(dto);

            UsingContext(HardDelete, obj);
        }
    }
}
