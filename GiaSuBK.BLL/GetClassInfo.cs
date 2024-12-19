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
    public class GetClassInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSGetClassInfoRes GSGetClassInfo(GSGetClassInfoReq objReq)
        {
            GSGetClassInfoRes objRes = new GSGetClassInfoRes
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
                            objRes.ClassInfo = new GSClass
                            {
                                RowID = existClass.RowID,
                                ClassID = existClass.ClassID,
                                NameParent = existClass.NameParent,
                                PhoneParent = existClass.PhoneEmail,
                                AddressParent = existClass.AddressParent,
                                District = existClass.District,
                                City = existClass.City,
                                Ward = existClass.Ward,
                                StudentID = existClass.StudentID,
                                StudentName = existClass.StudentName,
                                PhoneStudent = existClass.PhoneStudent,
                                Status = existClass.Status.HasValue ? existClass.Status.Value : 0,
                                Money = existClass.Money,
                                MoneyStatus = existClass.MoneyStatus,
                                Apply = existClass.Apply,
                                NumberApply = existClass.NumberApply.HasValue ? existClass.NumberApply.Value : 0,
                                FormTeach = existClass.FormTeach,
                                InfoMore = existClass.InfoMore,
                                Level = existClass.Level,
                                TimeCreate = existClass.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                NameSupports = existClass.NameSupports,
                                Subjects = existClass.Subjects,
                                
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
