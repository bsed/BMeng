/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-$today.year. All rights reserved.
**/


using BAMENG.CONFIG;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.MODEL
{
    /// <summary>
    /// 接口返回实体对象
    /// </summary>
    public class ResultModel
    {

        private string _statusText = "";
        private object _data = null;

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string statusText { get { return _statusText; } set { _statusText = value; } }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get { return _data; } set { _data = value; } }
        /// <summary>
        /// 
        /// </summary>
        public ResultModel()
        {
            this.status = (int)ApiStatusCode.OK;
            this.statusText = StringHelper.GetEnumDescription<ApiStatusCode>((int)ApiStatusCode.OK);
            this.data = new object();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        public ResultModel(ApiStatusCode statusCode)
        {
            this.status = (int)statusCode;
            this.statusText = StringHelper.GetEnumDescription<ApiStatusCode>((int)statusCode);
            this.data = new object();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        public ResultModel(ApiStatusCode statusCode, string description)
        {
            this.status = (int)statusCode;
            this.statusText = description;
            this.data = new object();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        /// <param name="obj"></param>
        public ResultModel(ApiStatusCode statusCode, string description, object obj)
        {
            this.status = (int)statusCode;
            this.statusText = description;
            this.data = obj;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="obj"></param>
        public ResultModel(ApiStatusCode statusCode, object obj)
        {
            this.status = (int)statusCode;
            this.statusText = StringHelper.GetEnumDescription<ApiStatusCode>((int)statusCode);
            this.data = obj;
        }

    }


    /// <summary>
    /// 分页数据结构
    /// </summary>
    [Serializable]
    public class ResultPageModel
    {

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数据大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总数据
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 总页码
        /// </summary>
        public int PageCount { get; set; }

        public object Rows
        {
            get
            {
                return _Rows;
            }

            set
            {
                _Rows = value;
            }
        }

        private object _Rows = new object();
    }

    /// <summary>
    /// 搜索实体
    /// </summary>
    public class SearchModel
    {
        private int _pageIndex = 1;
        private int _pageSize = 20;
        private int _status = -100;
        private int _storeId = 0;
        private int _userid = 0;
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            set
            {
                _pageIndex = value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }

        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sn { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        ///状态 -100标识为空
        /// </summary>
        public int Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int StoreId
        {
            get
            {
                return _storeId;
            }

            set
            {
                _storeId = value;
            }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get
            {
                return _userid;
            }

            set
            {
                _userid = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int searchType { get; set; }

        public int level { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }


        /// <summary>
        /// 排序类型，0盟友等级，1客户信息，2订单成交
        /// </summary>
        /// <value>The orderby code.</value>
        public int orderbyCode { get; set; }


        /// <summary>
        /// 是否降序
        /// </summary>
        /// <value>true if desc; otherwise, false.</value>
        public bool IsDesc { get; set; }

    }
}
