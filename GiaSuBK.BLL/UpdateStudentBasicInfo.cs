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
    public class UpdateStudentBasicInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSUpdateStudentBasicInfoRes GSUpdateStudentBasicInfo(GSUpdateStudentBasicInfoReq objReq)
        {
            GSUpdateStudentBasicInfoRes objRes = new GSUpdateStudentBasicInfoRes
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
                        var userToken = db.GS_ZaloUserInfos.Where(p => p.ZaloUserID == objReq.UserID && p.TokenCode == objReq.Token).FirstOrDefault();
                        var adminToken = db.UserTokens.Where(p => p.TokenCode == objReq.Token).FirstOrDefault();
                        if (userToken == null && adminToken == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Session expired or invalid token";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }

                        var existStudent = db.GS_Students.Where(p => p.StudentID == objReq.StudentInfo.StudentID).FirstOrDefault();
                        if (existStudent == null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            // Update new GS_Student instance
                            existStudent.StudentID = objReq.StudentInfo.StudentID;
                            existStudent.StudentName = objReq.StudentInfo.StudentName;
                            existStudent.Phone = objReq.StudentInfo.Phone;
                            existStudent.Address = objReq.StudentInfo.Address;
                            existStudent.FormTeach = objReq.StudentInfo.FormTeach;
                            existStudent.InfoMore = objReq.StudentInfo.InfoMore;
                            existStudent.SexStudent = objReq.StudentInfo.SexStudent;
                            existStudent.Subjects = objReq.StudentInfo.Subjects;
                            existStudent.TimeSupport = objReq.StudentInfo.TimeSupport;
                            existStudent.District = objReq.StudentInfo.District;
                            existStudent.City = objReq.StudentInfo.City;
                            existStudent.Ward = objReq.StudentInfo.Ward;
                            existStudent.Achievement = objReq.StudentInfo.Achievement;
                            existStudent.Experience = objReq.StudentInfo.Experience;

                            // Update done then save change
                            db.SubmitChanges();
                        }

                        objRes.RespCode = 0;
                        objRes.RespText = "Student info updated successfully";
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
