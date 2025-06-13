const urlDefault = location.href.split("CostEstimate")[0] + "\\CostEstimate";
const loader = document.getElementById("loading");
const loadingProcesser = document.getElementById("loadingProcess");
const ForwardModalID = "OTContent_step";
const ModalContentBase = "modal-new-content";
const ModalFooterBase = "FooterContent";
const NewOTRoadStyle = "color: black;font-family: 'LeelawaD Bold';";
const FooterID = "footer";
const apiSTPoint = "http://10.200.128.20/Mvcpublish/CostEstimate/";



//button tag
var home = document.querySelector("button.home");
var search = document.querySelector("button.search-box");
var create = document.querySelector("button.create");
var requestform = document.querySelector("button.RequestForm");
var myRequest = document.querySelector("button.my-request");
var approval = document.querySelector("button.approval");
var administrator = document.querySelector("button.administrator");
var mywork = document.querySelector("button.work");
var signOut = document.querySelector("button.signOut");
var addCard = document.querySelector("button.add_card");
var addProcess = document.querySelector("button.process_add");
var addModel = document.querySelector("button.model_add");
var addMaster = document.querySelector("button.Master_add");


//Mold Modify
var searchMold = document.querySelector("button.searchMold");
var createMold = document.querySelector("button.createMold");
var myRequestMold = document.querySelector("button.my-requestMold");
var approvalMold = document.querySelector("button.approvalMold");
var addMasterMold = document.querySelector("button.Master_addMold");



//a tag
var ahome = document.querySelector("div.app a.home");
var asearch = document.querySelector("div.app a.search-box");
var acreate = document.querySelector("div.app a.createMold");
var amyRequest = document.querySelector("div.app a.my-request");
var aapproval = document.querySelector("div.app a.approval");
var aadministrator = document.querySelector("div.app a.administrator");

//New sub marker
if (addMaster != null)
    addMaster.addEventListener("click", function () {
        GoSideMenu("MasterCost");
    });
if (addCard != null)
    addCard.addEventListener("click", function () {
        GoSideMenu("AddCost");
    });
if (addModel != null)
    addProcess.addEventListener("click", function () {
        GoSideMenu("addModel");
    });

if (addProcess != null)
    addProcess.addEventListener("click", function () {
        GoSideMenu("addProcess");
    });
if (home != null)
    home.addEventListener("click", function () {
        GoSideMenu("Home");
    });
if (search != null)
    search.addEventListener("click", function () {
        GoSideMenu("Search");
    });
if (create != null)
    create.addEventListener("click", function () {
        GoSideMenu("New");
    });
if (myRequest != null)
    myRequest.addEventListener("click", function () {
        GoSideMenu("MyRequest");
    });
if (approval != null)
    approval.addEventListener("click", function () {
        GoSideMenu("Approval");
    });
if (administrator != null)
    administrator.addEventListener("click", function () {
        GoSideMenu("Administrator");
    });
if (signOut != null)
    signOut.addEventListener("click", function () {
        window.location.href = urlDefault + "\\Login\\SignOut\\";
    });

if (ahome != null)
    ahome.addEventListener("click", function () {
        GoSideMenu("Home");
        $("#AppLuncher").modal("hide");
    });
if (asearch != null)
    asearch.addEventListener("click", function () {
        GoSideMenu("Search");
        $("#AppLuncher").modal("hide");
    });
if (acreate != null)
    acreate.addEventListener("click", function () {
        GoSideMenu("New");
        $("#AppLuncher").modal("hide");
    });
if (amyRequest != null)
    amyRequest.addEventListener("click", function () {
        GoSideMenu("MyRequest");
        $("#AppLuncher").modal("hide");
    });
if (aapproval != null)
    aapproval.addEventListener("click", function () {
        GoSideMenu("Approval");
        $("#AppLuncher").modal("hide");
    });
if (aadministrator != null)
    aadministrator.addEventListener("click", function () {
        GoSideMenu("Administrator");
        $("#AppLuncher").modal("hide");
    });


//Mold modify
if (searchMold != null)
    searchMold.addEventListener("click", function () {
        GoSideMenu("SearchMold");
    });
if (createMold != null)
    createMold.addEventListener("click", function () {
        GoSideMenu("NewMoldModify");
    });
if (myRequestMold != null)
    myRequestMold.addEventListener("click", function () {
        GoSideMenu("MyRequestMold");
    });
if (approvalMold != null)
    approvalMold.addEventListener("click", function () {
        GoSideMenu("ApprovalMold");
    });
if (addMasterMold != null)
    addMasterMold.addEventListener("click", function () {
        GoSideMenu("MasterMold");
    });





function GoSideMenuNewRequest(vpage) {
    GoSideMenu(vpage);
}

//function GoNewRequest(smLotNo, smOrderNo, smRevision) {
function GoNewRequest(smDocumentNo) {
    //smLotNo
    //smOrderNo
    //smRevision
    let url = "New?smDocumentNo=" + smDocumentNo;
    //let url = "New?smLotNo=" + smLotNo + "&smOrderNo=" + smOrderNo + "&smRevision=" + smRevision;
    GoSideMenu(url);
}
function GoNewMoldRequest(id) {
    //smLotNo
    //smOrderNo
    //smRevision
    let url = "NewMoldModify?id=" + id;
    //let url = "New?smLotNo=" + smLotNo + "&smOrderNo=" + smOrderNo + "&smRevision=" + smRevision;
    GoSideMenu(url);
}
function GoNewRevision(smDocumentNo, smRevision) {
    //smLotNo
    //smOrderNo
    //smRevision
    let url = "New?smDocumentNo=" + smDocumentNo + "&smRevision=" + smRevision;
    //let url = "New?smLotNo=" + smLotNo + "&smOrderNo=" + smOrderNo + "&smRevision=" + smRevision;
    GoSideMenu(url);
}
function GoNewMoldRevision(smDocumentNo, smRevision) {
    //smLotNo
    //smOrderNo
    //smRevision
    let url = "NewMoldModify?id=" + smDocumentNo + "&mfRevision=" + smRevision;
    //let url = "New?smLotNo=" + smLotNo + "&smOrderNo=" + smOrderNo + "&smRevision=" + smRevision;
    GoSideMenu(url);
}
function GoSideMenu(controller) {
    displayLoading();
    //console.time();
    var url = controller;
    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
    }).then(function (response) {
        // When the page is loaded convert it to text
        return response.text()
    }).then(function (html) {
        // Initialize the DOM parser
        var parser = new DOMParser();

        // Parse the text
        var doc = parser.parseFromString(html, "text/html");

        var ToContent = doc.getElementById("DisplayContent").innerHTML;

        //get div Display
        var displayContent = document.getElementById("DisplayContent");

        //pointer side menu
        PositionY(controller);

        //text view controller to html
        displayContent.innerHTML = ToContent;

        //change url
        window.history.replaceState(controller, controller, url);
        hideLoading();
        //console.timeEnd();
    })
        .catch(function (err) {
            hideLoading();
            alert('GoSideMenu : Failed to fetch page: ', err);
            window.location.href = urlDefault + "\\Login\\Index\\";
            //var url = '@Url.Action("Index", "Login")';
            //window.location.href = url;

        });

}


function PositionY(menu) {
    if (menu.search("New") > -1 && menu.search("NewMoldModify") == -1) {
        menu = "New";
    } else if (menu.search("NewMoldModify") > -1) {
        menu = "NewMoldModify";
    }
    let PY = 0;
    let opacity;
    switch (menu) {
        case "Home":
            //LoadScript(window.location.protocol + "\\" + "js\\" + "Home\\Index.js", "Home");
            //LoadScript("js/Home/Hour.js", "EventHomeHour");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            LoadScript("js/Home/Index.js", "Home");
            PY = "0px";
            opacity = "opacity-dot-7";
            break;
        case "Search":
            //LoadScript("js/New/Index.js", "NewItem");
            //LoadScript("js/New/EventMore.js", "EventNewMore");
            LoadScript("js/Home/Index.js", "Home");
            PY = "0px";
            //PY = "52px";
            opacity = "opacity-dot-3";
            break;
        case "SearchMold":
            //LoadScript("js/New/Index.js", "NewItem");
            //LoadScript("js/New/EventMore.js", "EventNewMore");
            LoadScript("js/Home/Index.js", "Home");

            PY = "0px";
            //PY = "52px";
            opacity = "opacity-dot-3";
            break;
        case "New":
            //LoadScript("js/New/Index.js", "NewItem");
            //LoadScript("js/New/EventMore.js", "EventNewMore");
            LoadScript("js/Home/Index.js", "Home");
            LoadScript("js/New/Index.js", "New");
            LoadScript("js/Home/Hour.js", "EventHomeHour");
            LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            PY = "52px";
            //PY = "114px";
            opacity = "opacity-dot-3";
            break;
        case "NewMoldModify":
            //console.log("New:NewMoldModify");
            LoadScript("js/Home/Index.js", "Home");
            LoadScript("js/New/IndexMold.js", "New");
            LoadScript("js/Home/Hour.js", "EventHomeHour");
            LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            PY = "52px";
            //PY = "114px";
            opacity = "opacity-dot-3";
            break;
        case "PageNotFound":
            //LoadScript("js/New/Index.js", "NewItem");
            //LoadScript("js/New/EventMore.js", "EventNewMore");
            LoadScript("js/Home/Index.js", "Home");
            LoadScript("js/New/Index.js", "New");
            //LoadScript("js/Home/Hour.js", "EventHomeHour");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            PY = "52px";
            //PY = "114px";
            //PY = "114px";
            opacity = "opacity-dot-3";
            break;
        case "MyRequest":
            //LoadScript("js/MyRequest/Index.js", "MyRequest");
            //LoadScript("js/New/EventMore.js", "EventMyRequestMore");
            LoadScript("js/Home/Index.js", "Home");
            PY = "114px";
            //PY = "176px";
            opacity = "opacity-dot-3";
            break;
        case "Approval":
            //LoadScript("js\\Approval\\Index.js", "Approval");
            //LoadScript("js\\New\\EventMore.js", "EventApprovalMore");
            PY = "176px";
            // PY = "238px";
            opacity = "opacity-dot-3";
            break;
        case "Administrator":
            PY = "238px";
            //LoadScript("js\\Admin\\Index.js", "AdminSetting");
            opacity = "opacity-dot-3";
            break;
        case "MasterCost":
            //LoadScript("js/New/Index.js", "NewItem");
            //LoadScript("js/New/EventMore.js", "EventNewMore");
            LoadScript("js/Home/Index.js", "Home");
            //LoadScript("js/New/Index.js", "New");
            //LoadScript("js/Home/Hour.js", "EventHomeHour");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            //LoadScript("js\\" + "Home\\Search\\HourControl.js", "HourControl");
            PY = "265px";
            //PY = "333px";
            opacity = "opacity-dot-3";
            break;
        case "MasterMold":
            LoadScript("js/Home/Index.js", "Home");
            PY = "265px";
            //PY = "333px";
            opacity = "opacity-dot-3";
            break;
        case "AddCost":
            LoadScript("js/Home/Index.js", "Home");
            PY = "265px";
            // PY = "331px";
            opacity = "opacity-dot-3";
            break;
        //addProcess
        case "AddProcess":
            LoadScript("js/Home/Index.js", "Home");
            PY = "265px";
            // PY = "331px";
            opacity = "opacity-dot-3";
            break;
        case "AddModel":
            LoadScript("js/Home/Index.js", "Home");
            PY = "265px";
            // PY = "331px";
            opacity = "opacity-dot-3";
            break;
        //Modify
        case "AddMCost":
            LoadScript("js/Home/Index.js", "Home");
            PY = "265px";
            // PY = "331px";
            opacity = "opacity-dot-3";
            break;

    }
    var Selector = document.getElementById("selector");
    var bg = document.getElementsByClassName("banner").item(0);
    var oldOpacity = Array.from(bg.classList).find(c => c.startsWith('opacity'));
    bg.classList.replace(oldOpacity, opacity);
    Selector.style.transform = "translate(0px, " + PY + ")";
}

