using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Caching;
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

        public void Set(IEnumerable<T> items)
        {
            cacheLock.EnterWriteLock();
            try
            {
                this.innerCache = items.ToList();
                this.limit = this.innerCache.Count;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public oCacheResult getAllJson()
        {
            string title = "CacheSynchronized.cs > getAllJson()";
            try
            {
                cacheLock.EnterReadLock();
                try
                {
                    try
                    {
                        JsonConvert.SerializeObject(innerCache);
                    }
                    catch (Exception ex1) {
                        return new oCacheResult().ToFailConvertJson(ex1.Message, title);
                    }
                    return new oCacheResult().ToOk(innerCache.Cast<dynamic>().ToArray(), Count);
                }
                finally
                {
                    cacheLock.ExitReadLock();
                }
            }
            catch (Exception ex)
            {
                return new oCacheResult().ToFailException(ex.Message, title);
            }
        }

        public string insertItemsByCacheKey(string cacheKey) {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheKey)) {
                cacheLock.EnterWriteLock();
                try
                {
                    try
                    {
                        T[] arr = (T[])cache.Get(cacheKey);
                        this.innerCache.AddRange(arr);
                        this.limit = this.innerCache.Count;
                        cache.Remove(cacheKey);
                    }
                    catch { }
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
            return string.Empty;
        }

        public string getAllJsonReplyCacheKey()
        {
            string key = Guid.NewGuid().ToString();
            oCacheResult rs = getAllJson();
            ObjectCache cache = MemoryCache.Default;
            cache.Set(key, rs, new CacheItemPolicy());
            return key;
        }

        public oCacheResult insertItems(IList items)
        {
            string title = "CacheSynchronized.cs > insertItems(IList items)";
            if (items == null || items.Count == 0)
                return new oCacheResult().ToFailInputNULL("IList items is NULL or empty", title);
            try
            {
                cacheLock.EnterWriteLock();
                try
                {
                    foreach (var o in items)
                        this.innerCache.Add((T)o);
                    this.limit = this.innerCache.Count;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                return new oCacheResult().ToFailException(ex.Message, title);
            }
            return new oCacheResult().ToOkEmpty();
        }

        public oCacheResult insertItem(object item)
        {
            string title = "CacheSynchronized.cs > insertItem(object item)";
            if (item == null)
                return new oCacheResult().ToFailInputNULL("Item for ADD_NEW is NULL or empty", title);
            try
            {
                cacheLock.EnterWriteLock();
                try
                {
                    this.innerCache.Add((T)item);
                    this.limit = this.innerCache.Count;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                return new oCacheResult().ToFailException(ex.Message, title);
            }
            return new oCacheResult().ToOkEmpty();
        }

        public string insertItemReplyCacheKey(string jsonItem)
        {
            string key = Guid.NewGuid().ToString();
            oCacheResult rs = null;

            T item = default(T);
            try
            {
                item = JsonConvert.DeserializeObject<T>(jsonItem);
                rs = insertItem(item);
            }
            catch (Exception ex)
            {
                rs = new oCacheResult().ToFailConvertJson(ex.Message);
            }

            ObjectCache cache = MemoryCache.Default;
            cache.Set(key, rs, new CacheItemPolicy());
            return key;
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

        //////public int[] Search(string condition)
        //////{
        //////    int[] arr = new int[] { };
        //////    cacheLock.EnterReadLock();
        //////    try
        //////    {
        //////        List<int> ls = new List<int>() { };

        //////        //for (int i = 0; i < limit; i++) if (predicate(innerCache[i])) ls.Add(i);
        //////        //var t = innerCache.Where("@0.Contains(\"cd\")").ToArray();
        //////        //arr = innerCache.Where(condition).ToArray();
        //////        //arr = ls.ToArray();
        //////    }
        //////    finally
        //////    {
        //////        cacheLock.ExitReadLock();
        //////    }
        //////    return arr;
        //////}

        public oCacheResult SearchDynamic(oCacheRequest request)
        {
            cacheLock.EnterReadLock();
            try
            {
                try
                {
                    dynamic[] arr = innerCache.Where(request.Conditions).Distinct().Cast<dynamic>().ToArray();
                    return new oCacheResult(request).ToOk(arr, this.Count);
                }
                catch (Exception ex)
                {
                    return new oCacheResult(request).ToFailException(ex.Message, "CacheSynchronized.cs > SearchDynamic(oCacheRequest request)");
                }
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public string searchDynamicReplyCacheKey(oCacheRequest request)
        {
            string key = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(request.RequestId))
                request.RequestId = key;
            else
                key = request.RequestId;
            oCacheResult rs = SearchDynamic(request);
            ObjectCache cache = MemoryCache.Default;
            cache.Set(key, rs, new CacheItemPolicy());
            return key;
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

        public string updateReplyCacheKey(UPDATE_TYPE type, oCacheModel cacheModel, string valKey, string jsonObject)
        {
            string key = Guid.NewGuid().ToString();
            oCacheResult rs = Update(type, cacheModel, valKey, jsonObject);
            ObjectCache cache = MemoryCache.Default;
            cache.Set(key, rs, new CacheItemPolicy());
            return key;
        }

        public oCacheResult Update(UPDATE_TYPE type, oCacheModel cacheModel, string valKey, string jsonObject)
        {
            oCacheRequest request = new oCacheRequest(cacheModel.ServiceName, jsonObject) { RequestId = valKey };
            try
            {
                bool isOk = false;

                oCacheField fkey = new oCacheField() { };
                string fielKey = string.Empty, typeKey = string.Empty;

                if (type != UPDATE_TYPE.ADD)
                {
                    if (cacheModel == null || cacheModel.Fields.Length == 0) return new oCacheResult(request).ToFailInputNULL("The CacheModel with Fields for EDIT|REMOVE is NULL or empty");

                    fkey = cacheModel.Fields.Where(x => x.iskey).SingleOrDefault();
                    if (fkey == null)
                        return new oCacheResult(request).ToFailInputNULL("The CacheModel with Fields for EDIT|REMOVE is NULL or empty");
                    else
                    {
                        fielKey = fkey.name;
                        typeKey = fkey.type;
                    }
                }

                if (string.IsNullOrWhiteSpace(jsonObject)) return new oCacheResult(request).ToFailInputNULL("The Json of item for update is NULL or empty");

                T item = default(T);
                try
                {
                    item = JsonConvert.DeserializeObject<T>(jsonObject);
                }
                catch (Exception ex1)
                {
                    return new oCacheResult(request).ToFailConvertJson(ex1.Message, "Cannot convert Json of item to update");
                }

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
                        return new oCacheResult(request).ToOkEmpty();
                    case UPDATE_TYPE.DELETE:
                        cacheLock.EnterWriteLock();
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(fielKey) && !string.IsNullOrWhiteSpace(typeKey))
                            {
                                fielKey = fkey.name;
                                typeKey = fkey.type;

                                object it = _getItemByKey(fielKey, typeKey, valKey);
                                if (it == null)
                                    return new oCacheResult(request).ToFailInputNULL("DELETE: Cannot not find item has config FIELD_KEY as follows: NAME = " + fielKey + " & KEY = " + valKey + " & TYPE = " + typeKey);
                                else
                                {
                                    itemOld = (T)it;
                                    if (itemOld != null) innerCache.Remove(itemOld);
                                    isOk = true;
                                }
                            }
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        if (isOk)
                            return new oCacheResult(request).ToOkEmpty();
                        else
                            return new oCacheResult(request).ToFailNotFound();
                    case UPDATE_TYPE.EDIT:
                        cacheLock.EnterWriteLock();
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(fielKey) && !string.IsNullOrWhiteSpace(typeKey))
                            {
                                object it = _getItemByKey(fielKey, typeKey, valKey);
                                if (it == null)
                                    return new oCacheResult(request).ToFailInputNULL("EDIT: Cannot not find item has config FIELD_KEY as follows: NAME = " + fielKey + " & KEY = " + valKey + " & TYPE = " + typeKey);
                                else
                                {
                                    itemOld = (T)it;
                                    if (itemOld != null) innerCache.Remove(itemOld);
                                    innerCache.Add(item);
                                    isOk = true;
                                }
                            }
                        }
                        finally
                        {
                            cacheLock.ExitWriteLock();
                        }
                        if (isOk)
                            return new oCacheResult(request).ToOkEmpty();
                        else
                            return new oCacheResult(request).ToFailNotFound();
                }
            }
            catch (Exception ex)
            {
                return new oCacheResult(request).ToFailException(ex.Message);
            }

            return new oCacheResult(request);
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