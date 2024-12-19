
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
    public class Users
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public UserLoginRes UserLogin(UserLoginReq objReq)
        {
            UserLoginRes objRes = new UserLoginRes
            {
                RespCode = -1,
                RespText = "Nothing"
            };

            try
            {
                //Ket noi du lieu kiem tra
                using (DataClassesGiaSuBKDataContext db = new DataClassesGiaSuBKDataContext())
                {
                    try
                    {

                        //var userToken = db.UserTokens.Where(p => p.UserID == objReq.UserID && p.Token == objReq.Token).FirstOrDefault();
                        //if (userToken == null)
                        //{
                        //    objRes.RespCode = -1;
                        //    objRes.RespText = "Phiên đăng nhập đã hết hạn";
                        //    Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                        //    return objRes;
                        //}
                        //var userCheck = db.UserInfos.Where(p => p.UserID == objReq.UserID).FirstOrDefault();
                        //if (userCheck != null)
                        //{
                        //    userCheck.FullName = objReq.Data.FullName;
                        //    userCheck.Sex = objReq.Data.Sex;
                        //    userCheck.Address = objReq.Data.Address;
                        //    userCheck.Birthday = UtilClass.convertTimeAPI(objReq.Data.Birthday);
                        //    userCheck.Email = objReq.Data.Email;
                        //    userCheck.Phone = objReq.Data.Phone;
                        //    userCheck.UserName = objReq.Data.UserName;
                        //    userCheck.City = objReq.Data.City;
                        //    userCheck.District = objReq.Data.District;
                        //    userCheck.Common = objReq.Data.Common;
                        //    userCheck.Specialize = objReq.Data.Specialize;
                        //    userCheck.Status = 2;
                        //    userCheck.VIP = objReq.Data.VIP;
                        //    userCheck.CodeID = objReq.Data.CodeID;

                        //    db.SubmitChanges();
                        //}

                        //objRes.RespCode = 0;
                        //objRes.RespText = "OK";
                        //return objRes;
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