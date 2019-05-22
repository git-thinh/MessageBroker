using CacheEngineShared;
using MessageShared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MessageBroker
{
    public class JobPdfExport : JobBase
    {
        public override void freeResource() { dbStop(); }

        ////////////////////////////////////////////////////////////////////
        /// 

        private static bool _inited = false;
        private readonly mDbUpdateRequest _requestUpdateDb;
        public JobPdfExport(mDbUpdateRequest requestUpdateDb = null)
        {
            _requestUpdateDb = requestUpdateDb;
        }

        ////////////////////////////////////////////////////////////////////
        /// 
         
        static void dbStart()
        { 
        }

        static void dbStop() { 
        }

        ////////////////////////////////////////////////////////////////////
        /// 

        public override void execute()
        {
            if (!_inited)
            {
                _inited = true;
                dbStart();
                return;
            }

            if (_requestUpdateDb == null) return; 
        }
         
    }
}
