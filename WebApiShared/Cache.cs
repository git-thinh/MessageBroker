using System;


namespace WebApiShared
{
    public interface ICache {
        string getByKey(string key);
    }

    public class StoreCache : ICache
    {
        public string getByKey(string key)
        {
            return DateTime.Now.ToString();
        }
    }
}