using System.ServiceModel;

namespace CacheEngineShared
{

    [ServiceContract]
    public interface ICacheFind
    {
        [OperationContract]
        string execute();
    }
}
