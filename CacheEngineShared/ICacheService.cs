using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace CacheEngineShared
{

    [ServiceContract]
    public interface ICacheService
    {
        [OperationContract]
        string insertItemsByCacheKey(string cacheKey);

        [OperationContract]
        string insertItemReplyCacheKey(string jsonItem);

        [OperationContract]
        bool insertItems(IList items);

        [OperationContract]
        bool push(string arrayItemJson);
        
        [OperationContract]
        string executeReplyCacheKey(string conditons);
        
        [OperationContract]
        string updateReplyCacheKey(UPDATE_TYPE type, string valKey, string jsonObject);

        [OperationContract]
        string getAllJsonReplyCacheKey();

        [OperationContract]
        void initDataFromDbStore(string storeName);         
    }
}
 

