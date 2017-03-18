// JQuery 3.1.1
// @codekit-prepend "../js-plugins/jquery-3.1.1.js"

// http://stackoverflow.com/questions/920236/how-can-i-detect-if-a-selector-returns-null
$.fn.exists = function () {
    return this.length !== 0;
}

// Bootstrap 3.3.7 JS
// @codekit-append "../js-plugins/affix.js"
// @codekit-append "../js-plugins/alert.js"
// @codekit-append "../js-plugins/button.js"
// @codekit-append "../js-plugins/carousel.js"
// @codekit-append "../js-plugins/collapse.js"
// @codekit-append "../js-plugins/dropdown.js"
// @codekit-append "../js-plugins/modal.js"
// @codekit-append "../js-plugins/tooltip.js"
// @codekit-append "../js-plugins/popover.js"
// @codekit-append "../js-plugins/scrollspy.js"
// @codekit-append "../js-plugins/tab.js"
// @codekit-append "../js-plugins/transition.js"

// Bootstrap-datetimepicker
// @codekit-append "../js-plugins/moment-with-locales.js"
// @codekit-append "../js-plugins/bootstrap-datetimepicker.js"

// Django-ajax
// @codekit-append "../js-plugins/jquery.ajax.js"


function toggleModalContent(display, htmlContent) {
    if (display) {
        if (htmlContent == "") {
            htmlContent = "<p>Nothing to see here</p>";
        }
        $('#modal-content').html(htmlContent);
        $('#modal-progressbar').delay(1000).slideUp('slow');
        $('#modal-content').delay(1000).slideDown('slow');
        // Reload the Datetime Picker for forms that use it - from inline code in wp4/templates/bootstrap3/layout/datetimefield.html
        load_datetime_for_id_date_recorded();
    } else {
        $('#modal-content').hide();
        $('#modal-action-button').text("Unset").toggleClass("hidden", true);
        $('#modal-progressbar').show();
    }
}

function getBaseURL() {
    // Return the protocol, host, and first level of path (i.e. locale)
    var newURL = window.location.protocol + "//" + window.location.host + "/";
    var pathArray = window.location.pathname.split("/");
    //console.log("DEBUG: newURL=" + newURL);
    //console.log("DEBUG: pathArray=" + pathArray);
    return newURL + pathArray[1] + "/";
}

// Methods for the ForeignKeyModal functionality I've added to CrispyForms
function openForeignKeyModalForEdit(keyName) {
    var dbValue = $("#" + keyName + " :selected").val();
    return openForeignKeyModal(keyName, dbValue);
}

function openForeignKeyModalForSearch(keyName) {
    return openForeignKeyModal(keyName, 0);
}

function openForeignKeyModal(keyName, dbValue) {
    // Open the modal, load the relevant url, display the relevant status
    // Presumes existence of "#myModal" in page

    var ajaxURL = getBaseURL();
    var ajaxDATA = {}
    var keyNameSplit = keyName.split("-");
    console.log("DEBUG: openForeignKeyModal(): keyName="+keyName+". dbValue="+dbValue);
    if (keyName == "id_donor-transplant_coordinator") {
        //console.log("DEBUG: loading modal for transplant co-ord");
        ajaxURL += (dbValue < 1 ? "wp4/staff/" : "wp4/staff/" + dbValue + "/");
        ajaxDATA = {"pk": dbValue, "return_id": keyName}  // "q": 2, TRANSPLANT_COORDINATOR
    } else if (keyNameSplit[0] == "id_allocation" && keyNameSplit[2] == "theatre_contact") {
        //console.log("DEBUG: loading modal for theatre contact");
        ajaxURL += (dbValue < 1 ? "wp4/staff/" : "wp4/staff/" + dbValue + "/");
        ajaxDATA = {"pk": dbValue, "return_id": keyName}  // "q": 15, THEATRE_CONTACT
    } else if (keyName == "id_contact") {
        //console.log("DEBUG: loading modal for adverse event contact");
        ajaxURL += (dbValue < 1 ? "wp4/staff/" : "wp4/staff/" + dbValue + "/");
        ajaxDATA = {"pk": dbValue, "q": 8, "return_id": keyName}  // LOCAL_INVESTIGATOR
    } else if (keyName == "id_quality_of_life") {
        // console.log("DEBUG: loading modal for follow up qol record");
        ajaxURL += (dbValue < 1 ? "wp4/health-economics/add" : "wp4/health-economics/" + dbValue + "/");
        ajaxDATA = {"pk": dbValue, "return_id": keyName}
    } else {
        alert("ERROR: Unknown id for the search request (" + keyName + ") \n\n" +
            "Please let the admin team know you've seen this error.");
        return false;
    }
    //console.log("DEBUG: openForeignKeyModal() calling url: " + ajaxURL);
    var ajaxSUCCESS = ajaxSUCCESS = function (returnHTML) {
        //console.log("DEBUG: openForeignKeyModal() returnHTML=" + returnHTML);
        $('#myModal').modal('show');
        toggleModalContent(true, returnHTML);
    };

    $.ajax({
        url: ajaxURL,
        type: "GET",  // This is comply with CBV premise that GET loads forms
        data: ajaxDATA,
        success: ajaxSUCCESS,
        error: function (xhr, errmsg, err) {
            // Show an error
            alert("ERROR: Problem when communicating with server.\n\n" +
                "Error message: (" + xhr.status + ") " + errmsg + "\n\n" +
                "Please let the admin team know you've seen this error.");
            console.log(xhr.status + ": " + xhr.responseText); // provide a bit more info about the error to the console
            $('#myModal').modal('hide');
        }
    });
    return true;
}

