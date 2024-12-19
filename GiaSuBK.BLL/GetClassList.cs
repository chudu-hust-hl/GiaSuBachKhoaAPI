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
    public class GetClassList
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSGetClassReqListRes GSGetClassReqList(GSGetClassReqListReq objReq)
        {
            GSGetClassReqListRes objRes = new GSGetClassReqListRes
            {
                RespCode = -1,
                RespText = "Nothing",
                TotalRows = 0,
                ClassList = new ClassList()
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

                    // Fetch Class List
                    var classListData = db.GS_GetClassLst(objReq.PageNumber, objReq.RowsPage, objReq.Search).ToList();

                    // Map database result to GSClassInfo objects
                    if (classListData != null)
                    {
                        foreach (var p in classListData)
                        {
                            var classInfo = new GSClass
                            {
                                RowID = p.RowID,
                                ClassID = p.ClassID,
                                ReqParentID = p.ReqParentID,
                                NameParent = p. NameParent,
                                PhoneParent = p.PhoneEmail,
                                AddressParent = p.AddressParent,
                                StudentID = p.StudentID,
                                StudentName =p.StudentName,
                                PhoneStudent = p.PhoneStudent ,
                                FormTeach = p.FormTeach,
                                InfoMore = p.InfoMore,
                                Level = p.Level,
                                ValueClass = p.ValueClass ,
                                Status = p.Status ?? 1,
                                TimeCreate = p.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                NameSupports = p.NameSupports,
                                Subjects = p.Subjects,
                                Money = p.Money ,
                                MoneyStatus =p.MoneyStatus ,
                                Apply = p.Apply,
                                NumberApply = p.NumberApply ?? 0,
                                District = p.District,
                                City = p.City,
                                Ward = p.Ward
                            };

                            // Add each GSClassInfo to the ClassList
                            objRes.ClassList.Add(classInfo);
                        }

                        // Extract TotalRows from the first row (if available)
                        if (classListData.Count > 0)
                        {
                            objRes.TotalRows = classListData[0].TotalRows ?? 0;
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