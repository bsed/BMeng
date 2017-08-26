using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using HotCoreUtils.Uploader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BAMENG.API.Controllers
{
    /// <summary>
    /// 客户接口
    /// </summary>
    public class CustomerController : BaseController
    {
        /// <summary>
        /// 客户列表 POST: customer/list
        /// </summary>
        /// <param name="type">0所有客户 1未处理  2已处理</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult list(int type, int pageIndex, int pageSize)
        {
            try
            {
                UserModel user = GetUserData();
                if (user != null)
                {
                    var data = CustomerLogic.GetAppCustomerList(user.UserId, user.UserIdentity, type, pageIndex, pageSize);
                    return Json(new ResultModel(ApiStatusCode.OK, data));
                }
                return Json(new ResultModel(ApiStatusCode.令牌失效));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("list:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }
        /// <summary>
        /// 客户资源列表 POST: customer/reslist
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult reslist(int pageIndex, int pageSize)
        {
            try
            {
                var userId = GetAuthUserId();

                var data = CustomerLogic.GetAppCustomerResList(userId, pageIndex, pageSize);
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("reslist:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }




        /// <summary>
        /// 审核 POST: customer/audit
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <param name="status">审核状态 1已同意  2已拒绝</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult audit(int cid, int status)
        {
            try
            {
                var user = GetUserData();
                if (CustomerLogic.UpdateStatus(cid, status, user.UserId))
                {                    
                    //添加客户操作日志
                    LogLogic.AddCustomerLog(new LogBaseModel()
                    {
                        objId = cid,
                        UserId = user.UserId,
                        ShopId = user.ShopId,
                        AppSystem = OS,
                        OperationType = status
                    });

                    return Json(new ResultModel(ApiStatusCode.OK));
                }
                else
                    return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("audit:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }
        /// <summary>
        /// 创建客户 POST: customer/create
        /// </summary>
        /// <param name="username">客户姓名</param>
        /// <param name="mobile">客户手机</param>
        /// <param name="address">客户地址</param>
        /// <param name="remark">客户备注</param>
        /// <returns><![CDATA[{status:200,statusText:"OK",data:{}}]]></returns>
        [ActionAuthorize]
        public ActionResult create(string username, string mobile, string address, string remark)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return Json(new ResultModel(ApiStatusCode.姓名不能为空));
                if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsValidMobileNo(mobile))
                    return Json(new ResultModel(ApiStatusCode.无效手机号));
                if (string.IsNullOrEmpty(address))
                    return Json(new ResultModel(ApiStatusCode.地址不能为空));


                var user = GetUserData();
                //信息以电话和客户地址做为唯一性的判断标准,判断客户所属
                if (!CustomerLogic.IsExist(mobile, address))
                {
                    int flag = CustomerLogic.InsertCustomerInfo(new CustomerModel()
                    {
                        BelongOne = user.UserId,
                        BelongTwo = user.UserIdentity == 1 ? user.UserId : user.BelongOne,
                        Addr = address,
                        Mobile = mobile,
                        ShopId = user.ShopId,
                        Name = username,
                        Remark = remark,
                        Status = user.UserIdentity == 1 ? 3 : 0,
                        BelongOneShopId = user.ShopBelongId
                    });

                    if (flag > 0)
                    {

                        //添加客户操作日志
                        LogLogic.AddCustomerLog(new LogBaseModel()
                        {
                            objId = flag,
                            UserId = user.UserId,
                            ShopId = user.ShopId,
                            AppSystem = OS,
                            OperationType = 0 //操作类型0提交，1有效 2无效
                        });

                        if (user.UserIdentity == 1)
                        {
                            UserLogic.AddUserCustomerAmount(user.UserId);
                            //如果是盟主创建客户，则需要添加一条有效客户日志
                            //添加客户操作日志
                            LogLogic.AddCustomerLog(new LogBaseModel()
                            {
                                objId = flag,
                                UserId = user.UserId,
                                ShopId = user.ShopId,
                                AppSystem = OS,
                                OperationType = 1
                            });
                        }
                    }
                    return Json(new ResultModel(ApiStatusCode.OK));
                }
                else
                    return Json(new ResultModel(ApiStatusCode.客户已存在));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("create:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }




        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="username">姓名</param>
        /// <param name="mobile">手机</param>
        /// <param name="address">地址</param>
        /// <param name="remark">备注</param>
        /// <param name="issave">是否保存留底</param>
        /// <param name="ids">发送个盟友，盟友ID,多个用|隔开</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult addInfo(string username, string mobile, string address, string remark, int issave, string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return Json(new ResultModel(ApiStatusCode.姓名不能为空));
                if (string.IsNullOrEmpty(mobile) || !RegexHelper.IsValidMobileNo(mobile))
                    return Json(new ResultModel(ApiStatusCode.无效手机号));
                if (string.IsNullOrEmpty(address))
                    return Json(new ResultModel(ApiStatusCode.地址不能为空));


                var user = GetUserData();
                string[] TargetIds = null;
                if (user.UserIdentity == 1 && !string.IsNullOrEmpty(ids))
                {
                    TargetIds = ids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (TargetIds.Length <= 0)
                        return Json(new ResultModel(ApiStatusCode.缺少发送目标));
                }





                //信息以电话和客户地址做为唯一性的判断标准,判断客户所属
                if (!CustomerLogic.IsExist(mobile, address))
                {
                    var data = new CustomerModel()
                    {
                        BelongOne = user.UserId,
                        BelongTwo = user.UserIdentity == 1 ? user.UserId : user.BelongOne,
                        Addr = address,
                        Mobile = mobile,
                        ShopId = user.ShopId,
                        Name = username,
                        Remark = remark,
                        Status = user.UserIdentity == 1 ? 3 : 0,
                        BelongOneShopId = user.ShopBelongId,
                        isSave = 0
                    };


                    int flag = issave == 1 ? CustomerLogic.InsertCustomerInfo(data) : 0;

                    if (flag > 0 && issave == 1)
                    {
                        //添加客户操作日志
                        LogLogic.AddCustomerLog(new LogBaseModel()
                        {
                            objId = flag,
                            UserId = user.UserId,
                            ShopId = user.ShopId,
                            AppSystem = OS,
                            OperationType = 0 //操作类型0提交，1有效 2无效
                        });

                        if (user.UserIdentity == 1)
                        {
                            UserLogic.AddUserCustomerAmount(user.UserId);
                            //如果是盟主创建客户，则需要添加一条有效客户日志
                            //添加客户操作日志
                            LogLogic.AddCustomerLog(new LogBaseModel()
                            {
                                objId = flag,
                                UserId = user.UserId,
                                ShopId = user.ShopId,
                                AppSystem = OS,
                                OperationType = 1
                            });
                        }
                    }

                    if (TargetIds != null)
                    {
                        foreach (var id in TargetIds)
                        {
                            CustomerLogic.InsertCustomerRes(new CustomerResModel()
                            {
                                UserId = Convert.ToInt32(id),
                                Name = username,
                                Remark = remark,
                                Mobile = mobile,
                                SubmitName = user.RealName,
                                Type = 0,
                                Addr = address,
                                DataImg = ""
                            });
                        }
                    }


                    return Json(new ResultModel(ApiStatusCode.OK));
                }
                else
                    return Json(new ResultModel(ApiStatusCode.客户已存在));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("addInfo:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 添加客户图片信息
        /// </summary>
        /// <param name="remark">备注</param>
        /// <param name="issave">是否保存留底</param>
        /// <param name="ids">发送个盟友，盟友ID,多个用|隔开</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult addImgInfo(string remark, int issave, string ids)
        {            
            try
            {
                var user = GetUserData();

                string[] TargetIds = null;
                if (user.UserIdentity == 1 && !string.IsNullOrEmpty(ids))
                {
                    TargetIds = ids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (TargetIds.Length <= 0)
                        return Json(new ResultModel(ApiStatusCode.缺少发送目标));
                }

                HttpPostedFileBase oFile = Request.Files.Count > 0 ? Request.Files[0] : null;
                if (oFile == null)
                {
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
                }
                string fileName = GetUploadImagePath();
                Stream stream = oFile.InputStream;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                if (!FileUploadHelper.UploadFile(bytes, fileName))
                {
                    return Json(new ResultModel(ApiStatusCode.请上传图片));
                }

                var data = new CustomerModel()
                {
                    BelongOne = user.UserId,
                    BelongTwo = user.UserIdentity == 1 ? user.UserId : user.BelongOne,
                    Addr = "",
                    Mobile = "",
                    ShopId = user.ShopId,
                    Name = "",
                    Remark = remark,
                    Status = user.UserIdentity == 1 ? 3 : 0,
                    BelongOneShopId = user.ShopBelongId,
                    isSave = 1,
                    DataImg = fileName
                };
                int flag = issave == 1 ? CustomerLogic.InsertCustomerInfo(data) : 0;

                if (flag > 0 && issave == 1)
                {
                    //添加客户操作日志
                    LogLogic.AddCustomerLog(new LogBaseModel()
                    {
                        objId = flag,
                        UserId = user.UserId,
                        ShopId = user.ShopId,
                        AppSystem = OS,
                        OperationType = 0 //操作类型0提交，1有效 2无效
                    });

                    if (user.UserIdentity == 1)
                    {
                        UserLogic.AddUserCustomerAmount(user.UserId);
                        //如果是盟主创建客户，则需要添加一条有效客户日志
                        //添加客户操作日志
                        LogLogic.AddCustomerLog(new LogBaseModel()
                        {
                            objId = flag,
                            UserId = user.UserId,
                            ShopId = user.ShopId,
                            AppSystem = OS,
                            OperationType = 1
                        });
                    }
                }

                if (TargetIds != null)
                {
                    foreach (var id in TargetIds)
                    {
                        CustomerLogic.InsertCustomerRes(new CustomerResModel()
                        {
                            UserId = Convert.ToInt32(id),
                            Name = "",
                            Remark = remark,
                            Mobile = "",
                            SubmitName = user.RealName,
                            Type = 1,
                            Addr = "",
                            DataImg = fileName
                        });
                    }
                }
                return Json(new ResultModel(ApiStatusCode.OK));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("addImgInfo:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult details(int cid)
        {
            try
            {
                var data = CustomerLogic.GetModel(cid);
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("details:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 更新客户进店状态
        /// </summary>
        /// <param name="cid">客户ID</param>
        /// <param name="status">1进店 0未进店</param>
        /// <returns>ActionResult.</returns>
        [ActionAuthorize]
        public ActionResult UpdateInShop(int cid, int status)
        {
            try
            {
                if (CustomerLogic.UpdateInShopStatus(cid, status))
                    return Json(new ResultModel(ApiStatusCode.OK, "保存成功"));
                else
                    return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("UpdateInShop:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 添加客户维护信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult addAssert(int cid, string content)
        {
            try
            {
                var user = GetUserData();

                CustomerAssertModel model = new CustomerAssertModel()
                {
                    CID = cid,
                    AssertContent = content,
                    UserId = user.UserId
                };

                if (CustomerLogic.AddCustomerAssert(model) > 0)
                    return Json(new ResultModel(ApiStatusCode.OK, "保存成功"));
                else
                    return Json(new ResultModel(ApiStatusCode.添加失败, "保存失败"));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("AddCustomerAssert:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }


        /// <summary>
        /// 客户维护信息列表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult assertList(int cid, int pageIndex, int pageSize)
        {
            try
            {
                var user = GetUserData();
                var data = CustomerLogic.GetCustomerAssertList(cid, pageIndex, pageSize);
                return Json(new ResultModel(ApiStatusCode.OK, data));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("GetCustomerAssertList:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

        /// <summary>
        /// 修改客户状态(只有在客户信息“未生成订单”和“已失效”状态时，才能修改)
        /// </summary>
        /// <param name="cid">客户ＩＤ</param>
        /// <param name="status">１＝已失效　２＝未生成订单</param>
        /// <returns></returns>
        [ActionAuthorize]
        public ActionResult updateStatus(int cid, int status)
        {
            try
            {
                var user = GetUserData();
                if (user != null && user.UserIdentity == 1)
                {
                    var data = CustomerLogic.GetModel(cid);
                    if (data != null && (data.Status == 3 || data.Status == 5))
                    {
                        if (CustomerLogic.UpdateStatus(cid, status == 1 ? 5 : 3, user.UserId))
                            return Json(new ResultModel(ApiStatusCode.OK, "保存成功"));
                        else
                            return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
                    }
                }
                return Json(new ResultModel(ApiStatusCode.无操作权限));
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("updateStatus:message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                return Json(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
        }

    }
}