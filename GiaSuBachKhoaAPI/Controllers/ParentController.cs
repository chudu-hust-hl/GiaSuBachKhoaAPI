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

namespace GiaSuBKAPI.Controllers
{
    public class ParentController : ApiController
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

        //Create Parent Request
        [Route("GSParent/CreateParentInfo")]
        [HttpPost]
        public GSCreateParentInfoRes GSCreateParentInfo(GSCreateParentInfoReq objReq)
        {
            var objRes = new GSCreateParentInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Create Parent Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.ParentRequest().GSCreateParentInfo(objReq);
                if (!WriteIncommingMessage2Log("Create Parent request", js.Serialize(objRes), 1))
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

        //Update Parent Request
        [Route("GSParent/UpdateParentInfo")]
        [HttpPost]
        public GSUpdateParentInfoRes GSUpdateParentInfo(GSUpdateParentInfoReq objReq)
        {
            var objRes = new GSUpdateParentInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Update Parent Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.UpdateParentInfo().GSUpdateParentInfo(objReq);
                if (!WriteIncommingMessage2Log("Update Parent request", js.Serialize(objRes), 1))
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


        //Get Request Parent Info
        [Route("GSParent/GetParentReqInfo")]
        [HttpPost]
        public GSGetParentReqInfoRes GSGetParentReqInfo(GSGetParentReqInfoReq objReq)
        {
            var objRes = new GSGetParentReqInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Parent Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetParentReqInfo().GSGetParentReqInfo(objReq);
                if (!WriteIncommingMessage2Log("Get Parent request", js.Serialize(objRes), 1))
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

        [Route("GSParent/GetParentReqList")]
        [HttpPost]
        public GSGetParentReqListRes GSGetParentReqList(GSGetParentReqListReq objReq)
        {
            var objRes = new GSGetParentReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Parent Request List", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetParentReqList().GSGetParentReqList(objReq);
                if (!WriteIncommingMessage2Log("Get Parent request list", js.Serialize(objRes), 1))
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