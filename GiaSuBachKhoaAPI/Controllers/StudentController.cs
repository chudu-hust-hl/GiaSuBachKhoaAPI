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
    public class StudentController : ApiController
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

        //Create Student Request
        [Route("GSStudent/CreateStudentInfo")]
        [HttpPost]
        public GSCreateStudentInfoRes GSCreateStudentInfo(GSCreateStudentInfoReq objReq)
        {
            var objRes = new GSCreateStudentInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Create Student Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.StudentRequest().GSCreateStudentInfo(objReq);
                if (!WriteIncommingMessage2Log("Create Student request", js.Serialize(objRes), 1))
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

        //Update Student Request
        [Route("GSStudent/UpdateStudentReqInfo")]
        [HttpPost]
        public GSUpdateStudentInfoRes GSUpdateStudentInfo(GSUpdateStudentInfoReq objReq)
        {
            var objRes = new GSUpdateStudentInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Update Student Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.UpdateStudentInfo().GSUpdateStudentInfo(objReq);
                if (!WriteIncommingMessage2Log("Update Student request", js.Serialize(objRes), 1))
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


        //Get Request Student Info
        [Route("GSStudent/GetStudentInfo")]
        [HttpPost]
        public GSGetStudentReqInfoRes GSGetStudentReqInfo(GSGetStudentReqInfoReq objReq)
        {
            var objRes = new GSGetStudentReqInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Request", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetStudentReqInfo().GSGetStudentReqInfo(objReq);
                if (!WriteIncommingMessage2Log("Get Student request", js.Serialize(objRes), 1))
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

        //Get Student Basic info
        [Route("GSStudent/GetStudentBasicInfo")]
        [HttpPost]
        public GSGetStudentBasicInfoRes GSGetStudentBasicInfo(GSGetStudentBasicInfoReq objReq)
        {
            var objRes = new GSGetStudentBasicInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Basic Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetStudentBasicInfo().GSGetStudentBasicInfo(objReq);
                if (!WriteIncommingMessage2Log("Get Student Basic Info", js.Serialize(objRes), 1))
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


        //Update Student Basic info
        [Route("GSStudent/UpdateStudentBasicInfo")]
        [HttpPost]
        public GSUpdateStudentBasicInfoRes GSUpdateStudentBasicInfo(GSUpdateStudentBasicInfoReq objReq)
        {
            var objRes = new GSUpdateStudentBasicInfoRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Update Student Basic Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.UpdateStudentBasicInfo().GSUpdateStudentBasicInfo(objReq);
                if (!WriteIncommingMessage2Log("Update Student Basic Info", js.Serialize(objRes), 1))
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

        //Update Student Basic info
        [Route("GSStudent/StudentApplyClass")]
        [HttpPost]
        public GSStudentApplyClassRes GSStudentApplyClass(GSStudentApplyClassReq objReq)
        {
            var objRes = new GSStudentApplyClassRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Student Apply a Class", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.StudentApplyClass().GSStudentApplyClass(objReq);
                if (!WriteIncommingMessage2Log("Student Apply a Class", js.Serialize(objRes), 1))
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
        [Route("GSStudent/GetStudentReqList")]
        [HttpPost]
        public GSGetStudentReqListRes GSGetStudentReqList(GSGetStudentReqListReq objReq)
        {
            var objRes = new GSGetStudentReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Req List Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetStudentReqList().GSGetStudentReqList(objReq);
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


        //Get Student Excel
        [Route("GSStudent/GetExcelStudentReqList")]
        [HttpPost]
        public GSGetExcelStudentReqListRes GSGetExcelStudentReqList(GSGetExcelStudentReqListReq objReq)
        {
            var objRes = new GSGetExcelStudentReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
            };
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                if (!WriteIncommingMessage2Log("Get Student Excel Req List Info", js.Serialize(objReq), 0))
                    Log.Warn("Loi ghi log ban tin request");
                objRes = new GiaSuBK.BLL.GetExcelStudentReqList().GSGetExcelStudentReqList(objReq);
                if (!WriteIncommingMessage2Log("Get Student Excel Req List Info", js.Serialize(objRes), 1))
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