
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
    public class Admin
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");
        public AdminLoginRes AdminLogin(AdminLoginReq objReq)
        {
            AdminLoginRes objRes = new AdminLoginRes
            {
                RespCode = -1,
                RespText = "Nothing",
                TokenCode = string.Empty,
                UserInfo = null
            };

            try
            {
                //Ket noi du lieu kiem tra
                using (DataClassesGiaSuBKDataContext db = new DataClassesGiaSuBKDataContext())
                {
                    try
                    {
                        // Find admin user by username and password
                        var admin = db.Users.Where(p => p.UserName == objReq.UserName && p.Password == objReq.Password).FirstOrDefault();

                        if (admin == null)
                        {
                            objRes.RespCode = -1;
                            objRes.RespText = "Invalid credentials";
                            Log.Warn(string.Format("[{0}:{1}]", objRes.RespCode, objRes.RespText));
                            return objRes;
                        }

                        // Generate a token for the session (for simplicity, use a GUID here)
                        string tokenCode = Guid.NewGuid().ToString();

                        // Save the token to the database
                        db.UserTokens.InsertOnSubmit(new UserToken
                        {
                            UserName = admin.UserName,
                            TokenCode = tokenCode,
                            TimeCreate = DateTime.UtcNow
                        });
                        db.SubmitChanges();

                        // Prepare response
                        objRes.RespCode = 0;
                        objRes.RespText = "Login successful";
                        objRes.TokenCode = tokenCode;
                        objRes.UserInfo = new GSUserInfo
                        {
                            RowID = admin.RowID,
                            UserName = admin.UserName,
                            FullName = admin.FullName,
                            PhoneNumber = admin.PhoneNumber,
                            Email = admin.Email,
                            Password = "", // Do not expose password
                            Status = Convert.ToInt32(admin.Status),
                            TimeCreate = admin.TimeCreate?.ToString("yyyy-MM-dd HH:mm:ss")
                        };

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