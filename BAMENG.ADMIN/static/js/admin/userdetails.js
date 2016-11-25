/// <reference path="../plugins/sweetalert/sweetalert.min.js" />
/// <reference path="../jquery.min.js" />
/// <reference path="../plugins/hot/Jquery.util.js" />

/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

var userdetailsHelper = {
    ajaxUrl: "/handler/HQ.ashx",
    currentUserId: hotUtil.getQuery("userid"),
    isAlly: hotUtil.getQuery("type"),
    hisUserLoaded: false,
    hisCustomerLoaded: false,
    /*
     * 获取用户信息
     */
    getUserInfo: function () {
        var self = this;
        var postData = {
            action: "getuserinfo",
            userid: this.currentUserId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        $("#spanLoginName").text(ret.data.LoginName);
                        $("#spanUserName").text(ret.data.RealName);
                        $("#spanUserMobile").text(ret.data.UserMobile);
                        $("#spanShopName").text(ret.data.ShopProv + " " + ret.data.ShopCity + " " + ret.data.ShopName);
                        $("#spanNickName").text(ret.data.NickName);
                        $("#spanUserScore").text(ret.data.Score - ret.data.ScoreLocked);
                        $("#spanMengBeans").text(ret.data.MengBeans - ret.data.MengBeansLocked);
                        $("#spanOrderSuccessAmount").text(ret.data.OrderSuccessAmount);
                        $("#spanCustomerAmount").text(ret.data.CustomerAmount);
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    //他的盟友列表
    hisAllyList: function (page) {
        if (this.hisUserLoaded)
            return;
        var self = this;
        var postData = {
            action: "GetAllyList",
            pageIndex: page,
            pageSize: 20,
            userid: this.currentUserId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist").html();
                            tempHtml = tempHtml.replace("{LoginName}", item.LoginName);
                            tempHtml = tempHtml.replace(/{UserId}/gm, item.UserId);
                            tempHtml = tempHtml.replace(/{RealName}/g, item.RealName);
                            tempHtml = tempHtml.replace("{NickName}", item.NickName);
                            tempHtml = tempHtml.replace("{UserMobile}", item.UserMobile);
                            tempHtml = tempHtml.replace("{MengBeans}", item.MengBeans - item.MengBeansLocked);
                            tempHtml = tempHtml.replace("{Score}", item.Score - item.ScoreLocked);
                            tempHtml = tempHtml.replace("{CustomerAmount}", item.CustomerAmount);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);                            
                            if (!hotUtil.isNullOrEmpty(item.UserHeadImg))
                                tempHtml = tempHtml.replace("{UserHeadImg}", item.UserHeadImg);
                            else
                                tempHtml = tempHtml.replace("{UserHeadImg}", "/static/img/bg.png");
                            tempHtml = tempHtml.replace("{OrderSuccessAmount}", item.OrderSuccessAmount);
                            tempHtml = tempHtml.replace("{ActiveStatus}", item.IsActive == 1 ? "<span style='color:red;'>激活</span>" : "已冻结")
                            listhtml += tempHtml;
                        });
                        $("#listMode").html(listhtml);
                        userdetailsHelper.hisUserLoaded = true;
                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination1", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                userdetailsHelper.hisUserLoaded = false;
                                userdetailsHelper.hisAllyList(page);
                            });
                        });
                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    //他的客户
    hisCustomerList: function (page) {
        if (this.hisCustomerLoaded)
            return;
        var self = this;
        var postData = {
            action: "GetCustomerList",
            pageIndex: page,
            pageSize: 20,
            userid: this.currentUserId
        }
        hotUtil.loading.show();
        hotUtil.ajaxCall(this.ajaxUrl, postData, function (ret, err) {
            if (ret) {
                if (ret.status == 200) {
                    if (ret.data) {
                        var listhtml = "";
                        $.each(ret.data.Rows, function (i, item) {
                            var tempHtml = $("#templist2").html();
                            tempHtml = tempHtml.replace(/{ID}/gm, item.ID);
                            tempHtml = tempHtml.replace("{Name}", item.Name);
                            tempHtml = tempHtml.replace("{BelongOneName}", item.BelongOneName);
                            tempHtml = tempHtml.replace("{BelongTwoName}", item.BelongTwoName);
                            tempHtml = tempHtml.replace("{Mobile}", item.Mobile);
                            tempHtml = tempHtml.replace("{Addr}", item.Addr);
                            tempHtml = tempHtml.replace("{CreateTime}", item.CreateTime);
                            listhtml += tempHtml;
                        });
                        $("#listMode2").html(listhtml);
                        userdetailsHelper.hisCustomerLoaded = true;

                        //初始化分页
                        var pageinate = new hotUtil.paging(".pagination2", ret.data.PageIndex, ret.data.PageSize, ret.data.PageCount, ret.data.Total);
                        pageinate.init((p) => {
                            goTo(p, function (page) {
                                userdetailsHelper.hisCustomerLoaded = false;
                                userdetailsHelper.hisCustomerList(page);
                            });
                        });

                    }
                }
            }
            hotUtil.loading.close();
        });
    },
    pageInit: function () {
        userdetailsHelper.getUserInfo();        
        if (this.isAlly == 1)
            $(".tabAlly").hide();
    },

}


$(function () {
    userdetailsHelper.pageInit();
});