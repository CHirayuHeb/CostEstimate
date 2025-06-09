
var today = new Date();
var formattedDate = today.getDate().toString().padStart(2, '0') + '/' +
    (today.getMonth() + 1).toString().padStart(2, '0') + '/' +
    today.getFullYear();

console.log(today); // ผลลัพธ์: 2025/02/19

if (document.getElementById("i_New_SDate").value == "") {
    document.getElementById("i_New_SDate").value = formattedDate;
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

