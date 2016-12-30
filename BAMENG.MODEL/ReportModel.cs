/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    public class UserReportModel
    {

        public int ID { get; set; }


        public int UserId { get; set; }


        public int ShopId { get; set; }


        public string ReportTitle { get; set; }


        public string Addr { get; set; }


        public string JsonContent { get; set; }


        public DateTime CreateTime { get; set; }


        public string time { get; set; }


        public string UserName { get; set; }

        public string UserMobile { get; set; }

    }

    /// <summary>
    /// 工作汇报列表
    /// </summary>
    public class AppUserReportListModel
    {
        public int ID { get; set; }

        public string reportTitle { get; set; }

        public string time { get; set; }


        /// <summary>
        /// 工作汇报详情
        /// </summary>
        /// <value>The report URL.</value>
        public string reportUrl { get; set; }

        [JsonIgnore()]
        public DateTime CreateTime { get; set; }
    }


    public class WorkReportModel
    {
        public int ID { get; set; }

        public string WorkTitle { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
