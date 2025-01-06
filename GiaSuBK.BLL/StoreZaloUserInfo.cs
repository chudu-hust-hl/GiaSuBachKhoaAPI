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
    public class StoreZaloUserInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSStoreZaloUserInfoRes GSStoreZaloUserInfo(GSStoreZaloUserInfoReq objReq)
        {
            GSStoreZaloUserInfoRes objRes = new GSStoreZaloUserInfoRes
            {
                RespCode = -1,
                RespText = "Nothing"
            };

            try
            {
                // Connect to the database
                using (DataClassesGiaSuBKDataContext db = new DataClassesGiaSuBKDataContext())
                {
                    try
                    {
                        // Validate token
                        if (objReq.Token == null || objReq.UserID == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Not a valid user authentication";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }

                        var existTokenCode = db.GS_ZaloUserInfos.Where(p => p.TokenCode == objReq.Token).FirstOrDefault();
                        if(existTokenCode != null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "The Token code have exist and do not need to update new one";
                            return objRes;
                        }

                        // Store new ZaloUserInfo
                        var newZaloUser = new GS_ZaloUserInfo
                        { 
                            ZaloUserID = objReq.UserID,
                            TokenCode = objReq.Token,
                            Avatar = objReq.ZaloUserInfo.Avatar,
                            PhoneNumber = objReq.ZaloUserInfo.PhoneNumber,
                            StudentID = objReq.ZaloUserInfo.StudentID                            
                        };

                        db.GS_ZaloUserInfos.InsertOnSubmit(newZaloUser);
                        db.SubmitChanges();


                        objRes.RespCode = 0;
                        objRes.RespText = "User Info Stored successfully";
                        return objRes;
                    }
                    catch (Exception ex)
                    {
                        objRes.RespCode = 109;
                        objRes.RespText = ex.Message;
                        Log.Error(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                    }
                }
            }
            catch (Exception ex)
            {
                objRes.RespCode = 109;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
            }

            return objRes;
        }
    }
}
