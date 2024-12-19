using System;
using System.Linq;
using GiaSuBK.MD.GiaSuBKMessages;
using GiaSuBK.DAL;
using System.Web.Configuration;
using log4net;

namespace GiaSuBK.BLL
{
    public class CreateStudentDailyCmt
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSCreateStudentDailyCmtRes GSCreateStudentDailyCmt(GSCreateStudentDailyCmtReq objReq)
        {
            GSCreateStudentDailyCmtRes objRes = new GSCreateStudentDailyCmtRes
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
                        var userToken = db.GS_ZaloUserInfos.FirstOrDefault(p => p.ZaloUserID == objReq.UserID && p.TokenCode == objReq.Token);
                        var adminToken = db.UserTokens.FirstOrDefault(p => p.TokenCode == objReq.Token);

                        if (userToken == null && adminToken == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Session expired or invalid token code";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        // Check if the student exists
                        var existStudent = db.GS_Students.FirstOrDefault(p => p.StudentID == objReq.StudentID);
                        if (existStudent == null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "Student not found or cannot fetch info";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        // Check if the class exists
                        var existClass = db.GS_Classes.FirstOrDefault(c => c.ClassID == objReq.ClassID);
                        if (existClass == null)
                        {
                            objRes.RespCode = -3;
                            objRes.RespText = "Class not found";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        var newLesson = new GS_Lesson
                        {
                            ClassID = objReq.ClassID,
                            LessonID = $"{objReq.ClassID}{objReq.Date}",
                            Date = objReq.Date,
                            Comment = objReq.Comment,
                        };

                        // Save changes to the database
                        db.GS_Lessons.InsertOnSubmit(newLesson);
                        db.SubmitChanges();

                        objRes.RespCode = 0;
                        objRes.RespText = "Comment added successfully";
                        return objRes;
                    }
                    catch (Exception ex)
                    {
                        objRes.RespCode = 109;
                        objRes.RespText = ex.Message;
                        Log.Error($"[{objRes.RespCode}:{objRes.RespText}]");
                    }
                }
            }
            catch (Exception ex)
            {
                objRes.RespCode = 109;
                objRes.RespText = ex.Message;
                Log.Error($"[{objRes.RespCode}:{objRes.RespText}]");
            }

            return objRes;
        }
    }
}
