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
    public class GetParentReqInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSGetParentReqInfoRes GSGetParentReqInfo(GSGetParentReqInfoReq objReq)
        {
            GSGetParentReqInfoRes objRes = new GSGetParentReqInfoRes
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

                        var existParent = db.GS_ReqParents.Where(p => p.ReqParentID == objReq.ReqParentID).FirstOrDefault();
                        if (existParent == null)
                        {
                            // Update the Apply list for the existing parent
                            objRes.RespCode = -2;
                            objRes.RespText = "No existing data of this request";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }
                        else
                        {
                            objRes.ParentInfo = new GSParentInfo
                            {
                                RowID = existParent.RowID,
                                ReqParentID = existParent.ReqParentID,
                                ParentID = existParent.ParentID,
                                NameParent = existParent.NameParent,
                                PhoneParent = existParent.PhoneEmail,
                                AddressParent = existParent.AddressParent,
                                FormTeach = existParent.FormTeach,
                                InfoMore = existParent.InfoMore,
                                Level = existParent.Level,
                                NeedMore = existParent.NeedMore,
                                SexTeacher = existParent.SexTeacher,
                                QuantityStudent = existParent.QuantityStudent,
                                SelectSchool = existParent.SelectSchool,
                                ValueClass = existParent.ValueClass,
                                Status = (int)existParent.Status,
                                TimeCreate = existParent.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                NameSupports = existParent.NameSupports,
                                Subjects = existParent.Subjects,
                                TimeSupport = existParent.TimeSupport,
                                SkillSupport = existParent.SkillSupport,
                                District = existParent.District,
                                City = existParent.City,
                                Ward = existParent.Ward,
                                TimeModify = existParent.TimeModify?.ToString("yyyy-MM-dd HH:mm:ss"),
                                ModifierID = existParent.ModifierID
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
