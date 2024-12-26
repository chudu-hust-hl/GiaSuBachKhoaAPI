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
    public class UpdateStudentInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSUpdateStudentInfoRes GSUpdateStudentInfo(GSUpdateStudentInfoReq objReq)
        {
            GSUpdateStudentInfoRes objRes = new GSUpdateStudentInfoRes
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

                        var existStudentReq = db.GS_ReqStudents.Where(p => p.RowID == objReq.StudentInfo.RowID).FirstOrDefault();
                        if (existStudentReq == null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            // Update new GS_Student instance
                            existStudentReq.StudentID = objReq.StudentInfo.StudentID;
                            existStudentReq.StudentName = objReq.StudentInfo.StudentName;
                            existStudentReq.Phone = objReq.StudentInfo.Phone;
                            existStudentReq.Address = objReq.StudentInfo.Address;
                            existStudentReq.FormTeach = objReq.StudentInfo.FormTeach;
                            existStudentReq.InfoMore = objReq.StudentInfo.InfoMore;
                            existStudentReq.Level = objReq.StudentInfo.Level;
                            existStudentReq.SexStudent = objReq.StudentInfo.SexStudent;
                            existStudentReq.SelectSchool = objReq.StudentInfo.SelectSchool;
                            existStudentReq.NameSupports = objReq.StudentInfo.NameSupports;
                            existStudentReq.Subjects = objReq.StudentInfo.Subjects;
                            existStudentReq.TimeSupport = objReq.StudentInfo.TimeSupport;
                            existStudentReq.SkillSupport = objReq.StudentInfo.SkillSupport;
                            existStudentReq.District = objReq.StudentInfo.District;
                            existStudentReq.City = objReq.StudentInfo.City;
                            existStudentReq.Ward = objReq.StudentInfo.Ward;
                            existStudentReq.Achievement = objReq.StudentInfo.Achievement;
                            existStudentReq.Experience = objReq.StudentInfo.Experience;
                            existStudentReq.TimeModify = DateTime.UtcNow;
                            existStudentReq.ModifierID = null;

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
