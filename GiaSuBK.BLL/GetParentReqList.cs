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
    public class GetParentReqList
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetParentReqListRes GSGetParentReqList(GSGetParentReqListReq objReq)
        {
            GSGetParentReqListRes objRes = new GSGetParentReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
                TotalRows = 0,
                ParentList = new ParentList()
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

                    // Fetch Parent List
                    var parentListData = db.GS_GetParentLst(objReq.PageNumber, objReq.RowsPage, objReq.Search).ToList();

                    // Map database result to GSParentInfo objects
                    if (parentListData != null)
                    {
                        foreach (var p in parentListData)
                        {
                            var parentInfo = new GSParentInfo
                            {
                                RowID = p.RowID,
                                ReqParentID = p.ReqParentID,
                                ParentID = p.ParentID,
                                NameParent = p.NameParent,
                                PhoneParent = p.PhoneEmail,
                                AddressParent = p.AddressParent,
                                FormTeach = p.FormTeach,
                                InfoMore = p.InfoMore,
                                Level = p.Level,
                                NeedMore = p.NeedMore,
                                SexTeacher = p.SexTeacher,
                                QuantityStudent = p.QuantityStudent,
                                SelectSchool = p.SelectSchool,
                                ValueClass = p.ValueClass,
                                TimeCreate = p.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                Status = (int)p.Status,
                                NameSupports = p.NameSupports,
                                Subjects = p.Subjects,
                                TimeSupport = p.TimeSupport,
                                SkillSupport = p.SkillSupport,
                                District = p.District,
                                City = p.City,
                                Ward = p.Ward
                            };

                            // Add each GSParentInfo to the ParentList
                            objRes.ParentList.Add(parentInfo);
                        }

                        // Extract TotalRows from the first row (if available)
                        if (parentListData.Count > 0)
                        {
                            objRes.TotalRows = parentListData[0].TotalRows ?? 0;
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