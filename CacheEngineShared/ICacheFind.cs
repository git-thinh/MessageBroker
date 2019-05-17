using System.ServiceModel;

namespace CacheEngineShared
{

    [ServiceContract]
    public interface ICacheFind
    {
        [OperationContract]
        bool push(string arrayItemJson);

        [OperationContract]
        string execute(string conditons);

        [OperationContract]
        bool update(UPDATE_TYPE type, string valKey, string jsonObject);
    }
}
