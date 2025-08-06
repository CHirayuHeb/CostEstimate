function sumGroupWK_Man() {
    console.log("sumGroupWK_Man");
    var groupSumsWT_Man = {};  // Object to store sums for each group
    // Iterate through each group
    $("tr").each(function () {
        var groupClass = $(this).attr("class");  // Get the class of the current row (group)
        if (groupClass) {
            var rowWT_Man = 0;
            var rowWT_Auto = 0;
            // Sum the values of .WT_Man and .WT_Auto in the current row
            $(this).find(".WK_Man").each(function () {
                rowWT_Man += parseFloat($(this).val()) || 0;  // Add value, default to 0 if invalid
            });
            if (!groupSumsWT_Man[groupClass]) {
                groupSumsWT_Man[groupClass] = 0;
            }
            groupSumsWT_Man[groupClass] += rowWT_Man;
        }
    });

    $.each(groupSumsWT_Man, function (group, total) {
        //dsWT_Auto HW.
        let str = group;
        //console.log('group' + group);
        // Find the last index of the period
        let lastIndex = str.lastIndexOf('.');
        if (lastIndex !== -1) {
            // Replace the last period with "\\."
            str = str.slice(0, lastIndex) + "\\." + str.slice(lastIndex + 1);
        }
        //console.log('WT_Man <p>' + str + ' Total: ' + total + '</p>');
        $(".dsWK_Man" + "." + str).each(function () {
            // parseFloat(total).toFixed(2);
            $(this).val(parseFloat(total).toFixed(2));
            // console.log("Value of textbox with classes 'dsWT_Man DESIGN': " + $(this).val());
        });
    });

}
function sumGroupWK_Auto() {
    var groupSumsWT_Auto = {};
    // Iterate through each group
    $("tr").each(function () {
        var groupClass = $(this).attr("class");  // Get the class of the current row (group)
        if (groupClass) {
            var rowWT_Man = 0;
            // Sum the values of .WT_Man and .WT_Auto in the current row
            $(this).find(".WK_Auto").each(function () {
                rowWT_Man += parseFloat($(this).val()) || 0;  // Add value, default to 0 if invalid
            });
            if (!groupSumsWT_Auto[groupClass]) {
                groupSumsWT_Auto[groupClass] = 0;
            }
            groupSumsWT_Auto[groupClass] += rowWT_Man;
        }
    });
    $.each(groupSumsWT_Auto, function (group, total) {
        //dsWT_Auto HW.
        let str = group;
        let lastIndex = str.lastIndexOf('.');
        if (lastIndex !== -1) {
            str = str.slice(0, lastIndex) + "\\." + str.slice(lastIndex + 1);
        }
        $(".dsWK_Auto" + "." + str).each(function () {
            $(this).val(parseFloat(total).toFixed(2));
        });
    });


}
function sumGroupKJ_Man() {
    //console.log("sumGroupKJ_Man");
    var groupSumsWT_Man = {};  // Object to store sums for each group
    // Iterate through each group
    $("tr").each(function () {
        var groupClass = $(this).attr("class");  // Get the class of the current row (group)
        if (groupClass) {
            var rowWT_Man = 0;
            var rowWT_Auto = 0;
            // Sum the values of .WT_Man and .WT_Auto in the current row
            $(this).find(".KIJUNWT_Man").each(function () {
                rowWT_Man += parseFloat($(this).val()) || 0;  // Add value, default to 0 if invalid
            });
            if (!groupSumsWT_Man[groupClass]) {
                groupSumsWT_Man[groupClass] = 0;
            }
            groupSumsWT_Man[groupClass] += rowWT_Man;
        }
    });

    $.each(groupSumsWT_Man, function (group, total) {
        //dsWT_Auto HW.
        let str = group;
        //console.log('group' + group);
        // Find the last index of the period
        let lastIndex = str.lastIndexOf('.');
        if (lastIndex !== -1) {
            // Replace the last period with "\\."
            str = str.slice(0, lastIndex) + "\\." + str.slice(lastIndex + 1);
        }
        //console.log('WT_Man <p>' + str + ' Total: ' + total + '</p>');
        $(".dsKIJUNWT_Man" + "." + str).each(function () {
            // parseFloat(total).toFixed(2);
            $(this).val(parseFloat(total).toFixed(2));
            if (parseFloat(total) < 0) {
                $(this).css("color", "red");
            } else {
                $(this).css("color", "black"); // หรือสีปกติที่ต้องการ
            }
        });
    });

}
function sumGroupKJ_Auto() {
    //console.log("sumGroupKJ_Auto");
    var groupSumsWT_Man = {};  // Object to store sums for each group
    // Iterate through each group
    $("tr").each(function () {
        var groupClass = $(this).attr("class");  // Get the class of the current row (group)
        if (groupClass) {
            var rowWT_Man = 0;
            var rowWT_Auto = 0;
            // Sum the values of .WT_Man and .WT_Auto in the current row
            $(this).find(".KIJUNWT_Auto").each(function () {
                rowWT_Man += parseFloat($(this).val()) || 0;  // Add value, default to 0 if invalid
            });
            if (!groupSumsWT_Man[groupClass]) {
                groupSumsWT_Man[groupClass] = 0;
            }
            groupSumsWT_Man[groupClass] += rowWT_Man;
        }
    });

    $.each(groupSumsWT_Man, function (group, total) {
        //dsWT_Auto HW.
        let str = group;
        //console.log('group' + group);
        // Find the last index of the period
        let lastIndex = str.lastIndexOf('.');
        if (lastIndex !== -1) {
            // Replace the last period with "\\."
            str = str.slice(0, lastIndex) + "\\." + str.slice(lastIndex + 1);
        }
        //console.log('WT_Man <p>' + str + ' Total: ' + total + '</p>');
        $(".dsKIJUNWT_Auto" + "." + str).each(function () {
            // parseFloat(total).toFixed(2);
            $(this).val(parseFloat(total).toFixed(2));
            if (parseFloat(total) < 0) {
                $(this).css("color", "red");
            } else {
                $(this).css("color", "black"); // หรือสีปกติที่ต้องการ
            }

            // console.log("Value of textbox with classes 'dsWT_Man DESIGN': " + $(this).val());
        });
    });

}
function sumTotalProcess() {

    let TotalsumMAN = 0;
    let TotalsumTOTAL = 0;
    $('input.dsWK_Auto').each(function () {
        const className = ($(this).attr('class') || '').toLowerCase();
        let value = 0;
        if (className.includes('nc.')) {
            // ทำอะไรกับ element ที่มี "NC."
            let value = parseFloat($(this).val()) || 0; // อ่านค่า + เช็คถ้าไม่มีให้เป็น 0
            TotalsumMAN += value
        }
    });
    $('input.dsWK_Man').each(function () {
        const className = ($(this).attr('class') || '').toLowerCase();
        let value = 0;
        if (!className.includes('nc.')) {
            // ทำอะไรกับ element ที่มี "NC."
            value = parseFloat($(this).val()) || 0; // อ่านค่า + เช็คถ้าไม่มีให้เป็น 0
            TotalsumTOTAL += value;


        }
    });



    let TotalsumKJMAN = 0;
    let TotalsumKJTOTAL = 0;
    $('input.dsKIJUNWT_Auto').each(function () {
        const className = ($(this).attr('class') || '').toLowerCase();
        let value = 0;
        if (className.includes('nc.')) {
            // ทำอะไรกับ element ที่มี "NC."
            let value = parseFloat($(this).val()) || 0; // อ่านค่า + เช็คถ้าไม่มีให้เป็น 0
            TotalsumKJTOTAL += value
        }
    });
    $('input.dsKIJUNWT_Man').each(function () {
        const className = ($(this).attr('class') || '').toLowerCase();
        let value = 0;
        if (!className.includes('nc.')) {
            // ทำอะไรกับ element ที่มี "NC."
            value = parseFloat($(this).val()) || 0; // อ่านค่า + เช็คถ้าไม่มีให้เป็น 0
            TotalsumKJMAN += value;


        }
    });


    let TotalsumKJM = 0;
    let TotalsumKJA = 0;

    document.querySelectorAll('.dsKIJUNWT_Man').forEach(function (input) {
        let value = parseFloat(input.value) || 0;
        TotalsumKJM += value;
    });
 

    document.querySelectorAll('.dsKIJUNWT_Auto').forEach(function (input) {
        let value = parseFloat(input.value) || 0;
        TotalsumKJA += value;
    });
 








    //console.log("TotalsumTOTAL" + TotalsumTOTAL);

    console.log("TotalsumKJA" + TotalsumKJA);

    console.log("TotalsumKJM" + TotalsumKJM);

    $("#i_TotalPocessMan").val((TotalsumTOTAL + TotalsumMAN).toFixed(2)); // Format ทศนิยม 2 ตำแหน่ง

    $("#i_TotalPocessKJProcess").val((TotalsumKJTOTAL + TotalsumKJMAN).toFixed(2)); // Format ทศนิยม 2 ตำแหน่ง
    $("#i_TotalPocessKJMan").val((TotalsumKJM).toFixed(2)); // Format ทศนิยม 2 ตำแหน่ง
    $("#i_TotalPocessKJAuto").val((TotalsumKJA).toFixed(2)); // Format ทศนิยม 2 ตำแหน่ง


    setInputColor("#i_TotalPocessKJProcess", TotalsumKJTOTAL + TotalsumKJMAN);
    setInputColor("#i_TotalPocessKJMan", TotalsumKJM);
    setInputColor("#i_TotalPocessKJAuto", TotalsumKJA);


}

