namespace MessageShared
{
    public enum MESSAGE_TYPE
    {
        LOG = 1,

        DB_UPDATE = 1000,

        CACHE_WEBAPI_REGISTER = 2001,
        CACHE_SETUP = 2002,

        CACHE_UPDATE_ADD = 2003,
        CACHE_UPDATE_EDIT = 2004,
        CACHE_UPDATE_DELETE = 2005,

        BROADCAST_ALL = 3000
    }
}
