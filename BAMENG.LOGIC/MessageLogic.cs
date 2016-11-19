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
using System.Transactions;

namespace BAMENG.LOGIC
{
    public class MessageLogic
    {
        /// <summary>
        /// 获取消息列表作者身份 
        /// </summary>
        /// <param name="shopId">门店ID 0为总后台，</param>
        /// <param name="AuthorIdentity">0总后台，1总店，2分店</param>
        /// <param name="model">The model.</param>
        /// <returns>ResultPageModel.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static ResultPageModel GetMessageList(int shopId, int AuthorIdentity, SearchModel model)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.GetMessageList(shopId, AuthorIdentity, model);
            }
        }

        /// <summary>
        /// 编辑信息
        /// </summary>
        /// <param name="userIdentity">0集团，1总店，2分店</param>
        /// <param name="ShopBelongId">门店所属总店ID</param>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool EditMessage(int userIdentity, int ShopBelongId, MessageModel model)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                if (model.ID > 0)
                    return dal.UpdateMessageInfo(model);
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {

                        int messageId = dal.AddMessageInfo(model);
                        if (messageId > 0)
                        {
                            // TODO;添加门店记录
                            //如果给总后台发布消息通知
                            if (userIdentity == 1 && model.IsSendBelongShopId == 1)
                                dal.AddMessageSendTarget(messageId, -1);

                            if (userIdentity == 1 || userIdentity == 0)
                            {
                                if (string.IsNullOrEmpty(model.SendTargetIds))
                                    return false;
                                string[] targetIds = model.SendTargetIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                if (targetIds == null || targetIds.Count() == 0)
                                    return false;

                                foreach (var targetid in targetIds)
                                {
                                    dal.AddMessageSendTarget(messageId, Convert.ToInt32(targetid));
                                }
                            }
                            else
                            {
                                dal.AddMessageSendTarget(messageId, ShopBelongId);
                            }
                        }
                        scope.Complete();
                        return true;
                    }
                }
            }
        }
        /// <summary>
        /// 修改消息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool UpdateMessageInfo(MessageModel model)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.UpdateMessageInfo(model);
            }
        }
        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int AddMessageInfo(MessageModel model)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.AddMessageInfo(model);
            }
        }

        /// <summary>
        /// 获取消息信息
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <returns>MessageModel.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static MessageModel GetModel(int messageId)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.GetModel(messageId);
            }
        }

        /// <summary>
        /// 修改阅读状态
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <returns>true if XXXX, false otherwise.</returns>
        public static bool UpdateReadStatus(int messageId, int shopId)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.UpdateReadStatus(messageId, shopId);
            }
        }


        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="type">The type.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool DeleteMessageInfo(int messageId, int shopId, int type)
        {
            using (var dal = FactoryDispatcher.MessageFactory())
            {
                return dal.DeleteMessageInfo(messageId,shopId,type);
            }
        }
    }
}
