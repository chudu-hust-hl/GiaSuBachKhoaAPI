using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiaSuBK.MD.GiaSuBKMessages;
using GiaSuBK.DAL;

using System.Globalization;
using System.Web.Configuration;
using log4net;

namespace GiaSuBK.BLL
{
    public class GetCommune
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetCommuneRes GSGetCommune(GSGetCommuneReq objReq)
        {
            GSGetCommuneRes objRes = new GSGetCommuneRes
            {
                RespCode = -1,
                RespText = "Nothing",
                LocationLst = new LocationList()
            };

            try
            {
                // Connect to the database
                using (DataClassesGiaSuBKDataContext db = new DataClassesGiaSuBKDataContext())
                {
                    var CommuneData = db.GetCommune(objReq.City, objReq.District).ToList();

                    if (CommuneData != null)
                    {
                        foreach (var p in CommuneData)
                        {
                            objRes.LocationLst.Add(p.Commune);
                        }
                        // Success response
                        objRes.RespCode = 0;
                        objRes.RespText = "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                objRes.RespCode = 109;
                objRes.RespText = ex.Message;
                Log.Error($"[{objRes.RespCode}:{objRes.RespText}]");
            }

            return objRes;
        }
    }
}
