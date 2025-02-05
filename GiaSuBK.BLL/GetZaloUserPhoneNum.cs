using System;
using System.Net.Http;
using System.Threading.Tasks;
using GiaSuBK.MD.GiaSuBKMessages;
using GiaSuBK.DAL;
using Newtonsoft.Json;
using log4net;

namespace GiaSuBK.BLL
{
    public class GetZaloUserPhoneNum
    {
        private static readonly ILog Log = LogManager.GetLogger("GiaSuBKAppender");

        private const string ZaloApiUrl = "https://graph.zalo.me/v2.0/me/info";
        private const string SecretKey = "FwWAyfb2W7MCI4b8OVwA"; // Replace with your actual secret key

        public async Task<GSGetZaloUserPhoneNumRes> GSGetZaloUserPhoneNum(GSGetZaloUserPhoneNumReq objReq)
        {
            GSGetZaloUserPhoneNumRes objRes = new GSGetZaloUserPhoneNumRes
            {
                RespCode = -1,
                RespText = "Nothing"
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, ZaloApiUrl);
                    request.Headers.Add("access_token", objReq.Token);
                    request.Headers.Add("code", objReq.PhoneToken);
                    request.Headers.Add("secret_key", SecretKey);

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var zaloResponse = JsonConvert.DeserializeObject<ZaloUserResponse>(responseBody);

                        objRes.RespCode = 0;
                        objRes.RespText = "User Info Retrieved Successfully";
                        objRes.ZaloUserPhoneNum = zaloResponse.Data?.PhoneNumber;
                    }
                    else
                    {
                        objRes.RespCode = response.StatusCode.GetHashCode();
                        objRes.RespText = "Failed to retrieve user phone number";
                    }
                }
            }
            catch (Exception ex)
            {
                objRes.RespCode = 109;
                objRes.RespText = ex.Message;
                Log.Error($"[{objRes.RespCode}: {objRes.RespText}]");
            }

            return objRes;
        }
    }

    public class ZaloUserResponse
    {
        public int Error { get; set; }
        public ZaloUserData Data { get; set; }
    }

    public class ZaloUserData
    {
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}
