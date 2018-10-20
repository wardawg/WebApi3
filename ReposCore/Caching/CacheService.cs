using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace ReposCore.Caching
{

    public enum CacheTimes
    {
        OneMinute = 1,
        OneHour = 60,
        TwoHours = 120,
        SixHours = 360,
        TwelveHours = 720,
        OneDay = 1440
    }

    public partial interface ICacheService
    {
        //T Get<T>(string key);
        T Get<T>(string key, CacheTimes cacheTime, Func<T> acquire);

        /// <summary>
        /// Cache objects for a specified amount of time
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="data">Object / Data to cache</param>
        /// <param name="minutesToCache">How many minutes to cache them for</param>
        void Set(string key, object data, CacheTimes minutesToCache);
        bool IsSet(string key);
        void Invalidate(string key);
        void Clear();
        void ClearStartsWith(string keyStartsWith);
        void ClearStartsWith(List<string> keysStartsWith);
        T CachePerRequest<T>(string cacheKey, Func<T> getCacheItem);
        void SetPerRequest(string cacheKey, object objectToCache);
    }

    public partial class CacheService : ICacheService
    {
        #region Long Cache
        private static ObjectCache Cache => MemoryCache.Default;

        private static IDictionaryEnumerator GetCacheToEnumerate()
        {
            return (IDictionaryEnumerator)((IEnumerable)Cache).GetEnumerator();
        }

        public T Get<T>(string key, CacheTimes cacheTime, Func<T> acquire)
        {
            if (IsSet(key))
            {
                return Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                Set(key, result, CacheTimes.OneHour);

            return result;
        }

        private T Get<T>(string key)
        {
            var objectToReturn = Cache[key];
            if (objectToReturn != null)
            {
                if (objectToReturn is T)
                {
                    return (T)objectToReturn;
                }
                try
                {
                    return (T)Convert.ChangeType(objectToReturn, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Cache objects for a specified amount of time
        /// </summary>
        /// <param name="key">The cache key</param>
        /// <param name="data">Object / Data to cache</param>
        /// <param name="minutesToCache">How many minutes to cache them for</param>
        public void Set(string key, object data, CacheTimes minutesToCache)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes((int)minutesToCache)
            };

            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }

        public void Clear()
        {
            var keys = new List<string>();
            var enumerator = GetCacheToEnumerate();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            foreach (var t in keys)
            {
                Cache.Remove(t);
            }
        }

        public void ClearStartsWith(string keyStartsWith)
        {
            var keys = new List<string>();
            var enumerator = GetCacheToEnumerate();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            foreach (var t in keys.Where(x => x.StartsWith(keyStartsWith)))
            {
                Cache.Remove(t);
            }
        }

        public void ClearStartsWith(List<string> keysStartsWith)
        {
            var keys = new List<string>();
            var enumerator = GetCacheToEnumerate();

            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            foreach (var keyStartsWith in keysStartsWith)
            {
                var startsWith = keyStartsWith;
                foreach (var t in keys.Where(x => x.StartsWith(startsWith)))
                {
                    Cache.Remove(t);
                }
            }

        }
        #endregion

        #region Short Per Request Cache

        public T CachePerRequest<T>(string cacheKey, Func<T> getCacheItem)
        {
            if (HttpContext.Current != null)
            {
                if (!HttpContext.Current.Items.Contains(cacheKey))
                {
                    var result = getCacheItem();
                    if (result != null)
                    {
                        SetPerRequest(cacheKey, result);
                        return result;
                    }
                    return default(T);
                }
                return (T)HttpContext.Current.Items[cacheKey];
            }
            return getCacheItem();
        }

        public void SetPerRequest(string cacheKey, object objectToCache)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Add(cacheKey, objectToCache);
            }
        }

        #endregion

    }
}