function setInputColor(selector, value) {
    if (value < 0) {
        $(selector).css("color", "red");
    } else {
        $(selector).css("color", "black");
    }
}

function sumProcess() {
    // console.log("sumProcess");
    //document.getElementById("i_HissubCostsubMaker").value = "11111";


    let TotalsumCE = 0;
    let TotalNGsumCE = 0;
    $(".SumbyCE").each(function () {
        let value = parseFloat($(this).val()) || 0; // อ่านค่า + เช็คถ้าไม่มีให้เป็น 0
        //sum += value; // บวกค่า
        TotalsumCE += value;
    });
    TotalNGsumCE = TotalsumCE * -1;

    $("#i_HissubCostsubMaker").val(TotalsumCE.toFixed(2));
    $("#i_HissubshCSmMat").val(TotalsumCE.toFixed(2));
    $("#i_HissubshCSmWorkingTime").val(TotalNGsumCE.toFixed(2));



    let vCKMat = parseFloat($("#i_HissubshCKjMat").val()) || 0;
    let vSmMat = parseFloat($("#i_HissubshCSmMat").val()) || 0;

    //$("#i_HissubshCMcMat").val((vCKMat + vSmMat).toFixed(2));
    $("#i_HissubshCMcMat").val((vCKMat + vSmMat));



}
function calcRow(row) {

    let WK_Man = parseFloat(row.find(".WK_Man").val()) || 0;
    let WK_Auto = parseFloat(row.find(".WK_Auto").val()) || 0;

    let KIJUNWT_Man = parseFloat(row.find(".KIJUNWT_Man").val()) || 0;
    let KIJUNWT_Auto = parseFloat(row.find(".KIJUNWT_Auto").val()) || 0;

    let WT_Man = parseFloat(row.find(".WTWT_Man").val()) || 0;
    let WT_Auto = parseFloat(row.find(".WTWT_Auto").val()) || 0;


    let sumKJMan = WK_Man - WT_Man;
    let sumKJAuto = WK_Auto - WT_Auto;


    row.find(".KIJUNWT_Man").val(sumKJMan.toFixed(2)); // Set Total
    row.find(".KIJUNWT_Auto").val(sumKJAuto.toFixed(2)); // Set Total



    // ตรวจสอบและเปลี่ยนสี
    if (sumKJMan < 0) {
        row.find(".KIJUNWT_Man").css("color", "red");
    } else {
        row.find(".KIJUNWT_Man").css("color", "black");
    }

    if (sumKJAuto < 0) {
        row.find(".KIJUNWT_Auto").css("color", "red");
    } else {
        row.find(".KIJUNWT_Auto").css("color", "black");
    }

}
function calcAll() {
    let sum = 0;
    //$("#tbDetailSubMakerRequest tbody tr").each(function () {
    $("#tbDetailSubMakerHissumRequest1 tr").each(function () {
        calcRow($(this)); // เรียก Function คำนวณแถว

    });

    // $("#i_New_ProcessCost").val(sum.toFixed(2)); // Set ค่า Sum Total
}
$(document).on("keyup change", ".WK_Man, .WK_Auto", function () {
    //console.log("1234567890");

    sumGroupWK_Man();
    sumGroupWK_Auto();

    sumGroupKJ_Man();
    sumGroupKJ_Auto();

    calcAll();
    sumTotalProcess();

});
sumGroupWK_Man();
sumProcess();