function LoadScript(sourceFile, name) {

    var Time = Date.now();
    var oldScript = document.getElementById(name);
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.src = sourceFile + "?t=" + Time;
    script.type = "text/javascript";
    script.id = name;

    if (oldScript != null) {
        oldScript.parentNode.removeChild(oldScript);
    }
    head.appendChild(script);
    return false;
}

function DisplayResult(url) {
    displayLoading();
    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
    }).then(function (response) {
        // When the page is loaded convert it to text
        return response.text()
    }).then(function (html) {
        // Initialize the DOM parser
        var parser = new DOMParser();

        // Parse the text
        var doc = parser.parseFromString(html, "text/html");

        var ToContent = doc.getElementsByClassName("just-group").item(0).outerHTML;

        //get div Display
        var displayContent = document.getElementsByClassName("search-box").item(0);

        //text view controller to html
        displayContent.innerHTML = ToContent;

        ScriptAppendAndReplace(doc.getElementsByTagName("div").item(0).id);
        //LoadScript("js\\" + "New\\EventMore.js", "EventNewMore");
        hideLoading();
        //change url
        //window.history.replaceState(controller, controller, url);
        return new Promise(function (resolve) { $("#RequestControl").collapse("hide"); resolve("resolved"); });
    })
        .catch(function (err) {
            hideLoading();
            alert('DisplayResult : Failed to fetch page: ', err);
            //var url = '@Url.Action("Index", "Login")';
            //window.location.href = url;
        });
}

function HomeSearch(url) {
    displayLoading();

    //effect from cbOTReqClick() need delay for new dateED changevalue
    let DateST = document.getElementById("dateOTStart");
    let DateED = document.getElementById("dateOTEnd");
    console.log(DateED);
    console.log(DateST);
    let jsonSearch = {};
    if (DateST != null)
        jsonSearch["start"] = DateST.value;
    if (DateED != null)
        jsonSearch["end"] = DateED.value;
    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(jsonSearch),
    }).then(function (response) {
        // When the page is loaded convert it to text
        return response.text()
    }).then(function (html) {
        // Initialize the DOM parser
        var parser = new DOMParser();
        console.log(html);
        // Parse the text
        var doc = parser.parseFromString(html, "text/html");
        var ToContent = doc.getElementsByClassName("just-group").item(0).outerHTML;

        //get div Display
        var displayContent = document.getElementsByClassName("search-box").item(1);

        //text view controller to html
        displayContent.innerHTML = ToContent;

        //ScriptAppendAndReplace(doc.getElementsByTagName("div").item(0).id);
        //LoadScript("js\\" + "New\\EventMore.js", "EventNewMore");
        hideLoading();
        //change url
        //window.history.replaceState(controller, controller, url);
    })
        .catch(function (err) {
            hideLoading();
            alert('HomeSearch: Failed to fetch page: ', err);
            //var url = '@Url.Action("Index", "Login")';
            //window.location.href = url;
        });
}

function ScriptAppendAndReplace(filename) {
    switch (filename) {
        case "Hour":
            LoadScript("js\\Home\\Search\\HourControl.js", "HourControl");
            break;
        case "Follow":
            LoadScript("js\\Home\\Search\\FollowControl.js", "FollowControl");
            break;
        case "Document":
            LoadScript("js\\Home\\Search\\DocumentControl.js", "DocumentControl");
            break;
        case "Graph":
            LoadScript("js\\Home\\Search\\GraphControl.js", "GraphControl");
            break;
        default:
            LoadScript("js\\New\\EventMore.js", "EventNewMore");
            break;
    }
    return;
}

function BtnActiive(ClassName) {
    var position;
    let oldActive;
    switch (ClassName) {
        case "hour": case "mytoday": case "FlowWaiting": case "FlowNewlate":
            position = 0;
            break;
        case "follow": case "myyesterday": case "FlowDone":
            position = 1;
            break;
        case "document": case "alltoday": case "FlowDisapproved":
            position = 2
            break;
        case "graph": case "allyesterday": case "DraftPage":
            position = 3
            break;
    }
    var buttonFilter = document.getElementsByClassName("item");
    for (var buttonAt = 0; buttonAt <= buttonFilter.length - 1; buttonAt++) {
        if (buttonAt == position) {
            oldActive = Array.from(buttonFilter.item(buttonAt).classList).find(c => c.startsWith('bg-'));
            buttonFilter.item(buttonAt).classList.replace(oldActive, "bg-active");
        } else {
            oldActive = Array.from(buttonFilter.item(buttonAt).classList).find(c => c.startsWith('bg-'));
            buttonFilter.item(buttonAt).classList.replace(oldActive, "bg-trans");
        }
    }
}

function resetStep(formID) {
    var form = document.getElementById(formID);
    var HisRoad = document.getElementsByClassName("istep");
    if (form.innerHTML.trim() != "") {
        form.innerHTML = "";
    }
    for (var item = 0; item <= HisRoad.length - 1; item++) {
        if (item == 0) { HisRoad.item(item).setAttribute("style", "display: block"); } else { HisRoad.item(item).setAttribute("style", "display: none"); }

    }
}

function Back(recentStep) {

    //History Link Road
    document.getElementsByClassName("istep").item(recentStep - 1).removeAttribute("style");
    document.getElementsByClassName("istep").item(recentStep - 2).setAttribute("style", NewOTRoadStyle);

    //content
    document.getElementById(ForwardModalID + recentStep).setAttribute("style", "display: none;");
    document.getElementById(ForwardModalID + (parseInt(recentStep) - 1)).removeAttribute("style");

    //footer
    document.getElementById(FooterID + recentStep).setAttribute("style", "display: none;");
    document.getElementById(FooterID + (parseInt(recentStep) - 1)).removeAttribute("style");
}

function createNextstep(nextStep) {
    var stepHasAlready = document.getElementById(ForwardModalID + nextStep);
    if (stepHasAlready == null) {
        let displayContent = document.getElementById(ModalContentBase);
        let displayFooter = document.getElementById(ModalFooterBase);
        let divContent = document.createElement("div");
        let divFooter = document.createElement("div");
        divContent.setAttribute("id", ForwardModalID + nextStep);
        displayContent.append(divContent);
        divFooter.setAttribute("id", FooterID + nextStep);
        displayFooter.append(divFooter);
    }
}

function GoToOTChoice(action, target) {
    var url = action;
    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
    }).then(function (response) {
        // When the page is loaded convert it to text
        return response.text()
    }).then(function (html) {
        resetStep(ModalContentBase);

        var parser = new DOMParser();
        var doc = parser.parseFromString(html, "text/html");
        var ToContent = doc.getElementById(ForwardModalID + "1").outerHTML;
        var footer = doc.getElementById("footer1").outerHTML;
        var displayContent = document.getElementById(target);
        var displayFooter = document.getElementById(ModalFooterBase);
        var displayHisRoad = document.getElementsByClassName("istep").item(0);

        displayHisRoad.setAttribute("style", NewOTRoadStyle);
        displayContent.innerHTML = ToContent;
        displayFooter.innerHTML = footer;


        //set div step2
        createNextstep(2);
        LoadScript("js\\" + "New\\EventOTType.js", "EventOTType");
    })
        .catch(function (err) {
            alert('GoToOTChoice: Failed to fetch page: ', err);
            var url = '@Url.Action("Index", "Login")';
            window.location.href = url;
        });
}

function GoToOTMyData(action, target, value) {
    var url = action;
    var displayHisRoad = document.getElementsByClassName("istep");
    displayHisRoad.item(0).removeAttribute("style");
    displayHisRoad.item(1).setAttribute("style", NewOTRoadStyle);
    document.getElementById("DaySelected").innerText = value;

    //send param to controller

    if (document.getElementById(ForwardModalID + "2").innerHTML.trim() == "") {
        fetch(url, {
            method: "POST",
            referrerPolicy: "strict-origin-when-cross-origin",
            credentials: "same-origin",
        }).then(function (response) {
            // When the page is loaded convert it to text
            return response.text()
        }).then(function (html) {
            var parser = new DOMParser();
            var doc = parser.parseFromString(html, "text/html");
            doc.getElementById("OTType").value = value;
            var ToContent = doc.getElementById(ForwardModalID + "2").outerHTML;
            var footer = doc.getElementById(FooterID + "2").outerHTML;


            document.getElementById(ForwardModalID + "1").setAttribute("style", "display:none;");
            document.getElementById(FooterID + "1").setAttribute("style", "display:none;");

            var displayContent = document.getElementById(target);
            var displayFooter = document.getElementById(FooterID + "2");


            displayContent.innerHTML = ToContent;
            displayFooter.innerHTML = footer;


            //LoadScript(urlHost + "js\\" + "New\\Index.js", "NewItem");
            LoadScript("js\\New\\EventOTMyData.js", "EventOTMyData");
        });
    } else {
        document.getElementById("OTType").value = value;
        if (document.getElementById(ForwardModalID + "2").style.display === "none") {
            //content
            document.getElementById(ForwardModalID + "2").style.display = "block";
            document.getElementById(ForwardModalID + "1").style.display = "none";
            //footer
            document.getElementById(FooterID + "2").style.display = "block";
            document.getElementById(FooterID + "1").style.display = "none";
        }
    }
}

function GoToNextStep(nextStep, ToAction) {
    var stepHasAlready = document.getElementById(ForwardModalID + nextStep);
    var displayHisRoad = document.getElementsByClassName("istep");
    displayHisRoad.item(nextStep - 2).removeAttribute("style");
    displayHisRoad.item(nextStep - 1).setAttribute("style", NewOTRoadStyle);
    if (stepHasAlready.innerHTML.trim() == "") {
        var url = ToAction;
        var targetContent = ForwardModalID + nextStep;
        var targetFooter = "footer" + nextStep;
        var data = new URLSearchParams();
        fetch(url, {
            method: "POST",
            body: data,
            referrerPolicy: "strict-origin-when-cross-origin",
            credentials: "same-origin",
        }).then(function (response) {
            // When the page is loaded convert it to text
            return response.text()
        }).then(function (html) {
            var parser = new DOMParser();
            var doc = parser.parseFromString(html, "text/html");
            var ToContent = doc.getElementById(ForwardModalID + nextStep).outerHTML;
            var ToFooter = doc.getElementById(FooterID + nextStep).outerHTML;

            //hide old display
            document.getElementById(ForwardModalID + (parseInt(nextStep) - 1)).setAttribute("style", "display:none;");
            document.getElementById(FooterID + (parseInt(nextStep) - 1)).setAttribute("style", "display:none;");

            var displayContent = document.getElementById(targetContent);
            var displayFooter = document.getElementById(targetFooter);
            displayContent.innerHTML = ToContent;
            displayFooter.innerHTML = ToFooter;

            BringScriptToPage(nextStep);

            return false;
        });
    } else {
        if (document.getElementById(ForwardModalID + nextStep).style.display === "none") {
            //content
            document.getElementById(ForwardModalID + nextStep).style.display = "block";
            document.getElementById(ForwardModalID + (parseInt(nextStep) - 1)).style.display = "none";
            //footer
            document.getElementById(FooterID + nextStep).style.display = "block";
            document.getElementById(FooterID + (parseInt(nextStep) - 1)).style.display = "none";
        }
    }
}