function changeForeignKeyModalToAdd(keyName) {
    var ajaxURL = getBaseURL();
    var ajaxDATA = {"return_id": keyName};
    var ajaxSUCCESS = function () {
    };
    var keyNameSplit = keyName.split("-");
    if (keyName == "id_donor-transplant_coordinator") {
        //console.log("DEBUG: loading modal for transplant co-ord");
        ajaxURL += "wp4/staff/add";
        ajaxSUCCESS = function (returnHTML) {
            $('#myModal').modal('show');
            toggleModalContent(true, returnHTML);
            $("#person_form").find("#id_jobs").val("[2]");  // Add the default job role
        };
    } else if (keyNameSplit[0] == "id_allocation" && keyNameSplit[2] == "theatre_contact") {
        //console.log("DEBUG: loading modal for transplant hospital");
        ajaxURL += "wp4/staff/add";
        ajaxSUCCESS = function (returnHTML) {
            $('#myModal').modal('show');
            toggleModalContent(true, returnHTML);
            $("#person_form").find("#id_jobs").val("[15]");  // Add the default job role
        };
    } else if (keyName == "id_donor-retrieval_hospital" ||
        (keyNameSplit[0] == "id_allocation" && keyNameSplit[2] == "transplant_hospital")) {
        //console.log("DEBUG: loading modal for retrieval hospital");
        ajaxURL += "wp4/location/add/";
        ajaxSUCCESS = function (returnHTML) {
            $('#myModal').modal('show');
            toggleModalContent(true, returnHTML);
        };
    } else {
        alert("ERROR: Unknown id for the search request (" + keyName + ") \n\n" +
            "Please let the admin team know you've seen this error.");
        return false;
    }
    //console.log("DEBUG: changeForeignKeyModalToAdd() calling url: " + ajaxURL);

    $.ajax({
        url: ajaxURL,
        type: "GET",  // This is comply with CBV premise that GET loads forms
        data: ajaxDATA,
        success: ajaxSUCCESS,
        error: function (xhr, errmsg, err) {
            // Show an error
            alert("ERROR: Problem when communicating with server.\n\n" +
                "Error message: (" + xhr.status + ") " + errmsg + "\n\n" +
                "Please let the admin team know you've seen this error.");
            console.log(xhr.status + ": " + xhr.responseText); // provide a bit more info about the error to the console
            $('#myModal').modal('hide');
        }
    });
}

function selectAndCloseForeignKeyModal(keyName, inputValue, inputText) {
    returnFromForeignKeyModal(keyName, inputValue, inputText);
    $('#myModal').modal('hide');
}

function returnFromForeignKeyModal(keyName, inputValue, inputText) {
    //console.log("DEBUG: returnFromForeignKeyModal " + '#' + keyName + '-display with ' + inputText);
    $('#' + keyName + '-display').val(inputText);
    //console.log("DEBUG: returnFromForeignKeyModal " + '#' + keyName + ' with ' + inputValue);
    // Hack, add the key regardless of if it already exists
    $('#' + keyName).append($("<option></option>").attr("value",inputValue).text(inputText));
    // Now select it
    $('#' + keyName).val(inputValue);
    $('#' + keyName + '-edit').toggleClass("hidden", false);
}

// Initialise common functions
document.addEventListener("DOMContentLoaded", function (event) {
    // Set initial states
});
