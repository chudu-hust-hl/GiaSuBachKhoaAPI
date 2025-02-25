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
    public class GetCity
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetCityRes GSGetCity(GSGetCityReq objReq)
        {
            GSGetCityRes objRes = new GSGetCityRes
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
                    var cityData = db.GetCity().ToList();

                    if (cityData != null)
                    {
                        foreach (var p in cityData)
                        {
                            objRes.LocationLst.Add(p.City);
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
