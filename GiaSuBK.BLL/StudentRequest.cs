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
    public class StudentRequest
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSCreateStudentInfoRes GSCreateStudentInfo(GSCreateStudentInfoReq objReq)
        {
            GSCreateStudentInfoRes objRes = new GSCreateStudentInfoRes
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
                        //if (userToken == null )
                        //{
                        //    objRes.RespCode = -1;
                        //    objRes.RespText = "Session expired or invalid token code";
                        //    Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                        //    return objRes;
                        //}

                        var random = new Random();
                        var randomCode = random.Next(10000, 99999); // Generate a 5-digit random number
                        var reqParentID = $"{objReq.StudentInfo.Phone}-{randomCode}";

                        var existStudent = db.GS_Students.Where(p => p.StudentID == objReq.StudentInfo.StudentID).FirstOrDefault();
                        if (existStudent == null)
                        {
                            // Create new GS_Student instance
                            var newStudent = new GS_Student
                            {
                                StudentID = objReq.StudentInfo.StudentID,
                                Phone = objReq.StudentInfo.Phone,
                                StudentName = objReq.StudentInfo.StudentName,
                                City = objReq.StudentInfo.City,
                                District = objReq.StudentInfo.District,
                                Ward = objReq.StudentInfo.Ward,
                                Address = objReq.StudentInfo.Address,
                                Subjects = objReq.StudentInfo.Subjects,
                                FormTeach = objReq.StudentInfo.FormTeach,
                                Experience = objReq.StudentInfo.Experience,
                                Achievement = objReq.StudentInfo.Achievement,
                            };

                            db.GS_Students.InsertOnSubmit(newStudent);
                            db.SubmitChanges();
                        }

                        // Create new GS_ReqStudent object
                        var newStudentReq = new GS_ReqStudent
                        {
                            StudentID = objReq.StudentInfo.StudentID,
                            StudentName = objReq.StudentInfo.StudentName,
                            Phone = objReq.StudentInfo.Phone,
                            Address = objReq.StudentInfo.Address,
                            FormTeach = objReq.StudentInfo.FormTeach,
                            InfoMore = objReq.StudentInfo.InfoMore,
                            Level = objReq.StudentInfo.Level,
                            SexStudent = objReq.StudentInfo.SexStudent,
                            SelectSchool = objReq.StudentInfo.SelectSchool,
                            TimeCreate = DateTime.UtcNow,
                            NameSupports = objReq.StudentInfo.NameSupports,
                            Subjects = objReq.StudentInfo.Subjects,
                            TimeSupport = objReq.StudentInfo.TimeSupport,
                            SkillSupport = objReq.StudentInfo.SkillSupport,
                            District = objReq.StudentInfo.District,
                            City = objReq.StudentInfo.City,
                            Ward = objReq.StudentInfo.Ward,
                            Experience = objReq.StudentInfo.Experience,
                            Achievement = objReq.StudentInfo.Achievement,
                            TimeModify = null,
                            ModifierID = null
                        };

                        // Add and save changes
                        db.GS_ReqStudents.InsertOnSubmit(newStudentReq);
                        db.SubmitChanges();

                        objRes.RespCode = 0;
                        objRes.RespText = "Student info created successfully";
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