function CheckedMyChildren(checkboxEle) {
    var childrenEle = document.getElementById(checkboxEle.value);
    var checkboxsInChildren = childrenEle.querySelectorAll("input[type=checkbox]");
    checkboxsInChildren.forEach(function (ele) {
        ele.checked = checkboxEle.checked;
    });
}

function cbOTReqClick() {
    let cbOTReq = document.getElementById("cbOTReq");
    var dateOTStart = document.getElementById("dateOTStart");
    let dateOTEnd = document.getElementById("dateOTEnd");
    dateOTStart.disabled = !cbOTReq.checked;
    dateOTEnd.disabled = !cbOTReq.checked;
    //dateOTStart.addEventListener("change", function () {
    //    dateOTEnd.setAttribute("min", dateOTStart.value);
    //    if (Date.parse(dateOTEnd.value) < Date.parse(dateOTStart.value))
    //        dateOTEnd.value = dateOTStart.value;
    //    dateOTEnd.disabled = false;
    //    HomeSearch("Home\\SearchFollow");
    //});
}

function ddlLineChange() {
    let ddlLine = document.getElementsByClassName("ddlLine").item(0);
    let ddlModel = document.getElementsByClassName("ddlModel").item(0);
    let url = "Functions/ModelsOfProdLine"
    let jsonProdLine = {};
    if (ddlLine != null)
        jsonProdLine["name"] = ddlLine.value;

    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(jsonProdLine),
    }).then(function (response) {
        // When the page is loaded convert it to text
        return response.text()
    }).then(function (json) {
        let str = "";
        json = JSON.parse(json);
        for (var index = 0; index <= json.length - 1; index++) {
            str += "<option value='" + json + "'> " + json + "</option>";
        }
        ddlModel.innerHTML = str;
    });
}

function draftOTDocument() {
    displayLoading();

    let formCrateNew = new FormData(document.getElementById("formCreateNew"));
    var param1 = new URLSearchParams(formCrateNew);
    param1.append("mrOTType", document.getElementById("OTType").value);
    let poiterWorker = document.querySelectorAll(".worker-newot-details");
    poiterWorker.forEach(function (div) {
        param1.append("NewWorkerList", JSON.stringify({
            "drEmpCode": div.getElementsByClassName("empcode").item(0).textContent,
            "drJobCode": div.getElementsByClassName("job").item(0).value
        }));
    });
    let poiterMailCC = document.querySelectorAll("label.cc");
    poiterMailCC.forEach(function (label) {
        param1.append("MailCCs", label.textContent,
        );
    });

    let url = "New\\DraftDocument";
    return fetch(url, {
        method: "POST",
        body: param1,
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
    }).then(
        function (response) {
            return response.text();
        }).then(function (cmd) {
            hideLoading();

            //trans text to json
            cmd = JSON.parse(cmd);
            if (cmd.icon == "success") {
                if (document.getElementById("mrNoReq"))
                    document.getElementById("mrNoReq").value = cmd.req;
                return new Promise(function (resolve) { resolve("resolved"); });
            }
        }).catch(function (err) {
            hideLoading();
            alert('Something went wrong.', err);
            return false;
        });

}

function updateWorkerJob(Node) {
    let empcode = Node.parentNode.getElementsByClassName("empcode").item(0).innerHTML;
    let req = document.getElementById("mrNoReq").value;
    let jobselected = Node.value;
    let url = "New/UpdateWorkerJob?req=" + req + "&empcode=" + empcode + "&jobselected=" + jobselected;
    fetch(url, {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
    }).then(function (response) {
        return response.text();
    });
}

function updateWorkerAfterDelete(targetPaste, req) {
    let urlUpdateBasePage = "New\\WorkerList?req=" + req;
    fetch(urlUpdateBasePage).then(function (response) {
        return response.text();
    }).then(function (partialtext) {
        let parser = new DOMParser();
        let categoryhtml = parser.parseFromString(partialtext, "text/html");
        targetPaste.getElementsByClassName("workers-category").item(0).innerHTML = categoryhtml.getElementsByTagName("body").item(0).innerHTML;
    }).catch(function (err) {
        alert('Something went wrong.', err);
        return false;
    });
}

function ToXlsm(ele) {
    let value = ele.value;
    let url = "Functions\\ToXlsm?req=" + value;
    window.open(url, "_blank");
}

function LoadEmpPic(ele) {
    let empcode = ele.getElementsByClassName("empcode").item(0).innerHTML;
    let url = "Functions/LoadEmpPic?empcode=" + empcode;

    fetch(url).then(function (response) {
        return response.text();
    }).then(function (imgDataURL) {
        let containerImg = ele.getElementsByClassName("img").item(0);
        containerImg.innerHTML = "<img class='wx-100 border-rad' src='" + imgDataURL + "'>";
    });
}

async function ExportToXlsm(noInArray) {
    fetch("Functions/ToListXlsm", {
        method: "POST",
        referrerPolicy: "strict-origin-when-cross-origin",
        credentials: "same-origin",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(noInArray),
    }).then(function (response) {
        return response.text()
    }).then(function (xlsm) {
        location.href = "Functions/XlsxFromByte";
    });
}

function notEnter(e) { if (e.keyCode == 13) return false; }

//showing Loading
function displayLoading() {
    loader.style.display = "flex";
    //setTimeout(() => {
    //    loader.style.display = "none";
    //}, 300000);
}

//hiding Loading
function hideLoading() {
    loader.style.display = "none";
}

//showing Loading
function displayLoadingAndShowProcess(maxCount) {
    Swal.fire({
        html: "กำลังอัพเดทข้อมูล... <p><b id='bCounting'>0</b>" + "of <b>" + maxCount + "</b></p>",
        allowEscapeKey: false,
        allowOutsideClick: false,
        showConfirmButton: false,
        timerProgressBar: true,
    })
}

function displayExportingAndShowProcess() {
    Swal.fire({
        html: "กำลังสร้างไฟล์ Excel...",
        allowEscapeKey: false,
        allowOutsideClick: false,
        showConfirmButton: false,
        timerProgressBar: true,
    })
}

//hiding Loading
function hideLoadingAndShowProcess() {
    Swal.close();
    //loadingProcesser.style.display = "none";
}


function Menubar_sendmail(getID, action) {
    //const formdata = new FormData(document.forms.item(0)).serialize();
    const radios = document.getElementsByName("_ViewceMastSubMakerRequest.smRemark");
    const target = document.getElementById("radioGroupContainer");
    let isChecked = false;
    let msg = "";


    let table = document.getElementById('tbDetailSubMakerRequest');
    let rows = table.getElementsByTagName('tr');

    //if (rows.length > 3) { // Check if there is more than just the header row
    //    console.log("The table has more than 0 rows.");
    //} else {
    //    console.log("The table does not have rows greater than 0.");
    //}



    if (document.getElementById("i_New_OrderNo").value == "") {
        msg = "กรุณากรอกข้อมูล Order No !!!";
    }
    else if (document.getElementById("i_New_LotNo").value == "") {
        msg = "กรุณากรอกข้อมูล Lot No !!!";
    }

    else if (document.getElementById("i_New_CustomerName").value == "") {
        msg = "กรุณากรอกข้อมูล CustomerName !!!";
    }
    else if (document.getElementById("i_New_MoldName").value == "") {
        msg = "กรุณากรอกข้อมูล MoldName/Mold No !!!";
    }
    else if (document.getElementById("i_New_ModelName").value == "") {
        msg = "กรุณากรอกข้อมูล ModelName !!!";
    }
    else if (rows.length < 3) { // Check if there is more than just the header row
        msg = "กรุณากรอกข้อมูล Model Name ให้ถูกต้อง !!!";
        console.log("The table has more than 0 rows.");
    }
    else if (document.getElementById("i_New_CavityNo").value == "") {
        msg = "กรุณากรอกข้อมูล Cavity No !!!";
    }
    else if (document.getElementById("i_New_Function").value == "") {
        msg = "กรุณากรอกข้อมูล Function !!!";
    }
    else if (document.getElementById("i_New_DevelopmentStage").value == "") {
        msg = "กรุณากรอกข้อมูล Developmentn Stage !!!";
    }
    else if (document.getElementById("i_New_MoldNo").value == "") {
        msg = "กรุณากรอกข้อมูล Mold Mass !!!";
    }
    else if (document.getElementById("i_New_TypeCavity").value == "") {
        msg = "กรุณากรอกข้อมูล Type Cavity !!!";
    }
    else if (document.getElementById("i_New_MatOutDate").value == "") {
        msg = "กรุณากรอกข้อมูล Mat Out Date !!!";
    }
    else if (document.getElementById("i_New_MatOutTime").value == "") {
        msg = "กรุณากรอกข้อมูล Mat Out Time !!!";
    }
    else if (document.getElementById("i_New_DeliveryDate").value == "") {
        msg = "กรุณากรอกข้อมูล Delivery Date !!!";
    }
    else if (document.getElementById("i_New_DeliveryTime").value == "") {
        msg = "กรุณากรอกข้อมูล Delivery Time !!!";
    }
    else if (radios.length > 0) {

        for (let i = 0; i < radios.length; i++) {
            if (radios[i].checked) {
                isChecked = true;
                i = radios.length - 1;
            }
        }

        if (!isChecked) {
            msg = "กรุณาเลือก REMARK !!!";

        }


    }
    else if (document.getElementById("i_New_Detail").value == "") {
        msg = "กรุณากรอกข้อมูล Detail !!!";
    }
    else if (document.getElementById("i_New_Weight").value == "") {
        msg = "กรุณากรอกข้อมูล Weight!!!";
    }
    else if (document.getElementById("i_New_Qty").value == "") {
        msg = "กรุณากรอกข้อมูล Qty!!!";
    }





    if (msg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: msg,
        })
            .then((result) => {

            });
    }
    else {

        let mydata = $("#formRequest").serialize();

        $.ajax({
            type: 'post',
            url: action,
            data: mydata,
            success: function (data) {

                console.log("fsendMail");

                var htmls = "";
                if (data.status == "hasHistory") {
                    htmls = " <div class='panel panel-default property'>"
                    // console.log(data.listHistory.length);
                    $.each(data.listHistory, function (i, item) {
                        //console.log('test' + item.htTo); console.log(data.listHistory[0].htTo);
                        console.log("OK")
                        htmls += "     <div class='panel-heading panel-heading-custom property' tabindex = '0' >"
                        htmls += "         <h4 class='panel-title faq-title-range collapsed' data-bs-toggle='collapse' data-bs-target='#Ans" + item.htStep + "' aria-expanded='false' aria-controls='collapseExample'>"
                        htmls += "             <label style='font-size: 13px;'>Step : "
                        if (item.htStep == 0) {
                            htmls += item.htStep
                        }
                        else {
                            htmls += (item.htStep + 1)
                        }

                        htmls += "  </label > <label class='lbV'></label>"
                        htmls += "         </h4>"
                        htmls += "     </div >"
                        htmls += "     <div class='panel-collapse collapse' style = 'overflow: auto;' id = 'Ans" + item.htStep + "' > "

                        htmls += "         <div class='panel-body'>"
                        htmls += "             <div style='font-size: x-small; clear: both; width: 100%; tetx-align: left; font-weight: bold;'>"
                        htmls += "                 <label> " + item.htDate + " :: " + item.htTime + " น.</label>"

                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>FROM : </label></br>"
                        htmls += "                 <label>" + item.htFrom + "</label > "
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>TO : </label></br>"
                        htmls += "                 <label>" + item.htTo + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        if (item.htCC == null) { item.htCC = "" }
                        else { item.htCC = item.htCC }
                        htmls += "                 <label>CC : </label>"
                        htmls += "                 <label>" + item.htCC + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: right; width: 20%; tetx-align: left;'>"
                        if (item.htRemark == null) { item.htRemark = "" }
                        else { item.htRemark = item.htRemark }
                        htmls += "                 <label>Remark : </label>"
                        htmls += "                 <label>" + item.htRemark + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: right; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>Status : </label>"
                        if (item.htStatus == null) { item.htStatus = "" }
                        else {
                            item.htStatus = item.htStatus
                            if (item.htStatus == "Finished") {
                                htmls += "                 <label><span style='color: green;'>" + item.htStatus + "</span></label>"
                            } else {
                                htmls += "                 <label><span style='color: darkkhaki;'>" + item.htStatus + "</span></label>"
                            }
                        }

                        htmls += "             </div>"
                        htmls += "         </div>"
                        htmls += "     </div>"

                    });
                    htmls += "</div>"
                }
                else {
                    htmls = " <div class='panel panel-default property'>"
                    htmls += "     <div class='panel-heading panel-heading-custom property' tabindex = '0' >"
                    htmls += " <label><span style='color: blue;'>ไม่มีประวัติการส่งอีเมล์</span></label>"
                    htmls += "</div>"
                    htmls += "</div>"
                }

                //var url = data.partial + "&vform=" + vform;

                //var url = data.partial + mydata;
                var url = data.partial;
                //console.log("url" + url);
                $("#myModalBodyDiv1").load(url, function () {
                    $('#divHistory').html(htmls);
                    $("#myModal1").modal("show");
                })

            }
        });
    }


}


