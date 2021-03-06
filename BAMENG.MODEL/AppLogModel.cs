﻿/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    /// <summary>
    /// Class ReadLogModel.
    /// </summary>
    public class ReadLogModel
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }


        /// <summary>
        /// Gets or sets the article identifier.
        /// </summary>
        /// <value>The article identifier.</value>
        public int ArticleId { get; set; }


        /// <summary>
        /// Gets or sets the is read.
        /// </summary>
        /// <value>The is read.</value>
        public int IsRead { get; set; }


        /// <summary>
        /// Gets or sets the client ip.
        /// </summary>
        /// <value>The client ip.</value>
        public string ClientIp { get; set; }

        /// <summary>
        /// Gets or sets the cookie.
        /// </summary>
        /// <value>The cookie.</value>
        public string cookie { get; set; }

        /// <summary>
        /// Gets or sets the read time.
        /// </summary>
        /// <value>The read time.</value>
        public DateTime ReadTime { get; set; }
        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

    }

    public class LogBaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        /// <value>The type of the operation.</value>
        public int OperationType { get; set; }


        /// <summary>
        /// Gets or sets the shop identifier.
        /// </summary>
        /// <value>The shop identifier.</value>
        public int ShopId { get; set; }
        /// <summary>
        /// 用户所属总店ID
        /// </summary>
        /// <value>The belong shop identifier.</value>
        public int BelongShopId { get; set; }

        /// <summary>
        /// Gets or sets the application system.
        /// </summary>
        /// <value>The application system.</value>
        public string AppSystem { get; set; }

        /// <summary>
        /// Gets or sets the money.
        /// </summary>
        /// <value>The money.</value>
        public decimal Money { get; set; }

        /// <summary>
        /// 操作对象ID
        /// </summary>
        /// <value>The object identifier.</value>
        public int objId { get; set; }

    }


    /// <summary>
    /// 登录操作日志
    /// </summary>
    /// <seealso cref="BAMENG.MODEL.LogBaseModel" />
    public class LoginLogModel : LogBaseModel
    {

        /// <summary>
        /// Gets or sets the user identity.
        /// </summary>
        /// <value>The user identity.</value>
        public int UserIdentity { get; set; }

        /// <summary>
        /// Gets or sets the belong one.
        /// </summary>
        /// <value>The belong one.</value>
        public int BelongOne { get; set; }



    }

    public class StatisticsModel
    {


        private List<string> _xData = new List<string>();

        /// <summary>
        /// 统计日期数组
        /// </summary>
        /// <value>The x data.</value>
        public List<string> xData
        {
            get { return _xData; }
            set { value = _xData; }
        }


        private List<int> _yData = new List<int>();


        /// <summary>
        /// 统计数数据数组
        /// </summary>
        /// <value>The y data.</value>
        public List<int> yData
        {
            get { return _yData; }
            set { value = _yData; }
        }


        /// <summary>
        /// 总数
        /// </summary>
        /// <value>The total.</value>
        public long total { get; set; }
    }


    /// <summary>
    /// Class StatisticsListModel.
    /// </summary>
    public class StatisticsListModel
    {
        /// <summary>
        /// 统计日期数组
        /// </summary>
        /// <value>The x data.</value>
        public string xData { get; set; }


        /// <summary>
        /// 统计数数据数组
        /// </summary>
        /// <value>The y data.</value>
        public int yData { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }

    }


    public class StatisticsMoneyModel
    {


        private List<string> _xData = new List<string>();

        /// <summary>
        /// 统计日期数组
        /// </summary>
        /// <value>The x data.</value>
        public List<string> xData
        {
            get { return _xData; }
            set { value = _xData; }
        }


        private List<decimal> _yData = new List<decimal>();


        /// <summary>
        /// 统计数数据数组
        /// </summary>
        /// <value>The y data.</value>
        public List<decimal> yData
        {
            get { return _yData; }
            set { value = _yData; }
        }


        /// <summary>
        /// 总额
        /// </summary>
        /// <value>The total.</value>
        public decimal total { get; set; }
    }


    /// <summary>
    /// Class StatisticsListModel.
    /// </summary>
    public class StatisticsMoneyListModel
    {
        /// <summary>
        /// 统计日期数组
        /// </summary>
        /// <value>The x data.</value>
        public string xData { get; set; }


        /// <summary>
        /// 统计数数据数组
        /// </summary>
        /// <value>The y data.</value>
        public decimal yData { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }

    }



    public class StatisticsMoneyPieModel
    {

        private List<string> _xData = new List<string>();
        public List<string> xData { get { return _xData; }
            set { value = _xData; } }

        private List<PieModel> _yData = new List<PieModel>();
        public List<PieModel> yData { get { return _yData; } set { value = _yData; } }
        /// <summary>
        /// 总额
        /// </summary>
        /// <value>The total.</value>
        public decimal total { get; set; }
    }

    public class PieModel
    {
        public string name { get; set; }
        public decimal value { get; set; }

       
    }


    public class StatisticsPieModel
    {

        private List<string> _xData = new List<string>();
        public List<string> xData
        {
            get { return _xData; }
            set { value = _xData; }
        }

        private List<PieCountModel> _yData = new List<PieCountModel>();
        public List<PieCountModel> yData { get { return _yData; } set { value = _yData; } }
        /// <summary>
        /// 总额
        /// </summary>
        /// <value>The total.</value>
        public int total { get; set; }
    }

    public class PieCountModel
    {

        public string name { get; set; }
        public int value { get; set; }
    }


}
