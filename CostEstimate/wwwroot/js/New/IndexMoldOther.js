
var today = new Date();
var formattedDate = today.getDate().toString().padStart(2, '0') + '/' +
    (today.getMonth() + 1).toString().padStart(2, '0') + '/' +
    today.getFullYear();

console.log(today); // ผลลัพธ์: 2025/02/19
//console.log("indexMold.jsss");
//if (document.getElementById("i_NewMold_SDate").value == "") {
//    document.getElementById("i_NewMold_SDate").value = formattedDate;//formattedDate;
//}

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

//function CheckStatus(status) {
//    $(document).ready(function () {
//        var checkStatusDis = status;
//        var step = $('#step').val();
//        console.log("step ==> " + step);
//        if (checkStatusDis == 'Other') {
//            $('#i_New_RemarkOther').removeAttr('disabled', 'disabled');
//        }
//        else {
//            $('#i_New_RemarkOther').attr("disabled", "disabled");
//            document.getElementById("i_New_RemarkOther").value = "";


//        }

//    });
//}
//$("#btnfile").click(function () {
//    $("#myModal2").modal("show");
//});

////New Mold Modify
//$("#btnfileMold").click(function () {
//    $("#myModalNewMold").modal("show");
//});

//$("#i_NewMold_Type").on("click", function () {
//    const selectedValue = $(this).val();
//    console.log("Selected value:", selectedValue);

//    // ตัวอย่างเช็กว่าเลือก "กรุณาเลือก" หรือยัง
//    if (selectedValue && selectedValue.toLowerCase().includes("claim")) {

//        //- แต่ถ้าเลือกงาน CLAIM MAKER
//        //ใช้คูณ 1.00
//        console.log("✅ เลือก claim แล้วเด้อ!");
//        $("#i_NewMoldRateUnit").val("1.0");
//    }
//    else {
//        const input = document.querySelector('#tbDetailMoldProcessDetail input.WT_Man.W\\.');
//        const value = input ? input.value : null;
//        console.log("#tbDetailMoldProcessDetail tr.GM\\. input#WT_Man\\ W\\. : == " + value);

//        if (value > 0) {
//            $("#i_NewMoldRateUnit").val("1.15");
//        }
//        else {
//            $("#i_NewMoldRateUnit").val("1.10");
//        }

//        //สำหรับงานที่ถูกเลือกเป็น MODIFICATION
//        //    - ถ้ามีการใส่ชั่วโมงงานที่ช่อง W จะใช้ตัวคูณที่ 1.15
//        //    - ถ้าไม่มีชั่วโมงงานที่ช่อง W จะใช้ตัวคูณ 1.10

//    }
//  FuntionTotalProCost();
//});
// });

//New Mold Other
$("#btnfileMoldOther").click(function () {
    $("#myModalNewMoldOther").modal("show");
});



$("#btnfileMoldOtherWK").click(function () {
    $("#myModalNewMoldOtherWK").modal("show");
});


$("#btnfileMoldOtherMT").click(function () {
    $("#myModalNewMoldOtherMT").modal("show");
});

$("#btnfileMoldOtherTGR").click(function () {
    console.log("btnfileMoldOtherTGR");
    $("#myModalNewMoldOtherTGR").modal("show");
    //myModalNewMoldOtherTGR
});

$("#btnfileMoldOtherSM").click(function () {
    $("#myModalNewMoldOtherSM").modal("show");
});

$("#btnUploadFileWK").click(function () {
    $('#filesImport').val('');
    $("#myModalNewMoldOtherWKUpload").modal("show");
});