function Menubar_saveDraft(action) {
    let vmsg = "";
    if (document.getElementById("i_New_OrderNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Order No !!!";
    }
    else if (document.getElementById("i_New_LotNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Lot No !!!";
    }


    if (vmsg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: vmsg,

        })
            .then((result) => {
            });
    }
    else {
        let formData = document.forms.namedItem("formRequest");
        let viewModel = new FormData(formData);

        $.each(formData, function (index, input) {
            viewModel.append(input.name, input.value);
        });

        $.ajax({
            type: "POST",
            url: action,
            data: viewModel,
            processData: false,
            contentType: false,
            beforeSend: function () {
                swal.fire({
                    html: '<h5>Loading...</h5>',
                    showConfirmButton: false,
                    onRender: function () {
                        // there will only ever be one sweet alert open.
                        //$('.swal2-content').prepend(sweet_loader);
                    }
                });
            },
            success: async function (config) {
                // alert(config.c1);
                if (config.c1 == "S" || config.c1 == "D") {
                    // $("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    swal.fire({
                        title: 'SUCCESS',
                        icon: 'success',
                        text: "Save data Already !!!!",
                    }).then((result) => {
                        if (result.isConfirmed) {
                            //console.log("config.c3" + config.c3);
                            GoSideMenu("Search");

                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        }
                    });
                }
                else if (config.c1 == "E") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'error',
                        title: 'ERROR',
                        text: config.c2,
                    })
                        .then((result) => {
                            $("#myModal1").modal("show");
                        });

                }
                else if (config.c1 == "P") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'warning',
                        title: 'warning',
                        text: config.c2,
                    })
                        .then((result) => {

                            //$("#myModal1").modal("show");
                        });

                }

            }
        });
    }





}
function CheckDis(status) {
    $(document).ready(function () {
        var checkStatusDis = status;
        var step = $('#step').val();
        console.log("step ==> " + step);
        if (checkStatusDis == 'Disapprove' || checkStatusDis == 'Cancel') {
            $('#searchInputTO').attr("disabled", "disabled");
            //document.getElementById("searchInputTO").value = "";
            //$('#EmailTo').removeAttr("name");
        }
        else {
            $('#searchInputTO').removeAttr('disabled', 'disabled');
            $('#EmailTo').removeAttr("name");
            if (step == 4) {
                console.log(step);
                document.getElementById("searchInputTO").value = $('#EmailTo').val();
            }

        }

    });
}
function Menubar_save_sendMail(action) {
    console.log("Menubar_save_sendMail");

    var vmsg = ""
    let formData = document.forms.namedItem("formData");
    let viewModel = new FormData(formData);

    //$.each(formData, function (index, input) {
    //    viewModel.append(input.name, input.value);
    //});

    $.each(formData.elements, function (index, input) {
        if (input.name) {
            viewModel.append(input.name, input.value);
        }
    });


    if (vmsg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: vmsg,
        })
            .then((result) => {


            });
    } else {
        $.ajax({
            type: "POST",
            url: action,
            data: viewModel,
            processData: false,
            contentType: false,
            beforeSend: function () {
                swal.fire({
                    html: '<h5>Loading...</h5>',
                    showConfirmButton: false,
                    onRender: function () {
                        // there will only ever be one sweet alert open.
                        //$('.swal2-content').prepend(sweet_loader);
                    }
                });
            },
            success: async function (config) {
                // alert(config.c1);
                if (config.c1 == "S") {
                    $("#loaderDiv").hide();
                    await $("#myModal1").modal("hide");
                    swal.fire({
                        title: 'SUCCESS',
                        icon: 'success',
                        text: "Send Mail Already",
                    }).then((result) => {
                        if (result.isConfirmed) {
                            console.log("config.c3" + config.c3);
                            GoSideMenu("Search");
                        }
                    });
                }
                else if (config.c1 == "E") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'error',
                        title: 'ERROR',
                        text: config.c2,
                    })
                        .then((result) => {
                            $("#myModal1").modal("show");
                        });

                }
                else if (config.c1 == "P") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'warning',
                        title: 'warning',
                        text: config.c2,
                    })
                        .then((result) => {
                            //$("#myModal1").modal("show");
                        });

                }

            }
        });
    }
}
function Menubar_ExportExcel(action) {
    console.log("Menubar_ExportExcel");
    var vmsg = ""
    let formData = document.forms.namedItem("formSearch");
    let viewModel = new FormData(formData);

    $.each(formData, function (index, input) {
        viewModel.append(input.name, input.value);
    });

    $.ajax({
        type: "POST",
        url: action,
        data: viewModel,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                swal.fire({
                    title: 'SUCCESS',
                    icon: 'success',
                    text: config.c2,
                }).then((result) => {
                    if (result.isConfirmed) {
                        console.log("config.c3" + config.c2);

                    }
                });
            }
            else if (config.c1 == "E") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: config.c2,
                })
                    .then((result) => {

                    });

            }


            else if (config.c1 == "W") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    title: 'Wairning',
                    icon: 'warning',
                    text: config.c2,
                })
                    .then((result) => {

                    });

            }
        }
    });

}
function DeleteFileUser(id, vname, action) {
    var getID = document.getElementById("i_NewMold_DocumentNo").value; //txtMIssueID


    //action, vForm, vTeam, vSubject, vSrNo


    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete File?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { id: id, vname: vname },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            //GoSideMenu("Home");
                            // GoNewRequest(getID)
                            GoNewMoldRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}
function DeleteMasterPlanning(DocNo, action) {
    //var getID = document.getElementById("i_New_DocumentNo").value; //txtMIssueID


    //action, vForm, vTeam, vSubject, vSrNo

    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete Master Planning ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { DocNo: DocNo },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            //$("#myModal3").modal("hide");
                            GoSideMenu("AddCost");
                            // GoNewRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}
function Menubar_AddCostMOdel(CostNo, Desc, action) {
    console.log("Menubar_AddCostMOdel");
    let mydata = $("#formRequestCost").serialize();
    //'@Url.Action("SearchbyModelName", "New")',
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            DocNo: CostNo,
            Desc: Desc,
            class: mydata

        },
        beforeSend: function () {
            //// Show Loading ก่อนส่งข้อมูล
            //$("#loading").show();
        },
        success: function (response) {
            $("#ResultMastCostModel").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        }
    });

    $("#myModal3").modal("show");
}
function Menubar_AddMaster(action) {
    let formData = document.forms.namedItem("formMastCostModel");
    let viewModel = new FormData(formData);

    $.each(formData, function (index, input) {
        viewModel.append(input.name, input.value);
    });

    $.ajax({
        type: "POST",
        url: action,
        data: viewModel,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                // $("#loaderDiv").hide();
                await $("#myModal3").modal("hide");
                swal.fire({
                    title: 'SUCCESS',
                    icon: 'success',
                    text: config.c2,
                }).then((result) => {
                    //if (result.isConfirmed) {
                    //    console.log("config.c3" + config.c3);
                    //    GoSideMenu("Search");
                    //}
                });
            }
            else if (config.c1 == "N") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'แจ้งเตือน',
                    title: 'warning',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal3").modal("show");
                    });

            }
        }
    });
}
function Menubar_AddCostPlanning(CostNo, Desc, action) {
    console.log("Menubar_AddCostPlanning");
    let mydata = $("#formRequestCost").serialize();
    //'@Url.Action("SearchbyModelName", "New")',
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            DocNo: CostNo,
            Desc: Desc,
            class: mydata

        },
        beforeSend: function () {
            console.log("Showing loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicator").css("display", "block"); // แสดง Loader
            $("#ResultMastCostPlanning").css("display", "none"); // ซ่อน Loader
        },
        success: function (response) {
            $("#ResultMastCostPlanning").css("display", "block"); // แสดง Loader
            $("#ResultMastCostPlanning").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        },
        complete: function () {
            // ซ่อนรูปโหลดเมื่อ request เสร็จ
            console.log("Hiding loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicator").css("display", "none"); // ซ่อน Loader
        }
    });

    $("#myModal4").modal("show");
}
function DeleteCostModel(DocNo, ModelName, action) {

    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete Model Name ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { DocNo: DocNo, ModelName: ModelName },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            // $("#myModal3").modal("hide");
                            GoSideMenu("AddCost");
                            // GoNewRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}
function Menubar_AddMasterCostPlanning(action) {
    let formData = document.forms.namedItem("formMastCostplanning");
    let viewModel = new FormData(formData);

    $.each(formData, function (index, input) {
        viewModel.append(input.name, input.value);
    });

    $.ajax({
        type: "POST",
        url: action,
        data: viewModel,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                // $("#loaderDiv").hide();
                await $("#myModal4").modal("hide");
                swal.fire({
                    title: 'SUCCESS',
                    icon: 'success',
                    text: config.c2,
                }).then((result) => {
                    //if (result.isConfirmed) {
                    //    console.log("config.c3" + config.c3);
                    GoSideMenu("AddCost");
                    //}
                });
            }
            else if (config.c1 == "E") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal4").modal("show");
                    });

            }
            else if (config.c1 == "N") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'แจ้งเตือน',
                    title: 'warning',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal4").modal("show");
                    });

            }
        }
    });
}
function DeleteCost(DocNo, ModelName, action) {
    //var getID = document.getElementById("i_New_DocumentNo").value; //txtMIssueID


    //action, vForm, vTeam, vSubject, vSrNo


    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete Master Process Name ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { DocNo: DocNo, ModelName: ModelName },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            // $("#myModal3").modal("hide");
                            //GoSideMenu("Home");
                            // GoNewRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}


//QUOTATION
function Menubar_PrintQUOTATION(action, mpNo) {

    console.log("Menubar_PrintQUOTATION");
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            mpNo: mpNo
        },
        beforeSend: function () {

            console.log("Showing loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorQuotation").css("display", "block"); // แสดง Loader
            $("#ResultQuotation").css("display", "none"); // ซ่อน Loader
        },
        success: function (response) {
            //$("#ResultQuotation").html(response); // เอา HTML Partial View มาใส่ใน Div

            $("#ResultQuotation").css("display", "block"); // แสดง Loader
            $("#ResultQuotation").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        },
        complete: function () {
            // ซ่อนรูปโหลดเมื่อ request เสร็จ
            console.log("Hiding loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorQuotation").css("display", "none"); // ซ่อน Loader

        }
    });
    $("#myModalQUOTATION").modal("show");
}
function printQuotationA4(mpNo, action, type) {
    var printContents;
    // MoldModify
    //SubMaker
    if (type == "SubMaker") {
        printContents = document.getElementById("ResultQuotation").innerHTML;
    } else {
        printContents = document.getElementById("ResultMoldQuotation").innerHTML;
    }
    //ResultMoldQuotation
    //ResultQuotation
    //printContents = document.getElementById("ResultQuotation").innerHTML;
    var originalContents = document.body.innerHTML;

    var printWindow = window.open('', '', 'height=800,width=1000');
    printWindow.document.write('<html><head><title>Quotation</title>');
    printWindow.document.write('<style>@media print { body { font-family: Arial; size: A4; } }</style>');
    printWindow.document.write('</head><body>');
    printWindow.document.write(printContents);
    printWindow.document.write('</body></html>');

    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();

    let url = action + mpNo;
    //let url = "New?smDocumentNo=" + mpNo;
    //let url = "New?smLotNo=" + smLotNo + "&smOrderNo=" + smOrderNo + "&smRevision=" + smRevision;
    GoSideMenu(url);

    //$("#myModalQUOTATION").hide();
}


