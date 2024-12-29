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
    public class ParentRequest
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public GSCreateParentInfoRes GSCreateParentInfo(GSCreateParentInfoReq objReq)
        {
            GSCreateParentInfoRes objRes = new GSCreateParentInfoRes
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
                        if (userToken == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Session expired or invalid token";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }

                        // Create new GS_ReqParent object
                        var random = new Random();
                        var randomCode = random.Next(10000, 99999); // Generate a 5-digit random number
                        var reqParentID = $"{objReq.ParentInfo.PhoneParent}-{randomCode}";

                        var existParent = db.GS_Parents.Where(p => p.ParentID == objReq.ParentInfo.PhoneParent).FirstOrDefault();
                        if (existParent != null)
                        {
                            // Update the Apply list for the existing parent
                            existParent.Apply = string.IsNullOrEmpty(existParent.Apply) ?
                            objReq.ParentInfo.ReqParentID :
                            string.Join(",", existParent.Apply.Split(',').Concat(new[] { reqParentID }));
                            
                            db.SubmitChanges();
                        }
                        else
                        {
                            // Create new GS_Parent instance
                            var newParent = new GS_Parent
                            {
                                ParentID = objReq.ParentInfo.PhoneParent,
                                Phone = objReq.ParentInfo.PhoneParent,
                                Address = objReq.ParentInfo.AddressParent,
                                Apply = reqParentID
                            };

                            db.GS_Parents.InsertOnSubmit(newParent);
                            db.SubmitChanges();
                        }

                        

                        var newParentReq = new GS_ReqParent
                        {
                            ReqParentID = reqParentID,
                            ParentID = objReq.ParentInfo.PhoneParent,
                            NameParent = objReq.ParentInfo.NameParent,
                            PhoneEmail = objReq.ParentInfo.PhoneParent,
                            AddressParent = objReq.ParentInfo.AddressParent,
                            FormTeach = objReq.ParentInfo.FormTeach,
                            InfoMore = objReq.ParentInfo.InfoMore,
                            Level = objReq.ParentInfo.Level,
                            NeedMore = objReq.ParentInfo.NeedMore,
                            SexTeacher = objReq.ParentInfo.SexTeacher,
                            QuantityStudent = objReq.ParentInfo.QuantityStudent,
                            SelectSchool = objReq.ParentInfo.SelectSchool,
                            ValueClass = objReq.ParentInfo.ValueClass,
                            Status = objReq.ParentInfo.Status,
                            TimeCreate = DateTime.UtcNow,
                            NameSupports = objReq.ParentInfo.NameSupports,
                            Subjects = objReq.ParentInfo.Subjects,
                            TimeSupport = objReq.ParentInfo.TimeSupport,
                            SkillSupport = objReq.ParentInfo.SkillSupport,
                            District = objReq.ParentInfo.District,
                            City = objReq.ParentInfo.City,
                            Ward = objReq.ParentInfo.Ward,
                            TimeModify = null,
                            ModifierID = null
                        };

                        // Add and save changes
                        db.GS_ReqParents.InsertOnSubmit(newParentReq);
                        db.SubmitChanges();

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
