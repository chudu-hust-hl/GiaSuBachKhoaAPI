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
    public class GetExcelStudentReqList
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetExcelStudentReqListRes GSGetExcelStudentReqList(GSGetExcelStudentReqListReq objReq)
        {
            GSGetExcelStudentReqListRes objRes = new GSGetExcelStudentReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
                TotalRows = 0,
                StudentList = new StudentList()
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
                        objRes.RespText = "Session expired or invalid token";
                        Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                        return objRes;
                    }

                    // Fetch Student List
                    var studentListData = db.GS_GetExcelStudentLst(objReq.PageNumber, objReq.RowsPage, objReq.Search).ToList();

                    // Map database result to GSStudentInfo objects
                    if (studentListData != null)
                    {
                        foreach (var p in studentListData)
                        {
                            var studentInfo = new GSStudentInfo
                            {
                                RowID = p.RowID,
                                StudentID = p.StudentID,
                                StudentName = p.StudentName,
                                Phone = p.Phone,
                                Address = p.Address,
                                FormTeach = p.FormTeach,
                                InfoMore = p.InfoMore,
                                Level = p.Level,
                                SexStudent = p.SexStudent,
                                SelectSchool = p.SelectSchool,
                                Status = (int)p.Status,
                                TimeCreate = p.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                NameSupports = p.NameSupports,
                                Subjects = p.Subjects,
                                TimeSupport = p.TimeSupport,
                                SkillSupport = p.SkillSupport,
                                District = p.District,
                                City = p.City,
                                Ward = p.Ward,
                                Experience = p.Experience,
                                Achievement = p.Achievement,
                                TimeModify = p.TimeModify?.ToString("yyyy-MM-dd HH:mm:ss"),
                                ModifierID = p.ModifierID
                            };

                            // Add each GSStudentInfo to the StudentList
                            objRes.StudentList.Add(studentInfo);
                        }

                        // Extract TotalRows from the first row (if available)
                        if (studentListData.Count > 0)
                        {
                            objRes.TotalRows = studentListData[0].TotalRows ?? 0;
                        }
                    }

                    // Success response
                    objRes.RespCode = 0;
                    objRes.RespText = "OK";
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