//Process
function Menubar_EditMasterProcess(mpNo, action) {
    console.log("Menubar_AddCostPlanning");
    let mydata = $("#formRequestCost").serialize();
    //'@Url.Action("SearchbyModelName", "New")',
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            mpNo: mpNo
        },
        beforeSend: function () {
            console.log("Showing loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorProcess").css("display", "block"); // แสดง Loader
            $("#ResultMastProcess").css("display", "none"); // ซ่อน Loader
        },
        success: function (response) {
            $("#ResultMastProcess").css("display", "block"); // แสดง Loader
            $("#ResultMastProcess").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        },
        complete: function () {
            // ซ่อนรูปโหลดเมื่อ request เสร็จ
            console.log("Hiding loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorProcess").css("display", "none"); // ซ่อน Loader
        }
    });

    $("#myModal5").modal("show");
}

function Menubar_DeleteMasterProcess(processGroup, processName, action) {

    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete Master Process ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { processGroup: processGroup, processName: processName },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            // $("#myModal3").modal("hide");
                            GoSideMenu("AddProcess");
                            // GoNewRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}

function Menubar_AddMasterProcess(action) {
    let formData = document.forms.namedItem("formMastProcess");
    let viewModel = new FormData(formData);

    $.each(formData, function (index, input) {
        viewModel.append(input.name, input.value);
    });

    $.ajax({
        type: "POST",
        url: action,
        data: viewModel,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                // $("#loaderDiv").hide();
                await $("#myModal5").modal("hide");
                swal.fire({
                    title: 'SUCCESS',
                    icon: 'success',
                    text: config.c2,
                }).then((result) => {
                    //if (result.isConfirmed) {
                    //    console.log("config.c3" + config.c3);
                    GoSideMenu("AddProcess");
                    //}
                });
            }
            else if (config.c1 == "E") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal5").modal("show");
                    });

            }
            else if (config.c1 == "N") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'แจ้งเตือน',
                    title: 'warning',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal5").modal("show");
                    });

            }
        }
    });
}



//Master Model
function Menubar_EditMasterModel(mmNo, mmmodelName, action) {
    console.log("Menubar_AddMast Model");
    // let mydata = $("#formRequestCost").serialize();
    //'@Url.Action("SearchbyModelName", "New")',
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            mmNo: mmNo,
            ModelName: mmmodelName,
        },
        beforeSend: function () {
            console.log("Showing loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorModel").css("display", "block"); // แสดง Loader
            $("#ResultMastModel").css("display", "none"); // ซ่อน Loader
        },
        success: function (response) {
            $("#ResultMastModel").css("display", "block"); // แสดง Loader
            $("#ResultMastModel").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        },
        complete: function () {
            // ซ่อนรูปโหลดเมื่อ request เสร็จ
            console.log("Hiding loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorModel").css("display", "none"); // ซ่อน Loader
        }
    });

    $("#myModal6").modal("show");
}

function Menubar_DeleteMasterModel(mmNo, ModelName, action) {

    Swal.fire({
        title: "Are you sure?",
        text: "Are you sure delete Master Model ?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: "No"
    }).then((result) => {
        if (result['isConfirmed']) {
            $.ajax({
                type: 'post',
                url: action,
                data: { mmNo: mmNo, ModelName: ModelName },
                success: function (res) {
                    swal.fire({
                        title: 'แจ้งเตือน',
                        icon: res.res,
                        text: res.res,
                    })
                        .then((result) => {
                            // $("#myModal3").modal("hide");
                            GoSideMenu("AddModel");
                            // GoNewRequest(getID)
                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                        });



                }
            });
        } else {
            //console.log('Cancel');
            return false;
        }
    });
}

function Menubar_AddMasterModel(action) {
    let formData = document.forms.namedItem("formMastModel");
    let viewModel = new FormData(formData);

    $.each(formData, function (index, input) {
        viewModel.append(input.name, input.value);
    });

    $.ajax({
        type: "POST",
        url: action,
        data: viewModel,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                // $("#loaderDiv").hide();
                await $("#myModal6").modal("hide");
                swal.fire({
                    title: 'SUCCESS',
                    icon: 'success',
                    text: config.c2,
                }).then((result) => {
                    //if (result.isConfirmed) {
                    //    console.log("config.c3" + config.c3);
                    GoSideMenu("AddModel");
                    //}
                });
            }
            else if (config.c1 == "E") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal6").modal("show");
                    });

            }
            else if (config.c1 == "N") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'แจ้งเตือน',
                    title: 'warning',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModal6").modal("show");
                    });

            }
        }
    });
}


