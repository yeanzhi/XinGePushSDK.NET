using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XinGePushSDK.NET;

namespace XinGeConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            XingeApp xinge = new XingeApp("accessId", "secretKey");
            Payload pl = new Payload("这是一个简单的alert");
            Msg_IOS mios = new Msg_IOS(pl);
            Msg_Android mandroid = new Msg_Android_TouChuan("测试", XinGeConfig.message_type_touchuan)
            {
                content = "测试"
            };

            //Push消息（包括通知和透传消息）给单个设备
            xinge.PushToSingleDevice("DeviceToken", mios, XinGeConfig.IOSENV_DEV);
            xinge.PushToSingleDevice("DeviceToken", mandroid);
            //Push消息（包括通知和透传消息）给单个账户或别名
            xinge.PushToAccount("account", mandroid);
            xinge.PushToAccount("account", mios, XinGeConfig.IOSENV_DEV);
            //Push消息（包括通知和透传消息）给多个账户或别名（批量推送）
            xinge.PushAccountList(new List<string>() { "account1" ,"account2"}, mandroid);
            xinge.PushAccountList(new List<string>() { "account1", "account2" }, mios, XinGeConfig.IOSENV_DEV);
            //Push消息（包括通知和透传消息）给app的所有设备
            xinge.PushAllDevice(mandroid);
            xinge.PushAllDevice(mios,XinGeConfig.IOSENV_DEV);
            //Push消息（包括通知和透传消息）给tags指定的设备
            xinge.PushTags(new List<string>() { "tag1", "tag1" }, "OR", mandroid);
            xinge.PushTags(new List<string>() { "tag1", "tag1" }, "OR", mios, XinGeConfig.IOSENV_DEV);
            //查询群发消息发送状态
            xinge.QueryPushStatus(new List<string>() { "pushId1", "pushId1" });
            //查询应用覆盖的设备数
            xinge.QueryDeviceCount(new List<string>() { "pushId1", "pushId1" });
            //查询应用的Tags
            xinge.QueryTags(0, 100);
            //取消尚未触发的定时群发任务
            xinge.CancelTimingPush("pushId1");
            //批量设置标签
            var tags = new Dictionary<string, string>();
            tags.Add("tag1", "token1");
            xinge.BatchSetTag(tags);
            //批量删除标签
            xinge.BatchDelTag(new List<string>() { "tag1", "tag2" });
            //查询应用某token设置的标签
            xinge.QueryTokenTags("deviceToken");
            //查询应用某标签关联的设备数量
            xinge.QueryTagTokenNum("tag");

            
        }

    }
}
