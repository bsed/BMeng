/*
    版权所有:杭州火图科技有限公司
    地址:浙江省杭州市滨江区西兴街道阡陌路智慧E谷B幢4楼在地图中查看
    (c) Copyright Hangzhou Hot Technology Co., Ltd.
    Floor 4,Block B,Wisdom E Valley,Qianmo Road,Binjiang District
    2013-2016. All rights reserved.
**/

function getDemoData() {
    var menuList = [];

    //menuList.push({
    //    ItemCode: "01",
    //    ItemNavLabel: "首页",
    //    ItemUrl: "home.html",
    //    ItemParentCode: "0",
    //    ItemShow: 1,
    //    ItemIcons: "fa-home"
    //});
    menuList.push({
        ItemCode: "0201",
        ItemNavLabel: "总店列表",
        ItemUrl: "admin/shoplist.html",
        ItemParentCode: "0",
        ItemShow: 1,
        ItemIcons: "fa-home"
    });

    menuList.push({
        ItemCode: "0202",
        ItemNavLabel: "盟主列表",
        ItemUrl: "admin/userlist.html",
        ItemParentCode: "0",
        ItemShow: 1,
        ItemIcons: "fa-home"
    });
    menuList.push({
        ItemCode: "0203",
        ItemNavLabel: "盟友列表",
        ItemUrl: "admin/userlist.html?type=1",
        ItemParentCode: "0",
        ItemShow: 1,
        ItemIcons: "fa-home"
    });

    menuList.push({
        ItemCode: "0204",
        ItemNavLabel: "等级设置",
        ItemUrl: "admin/userlevel.html",
        ItemParentCode: "0",
        ItemShow: 1,
        ItemIcons: "fa-home"
    });

    menuList.push({
        ItemCode: "0205",
        ItemNavLabel: "资讯列表",
        ItemUrl: "admin/articlelist.html",
        ItemParentCode: "0",
        ItemShow: 1,
        ItemIcons: "fa-home"
    });



    return menuList;
}