////////////////Mold Modify
function Menubar_PrintMoldQUOTATION(action, mpNo) {

    console.log("myModalMoldQUOTATION");
    $.ajax({
        url: action,//'/New/SearchbyModelName', // URL ของ Controller
        type: 'POST',
        data: {
            mpNo: mpNo
        },
        beforeSend: function () {

            console.log("Showing loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorMoldQuotation").css("display", "block"); // แสดง Loader
            $("#ResultMoldQuotation").css("display", "none"); // ซ่อน Loader
        },
        success: function (response) {
            //$("#ResultQuotation").html(response); // เอา HTML Partial View มาใส่ใน Div

            $("#ResultMoldQuotation").css("display", "block"); // แสดง Loader
            $("#ResultMoldQuotation").html(response); // เอา HTML Partial View มาใส่ใน Div
        },
        error: function () {
            alert("Error!!");
        },
        complete: function () {
            // ซ่อนรูปโหลดเมื่อ request เสร็จ
            console.log("Hiding loader..."); // ตรวจสอบว่าทำงานจริง
            $("#loadingIndicatorMoldQuotation").css("display", "none"); // ซ่อน Loader

        }
    });
    $("#myModalMoldQUOTATION").modal("show");
}
function Menubar_Moldsendmail(getID, action) {

    let vmsg = "";
    if (document.getElementById("i_NewMold_ReferenceNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Reference No !!!";
        document.getElementById("i_NewMold_ReferenceNo").focus();
    } else if (document.getElementById("i_NewMold_LotNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Lot No !!!";
        document.getElementById("i_NewMold_LotNo").focus();
    } else if (document.getElementById("i_NewMold_CustomerName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Customer Name !!!";
        document.getElementById("i_NewMold_CustomerName").focus();
    } else if (document.getElementById("i_NewMold_MoldName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Name !!!";
        document.getElementById("i_NewMold_MoldName").focus();
    } else if (document.getElementById("i_NewMold_ModelName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Model Name !!!";
        document.getElementById("i_NewMold_ModelName").focus();
    } else if (document.getElementById("i_NewMold_CavityNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cavity No !!!";
        document.getElementById("i_NewMold_CavityNo").focus();
    } else if (document.getElementById("i_NewMold_Function").value == "") {
        vmsg = "กรุณากรอกข้อมูล Function !!!";
        document.getElementById("i_NewMold_Function").focus();
    } else if (document.getElementById("i_NewMold_RequestBy").value == "") {
        vmsg = "กรุณากรอกข้อมูล Request By !!!";
        document.getElementById("i_NewMold_RequestBy").focus();
    } else if (document.getElementById("i_NewMold_MoldMass").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Mass !!!";
        document.getElementById("i_NewMold_MoldMass").focus();
    } else if (document.getElementById("i_NewMold_TypeCavity").value == "") {
        vmsg = "กรุณากรอกข้อมูล Type Cavity !!!";
        document.getElementById("i_NewMold_TypeCavity").focus();
    } else if (document.getElementById("i_NewMold_LeadTime").value == "") {
        vmsg = "กรุณากรอกข้อมูล Lead Time !!!";
        document.getElementById("i_NewMold_LeadTime").focus();
    } else if (document.getElementById("i_NewMold_EstimateCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล Estimate Cost !!!";
        document.getElementById("i_NewMold_EstimateCost").focus();
    } else if (document.getElementById("i_NewMold_MKPriceCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล MK Price Cost !!!";
        document.getElementById("i_NewMold_MKPriceCost").focus();
    } else if (document.getElementById("i_NewMold_ResultCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล Result Cost !!!";
        document.getElementById("i_NewMold_ResultCost").focus();
    } else if (document.getElementById("i_NewMold_IssueRate").value == "") {
        vmsg = "กรุณากรอกข้อมูล Issue Rate !!!";
        document.getElementById("i_NewMold_IssueRate").focus();
    } else if (document.getElementById("i_NewMold_CostRate").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cost Rate !!!";
        document.getElementById("i_NewMold_CostRate").focus();
    } else if (document.getElementById("i_NewMold_CostRateType").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cost Rate Type !!!";
        document.getElementById("i_NewMold_CostRateType").focus();
    } else if (document.getElementById("i_NewMold_Type").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Type !!!";
        document.getElementById("i_NewMold_Type").focus();
        //} else if (document.getElementById("i_NewMold_mfProcess").value == "") {
        //    vmsg = "กรุณากรอกข้อมูล MF Process !!!";
        //    document.getElementById("i_NewMold_mfProcess").focus();
    } else if (document.getElementById("i_NewMold_Detail").value == "") {
        vmsg = "กรุณากรอกข้อมูล Detail !!!";
        document.getElementById("i_NewMold_Detail").focus();
    }

    let table1 = document.getElementById('tbDetailMoldProcessDetail');
    let rows1 = table1.getElementsByTagName('tr');


    if (rows1.length < 3) { // Check if there is more than just the header row
        msg = "กรุณากรอกข้อมูล Model Name ให้ถูกต้อง !!!";
        document.getElementById("i_NewMold_ModelName").focus();
        console.log("The table has more than 0 rows.");
    }


    const rows = document.querySelectorAll("#tableBody tr"); //table id material body
    //table matrerail body
    ceItemModifyRequest = [];
    rows.forEach((row, index) => {

        const itemNameInput = row.querySelector(".imItemName");
        const itemName = itemNameInput.value.trim();

        if (!itemName) {
            alert(`กรุณากรอกชื่อ Item (บรรทัดที่ ${index + 1})`);
            itemNameInput.focus(); // optional: focus ช่องนั้น
            throw new Error("Item name is required."); // ❌ หยุดการทำงาน (ถ้าใช้ใน loop ใหญ่)
        }

        ceItemModifyRequest.push({
            imCENo: row.querySelector(".imCENo").value.trim() || "CE-M",
            imItemNo: index + 1, // ✅ เริ่มจาก 1
            imItemName: row.querySelector(".imItemName").value.trim(),
            imPCS: parseInt(row.querySelector(".imPCS").value) || 0,
            imAmount: parseFloat(row.querySelector(".imAmount").value) || 0
        });
    });




    if (vmsg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: vmsg,
        })
            .then((result) => {

            });
    }
    else {
        const form1 = document.forms.namedItem("formMoldData1");
        const form2 = document.forms.namedItem("formMoldData2");
        const form3 = document.forms.namedItem("formMoldData3");
        let viewModel1 = new FormData(form1);
        let viewModel2 = new FormData(form2);
        let viewModel3 = new FormData(form3);


        $.each(form1, function (index, input) {
            viewModel1.append(input.name, input.value);
        });
        $.each(form3, function (index, input) {
            viewModel3.append(input.name, input.value);
        });


        // สร้าง FormData ใหม่เพื่อรวมทั้งสองอัน
        let combinedFormData = new FormData();
        // ใส่ข้อมูลจาก viewModel1
        viewModel1.forEach((value, key) => {
            combinedFormData.append(key, value);
        });
        // ใส่ข้อมูลจาก viewModel2
        viewModel3.forEach((value, key) => {
            combinedFormData.append(key, value);
        });


        //tab mat
        $.each(form2, function (index, input) {
            viewModel2.append(input.name, input.value);
        });






        $.ajax({
            type: 'post',
            url: action,
            data: combinedFormData,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log("fsendMoldMail");
                var htmls = "";
                if (data.status == "hasHistory") {
                    htmls = " <div class='panel panel-default property'>"
                    // console.log(data.listHistory.length);
                    $.each(data.listHistory, function (i, item) {
                        //console.log('test' + item.htTo); console.log(data.listHistory[0].htTo);
                        console.log("OK")
                        htmls += "     <div class='panel-heading panel-heading-custom property' tabindex = '0' >"
                        htmls += "         <h4 class='panel-title faq-title-range collapsed' data-bs-toggle='collapse' data-bs-target='#Ans" + item.htStep + "' aria-expanded='false' aria-controls='collapseExample'>"
                        htmls += "             <label style='font-size: 13px;'>Step : "
                        if (item.htStep == 0) {
                            htmls += item.htStep
                        }
                        else {
                            htmls += (item.htStep + 1)
                        }

                        htmls += "  </label > <label class='lbV'></label>"
                        htmls += "         </h4>"
                        htmls += "     </div >"
                        htmls += "     <div class='panel-collapse collapse' style = 'overflow: auto;' id = 'Ans" + item.htStep + "' > "

                        htmls += "         <div class='panel-body'>"
                        htmls += "             <div style='font-size: x-small; clear: both; width: 100%; tetx-align: left; font-weight: bold;'>"
                        htmls += "                 <label> " + item.htDate + " :: " + item.htTime + " น.</label>"

                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>FROM : </label></br>"
                        htmls += "                 <label>" + item.htFrom + "</label > "
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>TO : </label></br>"
                        htmls += "                 <label>" + item.htTo + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: left; width: 20%; tetx-align: left;'>"
                        if (item.htCC == null) { item.htCC = "" }
                        else { item.htCC = item.htCC }
                        htmls += "                 <label>CC : </label>"
                        htmls += "                 <label>" + item.htCC + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: right; width: 20%; tetx-align: left;'>"
                        if (item.htRemark == null) { item.htRemark = "" }
                        else { item.htRemark = item.htRemark }
                        htmls += "                 <label>Remark : </label>"
                        htmls += "                 <label>" + item.htRemark + "</label>"
                        htmls += "             </div>"
                        htmls += "             <div style='font-size: x-small; float: right; width: 20%; tetx-align: left;'>"
                        htmls += "                 <label>Status : </label>"
                        if (item.htStatus == null) { item.htStatus = "" }
                        else {
                            item.htStatus = item.htStatus
                            if (item.htStatus == "Finished") {
                                htmls += "                 <label><span style='color: green;'>" + item.htStatus + "</span></label>"
                            } else {
                                htmls += "                 <label><span style='color: darkkhaki;'>" + item.htStatus + "</span></label>"
                            }
                        }

                        htmls += "             </div>"
                        htmls += "         </div>"
                        htmls += "     </div>"

                    });
                    htmls += "</div>"
                }
                else {
                    htmls = " <div class='panel panel-default property'>"
                    htmls += "     <div class='panel-heading panel-heading-custom property' tabindex = '0' >"
                    htmls += " <label><span style='color: blue;'>ไม่มีประวัติการส่งอีเมล์</span></label>"
                    htmls += "</div>"
                    htmls += "</div>"
                }

                //var url = data.partial + "&vform=" + vform;

                //var url = data.partial + mydata;
                var url = data.partial;
                //console.log("url" + url);
                $("#myModalBodySendMold").load(url, function () {
                    $('#divHistory').html(htmls);
                    $("#myModalSendMold").modal("show");
                })

            }
        });
    }


}
function Menubar_save_sendMailData(action, action2) {
    console.log("call Menubar Mold send Mail");
    const rows = document.querySelectorAll("#tableBody tr"); //table id material body
    var vmsg = ""
    const form1 = document.forms.namedItem("formMoldData1"); // form header
    const form2 = document.forms.namedItem("formMoldData2"); // form Process Detail.
    const form3 = document.forms.namedItem("formMoldData3"); // form Mat Detail, Summary ,email


    let viewModel1 = new FormData(form1);  // form header
    let viewModel2 = new FormData(form2);  // form Process Detail.
    let viewModel3 = new FormData(form3);  // form Mat Detail, Summary ,email



    $.each(form1, function (index, input) {
        viewModel1.append(input.name, input.value);
    });
    $.each(form2, function (index, input) {
        viewModel2.append(input.name, input.value);
    });
    $.each(form3, function (index, input) {
        viewModel3.append(input.name, input.value);
    });


    //form header form Mat Detail, Summary ,email
    // สร้าง FormData ใหม่เพื่อรวมทั้งสองอัน
    let headerFormData = new FormData();
    // ใส่ข้อมูลจาก viewModel1
    viewModel1.forEach((value, key) => {
        headerFormData.append(key, value);
    });
    // ใส่ข้อมูลจาก viewModel3
    viewModel3.forEach((value, key) => {
        headerFormData.append(key, value);
    });



    //tab Process
    //$.each(form2, function (index, input) {
    //    viewModel2.append(input.name, input.value);
    //});



    //table matrerail body
    _listViewceItemModifyRequest = [];
    rows.forEach((row, index) => {

        const itemNameInput = row.querySelector(".imItemName");
        const itemName = itemNameInput.value.trim();

        if (!itemName) {
            alert(`กรุณากรอกชื่อ Item (บรรทัดที่ ${index + 1})`);
            itemNameInput.focus(); // optional: focus ช่องนั้น
            throw new Error("Item name is required."); // ❌ หยุดการทำงาน (ถ้าใช้ใน loop ใหญ่)
        }

        _listViewceItemModifyRequest.push({
            imCENo: row.querySelector(".imCENo").value.trim() || (index + 1).toString(),
            imItemNo: index + 1, // ✅ เริ่มจาก 1
            imItemName: row.querySelector(".imItemName").value.trim(),
            imPCS: parseInt(row.querySelector(".imPCS").value),
            imAmount: parseFloat(row.querySelector(".imAmount").value)
        });


    });

    headerFormData.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));

    //1.check send mail
    //2.save form
    //3.send mail
    //4.save item
    $.ajax({
        type: "POST",
        url: action,
        data: headerFormData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            swal.fire({
                html: '<h5>Loading...</h5>',
                showConfirmButton: false,
                onRender: function () {
                    // there will only ever be one sweet alert open.
                    //$('.swal2-content').prepend(sweet_loader);
                }
            });
        },
        success: async function (config) {
            // alert(config.c1);
            if (config.c1 == "S") {
                await $("#myModalSendMold").modal("hide");
                var v_RonDoc = config.c3;
                var v_lotNo = config.c_lotNo;
                var v_RefNo = config.c_RefNo;
                var v_RevNo = config.c_RevNo;

                viewModel2.append("_runNo", v_RonDoc);
                viewModel2.append("_LotNo", v_lotNo);
                viewModel2.append("_RefNo", v_RefNo);
                viewModel2.append("_RevNo", v_RevNo);

                $.ajax({
                    type: "POST",
                    url: action2,//'@Url.Action("SaveCeItem","NewMoldModify")', //action,
                    data: viewModel2,
                    processData: false,
                    contentType: false,
                    beforeSend: function () {
                        swal.fire({
                            html: '<h5>Loading...</h5>',
                            showConfirmButton: false,
                            onRender: function () {
                                // there will only ever be one sweet alert open.
                                //$('.swal2-content').prepend(sweet_loader);
                            }
                        });
                    },
                    success: async function (config) {
                        swal.fire({
                            title: 'SUCCESS',
                            icon: 'success',
                            text: config.c2,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                //console.log("config.c3" + config.c3);
                                GoSideMenu("SearchMold");
                                //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                            }
                        });


                    },
                    error: function (xhr, status, err) {
                        Swal.close(); // <<== ให้ปิด Loading ตอน error
                        Swal.fire({
                            icon: "error",
                            title: "error",
                            text: "An error occurred while saving. Please try again.",
                        });
                    }
                });
            }
            else if (config.c1 == "E") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: config.c2,
                })
                    .then((result) => {
                        $("#myModalSendMold").modal("show");
                    });

            }
            else if (config.c1 == "P") {
                //$("#loaderDiv").hide();
                //await $("#myModal1").modal("hide");
                await $("#myModalSendMold").modal("hide");
                Swal.fire({
                    icon: 'warning',
                    title: 'warning',
                    text: config.c2,
                })
                    .then((result) => {
                        //$("#myModal1").modal("show");
                    });

            }

        }
    });

}
function Menubar_MoldsaveDraft(action, action2) {
    console.log("call Menubar Mold saveDraft");
    const rows = document.querySelectorAll("#tableBody tr"); //table id material body

    let table1 = document.getElementById('tbDetailMoldProcessDetail');
    let rows1 = table1.getElementsByTagName('tr');

    let vmsg = "";
    if (document.getElementById("i_NewMold_ReferenceNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Reference No !!!";
        document.getElementById("i_NewMold_ReferenceNo").focus();
    } else if (document.getElementById("i_NewMold_LotNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Lot No !!!";
        document.getElementById("i_NewMold_LotNo").focus();
    } else if (document.getElementById("i_NewMold_CustomerName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Customer Name !!!";
        document.getElementById("i_NewMold_CustomerName").focus();
    } else if (document.getElementById("i_NewMold_MoldName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Name !!!";
        document.getElementById("i_NewMold_MoldName").focus();
    } else if (document.getElementById("i_NewMold_ModelName").value == "") {
        vmsg = "กรุณากรอกข้อมูล Model Name !!!";
        document.getElementById("i_NewMold_ModelName").focus();
    } else if (document.getElementById("i_NewMold_CavityNo").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cavity No !!!";
        document.getElementById("i_NewMold_CavityNo").focus();
    } else if (document.getElementById("i_NewMold_Function").value == "") {
        vmsg = "กรุณากรอกข้อมูล Function !!!";
        document.getElementById("i_NewMold_Function").focus();
    } else if (document.getElementById("i_NewMold_RequestBy").value == "") {
        vmsg = "กรุณากรอกข้อมูล Request By !!!";
        document.getElementById("i_NewMold_RequestBy").focus();
    } else if (document.getElementById("i_NewMold_MoldMass").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Mass !!!";
        document.getElementById("i_NewMold_MoldMass").focus();
    } else if (document.getElementById("i_NewMold_TypeCavity").value == "") {
        vmsg = "กรุณากรอกข้อมูล Type Cavity !!!";
        document.getElementById("i_NewMold_TypeCavity").focus();
    } else if (document.getElementById("i_NewMold_LeadTime").value == "") {
        vmsg = "กรุณากรอกข้อมูล Lead Time !!!";
        document.getElementById("i_NewMold_LeadTime").focus();
    } else if (document.getElementById("i_NewMold_EstimateCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล Estimate Cost !!!";
        document.getElementById("i_NewMold_EstimateCost").focus();
    } else if (document.getElementById("i_NewMold_MKPriceCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล MK Price Cost !!!";
        document.getElementById("i_NewMold_MKPriceCost").focus();
    } else if (document.getElementById("i_NewMold_ResultCost").value == "") {
        vmsg = "กรุณากรอกข้อมูล Result Cost !!!";
        document.getElementById("i_NewMold_ResultCost").focus();
    } else if (document.getElementById("i_NewMold_IssueRate").value == "") {
        vmsg = "กรุณากรอกข้อมูล Issue Rate !!!";
        document.getElementById("i_NewMold_IssueRate").focus();
    } else if (document.getElementById("i_NewMold_CostRate").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cost Rate !!!";
        document.getElementById("i_NewMold_CostRate").focus();
    } else if (document.getElementById("i_NewMold_CostRateType").value == "") {
        vmsg = "กรุณากรอกข้อมูล Cost Rate Type !!!";
        document.getElementById("i_NewMold_CostRateType").focus();
    } else if (document.getElementById("i_NewMold_Type").value == "") {
        vmsg = "กรุณากรอกข้อมูล Mold Type !!!";
        document.getElementById("i_NewMold_Type").focus();
        //} else if (document.getElementById("i_NewMold_mfProcess").value == "") {
        //    vmsg = "กรุณากรอกข้อมูล MF Process !!!";
        //    document.getElementById("i_NewMold_mfProcess").focus();
    } else if (document.getElementById("i_NewMold_Detail").value == "") {
        vmsg = "กรุณากรอกข้อมูล Detail !!!";
        document.getElementById("i_NewMold_Detail").focus();
    }
    else if (rows1.length < 3) { // Check if there is more than just the header row
        msg = "กรุณากรอกข้อมูล Model Name ให้ถูกต้อง !!!";
        document.getElementById("i_NewMold_ModelName").focus();
        console.log("The table has more than 0 rows.");
    }




    if (vmsg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: vmsg,

        })
            .then((result) => {
            });
    }
    else {

        console.log("call Menubar Mold send Mail");
        const rows = document.querySelectorAll("#tableBody tr"); //table id material body

        const form1 = document.forms.namedItem("formMoldData1"); // form header
        const form2 = document.forms.namedItem("formMoldData2"); // form Process Detail.
        const form3 = document.forms.namedItem("formMoldData3"); // form Mat Detail, Summary ,email


        let viewModel1 = new FormData(form1);  // form header
        let viewModel2 = new FormData(form2);  // form Process Detail.
        let viewModel3 = new FormData(form3);  // form Mat Detail, Summary ,email



        $.each(form1, function (index, input) {
            viewModel1.append(input.name, input.value);
        });
        $.each(form2, function (index, input) {
            viewModel2.append(input.name, input.value);
        });
        $.each(form3, function (index, input) {
            viewModel3.append(input.name, input.value);
        });


        //form header form Mat Detail, Summary ,email
        // สร้าง FormData ใหม่เพื่อรวมทั้งสองอัน
        let headerFormData = new FormData();
        // ใส่ข้อมูลจาก viewModel1
        viewModel1.forEach((value, key) => {
            headerFormData.append(key, value);
        });
        // ใส่ข้อมูลจาก viewModel3
        viewModel3.forEach((value, key) => {
            headerFormData.append(key, value);
        });



        //tab Process
        //$.each(form2, function (index, input) {
        //    viewModel2.append(input.name, input.value);
        //});



        //table matrerail body
        _listViewceItemModifyRequest = [];
        rows.forEach((row, index) => {

            const itemNameInput = row.querySelector(".imItemName");
            const itemName = itemNameInput.value.trim();

            if (!itemName) {
                alert(`กรุณากรอกชื่อ Item (บรรทัดที่ ${index + 1})`);
                itemNameInput.focus(); // optional: focus ช่องนั้น
                throw new Error("Item name is required."); // ❌ หยุดการทำงาน (ถ้าใช้ใน loop ใหญ่)
            }

            _listViewceItemModifyRequest.push({
                imCENo: row.querySelector(".imCENo").value.trim() || (index + 1).toString(),
                imItemNo: index + 1, // ✅ เริ่มจาก 1
                imItemName: row.querySelector(".imItemName").value.trim(),
                imPCS: parseInt(row.querySelector(".imPCS").value),
                imAmount: parseFloat(row.querySelector(".imAmount").value)
            });


        });

        headerFormData.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));

        $.ajax({
            type: "POST",
            url: action, //action,
            data: headerFormData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                swal.fire({
                    html: '<h5>Loading...</h5>',
                    showConfirmButton: false,
                    onRender: function () {
                        // there will only ever be one sweet alert open.
                        //$('.swal2-content').prepend(sweet_loader);
                    }
                });
            },
            success: async function (config) {
                if (config.c1 == "S") {
                    //await $("#myModalSendMold").modal("hide");
                    var v_RonDoc = config.c3;
                    var v_lotNo = config.c_lotNo;
                    var v_RefNo = config.c_RefNo;
                    var v_RevNo = config.c_RevNo;

                    viewModel2.append("_runNo", v_RonDoc);
                    viewModel2.append("_LotNo", v_lotNo);
                    viewModel2.append("_RefNo", v_RefNo);
                    viewModel2.append("_RevNo", v_RevNo);

                    $.ajax({
                        type: "POST",
                        url: action2,//'@Url.Action("SaveCeItem","NewMoldModify")', //action,
                        data: viewModel2,
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            swal.fire({
                                html: '<h5>Loading...</h5>',
                                showConfirmButton: false,
                                onRender: function () {
                                    // there will only ever be one sweet alert open.
                                    //$('.swal2-content').prepend(sweet_loader);
                                }
                            });
                        },
                        success: async function (config) {
                            swal.fire({
                                title: 'SUCCESS',
                                icon: 'success',
                                text: config.c2,
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    //console.log("config.c3" + config.c3);
                                    GoSideMenu("SearchMold");
                                    //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                                }
                            });


                        },
                        error: function (xhr, status, err) {
                            Swal.close(); // <<== ให้ปิด Loading ตอน error
                            Swal.fire({
                                icon: "error",
                                title: "error",
                                text: "An error occurred while saving. Please try again.",
                            });
                            console.error("Error:", status, err, xhr.responseText);
                            console.log("Response Text:", xhr.responseText); // ดูข้อความจากเซิร์ฟเวอร์
                            console.log("FAILED", xhr.responseText);
                        }
                    });
                }
                else if (config.c1 == "E") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'error',
                        title: 'ERROR',
                        text: config.c2,
                    })
                        .then((result) => {
                            $("#myModal1").modal("show");
                        });






                }
                else if (config.c1 == "P") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'warning',
                        title: 'warning',
                        text: config.c2,
                    })
                        .then((result) => {

                            //$("#myModal1").modal("show");
                        });

                }

            }
            ,
            error: function (xhr, status, err) {
                Swal.close(); // <<== ให้ปิด Loading ตอน error
                Swal.fire({
                    icon: "error",
                    title: "error",
                    text: "An error occurred while saving. Please try again.",
                });
                //console.error("Error:", status, err, xhr.responseText);
                //console.log("Response Text:", xhr.responseText); // ดูข้อความจากเซิร์ฟเวอร์
                //console.log("FAILED", xhr.responseText);
            }
        });
    }





}

