using System;
using System.Linq;
using GiaSuBK.MD.GiaSuBKMessages;
using GiaSuBK.DAL;
using System.Web.Configuration;
using log4net;

namespace GiaSuBK.BLL
{
    public class GetParentComment
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        public GSParentCmtRes GSParentCmt(GSParentCmtReq objReq)
        {
            GSParentCmtRes objRes = new GSParentCmtRes
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
                        var userToken = db.GS_ZaloUserInfos.FirstOrDefault(p => p.ZaloUserID == objReq.UserID && p.TokenCode == objReq.Token);
                        var adminToken = db.UserTokens.FirstOrDefault(p => p.TokenCode == objReq.Token);

                        if (userToken == null && adminToken == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Session expired or invalid token code";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        // Check if the Parent exists
                        var existParent = db.GS_Parents.FirstOrDefault(p => p.Phone == objReq.ParentPhone);
                        if (existParent == null)
                        {
                            objRes.RespCode = -2;
                            objRes.RespText = "Parent not found or cannot fetch info";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        // Check if the class exists
                        var existClass = db.GS_Classes.FirstOrDefault(c => c.ClassID == objReq.ClassID);
                        if (existClass == null)
                        {
                            objRes.RespCode = -3;
                            objRes.RespText = "Class not found";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        // Append the comment to the ParentCmt field
                        string newComment = objReq.ParentComment?.Trim();
                        if (string.IsNullOrEmpty(newComment))
                        {
                            objRes.RespCode = -4;
                            objRes.RespText = "Comment cannot be empty";
                            Log.Warn($"[{objRes.RespCode}:{objRes.RespText}]");
                            return objRes;
                        }

                        if (string.IsNullOrEmpty(existClass.ParentCmt))
                        {
                            existClass.ParentCmt = newComment;
                        }
                        else
                        {
                            existClass.ParentCmt = $"{existClass.ParentCmt}, {newComment}";
                        }

                        // Save changes to the database
                        db.SubmitChanges();

                        objRes.RespCode = 0;
                        objRes.RespText = "Comment added successfully";
                        return objRes;
                    }
                    catch (Exception ex)
                    {
                        objRes.RespCode = 109;
                        objRes.RespText = ex.Message;
                        Log.Error($"[{objRes.RespCode}:{objRes.RespText}]");
                    }
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