document.querySelectorAll(".form-control").forEach(input => {
    input.addEventListener("input", function () {
        this.style.color = parseFloat(this.value) < 0 ? "red" : "black";
    });
});





$(document).on("input change", "#i_HissubshCKjMat", function () {
    const value1 = parseFloat($(this).val()) || 0;
    const value2 = parseFloat($("#i_HissubshCSmMat").val()) || 0;
    console.log("value1 : " + value1);
    console.log("value2 : " + value2);
    $("#i_HissubshCMcMat").val((value1 + value2).toFixed(2));


    let v1 = parseFloat($("#i_HissubshCKjWorkingTime").val()) || 0;
    let v2 = parseFloat($("#i_HissubshCKjTotal").val()) || 0;

    //sum
    const result = (v2 - v1 - value1).toFixed(2);
    $("#i_HissubshCKjCofficient").val((v2 - v1 - value1).toFixed(2));
    $("#i_HissubshCMcCofficient").val((v2 - v1 - value1).toFixed(2));

    // เช็กค่าและเปลี่ยนสี
    setInputColor("#i_HissubshCKjCofficient", result);
    setInputColor("#i_HissubshCMcCofficient", result);


});




$(document).on("input change", "#i_HissubshCKjWorkingTime", function () {
    const vSmWk = parseFloat($(this).val()) || 0;
    let vCKj = parseFloat($("#i_HissubshCSmWorkingTime").val()) || 0;

    const resultWK = (vCKj + vSmWk).toFixed(2);
    $("#i_HissubshCMcWorkingTime").val((vCKj + vSmWk).toFixed(2));
    setInputColor("#i_HissubshCMcWorkingTime", resultWK);


    let v1 = parseFloat($("#i_HissubshCKjMat").val()) || 0;
    let v2 = parseFloat($("#i_HissubshCKjTotal").val()) || 0;

    //sum
    const result = (v2 - vSmWk - v1).toFixed(2);

    $("#i_HissubshCKjCofficient").val((v2 - vSmWk - v1).toFixed(2));
    $("#i_HissubshCMcCofficient").val((v2 - vSmWk - v1).toFixed(2));

    setInputColor("#i_HissubshCKjCofficient", result);
    setInputColor("#i_HissubshCMcCofficient", result);

});

$(document).on("input change", "#i_HissubshCKjTotal", function () {
    const value1 = parseFloat($(this).val()) || 0;
    $("#i_HissubshCMcTotal").val((value1).toFixed(2));


    let v1 = parseFloat($("#i_HissubshCKjWorkingTime").val()) || 0;
    let v2 = parseFloat($("#i_HissubshCKjMat").val()) || 0;
    //sum

    const result = (value1 - v1 - v2).toFixed(2);
    $("#i_HissubshCKjCofficient").val((value1 - v1 - v2).toFixed(2));
    $("#i_HissubshCMcCofficient").val((value1 - v1 - v2).toFixed(2));

    setInputColor("#i_HissubshCKjCofficient", result);
    setInputColor("#i_HissubshCMcCofficient", result);

});


//



var today = new Date();
var formattedDate = today.getDate().toString().padStart(2, '0') + '/' +
    (today.getMonth() + 1).toString().padStart(2, '0') + '/' +
    today.getFullYear();

console.log(today); // ผลลัพธ์: 2025/02/19

if (document.getElementById("i_HissubIssue").value == "") {
    document.getElementById("i_HissubIssue").value = formattedDate;
}






