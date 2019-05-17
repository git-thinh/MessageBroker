using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CacheEngineShared
{


    public class CacheSynchronized<T>
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private List<T> innerCache;
        private int limit;

        public CacheSynchronized()
        {
            limit = 0;
            innerCache = new List<T>() { };
        }

        public int Count { get { return innerCache.Count; } }

        public void Set(IEnumerable<T> _cache)
        {
            cacheLock.EnterWriteLock();
            try
            {
                this.innerCache = _cache.ToList();
                this.limit = this.innerCache.Count;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public T Read(int index)
        {
            cacheLock.EnterReadLock();
            try
            {
                if (index < Count) return innerCache[index];
                return default(T);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public IEnumerable<T> getAll()
        {
            cacheLock.EnterReadLock();
            try
            {
                return innerCache;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public int[] Search(string condition)
        {
            int[] arr = new int[] { };
            cacheLock.EnterReadLock();
            try
            {
                List<int> ls = new List<int>() { };

                //for (int i = 0; i < limit; i++) if (predicate(innerCache[i])) ls.Add(i);
                //var t = innerCache.Where("@0.Contains(\"cd\")").ToArray();
                //arr = innerCache.Where(condition).ToArray();
                //arr = ls.ToArray();
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
            return arr;
        }

        public T[] SearchDynamic(string condition)
        {
            T[] arr = new T[] { };
            cacheLock.EnterReadLock();
            try
            {
                arr = innerCache.Where(condition).ToArray();
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
            return arr;
        }

        public int[] Search(Func<T, bool> predicate)
        {
            int[] arr = new int[] { };
            cacheLock.EnterReadLock();
            try
            {
                List<int> ls = new List<int>() { };

                //for (int i = 0; i < limit; i++) if (predicate(innerCache[i])) ls.Add(i);
                Parallel.For(0, limit, i => { if (predicate(innerCache[i])) lock (ls) ls.Add(i); });

                arr = ls.ToArray();
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
            return arr;
        }

        public void Add(int index, T value)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (index < Count) innerCache[index] = value;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        //////private static Func<User, bool> GetDynamicQueryWithFunc(string propName, object val)
        //////{
        //////    Func<User, bool> exp = (t) => true;
        //////    switch (propName)
        //////    {
        //////        case "ID":
        //////            exp = d => d.ID == Convert.ToInt32(val);
        //////            break;
        //////        case "FirstName":
        //////            exp = f => f.FirstName == Convert.ToString(val);
        //////            break;
        //////        case "LastName":
        //////            exp = l => l.LastName == Convert.ToString(val);
        //////            break;
        //////        default:
        //////            break;
        //////    }
        //////    return exp;
        //////}

        private static Func<T, bool> GetDynamicQueryWithExpresionTrees(string propertyName, string val)
        {
            //x =>
            var param = Expression.Parameter(typeof(T), "x");

            #region Convert to specific data type
            MemberExpression member = Expression.Property(param, propertyName);
            UnaryExpression valueExpression = GetValueExpression(propertyName, val, param);
            #endregion
            Expression body = Expression.Equal(member, valueExpression);
            var final = Expression.Lambda<Func<T, bool>>(body: body, parameters: param);
            return final.Compile();
        }

        private static UnaryExpression GetValueExpression(string propertyName, string val, ParameterExpression param)
        {
            var member = Expression.Property(param, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);

            if (!converter.CanConvertFrom(typeof(string)))
                throw new NotSupportedException();

            var propertyValue = converter.ConvertFromInvariantString(val);
            var constant = Expression.Constant(propertyValue);
            return Expression.Convert(constant, propertyType);
        }

        object _getItemByKey(string fielKey, string typeKey, string valKey)
        {
            try
            {
                object id;
                switch (typeKey)
                {
                    case "int":
                        id = Convert.ToInt32(valKey);
                        break;
                    case "long":
                        id = Convert.ToInt64(valKey);
                        break;
                    default:
                        id = valKey;
                        break;
                }
                //var query = innerCache.Where("City == @0 and Orders.Count >= @1", "London", 10).SingleOrDefault();
                return innerCache.Where(fielKey + " == @0", id).SingleOrDefault();
            }
            catch { }
            return null;
        }

        public bool Update(UPDATE_TYPE type, oCacheField[] cacheFields, string valKey, string jsonObject)
        {
            string fielKey = string.Empty, typeKey = string.Empty;
            var fkey = cacheFields.Where(x => x.iskey).SingleOrDefault();
            if (fkey != null)
            {
                fielKey = fkey.name;
                typeKey = fkey.type;
            }

            if (string.IsNullOrWhiteSpace(jsonObject)) return false;
            try
            {
                T item = JsonConvert.DeserializeObject<T>(jsonObject);
                T itemOld;
                switch (type)
                {
                    case UPDATE_TYPE.ADD:
                        cacheLock.EnterWriteLock();
                        try
                        {
                            innerCache.Add(item);
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        return true;
                    case UPDATE_TYPE.DELETE:
                        cacheLock.EnterWriteLock();
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(fielKey) && !string.IsNullOrWhiteSpace(typeKey))
                            {
                                fielKey = fkey.name;
                                typeKey = fkey.type;

                                object it = _getItemByKey(fielKey, typeKey, valKey);
                                if (it != null)
                                {
                                    itemOld = (T)it;
                                    if (itemOld != null) innerCache.Remove(itemOld);
                                }
                            }
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        return true;
                    case UPDATE_TYPE.EDIT:
                        cacheLock.EnterWriteLock();
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(fielKey) && !string.IsNullOrWhiteSpace(typeKey))
                            {
                                object it = _getItemByKey(fielKey, typeKey, valKey);
                                if (it != null)
                                {
                                    itemOld = (T)it;
                                    if (itemOld != null) innerCache.Remove(itemOld);
                                    innerCache.Add(item);
                                }
                            }
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        break;
                }
            }
            catch { }
            return false;
        }

        ~CacheSynchronized()
        {
            if (cacheLock != null) cacheLock.Dispose();
        }
    }

    public enum UPDATE_TYPE
    {
        ADD = 1,
        EDIT = 2,
        DELETE = 3
    }
}