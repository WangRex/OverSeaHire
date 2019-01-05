var Constants = (function (con) {
    con.globalFlashUrl = "http://dltongjian.net:8005/";
    con.globalHomeUrl = "http://dlanqi.com:1001/";
    con.Employee = {
        StatisticEmployee: "/api/Employee/StatisticEmployee",
    };
    con.Attach = {
        GetAttachList: "api/Attach/GetAttachList",
        GetAttachListGrid: "api/Attach/GetAttachListGrid",
        DeleteAttach: "api/Attach/DeleteAttach",
        ImageUpload: "api/ImageUpload/PostFile",
    };
    con.Requirement = {
        GetCountries: "api/Requirement/GetCountries",
    };
    return con;
}(Constants || {}));