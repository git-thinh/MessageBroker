using CacheEngineShared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MessageBroker
{
    public class PawnImagesService : BaseServiceCache<oPawnImages> { public PawnImagesService(IDataflowSubscribers dataflow, oCacheModel cacheModel) : base(dataflow, cacheModel) { } }
    public class PawnImagesBehavior : BaseServiceCacheBehavior { public PawnImagesBehavior(object instance) : base(instance) { } }

    public class PawnImagesController : BaseController
    {
        static PawnImagesController()
        {
            initCacheService(_API_CONST.PAWN_IMAGES);
        }

        public static DateTime? ToDateTime(object str)
        {
            try
            {
                return Convert.ToDateTime(str);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string GetTimestamp(object date)
        {
            try
            {
                return ToDateTime(date).Value.ToString("yyyyMMddHHmmssfffffff");
            }
            catch (Exception)
            {
                return DateTime.MinValue.ToString("yyyyMMddHHmmssfffffff");
            }
        }

        [MimeUpload]
        public async Task<oCacheResult> post_uploadImage_DKXM([FromUri]string Pawn_CodeNo, [FromUri]string sPawn_ID, [FromUri]string sPawnImageType_ID)
        {
            if (string.IsNullOrWhiteSpace(sPawn_ID)
                || string.IsNullOrWhiteSpace(sPawnImageType_ID)
                || string.IsNullOrWhiteSpace(Pawn_CodeNo))
                return await Task.FromResult<oCacheResult>(new oCacheResult().ToFailConvertJson("Please check format string json of input."));

            string Asset_ID = "00000017";
            int PawnImageType_ID = 0;
            int Pawn_ID = 0;

            if (int.TryParse(sPawn_ID, out Pawn_ID) == false)
                return await Task.FromResult<oCacheResult>(new oCacheResult().ToFailConvertJson("Please check format string json of input."));

            if (int.TryParse(sPawnImageType_ID, out PawnImageType_ID) == false)
                return await Task.FromResult<oCacheResult>(new oCacheResult().ToFailConvertJson("Please check format string json of input."));

            string folderPath = "Uploads/" + Pawn_CodeNo + "/" + Asset_ID + "/";
            string root = Path.Combine(Path.GetFullPath("../"), "MessageUI");

            string fullPath = Path.Combine(root, folderPath);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
             
            var multiFormDataStreamProvider = new MultiFileUploadProvider(fullPath);
            var file = await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);
            string uploadingFileName = multiFormDataStreamProvider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

            string newName = Path.Combine(fullPath, DateTime.Now.ToString("yyyyMMddHHmmssfff-") + Path.GetFileName(uploadingFileName));
            File.Move(uploadingFileName, newName);

            string absolutePath = Path.Combine(folderPath, Path.GetFileName(newName));

            string _msg_error = "";
             
            var obj = new dtoPawnImages_addNew
            {
                Host = string.Empty,
                Path = absolutePath,
                PawnImageType_ID = PawnImageType_ID,
                Pawn_ID = Pawn_ID
            };

            var rs = sqlExecute<oPawnImages, dtoPawnImages_addNew>("pawn_images_createNew", obj);
            if (rs.Ok == false) _msg_error = absolutePath + " -> " + rs.Message;

            if (rs.Ok && rs.Result.Length > 0)
                obj.PawnImage_ID = ((oPawnImages)rs.Result[0]).PawnImage_ID;

            if (_msg_error.Length > 0) return await Task.FromResult<oCacheResult>(new oCacheResult().ToFailConvertJson(_msg_error));

            reloadCacheByServiceNameArray(new string[] { _API_CONST.PAWN_IMAGES });

            return await Task.FromResult<oCacheResult>(new oCacheResult().ToOk(new dynamic[] { obj }));
        }
    }

    public class dtoPawnImages_addNew
    {
        public int PawnImage_ID { set; get; }
        public long Pawn_ID { set; get; }
        public int PawnImageType_ID { set; get; }
        public string Path { set; get; }
        public string Host { set; get; } 
    }
}