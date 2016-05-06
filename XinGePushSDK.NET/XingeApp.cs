using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using XinGePushSDK.NET.Res;
using XinGePushSDK.NET.Utility;

namespace XinGePushSDK.NET
{
    public class XingeApp
    {
        private readonly string _accessId;
        private readonly string _secretKey;
        public uint ValidTime;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accessId">_accessId</param>
        /// <param name="secretKey">_secretKey</param>
        /// <param name="validTime">配合timestamp确定请求的有效期，单位为秒，
        /// 最大值为600。若不设置此参数或参数值非法，则按默认值600秒计算有效期</param>
        public XingeApp(string accessId, string secretKey, uint validTime = 600)
        {
            if (string.IsNullOrEmpty(accessId))
            {
                throw new ArgumentNullException(nameof(accessId));
            }
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }
            ValidTime = validTime;
            _accessId = accessId;
            _secretKey = secretKey;
        }

        /// <summary>
        /// 发起推送请求到信鸽并获得相应
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">字段</param>
        /// <returns>返回值json反序列化后的类</returns>
        private Ret CallRestful(String url, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            try
            {
                parameters.Add("access_id", _accessId);
                parameters.Add("timestamp", ((int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(1970, 1, 1))).TotalSeconds).ToString());
                parameters.Add("valid_time", ValidTime.ToString());
                string md5Sing = SignUtility.GetSignature(parameters, _secretKey, url);
                parameters.Add("sign", md5Sing);
                var res = HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, Encoding.UTF8, null);
                var resstr = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(resstr);
                var resstring = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<Ret>(resstring);
            }
            catch (Exception e)
            {
                return new Ret { ret_code = -1, err_msg = e.Message };
            }
        }

        /// <summary>
        /// 推送到 单个设备 IOS
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="msg"></param>
        /// <param name="environment"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushToSingleDevice(string deviceToken, Msg_IOS msg, uint environment)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                throw new ArgumentNullException(nameof(deviceToken));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("device_token", deviceToken);
            parameters.Add("send_time", msg.send_time);
            parameters.Add("environment", environment.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHSINGLEDEVICE, parameters);
        }

        /// <summary>
        /// 推送到 单个设备 安卓
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="msg"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushToSingleDevice(string deviceToken, Msg_Android msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                throw new ArgumentNullException(nameof(deviceToken));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("device_token", deviceToken);
            parameters.Add("send_time", msg.send_time);
            parameters.Add("multi_pkg", msg.multi_pkg.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHSINGLEDEVICE, parameters);
        }






        /// <summary>
        /// 推送到 单个用户 IOS
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="msg">信息</param>
        /// <param name="environment">推送环境(开发or在线)</param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushToAccount(string account, Msg_IOS msg, uint environment)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account", account);
            parameters.Add("send_time", msg.send_time);
            parameters.Add("environment", environment.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHSINGLEACCOUNT, parameters);
        }

        /// <summary>
        /// 推送到 单个用户 Android
        /// </summary>
        /// <param name="account"></param>
        /// <param name="msg"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushToAccount(string account, Msg_Android msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException(nameof(account));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account", account);
            parameters.Add("send_time", msg.send_time);
            parameters.Add("multi_pkg", msg.multi_pkg.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHSINGLEACCOUNT, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountList"></param>
        /// <param name="msg"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushAccountList(List<String> accountList, Msg_Android msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (accountList.Count == 0)
            {
                throw new ArgumentNullException(nameof(accountList));
            }
            if (accountList.Count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(accountList));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account_list", JsonConvert.SerializeObject(accountList));
            parameters.Add("multi_pkg", msg.multi_pkg.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHACCOUNTLIST, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountList"></param>
        /// <param name="msg"></param>
        /// <param name="environment"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushAccountList(List<String> accountList, Msg_IOS msg, uint environment)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            if (accountList.Count == 0)
            {
                throw new ArgumentNullException(nameof(accountList));
            }
            if (accountList.Count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(accountList));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account_list", JsonConvert.SerializeObject(accountList));
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("message", msg.ToJson());
            parameters.Add("environment", environment.ToString());
            parameters.Add("message_type", msg.message_type.ToString());
            return CallRestful(XinGeConfig.RESTAPI_PUSHACCOUNTLIST, parameters);
        }





        /// <summary>
        /// 推送到所有ios设备
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushAllDevice(Msg_Android msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message_type", msg.message_type.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("send_time", msg.send_time);
            parameters.Add("multi_pkg", msg.multi_pkg.ToString());
            parameters.Add("message", msg.ToJson());
            if (msg.loop_times.HasValue)
            {
                parameters.Add("loop_times", msg.loop_times.Value.ToString());
            }
            if (msg.loop_interval.HasValue)
            {
                parameters.Add("loop_interval", msg.ToJson());
            }
            return CallRestful(XinGeConfig.RESTAPI_PUSHALLDEVICE, parameters);
        }

        /// <summary>
        /// 推送到所有android设备
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="environment"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushAllDevice(Msg_IOS msg, uint environment)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message_type", msg.message_type.ToString());
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("send_time", msg.send_time);
            parameters.Add("multi_pkg", environment.ToString());
            parameters.Add("message", msg.ToJson());
            if (msg.loop_times.HasValue)
            {
                parameters.Add("loop_times", msg.loop_times.Value.ToString());
            }
            if (msg.loop_interval.HasValue)
            {
                parameters.Add("loop_interval", msg.ToJson());
            }
            return CallRestful(XinGeConfig.RESTAPI_PUSHALLDEVICE, parameters);
        }

        /// <summary>
        /// 通过Tag推送到android设备
        /// </summary>
        /// <param name="tagList"></param>
        /// <param name="tagOp"></param>
        /// <param name="msg"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushTags(List<String> tagList, String tagOp, Msg_Android msg)
        {
            if (tagList == null || tagList.Count == 0)
            {
                throw new ArgumentNullException(nameof(tagList));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            parameters.Add("tags_list", JsonConvert.SerializeObject(tagList));
            parameters.Add("tags_op", tagOp);
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("send_time", msg.send_time);

            parameters.Add("multi_pkg", msg.multi_pkg.ToString());

            if (msg.loop_times.HasValue)
            {
                parameters.Add("loop_times", msg.loop_times.Value.ToString());
            }
            if (msg.loop_interval.HasValue)
            {
                parameters.Add("loop_interval", msg.ToJson());
            }
            return CallRestful(XinGeConfig.RESTAPI_PUSHTAGS, parameters);
        }

        /// <summary>
        /// 通过tag推送到ios设备
        /// </summary>
        /// <param name="tagList"></param>
        /// <param name="tagOp"></param>
        /// <param name="msg"></param>
        /// <param name="environment"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret PushTags(List<String> tagList, String tagOp, Msg_IOS msg, uint environment)
        {
            if (tagList == null || tagList.Count == 0)
            {
                throw new ArgumentNullException(nameof(tagList));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message", msg.ToJson());
            parameters.Add("message_type", msg.message_type.ToString());
            parameters.Add("tags_list", JsonConvert.SerializeObject(tagList));
            parameters.Add("tags_op", tagOp);
            if (msg.expire_time.HasValue)
            {
                parameters.Add("expire_time", msg.expire_time.Value.ToString());
            }
            parameters.Add("send_time", msg.send_time);

            parameters.Add("environment", environment.ToString());

            if (msg.loop_times.HasValue)
            {
                parameters.Add("loop_times", msg.loop_times.Value.ToString());
            }
            if (msg.loop_interval.HasValue)
            {
                parameters.Add("loop_interval", msg.ToJson());
            }
            return CallRestful(XinGeConfig.RESTAPI_PUSHTAGS, parameters);
        }

        /// <summary>
        /// 查询群发消息发送状态
        /// </summary>
        /// <param name="pushIdList"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryPushStatus(List<String> pushIdList)
        {
            JObject jObject = new JObject();
            foreach (var item in pushIdList)
            {
                jObject.Add("push_id", item);
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("push_ids", jObject.ToString());
            return CallRestful(XinGeConfig.RESTAPI_QUERYPUSHSTATUS, parameters);
        }

        /// <summary>
        /// 查询应用覆盖的设备数
        /// </summary>
        /// <param name="pushIdList"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryDeviceCount(List<String> pushIdList)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            return CallRestful(XinGeConfig.RESTAPI_QUERYDEVICECOUNT, parameters);
        }

        /// <summary>
        /// 查询应用的Tags
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryTags(int start, int limit)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("start", start.ToString());
            parameters.Add("limt", limit.ToString());
            return CallRestful(XinGeConfig.RESTAPI_QUERYTAGS, parameters);
        }

        /// <summary>
        /// 查询应用的Tags
        /// </summary>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryTags()
        {
            return QueryTags(0, 100);
        }


        /// <summary>
        /// 取消尚未触发的定时群发任务
        /// </summary>
        /// <param name="pushId"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret CancelTimingPush(String pushId)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("push_id", pushId);
            return CallRestful(XinGeConfig.RESTAPI_CANCELTIMINGPUSH, parameters);
        }

        /// <summary>
        /// 批量设置标签
        /// </summary>
        /// <param name="tagTokenPairs"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret BatchSetTag(IDictionary<string, string> tagTokenPairs)
        {

            if (tagTokenPairs == null)
            {
                throw new ArgumentNullException(nameof(tagTokenPairs));
            }
            if (tagTokenPairs.Count > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(tagTokenPairs));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            JArray jarray = new JArray();
            foreach (var item in tagTokenPairs)
            {
                JArray ja = new JArray {item.Key, item.Value};
                jarray.Add(ja);
            }
            parameters.Add("tag_token_list", jarray.ToString());
            return CallRestful(XinGeConfig.RESTAPI_BATCHSETTAG, parameters);
        }

        /// <summary>
        /// 批量删除标签
        /// </summary>
        /// <param name="tagTokenPairKeys"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret BatchDelTag(List<string> tagTokenPairKeys)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tag_token_list", JsonConvert.SerializeObject(tagTokenPairKeys));

            return CallRestful(XinGeConfig.RESTAPI_BATCHDELTAG, parameters);
        }

        /// <summary>
        /// 查询应用某token设置的标签
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryTokenTags(String deviceToken)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("device_token", deviceToken);
            return CallRestful(XinGeConfig.RESTAPI_QUERYTOKENTAGS, parameters);
        }

        /// <summary>
        /// 查询应用某标签关联的设备数量
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>返回值json反序列化后的类</returns>
        public Ret QueryTagTokenNum(String tag)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tag", tag);
            return CallRestful(XinGeConfig.RESTAPI_QUERYTAGTOKENNUM, parameters);
        }





        /// <summary>
        /// 删除应用中某个account映射的某个token
        /// </summary>
        /// <param name="account">账号，可以是邮箱号、手机号、QQ号等任意形式的业务帐号</param>
        /// <param name="deviceToken">token，设备的唯一识别ID</param>
        /// <returns></returns>
        public Ret DelAccountToken(string account, string deviceToken)
        {
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException(nameof(account));
            }
            if (string.IsNullOrEmpty(deviceToken))
            {
                throw new ArgumentNullException(nameof(deviceToken));
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account", account);
            parameters.Add("device_token", deviceToken);

            return CallRestful(XinGeConfig.RESTAPI_DELAPPACCOUNTTOKENS, parameters);
        }

        /// <summary>
        /// 删除应用中某account映射的所有token
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        public Ret DelAccount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentNullException(nameof(account));
            }

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account", account);

            return CallRestful(XinGeConfig.RESTAPI_DELAPPACCOUNTALLTOKENS, parameters);
        }


    }
}