function Menubar_MoldsaveDraftBG(action, action2) {
    console.log("call Menubar Mold saveDraft");
    const rows = document.querySelectorAll("#tableBody tr"); //table id material body
    let vmsg = "";
    //if (document.getElementById("i_New_OrderNo").value == "") {
    //    vmsg = "กรุณากรอกข้อมูล Order No !!!";
    //}
    //else if (document.getElementById("i_New_LotNo").value == "") {
    //    vmsg = "กรุณากรอกข้อมูล Lot No !!!";
    //}


    if (vmsg != "") {
        swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: vmsg,

        })
            .then((result) => {
            });
    }
    else {
        const form1 = document.getElementById('formMoldData1');
        const form2 = document.getElementById('formMoldData2');
        const form3 = document.getElementById('formMoldRequest3');

        let viewModel1 = new FormData(form1);
        let viewModel2 = new FormData(form2);
        let viewModel3 = new FormData(form3);


        $.each(form1, function (index, input) {
            viewModel1.append(input.name, input.value);
        });
        $.each(form3, function (index, input) {
            viewModel3.append(input.name, input.value);
        });

        $.each(form2, function (index, input) {
            viewModel2.append(input.name, input.value);
        });

        // สร้าง FormData ใหม่เพื่อรวมทั้งสองอัน
        let combinedFormData = new FormData();
        // ใส่ข้อมูลจาก viewModel1
        viewModel1.forEach((value, key) => {
            combinedFormData.append(key, value);
        });

        // ใส่ข้อมูลจาก viewModel2
        viewModel2.forEach((value, key) => {
            combinedFormData.append(key, value);
        });





        // form1.appendChild(form2);  // ย้าย form2 เข้าไปใน form1


        let formData = document.forms.namedItem("formMoldData");
        let viewModel = new FormData(formData);

        let viewModelCeItem = new FormData();

        //table matrerail body
        _listViewceItemModifyRequest = [];
        rows.forEach((row, index) => {

            const itemNameInput = row.querySelector(".imItemName");
            const itemName = itemNameInput.value.trim();

            if (!itemName) {
                alert(`กรุณากรอกชื่อ Item (บรรทัดที่ ${index + 1})`);
                itemNameInput.focus(); // optional: focus ช่องนั้น
                throw new Error("Item name is required."); // ❌ หยุดการทำงาน (ถ้าใช้ใน loop ใหญ่)
            }

            _listViewceItemModifyRequest.push({
                imCENo: row.querySelector(".imCENo").value.trim() || (index + 1).toString(),
                imItemNo: index + 1, // ✅ เริ่มจาก 1
                imItemName: row.querySelector(".imItemName").value.trim(),
                imPCS: parseInt(row.querySelector(".imPCS").value),
                imAmount: parseFloat(row.querySelector(".imAmount").value)
            });


        });

        viewModel3.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));

        //$.each(formData, function (index, input) {
        //    viewModel.append(input.name, input.value);
        //});

        //_listViewceItemModifyRequest.forEach(item => {
        //    viewModel.append("_ceItemModifyRequest", JSON.stringify(item)); // ❌ ทำให้ key ซ้ำ
        //});

        // viewModel3.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));

        //console.log("รวมจำนวนตัวอักษร (หรือ byte ถ้าเป็นไฟล์):", totalLength);
        //_listViewceItemModifyRequest.forEach((item, index) => {
        //    viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imCENo", item.imCENo);
        //    viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imItemNo", parseInt(item.imItemNo));
        //    viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imItemName", item.imItemName);
        //    viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imPCS", parseFloat(item.imPCS));
        //    viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imAmount", parseFloat(item.imAmount));
        //});

        //viewModel.append("_listViewceItemModifyRequest[0].imCENo", "1");
        //viewModel.append("_listViewceItemModifyRequest[0].imItemNo",1);
        //viewModel.append("_listViewceItemModifyRequest[0].imItemName","1");
        //viewModel.append("_listViewceItemModifyRequest[0].imPCS",1);
        //viewModel.append("_listViewceItemModifyRequest[0].imAmount", 1);


        //let totalSize = 0;
        //for (let [key, value] of viewModel.entries()) {
        //    if (value instanceof File) {
        //        totalSize += value.size;
        //    } else {
        //        totalSize += new Blob([value]).size;
        //    }
        //}
        //console.log("Total payload size (bytes):", totalSize);


        //        let vjson = `
        //[
        //  {
        //    "imCENo": "1",
        //    "imItemNo": 1,
        //    "imItemName": "wewr",
        //    "imPCS": 0,
        //    "imAmount": 0
        //  },
        //  {
        //    "imCENo": "2",
        //    "imItemNo": 2,
        //    "imItemName": "wewr",
        //    "imPCS": 0,
        //    "imAmount": 0
        //  }
        //]`;
        //        let parsedJson = JSON.parse(vjson);
        //        viewModel.append("_vceItemModifyRequest", JSON.stringify(parsedJson));

        //JSON.stringify(_listViewceItemModifyRequest);
        //let serialized = serializeFormData(viewModel);
        ////console.log(serialized.length); // key1=value1&key2=value2...
        //console.log(serialized)


        //viewModelCeItem.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest)); 
        //let serializedc1 = serializeFormData(viewModelCeItem);
        //console.log(encodeURIComponent(serializedc1));

        // let urllink = action + "?_listItemModify =" + _listViewceItemModifyRequest;

        $.ajax({
            type: "POST",
            url: action, //action,
            data: viewModel,
            processData: false,
            contentType: false,
            beforeSend: function () {
                swal.fire({
                    html: '<h5>Loading...</h5>',
                    showConfirmButton: false,
                    onRender: function () {
                        // there will only ever be one sweet alert open.
                        //$('.swal2-content').prepend(sweet_loader);
                    }
                });
            },
            success: async function (config) {
                // alert(config.c1);
                if (config.c1 == "S" || config.c1 == "D") {
                    // $("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");

                    //swal.fire({
                    //    title: 'SUCCESS',
                    //    icon: 'success',
                    //    text: "Save data Already !!!!",
                    //}).then((result) => {
                    //    if (result.isConfirmed) {
                    //        //console.log("config.c3" + config.c3);
                    //        GoSideMenu("Search");

                    //        //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                    //    }
                    //});


                    let formDatamt = document.forms.namedItem("formMoldDataMat");
                    let viewModelmt = new FormData(formDatamt);

                    _listViewceItemModifyRequest.forEach((item, index) => {
                        viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imCENo", config.c3);
                        viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imItemNo", parseInt(item.imItemNo));
                        viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imItemName", item.imItemName);
                        viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imPCS", parseFloat(item.imPCS));
                        viewModelCeItem.append("_listViewceItemModifyRequest[" + index + "].imAmount", parseFloat(item.imAmount));
                    });

                    viewModelmt.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));

                    $.ajax({
                        type: "POST",
                        url: action2,//'@Url.Action("SaveCeItem","NewMoldModify")', //action,
                        data: viewModelmt,
                        processData: false,
                        contentType: false,
                        beforeSend: function () {
                            swal.fire({
                                html: '<h5>Loading...</h5>',
                                showConfirmButton: false,
                                onRender: function () {
                                    // there will only ever be one sweet alert open.
                                    //$('.swal2-content').prepend(sweet_loader);
                                }
                            });
                        },
                        success: async function (config) {
                            swal.fire({
                                title: 'SUCCESS',
                                icon: 'success',
                                text: "Save data Already !!!!",
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    //console.log("config.c3" + config.c3);
                                    GoSideMenu("SearchMold");
                                    //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
                                }
                            });


                        },
                        error: function (xhr, status, err) {
                            //console.error("Error:", status, err, xhr.responseText);
                            //console.log("Response Text:", xhr.responseText); // ดูข้อความจากเซิร์ฟเวอร์
                            console.log("FAILED", xhr.responseText);
                        }
                    });


                }
                else if (config.c1 == "E") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'error',
                        title: 'ERROR',
                        text: config.c2,
                    })
                        .then((result) => {
                            $("#myModal1").modal("show");
                        });






                }
                else if (config.c1 == "P") {
                    //$("#loaderDiv").hide();
                    //await $("#myModal1").modal("hide");
                    await $("#myModal1").modal("hide");
                    Swal.fire({
                        icon: 'warning',
                        title: 'warning',
                        text: config.c2,
                    })
                        .then((result) => {

                            //$("#myModal1").modal("show");
                        });

                }

            },
            error: function (xhr, status, err) {
                //console.error("Error:", status, err, xhr.responseText);
                //console.log("Response Text:", xhr.responseText); // ดูข้อความจากเซิร์ฟเวอร์
                console.log("FAILED", xhr.responseText);
            }
        });
    }





}
function Menubar_save_sendMailBG(action) {
    console.log("call Menubar Mold send Mail");
    const rows = document.querySelectorAll("#tableBody tr"); //table id material body
    var vmsg = ""
    const form1 = document.forms.namedItem("formMoldData1");
    const form2 = document.forms.namedItem("formMoldData2");
    const form3 = document.forms.namedItem("formMoldData3");
    const form4 = document.forms.namedItem("formMoldData4"); //email

    let viewModel1 = new FormData(form1);
    let viewModel2 = new FormData(form2);
    let viewModel3 = new FormData(form3);
    let viewModel4 = new FormData(form4); //email


    $.each(form1, function (index, input) {
        viewModel1.append(input.name, input.value);
    });
    $.each(form3, function (index, input) {
        viewModel3.append(input.name, input.value);
    });
    $.each(form4, function (index, input) {
        viewModel4.append(input.name, input.value);
    });


    // สร้าง FormData ใหม่เพื่อรวมทั้งสองอัน
    let combinedFormData = new FormData();
    // ใส่ข้อมูลจาก viewModel1
    viewModel1.forEach((value, key) => {
        combinedFormData.append(key, value);
    });
    // ใส่ข้อมูลจาก viewModel2
    viewModel3.forEach((value, key) => {
        combinedFormData.append(key, value);
    });
    //viewModel4.forEach((value, key) => {
    //    combinedFormData.append(key, value);
    //});



    //tab mat
    $.each(form2, function (index, input) {
        viewModel2.append(input.name, input.value);
    });



    //table matrerail body
    _listViewceItemModifyRequest = [];
    rows.forEach((row, index) => {

        const itemNameInput = row.querySelector(".imItemName");
        const itemName = itemNameInput.value.trim();

        if (!itemName) {
            alert(`กรุณากรอกชื่อ Item (บรรทัดที่ ${index + 1})`);
            itemNameInput.focus(); // optional: focus ช่องนั้น
            throw new Error("Item name is required."); // ❌ หยุดการทำงาน (ถ้าใช้ใน loop ใหญ่)
        }

        _listViewceItemModifyRequest.push({
            imCENo: row.querySelector(".imCENo").value.trim() || (index + 1).toString(),
            imItemNo: index + 1, // ✅ เริ่มจาก 1
            imItemName: row.querySelector(".imItemName").value.trim(),
            imPCS: parseInt(row.querySelector(".imPCS").value),
            imAmount: parseFloat(row.querySelector(".imAmount").value)
        });


    });

    viewModel2.append("_ceItemModifyRequest", JSON.stringify(_listViewceItemModifyRequest));




    // รวมข้อมูลจากทุก form


    function formDataToObjectFromFormData(formData) {
        const obj = {};
        for (let [key, value] of formData.entries()) {
            obj[key] = value;
        }
        return obj;
    }
    var JScombinedFormData = formDataToObjectFromFormData(combinedFormData);
    var JSceItemModifyRequest = formDataToObjectFromFormData(viewModel3);



    // สร้างออบเจกต์รวมให้ตรงกับ model ฝั่ง C#
    let payload = {
        _ViewceMastModifyRequest: JScombinedFormData, // ✅ ไม่ใช่ array
        _ViewceItemModifyRequest: JSceItemModifyRequest
    };
    console.log("Payload:", JSON.stringify(payload));

    // ส่งไป Controller แบบ JSON
    var url = action;
    fetch('/NewMoldModify/YourAction', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
            alert(data.message);
        })
        .catch(error => {
            console.error('Error:', error);
            alert('เกิดข้อผิดพลาด!');
        });





    //1.check send mail
    //2.save form
    //3.send mail
    //4.save item









    //$.ajax({
    //    type: "POST",
    //    url: action,
    //    data: combinedFormData,
    //    processData: false,
    //    contentType: false,
    //    beforeSend: function () {
    //        swal.fire({
    //            html: '<h5>Loading...</h5>',
    //            showConfirmButton: false,
    //            onRender: function () {
    //                // there will only ever be one sweet alert open.
    //                //$('.swal2-content').prepend(sweet_loader);
    //            }
    //        });
    //    },
    //    success: async function (config) {
    //        // alert(config.c1);
    //        if (config.c1 == "S") {
    //            await $("#myModalSendMold").modal("hide");
    //            var v_RonDoc = config.c3;
    //            viewModel2.append("_runNo", v_RonDoc);
    //            $.ajax({
    //                type: "POST",
    //                url: action2,//'@Url.Action("SaveCeItem","NewMoldModify")', //action,
    //                data: viewModel2,
    //                processData: false,
    //                contentType: false,
    //                beforeSend: function () {
    //                    swal.fire({
    //                        html: '<h5>Loading...</h5>',
    //                        showConfirmButton: false,
    //                        onRender: function () {
    //                            // there will only ever be one sweet alert open.
    //                            //$('.swal2-content').prepend(sweet_loader);
    //                        }
    //                    });
    //                },
    //                success: async function (config) {
    //                    swal.fire({
    //                        title: 'SUCCESS',
    //                        icon: 'success',
    //                        text: config.c2,
    //                    }).then((result) => {
    //                        if (result.isConfirmed) {
    //                            //console.log("config.c3" + config.c3);
    //                            GoSideMenu("SearchMold");
    //                            //GoNewRequest(getID, getEvent, vaction, vForm, vTeam, vSubject, vSrNo)
    //                        }
    //                    });


    //                },
    //                error: function (xhr, status, err) {
    //                    Swal.close(); // <<== ให้ปิด Loading ตอน error
    //                    Swal.fire({
    //                        icon: "error",
    //                        title: "error",
    //                        text: "An error occurred while saving. Please try again.",
    //                    });
    //                }
    //            });
    //        }
    //        else if (config.c1 == "E") {
    //            //$("#loaderDiv").hide();
    //            //await $("#myModal1").modal("hide");
    //            Swal.fire({
    //                icon: 'error',
    //                title: 'ERROR',
    //                text: config.c2,
    //            })
    //                .then((result) => {
    //                    $("#myModalSendMold").modal("show");
    //                });

    //        }
    //        else if (config.c1 == "P") {
    //            //$("#loaderDiv").hide();
    //            //await $("#myModal1").modal("hide");
    //            await $("#myModalSendMold").modal("hide");
    //            Swal.fire({
    //                icon: 'warning',
    //                title: 'warning',
    //                text: config.c2,
    //            })
    //                .then((result) => {
    //                    //$("#myModal1").modal("show");
    //                });

    //        }

    //    }
    //});

}
function serializeFormData(formData) {

    const params = new URLSearchParams();

    for (let [key, value] of formData.entries()) {
        if (value instanceof File) continue; // ไม่รวมไฟล์
        params.append(key, value);
    }

    return params.toString(); // เหมือน $(form).serialize()
}
function Menubar_AddMCostChRate(A,b,c) {
    console.log("eeeeeeeeeeeeee");
    $("#myModalMChRate").modal("show");
}
