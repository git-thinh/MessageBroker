using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace CacheEngineShared
{

    [ServiceContract]
    public interface ICacheService
    {
        [OperationContract]
        bool insertItems(IList items);

        [OperationContract]
        bool push(string arrayItemJson);

        [OperationContract]
        string execute(string conditons);

        [OperationContract]
        bool update(UPDATE_TYPE type, string valKey, string jsonObject);
    }
}
