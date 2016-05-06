using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XinGePushSDK.NET
{
    public class XinGeConfig
    {
        public const string RESTAPI_PUSHSINGLEDEVICE = "http://openapi.xg.qq.com/v2/push/single_device";
        public const string RESTAPI_PUSHSINGLEACCOUNT = "http://openapi.xg.qq.com/v2/push/single_account";
        public const string RESTAPI_PUSHACCOUNTLIST = "http://openapi.xg.qq.com/v2/push/account_list";
        public const string RESTAPI_PUSHALLDEVICE = "http://openapi.xg.qq.com/v2/push/all_device";
        public const string RESTAPI_PUSHTAGS = "http://openapi.xg.qq.com/v2/push/tags_device";
        public const string RESTAPI_QUERYPUSHSTATUS = "http://openapi.xg.qq.com/v2/push/get_msg_status";
        public const string RESTAPI_QUERYDEVICECOUNT = "http://openapi.xg.qq.com/v2/application/get_app_device_num";
        public const string RESTAPI_QUERYTAGS = "http://openapi.xg.qq.com/v2/tags/query_app_tags";
        public const string RESTAPI_CANCELTIMINGPUSH = "http://openapi.xg.qq.com/v2/push/cancel_timing_task";
        public const string RESTAPI_BATCHSETTAG = "http://openapi.xg.qq.com/v2/tags/batch_set";
        public const string RESTAPI_BATCHDELTAG = "http://openapi.xg.qq.com/v2/tags/batch_del";
        public const string RESTAPI_QUERYTOKENTAGS = "http://openapi.xg.qq.com/v2/tags/query_token_tags";
        public const string RESTAPI_QUERYTAGTOKENNUM = "http://openapi.xg.qq.com/v2/tags/query_tag_token_num";
        public const string RESTAPI_DELAPPACCOUNTTOKENS = "http://openapi.xg.qq.com/v2/application/del_app_account_tokens";
        public const string RESTAPI_DELAPPACCOUNTALLTOKENS = "http://openapi.xg.qq.com/v2/application/del_app_account_all_tokens";


        public const string HTTP_POST = "POST";
        public const string HTTP_GET = "GET";

        public const int DEVICE_ALL = 0;
        public const int DEVICE_BROWSER = 1;
        public const int DEVICE_PC = 2;
        public const int DEVICE_ANDROID = 3;
        public const int DEVICE_IOS = 4;
        public const int DEVICE_WINPHONE = 5;

        /// <summary>
        /// IOS生产环境
        /// </summary>
        public const uint IOSENV_PROD = 1;
        /// <summary>
        /// IOS开发环境
        /// </summary>
        public const uint IOSENV_DEV = 2;

        /// <summary>
        /// android 通知消息
        /// </summary>
        public const uint message_type_info = 1;

        /// <summary>
        /// android 透传消息
        /// </summary>
        public const uint message_type_touchuan = 2;
    }
}
