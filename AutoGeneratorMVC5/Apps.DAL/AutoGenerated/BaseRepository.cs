using Apps.Common;
using Apps.IDAL;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DAL
{
    /// <summary>
    /// 数据上下文工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前线程的数据上下文
        /// </summary>
        /// <returns>数据上下文</returns>
        public static DBContainer CurrentContext()
        {
            DBContainer _nContext = CallContext.GetData(ConfigPara.EFDBConnection) as DBContainer;
            if (_nContext == null)
            {
                _nContext = new DBContainer();
                CallContext.SetData(ConfigPara.EFDBConnection, _nContext);
            }
            return _nContext;
        }
    }
    public abstract class BaseRepository<T> : IBaseRepository<T> where T:class
    {
        DBContainer db;
        public BaseRepository(DBContainer context)
        {
            if (context == null)
            {
                this.db = ContextFactory.CurrentContext();
            }
           else
            {
                this.db = context;
            }
        }

        public DBContainer Context
        {
            get { return db; }
        }

        public virtual bool Create(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges()>0;
        }

        public virtual bool Edit(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                return db.SaveChanges() > 0;
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    //T old = Find(model._ID);
                    //db.Entry<old>.CurrentValues.SetValues(model);
                    return false;
                }
                return db.SaveChanges()>0;
            }
            return false;
        }

        public virtual bool Delete(T model)
        {
            db.Set<T>().Remove(model);
            return db.SaveChanges()>0;
        }

        public virtual int Delete(params object[] keyValues)
        {
            foreach (var item in keyValues)
            {
                T model = GetById(item);
                if (model != null)
                {
                    db.Set<T>().Remove(model);
                }
            }
            return db.SaveChanges();
        }

        public virtual T GetById(params object[] keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }

        public virtual IQueryable<T> GetList()
        {
            return db.Set<T>();
        }

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }
        public virtual IQueryable<T> GetList<S>(int pageSize, int pageIndex, out int total
            ,Expression<Func<T,bool>> whereLambda,bool isAsc, Expression<Func<T,bool>> orderByLambda)
        {
            var queryable = db.Set<T>().Where(whereLambda);
            total = queryable.Count();
            if (isAsc)
            {
                queryable = queryable.OrderBy(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            else {
                queryable = queryable.OrderByDescending(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            return queryable;
        }

        public virtual bool IsExist(object id)
        {
            return GetById(id)!=null;
        }

        /// <summary>
        /// 执行一条SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSqlCommand(string sql)
        {
            return Context.Database.ExecuteSqlCommand(sql);
        }
        /// <summary>
        /// 异步执行一条SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual Task<int> ExecuteSqlCommandAsync(string sql)
        {
            return Context.Database.ExecuteSqlCommandAsync(sql);
        }

        public virtual DbRawSqlQuery<T> SqlQuery(string sql)
        {
            return db.Database.SqlQuery<T>(sql);
        }
        /// <summary>
        /// 查询一条语句返回结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual DbRawSqlQuery<T> SqlQuery(string sql,params object[] paras)
        {
            return db.Database.SqlQuery<T>(sql,paras);
        }

        public int SaveChanges()
        {
           return db.SaveChanges();
        }


        //1、 Finalize只释放非托管资源；
        //2、 Dispose释放托管和非托管资源；
        //3、 重复调用Finalize和Dispose是没有问题的；
        //4、 Finalize和Dispose共享相同的资源释放策略，因此他们之间也是没有冲突的。
         public void Dispose()
         {
            Dispose(true);
            GC.SuppressFinalize(this);
         }

         public void Dispose(bool disposing)
         {
      
             if (disposing)
             {
                   Context.Dispose();
              }
        }

        //==============================================CODE FIRST============================================
        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> where)
        {
            IQueryable<T> queryable = db.Set<T>().Where(where);
            return queryable.Count() > 0 ? queryable.First() : null;
        }
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> FindList()
        {
            return db.Set<T>();
        }
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> where)
        {
            return db.Set<T>().Where(where);
        }
        /// <summary>
        /// 查找实体列表
        /// </summary>
        /// <param name="where">查询Lambda表达式</param>
        /// <param name="number">获取的记录数量</param>
        /// <returns></returns>
        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> where, int number)
        {
            return db.Set<T>().Where(where).Take(number);
        }
        public virtual IQueryable<T> FindPageList(ref GridPager pager, Expression<Func<T, bool>> whereLambda)
        {
            var queryable = FindList(whereLambda);
            //启用通用列头过滤
            if (!string.IsNullOrWhiteSpace(pager.filterRules))
            {
                List<DataFilterModel> dataFilterList = JsonHandler.Deserialize<List<DataFilterModel>>(pager.filterRules).Where(f => !string.IsNullOrWhiteSpace(f.value)).ToList();
                queryable = LinqHelper.DataFilter<T>(queryable, dataFilterList);
            }
            pager.totalRows = queryable.Count();
            queryable = LinqHelper.SortingAndPaging(queryable, pager.sort, pager.order, pager.page, pager.rows);
            return queryable;
        }


    }
}
