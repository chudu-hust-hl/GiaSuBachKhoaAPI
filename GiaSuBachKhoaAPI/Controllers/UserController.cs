using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using GiaSuBK.MD.GiaSuBKMessages;
using System.Web.Script.Serialization;
using System.Threading.Tasks;


namespace GiaSuBKAPI.Controllers
{
    public class UserController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        /// <summary>
        /// Hàm ghi log
        /// </summary>
        /// <param name="method"></param>
        /// <param name="inputStr"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static bool WriteIncommingMessage2Log(string method, string inputStr, int msgType)
        {
            if (WebConfigurationManager.AppSettings["IsSaveMessage"] != null && WebConfigurationManager.AppSettings["IsSaveMessage"] == "1")
            {
                try
                {
                    if (msgType == 0)
                        Log.Debug(method + "-Req: " + inputStr);
                    else
                        Log.Debug(method + "-Res: " + inputStr);
                    return true;
                }
                catch (Exception exx)
                {
                    Log.Warn("WriteLogEx: " + exx.Message.ToString());
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Kiểm tra OTP quên mật khảu
        /// </summary>
        /// <param name="objReq"></param>
        /// <returns></returns>
        [Route("User/UserLogin")]
        [HttpPost]
        public UserLoginRes UserLogin(UserLoginReq objReq)
        {
            var objRes = new UserLoginRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("UserLogin", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.Users().UserLogin(objReq);
                if (!WriteIncommingMessage2Log("UserLogin", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }


        //Start my own from here
        [Route("GSUser/LoginAdmin")]
        [HttpPost]
        public AdminLoginRes AdminLogin(AdminLoginReq objReq)
        {
            var objRes = new AdminLoginRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("UserLogin", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.Admin().AdminLogin(objReq);
                if (!WriteIncommingMessage2Log("UserLogin", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }


        [Route("User/GSGetZaloUserInfo")]
        [HttpPost]
        public GSGetZaloUserInfoRes GSGetZaloUserInfo(GSGetZaloUserInfoReq objReq)
        {
            var objRes = new GSGetZaloUserInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Zalo user Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetZaloUserInfo().GSGetZaloUserInfo(objReq);
                if (!WriteIncommingMessage2Log("Get Zalo user Info", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }

        [Route("User/GSStoreZaloUserInfo")]
        [HttpPost]
        public GSStoreZaloUserInfoRes GSStoreZaloUserInfo(GSStoreZaloUserInfoReq objReq)
        {
            var objRes = new GSStoreZaloUserInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Store Zalo user Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.StoreZaloUserInfo().GSStoreZaloUserInfo(objReq);
                if (!WriteIncommingMessage2Log("Store Zalo user Info", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }


        [Route("User/GSGetZaloUserPhoneNum")]
        [HttpPost]
        public async Task<GSGetZaloUserPhoneNumRes> GSGetZaloUserPhoneNum(GSGetZaloUserPhoneNumReq objReq)
        {
            var objRes = new GSGetZaloUserPhoneNumRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Zalo user PhoneNum", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = await new GiaSuBK.BLL.GetZaloUserPhoneNum().GSGetZaloUserPhoneNum(objReq);
                if (!WriteIncommingMessage2Log("Get Zalo user PhoneNum", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }


        [Route("User/GSGetCity")]
        [HttpPost]
        public GSGetCityRes GSGetCity(GSGetCityReq objReq)
        {
            var objRes = new GSGetCityRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get City", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetCity().GSGetCity(objReq);
                if (!WriteIncommingMessage2Log("Get City", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }

        [Route("User/GSGetDistrict")]
        [HttpPost]
        public GSGetDistrictRes GSGetDistrict(GSGetDistrictReq objReq)
        {
            var objRes = new GSGetDistrictRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get District", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetDistrict().GSGetDistrict(objReq);
                if (!WriteIncommingMessage2Log("Get District", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }

        [Route("User/GSGetCommune")]
        [HttpPost]
        public GSGetCommuneRes GSGetCommune(GSGetCommuneReq objReq)
        {
            var objRes = new GSGetCommuneRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Commune", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetCommune().GSGetCommune(objReq);
                if (!WriteIncommingMessage2Log("Get Commune", js.Serialize(objRes), 1))
                    Log.Warn("Loi ghi log ban tin response");
            }
            catch (WebException wex)
            {
                objRes.RespCode = 8;
                objRes.RespText = wex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));
            }
            catch (Exception ex)
            {
                objRes.RespCode = 9;
                objRes.RespText = ex.Message;
                Log.Error(string.Format("[{0}: {1}]", objRes.RespCode, objRes.RespText));

            }
            return objRes;
        }

    }
}