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
    public class UpdateParentInfo
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSUpdateParentInfoRes GSUpdateParentInfo(GSUpdateParentInfoReq objReq)
        {
            GSUpdateParentInfoRes objRes = new GSUpdateParentInfoRes
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

                        var existParent = db.GS_ReqParents.Where(p => p.ReqParentID == objReq.ParentInfo.ReqParentID).FirstOrDefault();
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
                            // Update new GS_Parent instance
                            existParent.ReqParentID = objReq.ParentInfo.ReqParentID;
                            existParent.ParentID = objReq.ParentInfo.ParentID;
                            existParent.NameParent = objReq.ParentInfo.NameParent;
                            existParent.PhoneEmail = objReq.ParentInfo.PhoneParent;
                            existParent.AddressParent = objReq.ParentInfo.AddressParent;
                            existParent.FormTeach = objReq.ParentInfo.FormTeach;
                            existParent.InfoMore = objReq.ParentInfo.InfoMore;
                            existParent.Level = objReq.ParentInfo.Level;
                            existParent.NeedMore = objReq.ParentInfo.NeedMore;
                            existParent.SexTeacher = objReq.ParentInfo.SexTeacher;
                            existParent.QuantityStudent = objReq.ParentInfo.QuantityStudent;
                            existParent.SelectSchool = objReq.ParentInfo.SelectSchool;
                            existParent.ValueClass = objReq.ParentInfo.ValueClass;
                            existParent.Status = objReq.ParentInfo.Status;
                            existParent.NameSupports = objReq.ParentInfo.NameSupports;
                            existParent.Subjects = objReq.ParentInfo.Subjects;
                            existParent.TimeSupport = objReq.ParentInfo.TimeSupport;
                            existParent.SkillSupport = objReq.ParentInfo.SkillSupport;
                            existParent.District = objReq.ParentInfo.District;
                            existParent.City = objReq.ParentInfo.City;
                            existParent.Ward = objReq.ParentInfo.Ward;
                            existParent.TimeModify = DateTime.UtcNow;
                            existParent.ModifierID = null;

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
