//var btnHour = document.getElementsByClassName("hour").item(0);
//var btnDocument = document.getElementsByClassName("document").item(0);
//var toHourAction = "Home\\DisplayHour";
//var toDocumentAction = "Home\\DisplayDocument";

//btnHour.addEventListener("click", function () {
//    let url = toHourAction;
//    DisplayResult(url);
//    BtnActiive("hour");
//});
//btnDocument.addEventListener("click", function () {
//    let url = toDocumentAction;
//    DisplayResult(url);
//    BtnActiive("document");
//});


$('input.datepicker').datepicker({
    format: 'dd/mm/yyyy',
   // format: 'yyyy/mm/dd',
    todayBtn: 'linked',
    todayHighlight: true,
    autoclose: true,
    orientation: "auto"

});

$('input.Monthpicker').datepicker({
    //format: 'yyyy/mm',
    format: 'mm/yyyy',
    todayBtn: 'linked',
    todayHighlight: true,
    autoclose: true,
    orientation: "auto"

});

$('.timepicker').timepicker({
    timeFormat: 'HH:mm', // ใช้รูปแบบ 24 ชั่วโมง
    //interval: 30,        // ให้เลือกเวลาได้ทีละ 30 นาที
    minTime: '00:00',    // เวลาต่ำสุด
    maxTime: '23:30',    // เวลาสูงสุด
    dynamic: false,
    dropdown: true,
    scrollbar: true
   // minuteStep: 30,
});

$('.timepicker').change(function () {
    //$('.timepicker').setTime('09:15')
    console.log("OK1111111");
})

$('input.MMYYpicker').datepicker({
    //format: 'yyyy/mm',
    format: 'mm/yy',
    todayBtn: 'linked',
    todayHighlight: true,
    autoclose: true,
    orientation: "auto"

});



//$('.timepicker').timepicker({
//    showMeridian: false,  // ใช้ระบบ 24 ชั่วโมง
//    minuteStep: 30,       // กำหนดให้เลือกได้ทีละ 30 นาที
//    defaultTime: false     // ไม่กำหนดค่าเริ่มต้น
//}).on('changeTime.timepicker', function (e) {
//    // ปรับเวลาให้เป็นค่าที่ลงตัวกับ 30 นาที
//    let minutes = e.time.minutes;
//    let roundedMinutes = Math.round(minutes / 30) * 30;

//    if (minutes !== roundedMinutes) {
//        let newTime = e.time.hours + ':' + (roundedMinutes === 60 ? '00' : roundedMinutes);
//        $(this).timepicker('setTime', newTime);
//    }
//});