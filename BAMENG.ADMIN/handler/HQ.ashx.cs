﻿using BAMENG.CONFIG;
using BAMENG.LOGIC;
using BAMENG.MODEL;
using HotCoreUtils.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BAMENG.ADMIN.handler
{
    /// <summary>
    /// HQ--总部
    /// </summary>
    public class HQ : BaseLogicFactory, IHttpHandler
    {
        public AdminLoginModel user { get; set; }
        public new void ProcessRequest(HttpContext context)
        {
            string resultMsg = string.Format(@"{0} header:{1} Form:{2} UserAgent:{3} IP:{4};referrer:{5}"
                  , context.Request.Url.ToString()
                  , context.Request.Headers.ToString()
                  , context.Request.Form.ToString()
                  , StringHelper.ToString(context.Request.UserAgent)
                  , StringHelper.GetClientIP()
                  , context.Request.UrlReferrer != null ? StringHelper.ToString(context.Request.UrlReferrer.AbsoluteUri) : ""
                 );
            try
            {
                DoRequest(context);
                LogHelper.Log(resultMsg, LogHelperTag.INFO, WebConfig.debugMode());
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("{0} StackTrace:{1} Message:{2}", resultMsg, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
            }
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public int ShopID
        {
            get
            {
                return GetFormValue("shopId", 0);
            }
        }

        public int UserId
        {
            get
            {
                return GetFormValue("userid", 0);
            }
        }


        public string action { get { return GetFormValue("action", ""); } }

        public string json { get; set; }



        private void DoRequest(HttpContext context)
        {
            user = GetCurrentUser();
            if (user == null)
            {
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.没有登录));
                context.Response.ContentType = "application/json";
                context.Response.Write(json);
                return;
            }
            //全部转换为大写
            try
            {
                switch (action.ToUpper())
                {

                    case "GETHOMEDATA":
                        GetHomeData();
                        break;

                    case "GETSHOPLIST": //获取门店列表
                        GetShopList();
                        break;
                    case "UPDATESHOP":
                        UpdateShopInfo();
                        break;

                    case "DELETESHOP":
                        DeleteShop();
                        break;
                    case "UPDATESHOPACTIVE":
                        UpdateShopActive();
                        break;

                    case "GETUSERLIST":
                        GetUserList();
                        break;
                    case "EDITUSER":
                        EditUser();
                        break;

                    case "DELETEUSER":
                        DeleteUser();
                        break;
                    case "UPDATEUSERACTIVE":
                        UpdateUserActive();
                        break;
                    case "GETUSERINFO":
                        GetUserInfo();
                        break;
                    case "GETALLYLIST":
                        GetAllyList();
                        break;
                    case "GETCUSTOMERLIST":
                        GetCustomerList();
                        break;

                    case "EDITCUSTOMERINFO":
                        EditCustomerInfo();
                        break;
                    case "DELETECUSTOMERINFO":
                        DeleteCustomerInfo();
                        break;

                    case "GETLEVELLIST":
                        GetLevelList();
                        break;
                    case "DELETELEVEL":
                        DeleteLevel();
                        break;
                    case "EDITLEVEL":
                        EditLevel();
                        break;
                    case "GETARTICLELIST":
                        GetArticleList();
                        break;
                    case "GETARTICLEINFO":
                        GetArticleInfo();
                        break;
                    case "UPDATEARTICLECODE":
                        UpdateArticleCode();
                        break;
                    case "EDITARTICLE":
                        EditArticle();
                        break;
                    case "GETMENULIST":
                        GetMenuList();
                        break;

                    case "GETFOCUSPICLIST":
                        GetFocusPicList();
                        break;
                    case "EDITFOCUSPIC":
                        EditFocusPic();
                        break;
                    case "DELETEFOCUSPIC":
                        DeleteFocusPic();
                        break;
                    case "SETFOCUSENABLE":
                        SetFocusEnable();
                        break;
                    case "GETCONFIGLIST":
                        GetConfiglist();
                        break;
                    case "EDITCONFIG":
                        EditConfig();
                        break;


                    case "EDITMANAGER":
                        EditManager();
                        break;
                    case "DELETEMANAGER":
                        DeleteManager();
                        break;
                    case "SETMANAGERUSERSTATUS":
                        SetManagerUserStatus();
                        break;
                    case "GETMANAGERLIST":
                        GetManagerList();
                        break;


                    case "GETCASHCOUPONLIST":
                        GetCashCouponList();
                        break;
                    case "DELETECASHCOUPON":
                        DeleteCashCoupon();
                        break;
                    case "EDITCASHCOUPON":
                        EditCashCoupon();
                        break;

                    case "SETCOUPONENABLE":
                        SetCouponEnable();
                        break;

                    case "GETMESSAGELIST":
                        GetMessageList();
                        break;
                    case "EDITMESSAGE":
                        EditMessage();
                        break;
                    case "DELETEMESSAGE":
                        DeleteMessage();
                        break;
                    case "GETMESSAGEINFO":
                        GetMessageInfo();
                        break;
                    case "GETMESSAGESHOPLIST":
                        GetMessageShopList();
                        break;

                    case "GETORDERLIST":
                        GetOrderList();
                        break;
                    case "UPDATEORDERSTATUS":
                        UpdateOrderStatus();
                        break;
                    case "GETORDERINFO":
                        GetOrderInfo();
                        break;


                    #region 统计


                    case "LOGINSTATISTICS":// 登录统计
                        LoginStatistics();
                        break;
                    case "GETUSERLOGINLIST": //登录流水
                        GetUserLoginList();
                        break;

                    case "USERSIGNSTATISTICS":// 签到统计
                        UserSignStatistics();
                        break;
                    case "GETSIGNLOGINLIST": //签到流水
                        GetSignLoginList();
                        break;

                    case "CUSTOMERSTATISTICS": //客户统计
                        CustomerStatistics();
                        break;

                    case "COUPONSTATISTICS": //现金券统计
                        CouponStatistics();
                        break;
                    case "ORDERSTATISTICS": //订单统计
                        OrderStatistics();
                        break;

                    case "COUPONSTATISTICSPIE":
                        CouponStatisticsPie();
                        break;

                    case "ORDERSTATISTICSPIE":
                        OrderStatisticsPie();
                        break;
                    case "CUSTOMERSTATISTICSPIE":
                        CustomerStatisticsPie();
                        break;
                    #endregion

                    case "GETCOUPONLOGLIST":
                        GetCouponlogList();
                        break;

                    case "MODIFYPASSWORD":
                        modifypassword();
                        break;

                    case "GETMAILLIST":
                        GetMailList();
                        break;
                    case "ADDREPLY":
                        addReply();
                        break;

                    case "GETWORKREPORTLIST":
                        getWorkReportList();
                        break;
                    case "EDITWORKREPORT":
                        EditWorkReport();
                        break;
                    case "DELETEWORKREPORT":
                        DeleteWorkReport();
                        break;

                    case "DELETEUSERREPORT":
                        DeleteUserReport();
                        break;
                    case "GETUSERREPORTLIST":
                        GetUserReportList();
                        break;
                    case "GETUSERREPORTMODEL":
                        GetUserReportModel();
                        break;

                    case "SETALLYREWARD":
                        SetAllyReward();
                        break;
                    case "GETALLYREWARD":
                        GetAllyReward();
                        break;


                    case "GETASSERTLIST"://获取维护列表
                        GetAssertList();
                        break;
                    default:
                        break;


                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(string.Format("action:{0} StackTrace:{1} Message:{2}", action, ex.StackTrace, ex.Message), LogHelperTag.ERROR);
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.SERVICEERROR));
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(json);

        }



        /// <summary>
        /// 获取首页数据
        /// </summary>
        private void GetHomeData()
        {
            List<AdminHomeDataModel> data = SystemLogic.GetHomeData(user);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["list"] = data;
            dict["identity"] = user.UserIndentity;
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, dict));
        }


        /// <summary>
        /// 获取门店列表
        /// </summary>
        private void GetShopList()
        {
            int shopType = user.UserIndentity == 0 ? 1 : 2;
            int shopBelongId = user.UserIndentity == 0 ? 0 : user.ID;
            int t = GetFormValue("type", 0);
            if (t == 2)
                shopType = 2;
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                city = GetFormValue("city", ""),
                province = GetFormValue("prov", ""),
                type = GetFormValue("type", 0)
            };
            var data = ShopLogic.GetShopList(shopType, shopBelongId, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 修改门店信息
        /// </summary>
        private void UpdateShopInfo()
        {
            string shopname = GetFormValue("shopname", "");
            string username = GetFormValue("username", "");
            string usermobile = GetFormValue("usermobile", "");
            string userloginname = GetFormValue("userloginname", "");
            string password = GetFormValue("password", "");
            string shopprov = GetFormValue("shopprov", "");
            string shopcity = GetFormValue("shopcity", "");
            string shopaddress = GetFormValue("shopaddress", "");
            ApiStatusCode apiCode;
            bool flag = ShopLogic.EditShopInfo(new ShopModel()
            {
                ShopID = ShopID,
                ShopName = shopname,
                ShopArea = "",
                ShopAddress = shopaddress,
                ShopBelongId = user.UserIndentity == 0 ? 0 : user.ID,
                ShopCity = shopcity,
                ShopProv = shopprov,
                Contacts = username,
                ContactWay = usermobile,
                LoginName = userloginname,
                LoginPassword = password,
                IsActive = 1,
                ShopType = user.UserIndentity == 0 ? 1 : 2
            }, out apiCode);
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(apiCode));
        }


        /// <summary>
        /// Deletes the shop.
        /// </summary>
        private void DeleteShop()
        {
            if (ShopLogic.DeleteShop(ShopID))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 冻结或解冻门店
        /// </summary>
        private void UpdateShopActive()
        {
            int active = GetFormValue("active", 1);
            if (ShopLogic.UpdateShopActive(ShopID, active))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }

        /// <summary>
        /// 获取用户列表(总后台，获取盟主列表)
        /// </summary>
        private void GetUserList()
        {

            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0),
                type = user.UserIndentity
            };
            int currentUserId = user.UserIndentity != 0 ? user.ID : 0;
            var data = UserLogic.GetUserList(currentUserId, GetFormValue("ally", 1), model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        private void EditUser()
        {
            ApiStatusCode apiCode = ApiStatusCode.OK;
            //方便测试，此处门店ID写死，真实逻辑是，根据当前登录的门店账户，获取门店ID
            bool flag = UserLogic.EditUserInfo(new UserRegisterModel()
            {
                loginName = GetFormValue("userloginname", ""),
                username = GetFormValue("username", ""),
                nickname = GetFormValue("usernickname", ""),
                loginPassword = EncryptHelper.MD5(GetFormValue("password", "")),
                mobile = GetFormValue("usermobile", ""),
                storeId = ConstConfig.storeId,
                ShopId = user.ID,
                BelongShopId = user.ShopBelongId,
                UserIdentity = GetFormValue("ally", 1),
                UserId = UserId,
                mengBeans=GetFormValue("mengbeans", 0),
                currentMengBeans = GetFormValue("currentmengbeans", 0)
            }, ref apiCode);
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(apiCode));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        private void DeleteUser()
        {
            if (UserLogic.DeleteUser(UserId))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 冻结、解冻用户账户
        /// </summary>
        private void UpdateUserActive()
        {
            int active = GetFormValue("active", 1);
            if (UserLogic.UpdateUserActive(UserId, active))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        private void GetUserInfo()
        {
            var data = UserLogic.GetModel(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 根据盟主ID，获取他的盟友列表
        /// </summary>
        private void GetAllyList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                UserId = UserId,
                IsDesc = true,
                orderbyCode = -1
            };
            var data = UserLogic.GetAllyList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 根据用户ID获取该用户下的客户列表
        /// </summary>
        private void GetCustomerList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                UserId = UserId,
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0),
                type = user.UserIndentity,
                Status = GetFormValue("status", -1)
            };
            var data = CustomerLogic.GetCustomerList(model, user.UserIndentity == 0 ? 0 : user.ID);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑客户信息
        /// </summary>
        private void EditCustomerInfo()
        {
            int sltuserstatus = GetFormValue("sltuserstatus", 1);
            int userdefaultstatus = GetFormValue("userdefaultstatus", 0);
            int status = 0;
            if (userdefaultstatus > 0)
                status = userdefaultstatus;
            else
                status = sltuserstatus;
            CustomerModel model = new CustomerModel()
            {
                ID = GetFormValue("customerid", 0),
                Name = GetFormValue("username", ""),
                Mobile = GetFormValue("usermobile", ""),
                Addr = GetFormValue("useraddress", ""),
                Status = status
            };

            if (CustomerLogic.UpdateCustomerInfo(model))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }


        /// <summary>
        /// 删除客户信息
        /// </summary>
        private void DeleteCustomerInfo()
        {
            if (CustomerLogic.DeleteCustomerInfo(GetFormValue("customerid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }


        /// <summary>
        /// 获取等级
        /// </summary>
        public void GetLevelList()
        {
            var data = UserLogic.GetLevelList(GetFormValue("type", -1));
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 删除等级
        /// </summary>
        private void DeleteLevel()
        {
            if (UserLogic.DeleteLevel(GetFormValue("levelId", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }
        /// <summary>
        /// 编辑等级
        /// </summary>
        private void EditLevel()
        {
            int levelId = GetFormValue("levelid", 0);
            int levelType = GetFormValue("leveltype", 0);
            string levelname = GetFormValue("levelname", "");
            int upgradeCount = GetFormValue("levelmembernum", 0);
            if (UserLogic.EditLevel(levelId, levelType, levelname, upgradeCount))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }


        /// <summary>
        /// 获取资讯列表
        /// </summary>
        public void GetArticleList()
        {


            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                searchType = GetFormValue("searchType", 0),
                Status = GetFormValue("type", 1)
            };

            //作者身份类型，0集团，1总店，2分店  3盟主 4盟友
            //AuthorIdentity 根据作者ID来判断作者身份           

            int AuthorIdentity = user.UserIndentity;
            int AuthorId = user.ID;
            var data = ArticleLogic.GetArticleList(AuthorId, AuthorIdentity, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }
        /// <summary>
        /// 获取资讯信息
        /// </summary>
        public void GetArticleInfo()
        {
            int articleId = GetFormValue("articleId", 0);
            var data = ArticleLogic.GetModel(articleId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 修改状态
        /// </summary>
        private void UpdateArticleCode()
        {
            int articleId = GetFormValue("articleId", 0);
            int type = GetFormValue("type", 0);//1修改置顶，2修改发布，3，删除 ,4审核
            int active = GetFormValue("active", 0);
            string remark = GetFormValue("remark", "");

            bool flag = false;
            if (type == 1)//置顶
                flag = ArticleLogic.SetArticleEnableTop(articleId, active == 1, user.UserIndentity);
            else if (type == 2)//发布
                flag = ArticleLogic.SetArticleEnablePublish(articleId, active == 1);
            else if (type == 3)//删除
                flag = ArticleLogic.DeleteArticle(articleId);
            else if (type == 4)//审核
                flag = ArticleLogic.SetArticleStatus(articleId, active, remark);

            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }

        /// <summary>
        /// 编辑资讯
        /// </summary>
        private void EditArticle()
        {
            int ArticleId = GetFormValue("articleid", 0);
            DateTime dtnow = DateTime.Now;
            //方便测试，此处门店ID写死，真实逻辑是，根据当前登录的门店账户，获取门店ID
            bool flag = ArticleLogic.EditArticle(new ArticleModel()
            {
                ArticleId = GetFormValue("articleid", 0),
                ArticleIntro = HttpUtility.UrlDecode(GetFormValue("intro", "")),
                ArticleTitle = HttpUtility.UrlDecode(GetFormValue("title", "")),
                EnableTop = GetFormValue("top", 0),
                EnablePublish = GetFormValue("publish", 0),
                ArticleCover = GetFormValue("cover", ""),
                ArticleBody = HttpUtility.UrlDecode(GetFormValue("content", "")),
                SendTargetId = GetFormValue("targetid", 0),
                ArticleSort = 0,
                ArticleStatus = user.UserIndentity == 0 ? 1 : 0,
                AuthorName = user.UserName,
                AuthorId = user.ID,
                AuthorIdentity = user.UserIndentity,
                PublishTime = dtnow,
                TopTime = dtnow
            });
            if (flag)
            {
                if (user.UserIndentity != 0)
                {
                    try
                    {
                        string errmsg = "";
                        string sendmobile = ConfigLogic.GetValue("BindMobile");
                        if (!string.IsNullOrEmpty(sendmobile) && RegexHelper.IsValidMobileNo(sendmobile))
                            SmsLogic.send(1, sendmobile, "您有一条新的待审核资讯", out errmsg);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(string.Format("Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                    }
                }
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ArticleId > 0 ? ApiStatusCode.更新失败 : ApiStatusCode.添加失败));
        }



        /// <summary>
        /// 获取菜单
        /// </summary>
        private void GetMenuList()
        {
            ApiStatusCode appCode = ApiStatusCode.OK;
            SystemLeftModel resultData = new SystemLeftModel();
            if (CheckLogin(ref appCode))
            {
                resultData.menuData = SystemLogic.GetMenuList(user.UserIndentity);
                resultData.userData = user;
                resultData.authority = "";
            }
            json = JsonHelper.JsonSerializer(new ResultModel(appCode, resultData));
        }


        /// <summary>
        /// 获取焦点广告图
        /// </summary>
        private void GetFocusPicList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                type = GetFormValue("type", 0),//0 资讯轮播图 1首页轮播图
            };
            var data = FocusPicLogic.GetList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 编辑焦点广告图
        /// </summary>
        private void EditFocusPic()
        {
            FocusPicModel model = new FocusPicModel()
            {
                ID = GetFormValue("focusid", 0),
                Title = GetFormValue("focustitle", ""),
                LinkUrl = GetFormValue("focuslinkurl", ""),
                PicUrl = GetFormValue("focuspicurl", ""),
                Type = GetFormValue("type", 0),
                IsEnable = GetFormValue("focusenable", 0),
                Sort = GetFormValue("focussort", 0),
                Description = GetFormValue("focusdescription", "")
            };
            if (FocusPicLogic.EditFocusPic(model))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }

        /// <summary>
        /// 删除焦点广告图
        /// </summary>
        private void DeleteFocusPic()
        {
            if (FocusPicLogic.DeleteFocusPic(GetFormValue("focusid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }

        /// <summary>
        /// 设置轮播图启用或禁用
        /// </summary>
        private void SetFocusEnable()
        {
            if (FocusPicLogic.SetEnable(GetFormValue("focusid", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }

        /// <summary>
        /// 编辑配置
        /// </summary>
        private void GetConfiglist()
        {
            var data = ConfigLogic.GetConfigList();
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 编辑配置
        /// </summary>
        private void EditConfig()
        {
            string config = HttpUtility.UrlDecode(GetFormValue("config", ""));
            if (!string.IsNullOrEmpty(config))
            {
                List<ConfigModel> lst = JsonHelper.JsonDeserialize<List<ConfigModel>>(config);
                ConfigLogic.UpdateValue(lst);
            }
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }



        /// <summary>
        /// Edits the user.
        /// </summary>
        private void EditManager()
        {
            bool flag = ManagerLogic.EditUser(new AdminLoginModel()
            {
                LoginName = GetFormValue("loginName", ""),
                LoginPassword = EncryptHelper.MD5(GetFormValue("password", "")),
                RoleId = GetFormValue("roleid", 0),
                UserEmail = GetFormValue("email", ""),
                UserMobile = GetFormValue("mobile", ""),
                UserName = GetFormValue("name", ""),
                UserStatus = 1,
                ID = UserId
            });
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Deletes the manager.
        /// </summary>
        private void DeleteManager()
        {
            ManagerLogic.Delete(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Sets the manager user status.
        /// </summary>
        private void SetManagerUserStatus()
        {
            ManagerLogic.SetUserStatus(UserId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 获取管理员列表
        /// </summary>
        private void GetManagerList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", "")
            };
            var data = ManagerLogic.GetManagerList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 获取现金券
        /// </summary>
        private void GetCashCouponList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                Status = GetFormValue("status", -100)
            };
            var data = CouponLogic.GetCashCouponList(user.ID, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 删除现金券
        /// </summary>
        private void DeleteCashCoupon()
        {
            int couponId = GetFormValue("couponId", 0);
            CouponLogic.DeleteCashCoupon(couponId);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }
        /// <summary>
        /// 编辑优惠券
        /// </summary>
        private void EditCashCoupon()
        {
            bool flag = CouponLogic.EditCashCoupon(new CashCouponModel()
            {
                CouponId = GetFormValue("couponid", 0),
                Money = GetFormValue("couponmoney", 0),
                Title = GetFormValue("coupontitle", ""),
                StartTime = Convert.ToDateTime(GetFormValue("couponstarttime", DateTime.Now.ToString("yyyy-MM-dd"))),
                EndTime = Convert.ToDateTime(GetFormValue("couponendtime", DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"))),
                ShopId = user.ID,
                IsEnable = GetFormValue("couponenable", 1),
                Remark = GetFormValue("couponremark", "")
            });
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// Sets the coupon enable.
        /// </summary>
        private void SetCouponEnable()
        {
            if (CouponLogic.SetCouponEnable(GetFormValue("couponId", 0)))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }


        /// <summary>
        /// 获取消息列表
        /// </summary>
        private void GetMessageList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                type = GetFormValue("type", 1)
            };

            var data = MessageLogic.GetMessageList(user.ID, user.UserIndentity, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        ///修改消息
        /// </summary>
        private void EditMessage()
        {

            string sendtarget = GetFormValue("sendtarget", "");

            bool flag = MessageLogic.EditMessage(user.UserIndentity, user.ShopBelongId, new MessageModel()
            {
                ID = GetFormValue("messageid", 0),
                MessageBody = HttpUtility.UrlDecode(GetFormValue("content", "")),
                Title = HttpUtility.UrlDecode(GetFormValue("title", "")),
                AuthorName = user.UserName,
                IsSend = GetFormValue("issend", 1),
                IsSendBelongShopId = GetFormValue("sendbelongshop", 0),
                SendTargetIds = GetFormValue("sendtarget", ""),
                AuthorId = user.ID,
                AuthorIdentity = user.UserIndentity
            });
            if (flag)
            {
                //if (user.UserIndentity == 1 && GetFormValue("issend", 1) == 1 && GetFormValue("sendbelongshop", 0) == 1)
                //{
                //    try
                //    {
                //        string errmsg = "";
                //        string sendmobile = ConfigLogic.GetValue("BindMobile");
                //        if (!string.IsNullOrEmpty(sendmobile) && RegexHelper.IsValidMobileNo(sendmobile))
                //            SmsLogic.send(1, sendmobile, "您有一条新的未读消息", out errmsg);
                //    }
                //    catch (Exception ex)
                //    {
                //        LogHelper.Log(string.Format("Message:{0},StackTrace:{1}", ex.Message, ex.StackTrace), LogHelperTag.ERROR);
                //    }
                //}
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.操作失败));
        }
        /// <summary>
        /// 删除信息
        /// </summary>
        private void DeleteMessage()
        {
            int type = GetFormValue("type", 1);
            if (MessageLogic.DeleteMessageInfo(GetFormValue("messageid", 0), user.UserIndentity == 0 ? -1 : user.ID, type))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.删除失败));
        }


        /// <summary>
        /// 获取消息通知
        /// </summary>
        private void GetMessageInfo()
        {
            int type = GetFormValue("type", 1);
            int messageId = GetFormValue("messageid", 0);
            var data = MessageLogic.GetModel(messageId);
            if (type == 2)
            {
                if (user.UserIndentity == 0)
                    MessageLogic.UpdateReadStatus(messageId);
                else
                    MessageLogic.UpdateReadStatus(messageId, user.ID);
            }
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 消息通知页面的门店列表
        /// </summary>
        private void GetMessageShopList()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (user.UserIndentity != 2)
            {
                var data = ShopLogic.GetShopList(user.UserIndentity == 0 ? 1 : 2, user.ID);
                dict["list"] = data;
            }
            else
            {
                dict["list"] = null;
            }
            dict["useridentity"] = user.UserIndentity;
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, dict));

        }



        /// <summary>
        /// 获取订单列表
        /// </summary>
        private void GetOrderList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                type = GetFormValue("type", -1)
            };
            if (user.UserIndentity != 0)
            {
                var data = OrderLogic.GetOrderList(user.ID, user.UserIndentity == 1 ? 1 : 0, model);
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.NULL));

        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        private void UpdateOrderStatus()
        {
            int type = GetFormValue("type", 1);
            if (OrderLogic.UpdateOrderStatus(GetFormValue("orderid", ""), type))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.更新失败));
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        private void GetOrderInfo()
        {
            var data = OrderLogic.GetOrderDetail(GetFormValue("orderid", ""));
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }





        #region 统计


        /// <summary>
        /// 登录统计
        /// </summary>
        private void LoginStatistics()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }
            var data = LogLogic.LoginStatistics(user, type, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 用户签到统计
        /// </summary>
        private void UserSignStatistics()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }
            var data = LogLogic.UserSignStatistics(user, type, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }



        /// <summary>
        ///获取客户统计
        /// </summary>
        /// <param name="shopId">The shop identifier.</param>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>List&lt;StatisticsListModel&gt;.</returns>
        private void CustomerStatistics()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }
            var data = LogLogic.CustomerStatistics(user, type, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 现金券统计
        /// </summary>
        private void CouponStatistics()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            var data = LogLogic.CouponStatistics(user, type, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        private void CouponStatisticsPie()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            var data = LogLogic.CouponStatisticsPie(user, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 订单统计
        /// </summary>
        private void OrderStatistics()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            var data = LogLogic.OrderStatistic(user, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 订单统计饼图
        /// </summary>
        private void OrderStatisticsPie()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            var data = LogLogic.OrderStatisticsPie(user, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        ///客户信息饼状图
        /// </summary>
        private void CustomerStatisticsPie()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            var data = LogLogic.CustomerStatisticsPie(user, beginTime, endTime);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        #endregion



        /// <summary>
        /// 获取领取记录
        /// </summary>
        private void GetCouponlogList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
                Status = GetFormValue("searchType", -1)
            };
            int couponId = GetFormValue("couponId", 0);
            var data = CouponLogic.GetUserCashCouponLogList(couponId, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        private void modifypassword()
        {
            string oldpwd = GetFormValue("oldpwd", "");
            string newpwd = GetFormValue("newpwd", "");
            if (ManagerLogic.ChanagePassword(user.ID, user.UserIndentity, oldpwd, newpwd))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.旧密码不对));
        }




        /// <summary>
        /// 登录流水
        /// </summary>
        private void GetUserLoginList()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = beginTime,
                endTime = endTime
            };
            var data = LogLogic.GetUserLoginList(user.ID, user.UserIndentity, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        /// <summary>
        /// 签到流水
        /// </summary>
        private void GetSignLoginList()
        {
            int type = GetFormValue("type", 0);
            string beginTime = GetFormValue("beginTime", "");
            string endTime = GetFormValue("endTime", "");
            if (type != 0)
            {
                beginTime = DateTime.Now.AddDays(-(type - 1)).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (string.IsNullOrEmpty(beginTime) || string.IsNullOrEmpty(endTime))
                {
                    beginTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    beginTime = Convert.ToDateTime(beginTime).ToString("yyyy-MM-dd");
                    endTime = Convert.ToDateTime(endTime).ToString("yyyy-MM-dd");
                }
            }

            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = beginTime,
                endTime = endTime
            };
            var data = LogLogic.GetSignLoginList(user.ID, user.UserIndentity, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        private void GetMailList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
            };
            var data = ArticleLogic.GetMailList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        private void addReply()
        {
            int mailid = GetFormValue("mailid", 0);
            int SendType = GetFormValue("sendtype", 0);
            string content = HttpUtility.UrlDecode(GetFormValue("content", ""));
            string title = HttpUtility.UrlDecode(GetFormValue("title", ""));
            int userId = 0;
            MailModel model = new MailModel();
            model.AuthorId = -1;
            model.AuthorName = "霸盟";
            model.Title = title;
            model.BodyContent = content;
            model.CoverUrl = WebConfig.articleDetailsDomain() + "/static/img/profile_small.jpg";
            model.SendType = 2;
            model.ReplyPid = mailid;
            model.ReplyUserId = user.ID;
            model.PhoneModel = "System admin";
            if (ArticleLogic.AddMailInfo(model) > 0)
            {
                //将该消息接收人的已阅读状态改为未读
                LogLogic.UpdateMailNotReadStatus(userId, mailid);
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            }
        }


        /// <summary>
        /// 获取工作汇报
        /// </summary>
        private void getWorkReportList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
            };
            var data = SystemLogic.GetWorkReportList(model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

        private void EditWorkReport()
        {
            int workid = GetFormValue("workid", 0);
            string content = HttpUtility.UrlDecode(GetFormValue("content", ""));
            bool flag = workid > 0 ? SystemLogic.UpdateWorkReport(workid, content) : SystemLogic.AddWorkReport(content) > 0;
            if (flag)
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.保存失败));
        }

        /// <summary>
        /// 删除工作汇报
        /// </summary>
        private void DeleteWorkReport()
        {
            int workid = GetFormValue("workid", 0);
            if (SystemLogic.DeleteWorkReport(workid))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.保存失败));

        }


        /// <summary>
        /// 获取用户工作汇报
        /// </summary>
        private void GetUserReportList()
        {
            SearchModel model = new SearchModel()
            {
                PageIndex = Convert.ToInt32(GetFormValue("pageIndex", 1)),
                PageSize = Convert.ToInt32(GetFormValue("pageSize", 20)),
                startTime = GetFormValue("startTime", ""),
                endTime = GetFormValue("endTime", ""),
                key = GetFormValue("key", ""),
            };
            var data = UserLogic.GetUserReportList(user.ID, model);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 获取工作汇报
        /// </summary>
        private void GetUserReportModel()
        {
            int workid = GetFormValue("workid", 0);
            var data = UserLogic.GetUserReportModel(workid);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }


        /// <summary>
        /// 删除用户工作汇报
        /// </summary>
        private void DeleteUserReport()
        {
            int workid = GetFormValue("workid", 0);
            if (UserLogic.DeleteUserReport(workid))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.保存失败));
        }


        /// <summary>
        /// 设置盟友奖励
        /// </summary>
        private void SetAllyReward()
        {
            decimal creward = GetFormValue("creward", 0);
            decimal orderreward = GetFormValue("orderreward", 0);
            string extrareward = GetFormValue("extrareward", "");

            int TipHours = GetFormValue("TipHours", 24);
            ShopLogic.AddShopTipHours(user.ID, TipHours);

            if (UserLogic.SetAllyRaward(user.ID, creward, orderreward, 0, extrareward))
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.保存失败));
        }
        /// <summary>
        /// 获取盟友奖励设置
        /// </summary>
        private void GetAllyReward()
        {
            RewardsSettingModel data = UserLogic.GetRewardModel(user.ID);
            if (data != null)
            {
                data.TipHours = ShopLogic.GetShopTipHours(user.ID);
                if (data.TipHours == 0)
                    data.TipHours = 24;
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
            }
            else
                json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK));
        }

        /// <summary>
        /// 获取维护信息列表
        /// </summary>
        private void GetAssertList()
        {
            int cid = GetFormValue("cid", 0);
            var data = CustomerLogic.GetCustomerAssertList(cid, 1, 100);
            json = JsonHelper.JsonSerializer(new ResultModel(ApiStatusCode.OK, data));
        }

    }
}