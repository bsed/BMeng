/*
 * 版权所有:杭州火图科技有限公司
 * 地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
 * (c) Copyright Hangzhou Hot Technology Co., Ltd.
 * Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
 * 2013-2016. All rights reserved.
 * author guomw
**/

using BAMENG.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMENG.IDAL
{
    public interface IConfigDAL : IDisposable
    {
        /// <summary>
        /// 更新配置信息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        bool UpdateValue(ConfigModel model);

        /// <summary>
        /// 根据CODE获取实体信息
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>ConfigModel.</returns>
        ConfigModel GetModel(string code);

        /// <summary>
        /// 根据CODE获取值
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
        string GetValue(string code);
        /// <summary>
        /// 获取所有的配置数据
        /// </summary>
        /// <returns>List&lt;ConfigModel&gt;.</returns>
        List<ConfigModel> List();
    }
}
