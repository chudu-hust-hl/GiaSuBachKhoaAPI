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
    public class StudentApplyClass
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSStudentApplyClassRes GSStudentApplyClass(GSStudentApplyClassReq objReq)
        {
            GSStudentApplyClassRes objRes = new GSStudentApplyClassRes
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
                        var existClass = db.GS_Classes.Where(c => c.ClassID == objReq.ClassID).FirstOrDefault();
                        if (existStudent == null || existClass == null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            // Add the student ID to the Apply list of the class
                            var applyList = existClass.Apply;
                            if (string.IsNullOrEmpty(applyList))
                            {
                                existClass.Apply = objReq.StudentID.ToString();
                            }
                            else
                            {
                                var studentIds = applyList.Split(',').Select(id => id.Trim()).ToList();
                                if (!studentIds.Contains(objReq.StudentID.ToString()))
                                {
                                    studentIds.Add(objReq.StudentID.ToString());
                                    existClass.Apply = string.Join(",", studentIds);
                                }
                            }
                            existClass.NumberApply = existClass.NumberApply + 1;

                            var studentApplyList = existStudent.Apply;
                            if (string.IsNullOrEmpty(studentApplyList))
                            {
                                existStudent.Apply = objReq.ClassID.ToString();
                            }
                            else
                            {
                                var classsIds = studentApplyList.Split(',').Select(id => id.Trim()).ToList();
                                if (!classsIds.Contains(objReq.ClassID.ToString()))
                                {
                                    classsIds.Add(objReq.ClassID.ToString());
                                    existStudent.Apply = string.Join(",", classsIds);
                                }
                            }


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
