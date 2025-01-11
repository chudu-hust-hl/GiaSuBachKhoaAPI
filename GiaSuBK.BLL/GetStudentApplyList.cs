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
    public class GetStudentApplyList
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetStudentApplyListRes GSGetStudentApplyList(GSGetStudentApplyListReq objReq)
        {
            GSGetStudentApplyListRes objRes = new GSGetStudentApplyListRes
            {
                RespCode = -1,
                RespText = "Nothing",
                StudentList = new StudentBasicList()
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

                    var existClass = db.GS_Classes.FirstOrDefault(p => p.ClassID == objReq.ClassID);
                    if(existClass == null)
                    {
                        objRes.RespCode = -2;
                        objRes.RespText = "No exist Class";
                        Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                        return objRes;
                    }

                    objRes.TotalApplyNumber = (int)existClass.NumberApply;

                    // Fetch Student List
                    var studentListData = db.GS_GetStudentApplyClass(objReq.ClassID).ToList();

                    // Map database result to GSStudentInfo objects
                    if (studentListData != null)
                    {
                        foreach (var p in studentListData)
                        {
                            var studentInfo = new GSStudentBasicInfo
                            {
                                StudentID = p.StudentID,
                                StudentName = p.StudentName,
                                Phone = p.Phone,
                                Address = p.Address,
                                FormTeach = p.FormTeach,
                                InfoMore = p.InfoMore,
                                SexStudent = p.SexStudent,
                                Subjects = p.Subjects,
                                TimeSupport = p.TimeSupport,
                                District = p.District,
                                City = p.City,
                                Ward = p.Ward,
                                Achievement = p.Achievement,
                                Experience = p.Experience,
                                Apply = p.Apply,
                                Teaching = p.Teaching,
                                Done = p.Done,
                            };

                            // Add each GSStudentInfo to the StudentList
                            objRes.StudentList.Add(studentInfo);
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