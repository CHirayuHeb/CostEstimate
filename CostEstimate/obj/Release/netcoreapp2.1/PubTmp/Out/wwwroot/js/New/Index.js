/// <reference path="../../lib/jqueryui/jquery-ui.min.js" />
//var urlHost = window.location.protocol + "\\";
//var actionOTType = "New\\OTType";
//var scriptFile = ["", "", "New\\EventOTMyData.js", "New\\EventOTForm.js", "New\\EventOTAddWorker.js", "New\\EventOTEmailForm.js"];

//var myToday = document.querySelector("div.new-menu .menu-item button.my-today");
//var myYesterday = document.querySelector("div.new-menu .menu-item button.my-yesterday");
//var allToday = document.querySelector("div.new-menu .menu-item button.all-today");
//var allYesterday = document.querySelector("div.new-menu .menu-item button.all-yesterday");
//var btnAdd = document.querySelector(".title .item-action button.btn.add");

////filter
//var btnMyToday = document.getElementsByClassName("filter").item(0);
//var btnMyYesterday = document.getElementsByClassName("filter").item(1);
//var btnAllToday = document.getElementsByClassName("filter").item(2);
//var btnAllYesterday = document.getElementsByClassName("filter").item(3);

////filter action
//var toMyToday = "New\\DisplayMyToday";
//var toMyYesterday = "New\\DisplaMyYesterday";
//var toAllToday = "New\\DisplayAllToday";
//var toAllYesterday = "New\\DisplayAllYesterday";

//btnMyToday.addEventListener("click", function () {
//    let url = toMyToday;
//    DisplayResult(url);
//    BtnActiive("mytoday");
//});
//btnMyYesterday.addEventListener("click", function () {
//    let url = toMyYesterday;
//    DisplayResult(url);
//    BtnActiive("myyesterday");
//});
//btnAllToday.addEventListener("click", function () {
//    let url = toAllToday;
//    DisplayResult(url);
//    BtnActiive("alltoday");
//});
//btnAllYesterday.addEventListener("click", function () {
//    let url = toAllYesterday;
//    DisplayResult(url);
//    BtnActiive("allyesterday");
//});

//btnAdd.addEventListener("click", function () {
//    GoToOTChoice(actionOTType, ModalContentBase);
//});

//async function BringScriptToPage(nextStep) {
//    //if (document.getElementById(scriptFile[nextStep].split("\\")[1]) == null) {
//    //    loadjscssfile("js\\" + scriptFile[nextStep], scriptFile[nextStep].split("\\")[1]);
//    //} else {
//    //    replacejscssfile("js\\" + scriptFile[nextStep], "js\\" + scriptFile[nextStep], "js", scriptFile[nextStep].split("\\")[1]);
//    //}
//    LoadScript("js\\" + scriptFile[nextStep], scriptFile[nextStep].split("\\")[1]);
//}



var today = new Date().toISOString().slice(0, 10).replace(/-/g, '/');
console.log(today); // ผลลัพธ์: 2025/02/19

if (document.getElementById("i_New_SDate").value == "") {
    document.getElementById("i_New_SDate").value = today;
}

(function () {
    if (window.localStorage) {
        if (!localStorage.getItem('firstLoad')) {
            localStorage['firstLoad'] = true;
            window.location.reload();
        }
        else {
            localStorage.removeItem('firstLoad');
        }  
    }
})();

function CheckStatus(status) {
    $(document).ready(function () {
        var checkStatusDis = status;
        var step = $('#step').val();
        console.log("step ==> " + step);
        if (checkStatusDis == 'Other') {
            $('#i_New_RemarkOther').removeAttr('disabled', 'disabled');
        }
        else {
            $('#i_New_RemarkOther').attr("disabled", "disabled");
            document.getElementById("i_New_RemarkOther").value = "";
           

        }

    });
}
$("#btnfile").click(function () {
    $("#myModal2").modal("show");
});
