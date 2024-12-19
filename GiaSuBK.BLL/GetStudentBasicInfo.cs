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
    public class GetStudentBasicInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSGetStudentBasicInfoRes GSGetStudentBasicInfo(GSGetStudentBasicInfoReq objReq)
        {
            GSGetStudentBasicInfoRes objRes = new GSGetStudentBasicInfoRes
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

                        var existStudent = db.GS_Students.Where(p => p.StudentID == objReq.StudentID).FirstOrDefault();
                        if (existStudent == null)
                        {
                            // Update the Apply list for the existing Student
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            objRes.StudentInfo = new GSStudentBasicInfo
                            {
                                RowID = existStudent.RowID,
                                StudentID = existStudent.StudentID,
                                StudentName = existStudent.StudentName,
                                Phone = existStudent.Phone,
                                Address = existStudent.Address,
                                FormTeach = existStudent.FormTeach,
                                InfoMore = existStudent.InfoMore,
                                SexStudent = existStudent.SexStudent,
                                Subjects = existStudent.Subjects,
                                TimeSupport = existStudent.TimeSupport,
                                District = existStudent.District,
                                City = existStudent.City,
                                Ward = existStudent.Ward,
                                Achievement = existStudent.Achievement,
                                Experience = existStudent.Experience,
                            };
                        }

                        objRes.RespCode = 0;
                        objRes.RespText = "Success";
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
