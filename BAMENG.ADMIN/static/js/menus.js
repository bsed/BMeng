/// <reference path="plugins/hot/Jquery.util.js" />
/// <reference path="jquery.min.js" />
/// <reference path="plugins/sweetalert/sweetalert.min.js" />
/// <reference path="plugins/metisMenu/jquery.metisMenu.js" />
/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

var menusulTemplate = '<ul class="nav {levelClass}">{li}</ul>'
var menusliTemplate = '<li><a class="{menuItemClass}" href="{linkUrl}"><i class="fa {icons}"></i><span class="nav-label">{menuName}</span><span class="fa {arrow}"></span></a>{childMenus}</li>';

$(function () {

    var authority = "";
    var firstList;
    var menuListProvider = {
        menuList: [],
        getChildMenu: function (parentid) {
            var resultList = [];
            $.each(this.menuList, function (o, item) {
                if (authority == "" || authority.indexOf("|" + item.ItemCode + "|") >= 0) {
                    if (item.ItemParentCode == parentid && item.ItemShow == 1) {
                        resultList.push(item);
                    }
                }
            });
            return resultList;
        }
    };


    //默认菜单，如果没有设置菜单的话，则显示默认菜单index_v1.html
    //function defaultMenus() {
    //    menuListProvider.menuList = getDemoData();
    //}


    function LoadMenu() {
        if (menuListProvider.menuList == null || menuListProvider.menuList.length == 0) {
            //请求头
            var postData = {
                action: "GetMenuList"
            }
            hotUtil.ajaxCall("handler/HQ.ashx", postData, function (ret, err) {
                if (ret != null) {
                    if (ret.status == 200) {
                        var userData = ret.data.userData;
                        if (userData != null) {
                            $(".loginname").text(userData.LoginName);
                            $(".loginusername").text(userData.UserName)
                            menuListProvider.menuList = ret.data.menuData;
                            authority = ret.data.authority;
                        }
                    }
                    else {
                        if (ret.status == 70034 || ret.status == 7003 || ret.status == 7002) {
                            swal({
                                title: ret.statusText,
                                text: "即将跳转登录页面.",
                                timer: 2000,
                                showConfirmButton: false
                            });
                            setTimeout(function () {
                                window.location.href = "/index.html";
                            }, 2000);
                        }
                    }
                }
                outputFirst();
            });
        }
        else {
            outputFirst();
        }
    }

    //输出第一级菜单
    function outputFirst() {
        firstList = menuListProvider.getChildMenu("0");
        var appendHtml = "";
        $.each(firstList, function (o, item) {
            var tempHtml = menusliTemplate;
            tempHtml = tempHtml.replace("{menuName}", item.ItemNavLabel);
            tempHtml = tempHtml.replace("{linkUrl}", item.ItemUrl);
            tempHtml = tempHtml.replace("{icons}", item.ItemIcons);

            var second = outputChild(item.ItemCode, 1);
            if (hotUtil.isNullOrEmpty(second)) {
                tempHtml = tempHtml.replace("{menuItemClass}", "J_menuItem");
                tempHtml = tempHtml.replace("{arrow}", "");
            }
            else {
                tempHtml = tempHtml.replace("{menuItemClass}", "");
                tempHtml = tempHtml.replace("{arrow}", "arrow");
            }
            tempHtml = tempHtml.replace("{childMenus}", outputChild(item.ItemCode, 1));
            appendHtml += tempHtml;
        });
        $("#side-menu").append(appendHtml);
        $("#side-menu").metisMenu();
        refreshmenu();//刷新菜单事件，只有在动态加载菜单时，才需执行此函数，否则，加载的菜单，点击之后，无法再右侧打开
    }
    //递归输出子菜单，最多两级
    function outputChild(parentid, level) {
        if (level > 2)
            return "";
        var levelClass = level == 1 ? "nav-second-level" : "nav-third-level";
        level = level + 1;
        var childHtml = "";
        var childList = menuListProvider.getChildMenu(parentid);
        if (childList == null || childList.length == 0)
            return childHtml;
        $.each(childList, function (o, item) {
            var tempHtml = menusulTemplate;
            tempHtml = tempHtml.replace("{li}", menusliTemplate);
            tempHtml = tempHtml.replace("{levelClass}", levelClass);
            tempHtml = tempHtml.replace("{icons}", item.ItemIcons);
            tempHtml = tempHtml.replace("{menuName}", item.ItemNavLabel);
            tempHtml = tempHtml.replace("{linkUrl}", item.ItemUrl);

            var child = outputChild(item.ItemCode, level);
            if (hotUtil.isNullOrEmpty(child)) {
                tempHtml = tempHtml.replace("{menuItemClass}", "J_menuItem");
                tempHtml = tempHtml.replace("{arrow}", "");
            }
            else {
                tempHtml = tempHtml.replace("{menuItemClass}", "");
                tempHtml = tempHtml.replace("{arrow}", "arrow");
            }

            tempHtml = tempHtml.replace("{childMenus}", outputChild(item.ItemCode, level));
            childHtml += tempHtml;
        });
        return childHtml;
    }


    //defaultMenus();
    LoadMenu();


    var def = hotUtil.GetCookie("SHOPID");
    setInterval(function () {
        if (!hotUtil.isNullOrEmpty(hotUtil.GetCookie("SHOPID"))) {
            if (hotUtil.GetCookie("SHOPID") != def)
                window.location.reload();
        }
        else
            window.location.reload();
    }, 5000);
});




function newTab(url, name) {
    var index = false;
    $(".page-tabs-content .J_menuTab").each(function () {
        var dataId = $(this).attr("data-id");
        if (dataId == url) {
            index = true;
            $(this).click();
            return false;
        }
    })
    if (!index) {
        var s = '<a href="javascript:;" class="active J_menuTab" data-id="' + url + '">' + name + ' <i class="fa fa-times-circle"></i></a>';
        $(".J_menuTab").removeClass("active");
        var r = '<iframe class="J_iframe" width="100%" height="100%" src="' + url + '" frameborder="0" data-id="' + url + '" seamless></iframe>';
        $(".J_mainContent").find("iframe.J_iframe").hide().parents(".J_mainContent").append(r);
        var o = layer.load();
        $(".J_mainContent iframe:visible").load(function () {
            layer.close(o)
        }),
        $(".J_menuTabs .page-tabs-content").append(s);
    }
}



function Logout() {
    swal({
        title: "您确定要注销系统",
        text: "",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        closeOnConfirm: false,
    }, function () {
        hotUtil.ajaxCall("handler/logout.ashx", null, function (ret, err) {
            if (ret != null) {
                if (ret.status == 200) {
                    window.location.href = "/index.html";
                }
                else {
                    swal(ret.statusText, "", "warning");
                }
            }
        });
    });
}