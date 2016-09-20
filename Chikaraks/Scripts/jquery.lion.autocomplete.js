/// <reference path="jquery-2.2.2.js" />
var intervalVar = null;
var longAutoTextPressed = true;
var longPressedFirstTime = false;
var currentAutoTextIndex = 0;
var autoTextListLength = 0;
var autoTextFoundLastIndex = 0;
var autoTextFoundFirstIndex = 0;
var ListMainIndexChanged = false;
var tempAutoTextSelectedValue = "";
var tempAutoTextsearchedElementsList = Object();
var showAutoTextAllList = false;
$(".autoTextBox").css("display", "none");
$(document).on("keydown", ".autoText", function (e) {
    if (e.keyCode == 13) {
        e.preventDefault()
    }
    var thi$ = $(this);
    if ($(thi$).data("auto-text-type") != "autoText+Readonly") {

        var boxElement = $(thi$).parent().find(".autoTextBox");
        var textToSearch = $(thi$).val().toString().trim().toLowerCase();
        var elementsOfLists = $(boxElement).find("ul li");
        if (longAutoTextPressed == true) {
            longAutoTextPressed = false;
            //Delay first time for 1 second
            if (longPressedFirstTime == false) {
                intervalVar = setTimeout(function () {
                    longKeypress(e, boxElement, textToSearch, elementsOfLists, thi$)
                }, 1000);
            }
                //Call faster to move down
            else {
                intervalVar = setTimeout(function () {
                    longKeypress(e, boxElement, textToSearch, elementsOfLists, thi$)
                }, 100);
            }
            longPressedFirstTime = true;

        }
    }
});
function longKeypress(e, boxElement, textToSearch, elementsOfLists, thi$) {

    $(boxElement).find("ul li").removeClass("autoTextOptionActive");
    var autoTextboxDisplay = $(boxElement).css("display");
    //UP Key
    if (e.keyCode == 38 && autoTextboxDisplay == "block") {

        //If First up Key Pressed then set last as selected
        if (currentAutoTextIndex == autoTextFoundFirstIndex && ListMainIndexChanged == false) {
            currentAutoTextIndex = autoTextFoundLastIndex;
            $(boxElement).scrollTop($(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex).offset().top);
        }
            //if up arrow pressed and previously it was on first item then set to last selected
        else if (currentAutoTextIndex == autoTextFoundFirstIndex) {
            currentAutoTextIndex = autoTextFoundLastIndex + 1;
            $(boxElement).scrollTop($(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex - 1).offset().top);
        }
        //if more then 1 time key pressed then do minus or if first time  key pressed then ignore index
        if (ListMainIndexChanged == true) {
            currentAutoTextIndex--;
        }
        //Set Current seletion
        var currentElementSelected = $(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex)
        $(currentElementSelected).addClass('autoTextOptionActive');
        $(thi$).val($(currentElementSelected).data("display-value"));
        tempAutoTextSelectedValue = $(currentElementSelected).data("value");
        //Set Scroll Position
        if ($(currentElementSelected).offset().top < $(boxElement).offset().top && $(boxElement).scrollTop() > 1) {

            $(boxElement).scrollTop($(boxElement).scrollTop() - 50);
        }
        else if (currentAutoTextIndex == autoTextFoundFirstIndex) {
            $(boxElement).scrollTop(0);
        }
        //set pressed one time as yes
        ListMainIndexChanged = true;
    }
        //Down Key
    else if (e.keyCode == 40 && autoTextboxDisplay == "block") {
        //if only one element and selection is same as last index
        if (currentAutoTextIndex == autoTextFoundLastIndex && autoTextListLength == 1) {
            currentAutoTextIndex = autoTextFoundFirstIndex;
            $(boxElement).scrollTop(0);
        }
            //if previous selection was last item then move to first index
        else if (currentAutoTextIndex == autoTextFoundLastIndex) {
            currentAutoTextIndex = autoTextFoundFirstIndex - 1;
            $(boxElement).scrollTop(0);
        }
        //increament only if second time pressed
        if (ListMainIndexChanged == true) {
            currentAutoTextIndex++;
        }
        //Set selection
        var currentElementSelected = $(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex);
        $(currentElementSelected).addClass('autoTextOptionActive');
        $(thi$).val($(currentElementSelected).data("display-value"));
        tempAutoTextSelectedValue = $(currentElementSelected).data("value");
        //Set Scroll
        if ($(currentElementSelected).offset().top > $(boxElement).offset().top + 390) {

            $(boxElement).scrollTop($(boxElement).scrollTop() + 50);
        }
        //set pressed one time as yes
        ListMainIndexChanged = true;
    }
    longAutoTextPressed = true;
}
$(document).on("keyup", ".autoText", function (e) {
    e.preventDefault()
    var thi$ = $(this);

    var boxElement = $(thi$).parent().find(".autoTextBox");
    var textToSearch = $(thi$).val().toString().trim().toLowerCase();
    var elementsOfLists = $(boxElement).find("ul li");
    var autoTextboxDisplay = $(boxElement).css("display");
    //Reset Long pressed
    longAutoTextPressed = true;
    longPressedFirstTime = false;
    $(elementsOfLists).removeClass("autoTextOptionActive")
    clearTimeout(intervalVar);

    if (textToSearch != "" && textToSearch != undefined && textToSearch != null) {

        //UP Key
        if (e.keyCode == 38 && autoTextboxDisplay == "block") {
            if ($(thi$).data("auto-text-type") != "autoText+Readonly") {
                //If First up Key Pressed then set last as selected
                if (currentAutoTextIndex == autoTextFoundFirstIndex && ListMainIndexChanged == false) {
                    currentAutoTextIndex = autoTextFoundLastIndex;
                    $(boxElement).scrollTop($(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex).offset().top);
                }
                    //if up arrow pressed and previously it was on first item then set to last selected
                else if (currentAutoTextIndex == autoTextFoundFirstIndex) {
                    currentAutoTextIndex = autoTextFoundLastIndex + 1;
                    $(boxElement).scrollTop($(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex - 1).offset().top);
                }
                //if more then 1 time key pressed then do minus or if first time  key pressed then ignore index
                if (ListMainIndexChanged == true) {
                    currentAutoTextIndex--;
                }
                //Set Current seletion
                var currentElementSelected = $(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex)
                $(currentElementSelected).addClass('autoTextOptionActive');
                $(thi$).val($(currentElementSelected).data("display-value"));
                tempAutoTextSelectedValue = $(currentElementSelected).data("value");
                //Set Scroll Position
                if ($(currentElementSelected).offset().top < $(boxElement).offset().top && $(boxElement).scrollTop() > 1) {

                    $(boxElement).scrollTop($(boxElement).scrollTop() - 50);
                }
                else if (currentAutoTextIndex == autoTextFoundFirstIndex) {
                    $(boxElement).scrollTop(0);
                }
                //set pressed one time as yes
                ListMainIndexChanged = true;
            }
        }
            //Down Key
        else if (e.keyCode == 40 && autoTextboxDisplay == "block") {
            if ($(thi$).data("auto-text-type") != "autoText+Readonly") {
                //if only one element and selection is same as last index
                if (currentAutoTextIndex == autoTextFoundLastIndex && autoTextListLength == 1) {
                    currentAutoTextIndex = autoTextFoundFirstIndex;
                    $(boxElement).scrollTop(0);
                }
                    //if previous selection was last item then move to first index
                else if (currentAutoTextIndex == autoTextFoundLastIndex) {
                    currentAutoTextIndex = autoTextFoundFirstIndex - 1;
                    $(boxElement).scrollTop(0);
                }
                //increament only if second time pressed
                if (ListMainIndexChanged == true) {
                    currentAutoTextIndex++;
                }
                //Set selection
                var currentElementSelected = $(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex);
                $(currentElementSelected).addClass('autoTextOptionActive');
                $(thi$).val($(currentElementSelected).data("display-value"));
                tempAutoTextSelectedValue = $(currentElementSelected).data("value");
                //Set Scroll
                if ($(currentElementSelected).offset().top > $(boxElement).offset().top + 390) {

                    $(boxElement).scrollTop($(boxElement).scrollTop() + 50);
                }
                //set pressed one time as yes
                ListMainIndexChanged = true;
            }
        }
            //Press Enter for selection
        else if (e.keyCode == 13 && textToSearch != "" && textToSearch != undefined && textToSearch != null && autoTextboxDisplay == "block") {
            if ($(thi$).data("auto-text-type") != "autoText+Readonly") {
                $(boxElement).css("display", "none");
                $(thi$).val($(thi$).val());
                var currentElementSelected = $(tempAutoTextsearchedElementsList).eq(currentAutoTextIndex);
                tempAutoTextSelectedValue = $(currentElementSelected).data("value");
              //  $("#" + $(thi$).data("targetid")).val(tempAutoTextSelectedValue);
                $(thi$).parent().find("#" + $(thi$).data("targetid")).val(tempAutoTextSelectedValue);
            }
        }
            //If other key pressed
        else if (textToSearch != "?" && textToSearch != "" && textToSearch != undefined && textToSearch != null && e.keyCode != 40 && e.keyCode != 38 && e.keyCode != 13 && e.keyCode != 37 && e.keyCode != 39) {
            //set as no key pressed
            ListMainIndexChanged = false;
            //set scroll to top
            $(boxElement).scrollTop(0);
            var hidelisttargetid = $(thi$).data("hidelisttargetid");
            var searchedElement = Object();

            if ($(thi$).data("auto-text-type") == "autoText+Readonly") {
                if (hidelisttargetid != "" && hidelisttargetid != undefined && hidelisttargetid != null) {
                    searchedElement = $(elementsOfLists).parent().find("[data-search^=\"" + $("#" + hidelisttargetid).val() + "|" + textToSearch + "\"]");
                }
                else {
                    searchedElement = $(elementsOfLists).parent().find("[data-search*=\"" + textToSearch + "\"]");
                }

            }
            else {
                if (hidelisttargetid != "" && hidelisttargetid != undefined && hidelisttargetid != null) {
                    searchedElement = $(elementsOfLists).parent().find("[data-search^=\"" + $("#" + hidelisttargetid).val() + "|" + textToSearch + "\"]");
                }
                else {
                    searchedElement = $(elementsOfLists).parent().find("[data-search^=\"" + textToSearch + "\"]");
                }
            }
            tempAutoTextsearchedElementsList = $(searchedElement);
            currentAutoTextIndex = 0;
            autoTextFoundFirstIndex = currentAutoTextIndex;
            autoTextFoundLastIndex = $(searchedElement).length - 1;
            autoTextListLength = searchedElement.length;
            $(elementsOfLists).css("display", "none");
            if ($(searchedElement).length > 0) {
                $(boxElement).css("display", "block");
                $(searchedElement).css("display", "block")
            }
            else if (textToSearch == "") {
                $(boxElement).css("display", "none");
                $(elementsOfLists).css("display", "none");
                currentAutoTextIndex = 0;
                autoTextFoundFirstIndex = 0;
            }


        } else if (textToSearch == "?") {


            $(boxElement).scrollTop(0);
            var hidelisttargetid = $(thi$).data("hidelisttargetid");
            var searchedElement = Object();
            if (hidelisttargetid != "" && hidelisttargetid != undefined && hidelisttargetid != null) {
                if ($(thi$).data("auto-text-type") == "autoText+Readonly") {
                    if (hidelisttargetid != "" && hidelisttargetid != undefined && hidelisttargetid != null) {
                        searchedElement = $(elementsOfLists).parent().find("[data-search^=\"" + $("#" + hidelisttargetid).val() + "\"]");
                    }
                }
                else {
                    if (hidelisttargetid != "" && hidelisttargetid != undefined && hidelisttargetid != null) {
                        searchedElement = $(elementsOfLists).parent().find("[data-search^=\"" + $("#" + hidelisttargetid).val() + "\"]");
                    }
                }

                tempAutoTextsearchedElementsList = $(searchedElement);
                currentAutoTextIndex = 0;
                autoTextFoundFirstIndex = currentAutoTextIndex;
                autoTextFoundLastIndex = $(searchedElement).length - 1;
                autoTextListLength = searchedElement.length;
                $(elementsOfLists).css("display", "none");
                if ($(searchedElement).length > 0) {
                    $(boxElement).css("display", "block");
                    $(searchedElement).css("display", "block")
                }
                else if (textToSearch == "") {
                    $(boxElement).css("display", "none");
                    $(elementsOfLists).css("display", "none");
                    currentAutoTextIndex = 0;
                    autoTextFoundFirstIndex = 0;
                }

            }
            else {
                $(boxElement).css("display", "block");
                $(elementsOfLists).css("display", "block");


                currentAutoTextIndex = 0;
                autoTextFoundFirstIndex = 0;
                autoTextFoundLastIndex = $(elementsOfLists).length - 1;
                autoTextListLength = autoTextFoundLastIndex;
                tempAutoTextsearchedElementsList = $(elementsOfLists);
            }
        }
        else {
            $(boxElement).css("display", "none");
        }


    }
    else {

        $(boxElement).css("display", "none");
        $(elementsOfLists).css("display", "none");
        currentAutoTextIndex = 0;
        autoTextFoundFirstIndex = 0;
    }


});
$(document).on("click", ".autoTextOption", function (e) {
    
    var thi$ = $(this);
    var autoText = $(thi$).parent().parent().parent().find("input[type='text']").eq(0);
    if ($(autoText).data("auto-text-type") != "autoText+Readonly") {
        var boxElement = $(autoText).parent().find(".autoTextBox");
        $(boxElement).css("display", "none");
        $(boxElement).find("ul li").removeClass('autoTextOptionActive');
        tempAutoTextSelectedValue = $(thi$).data("value");
        $(autoText).parent().find("#" + $(autoText).data("targetid")).val(tempAutoTextSelectedValue);
        $(autoText).val($(thi$).data("display-value"));
        var elementSelected = $(boxElement).find("ul li[data-value='" + tempAutoTextSelectedValue + "']");
        if ($(elementSelected).length > 0) {
            $(elementSelected).addClass('autoTextOptionActive');
        }
    }
    $(".autoTextBox ul li").css("display", "none");
    $(".autoTextBox").css("display", "none");

});
$(document).click(function (e) {
    if (showAutoTextAllList == false) {
        $(".autoTextBox ul li").css("display", "none");
        $(".autoTextBox").css("display", "none");
    }
});
$(document).on("click", ".autoTextButtonShowAll", function (e) {

    var thi$ = $(this);
    var boxElement = $(thi$).parent().find(".autoTextBox");
    var autoText = $(thi$).parent().find("input[type='text']").eq(0);
    $(boxElement).find("ul li").removeClass('autoTextOptionActive');
    tempAutoTextSelectedValue = $("#" + $(autoText).data("targetid")).val();
    var elementSelected = $(boxElement).find("ul li[data-value='" + $("#" + $(autoText).data("targetid")).val() + "']");
    currentAutoTextIndex = $(boxElement).find("ul li[data-value='" + $("#" + $(autoText).data("targetid")).val() + "']").index();
    autoTextFoundFirstIndex = 0;
    autoTextFoundLastIndex = $(boxElement).find("ul li").length - 1;
    tempAutoTextsearchedElementsList = $(boxElement).find("ul li");
    if ($(boxElement).css("display") == "none") {
        $(boxElement).css("display", "block");
        $(boxElement).find("ul li").css("display", "block");
        if ($(elementSelected).length > 0) {
            if (($(elementSelected).offset().top - $(boxElement).scrollTop()) < 0) {
                $(boxElement).scrollTop(($(elementSelected).offset().top - $(boxElement).scrollTop() * -1) - 1200)
            }
            else {
                $(boxElement).scrollTop($(elementSelected).offset().top - $(boxElement).scrollTop() - 1200);
            }
        }
        showAutoTextAllList = true;
    }
    else {
        $(boxElement).css("display", "none");
        $(boxElement).find("ul li").css("display", "none");
        showAutoTextAllList = false;
    }

    if ($(elementSelected).length > 0) {
        $(elementSelected).addClass('autoTextOptionActive');
    }


});
$(document).on("blur", ".autoText", function (e) {

    var thi$ = $(this);
    var value =$(thi$).parent().find("#" + $(thi$).data("targetid")).val();
    var boxElement = $(thi$).parent().find(".autoTextBox");
    if ($(boxElement).css("display") == "none") {
        if ($(thi$).data("auto-text-type") == "autoText+Restore") {

            $(boxElement).find("ul li").removeClass('autoTextOptionActive');
            tempAutoTextSelectedValue = value;
            var elementSelected = $(boxElement).find("ul li[data-value='" + value + "']");
            if ($(elementSelected).length > 0) {
                $(elementSelected).addClass('autoTextOptionActive');
            }
            $(thi$).val($(elementSelected).data("display-value"));
            $(boxElement).css("display", "none");
            $(boxElement).find("ul li").css("display", "none");
        } else if ($(thi$).data("auto-text-type") == "autoText+Readonly") {
            //$(boxElement).css("display", "none");
            //$(boxElement).find("ul li").css("display", "none");
            showAutoTextAllList = false;
            $("#" + $(thi$).data("targetid")).val($(thi$).val())
        }
    }

});
function lionInit()
{
   
    $.each($(".autoText"), function (index, element) {
        
        var thi$ = $(element);
        if ($(thi$).data("auto-text-type") != "autoText+Readonly") {

            var value =$(thi$).parent().find("#" + $(thi$).data("targetid")).val();
            var boxElement = $(thi$).parent().find(".autoTextBox");
            $(boxElement).find("ul li").removeClass('autoTextOptionActive');
            tempAutoTextSelectedValue = value;
            var elementSelected = $(boxElement).find("ul li[data-value='" + value + "']");
            if ($(elementSelected).length > 0) {
                $(elementSelected).addClass('autoTextOptionActive');
            }
            $(thi$).val($(elementSelected).data("display-value"));
        }
        else if ($(thi$).data("auto-text-type") == "autoText+Readonly") {

            var value = $(thi$).parent().find("#" + $(thi$).data("targetid")).val();
            var boxElement = $(thi$).parent().find(".autoTextBox");
            $(boxElement).find("ul li").removeClass('autoTextOptionActive');
            tempAutoTextSelectedValue = value;
            var elementSelected = $(boxElement).find("ul li[data-value='" + value + "']");
            if ($(elementSelected).length > 0) {
                $(elementSelected).addClass('autoTextOptionActive');
            }
            $(thi$).val($(elementSelected).data("display-value"));
        }
    });
    if (showAutoTextAllList == false) {
        $(".autoTextBox ul li").css("display", "none");
        $(".autoTextBox").css("display", "none");
    }
}
$(document).ready(function () {

    lionInit();

});