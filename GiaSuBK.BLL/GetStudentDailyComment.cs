using System;
using System.Linq;
using GiaSuBK.MD.GiaSuBKMessages;
using GiaSuBK.DAL;
using System.Web.Configuration;
using log4net;

namespace GiaSuBK.BLL
{
    public class GetStudentDailyComment
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetStudentDailyCmtRes GSGetStudentDailyCmt(GSGetStudentDailyCmtReq objReq)
        {
            GSGetStudentDailyCmtRes objRes = new GSGetStudentDailyCmtRes
            {
                RespCode = -1,
                RespText = "Nothing",
                LessonList = new LessonList(),
            };

            try
            {
                // Connect to the database
                using (DataClassesGiaSuBKDataContext db = new DataClassesGiaSuBKDataContext())
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


                    // Check if the class exists
                    var existClass = db.GS_Classes.FirstOrDefault(c => c.ClassID == objReq.ClassID);
                    if (existClass == null)
                    {
                        objRes.RespCode = -3;
                        objRes.RespText = "Class not found";
                        Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                        return objRes;
                    }
                    else
                    {
                        var lessonListData = db.GS_GetLessonLst(objReq.Month, objReq.ClassID);

                        if (lessonListData != null)
                        {
                            foreach ( var p in lessonListData)
                            {
                                var lessonInfo = new GSLesson
                                {
                                    ClassID = p.ClassID,
                                    LessonID = p.LessonID,
                                    Date = p.Date.ToString("yyyy-MM-dd"),
                                    Status = p.Status,
                                    Comment = p.Comment,
                                };

                                objRes.LessonList.Add(lessonInfo);
                            }
                        }
                    }


                    objRes.RespCode = 0;
                    objRes.RespText = "Comment added successfully";
                    return objRes;
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
