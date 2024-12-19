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
    public class UpdateClassInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSUpdateClassInfoRes GSUpdateClassInfo(GSUpdateClassInfoReq objReq)
        {
            GSUpdateClassInfoRes objRes = new GSUpdateClassInfoRes
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

                        var existClass = db.GS_Classes.Where(c => c.ClassID == objReq.ClassID).FirstOrDefault();
                        if (existClass == null)
                        {
                            // Update the Apply list for the existing Student
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            // Update new GS_Parent instance
                            existClass.ClassID = objReq.ClassInfo.ClassID;
                            existClass.NameParent = objReq.ClassInfo.NameParent;
                            existClass.PhoneEmail = objReq.ClassInfo.PhoneParent;
                            existClass.District = objReq.ClassInfo.District;
                            existClass.City = objReq.ClassInfo.City;
                            existClass.Ward = objReq.ClassInfo.Ward;
                            existClass.StudentID = objReq.ClassInfo.StudentID;
                            existClass.StudentName = objReq.ClassInfo.StudentName;
                            existClass.PhoneStudent = objReq.ClassInfo.PhoneStudent;
                            existClass.Status = objReq.ClassInfo.Status;
                            existClass.Apply = objReq.ClassInfo.Apply;
                            existClass.NumberApply = objReq.ClassInfo.NumberApply;
                            existClass.Money = objReq.ClassInfo.Money;
                            existClass.MoneyStatus = objReq.ClassInfo.MoneyStatus;
                            existClass.FormTeach = objReq.ClassInfo.FormTeach;
                            existClass.InfoMore = objReq.ClassInfo.InfoMore;
                            existClass.Level = objReq.ClassInfo.Level;
                            existClass.ValueClass = objReq.ClassInfo.ValueClass;
                            existClass.Status = objReq.ClassInfo.Status;
                            existClass.NameSupports = objReq.ClassInfo.NameSupports;
                            existClass.Subjects = objReq.ClassInfo.Subjects;

                            // Update done then save change
                            db.SubmitChanges();
                        }

                        objRes.RespCode = 0;
                        objRes.RespText = "Parent info created successfully";
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
