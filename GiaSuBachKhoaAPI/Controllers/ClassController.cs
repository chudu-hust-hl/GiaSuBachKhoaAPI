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
    public class ClassController : ApiController
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

        //Get Class Info
        [Route("GSClass/GetClassInfo")]
        [HttpPost]
        public GSGetClassInfoRes GSGetClassInfo(GSGetClassInfoReq objReq)
        {
            var objRes = new GSGetClassInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Class Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetClassInfo().GSGetClassInfo(objReq);
                if (!WriteIncommingMessage2Log("Get Class Info", js.Serialize(objRes), 1))
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


        //Update Class Info
        [Route("GSClass/UpdateClassInfo")]
        [HttpPost]
        public GSUpdateClassInfoRes GSUpdateClassInfo(GSUpdateClassInfoReq objReq)
        {
            var objRes = new GSUpdateClassInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Update Class Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.UpdateClassInfo().GSUpdateClassInfo(objReq);
                if (!WriteIncommingMessage2Log("Update Class Info", js.Serialize(objRes), 1))
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

        //GetStudent Comment
        [Route("GSClass/GetStudentComment")]
        [HttpPost]
        public GSStudentCmtRes GSStudentCmt(GSStudentCmtReq objReq)
        {
            var objRes = new GSStudentCmtRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Comment", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetStudentComment().GSStudentCmt(objReq);
                if (!WriteIncommingMessage2Log("Get Student Comment", js.Serialize(objRes), 1))
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

        //Get parent comment
        [Route("GSClass/GetParentComment")]
        [HttpPost]
        public GSParentCmtRes GSParentCmt(GSParentCmtReq objReq)
        {
            var objRes = new GSParentCmtRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get parent comment", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetParentComment().GSParentCmt(objReq);
                if (!WriteIncommingMessage2Log("Get parent comment", js.Serialize(objRes), 1))
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

        //Create Lesson
        [Route("GSClass/CreateStudentDailyComment")]
        [HttpPost]
        public GSCreateStudentDailyCmtRes GSCreateStudentDailyCmt(GSCreateStudentDailyCmtReq objReq)
        {
            var objRes = new GSCreateStudentDailyCmtRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Create lesson", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.CreateStudentDailyCmt().GSCreateStudentDailyCmt(objReq);
                if (!WriteIncommingMessage2Log("Create Lesson", js.Serialize(objRes), 1))
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

        //Get Student List
        [Route("GSStudent/GetClassReqList")]
        [HttpPost]
        public GSGetClassReqListRes GSGetClassReqList(GSGetClassReqListReq objReq)
        {
            var objRes = new GSGetClassReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Req List Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetClassList().GSGetClassReqList(objReq);
                if (!WriteIncommingMessage2Log("Get Student Req List Info", js.Serialize(objRes), 1))
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