
/************************ Jquery Validation Code is starts here**************************/

(function ($, W, D) {
    var JQUERY4LVITS = {};

    JQUERY4LVITS.UTIL =
    {
        setupFormValidation: function () {
            //form validation rules
            $("#idCreateCountryForm").validate({
                rules: {
                    title: {
                        required: true,
                        maxlength: '45'
                    },

                    isActive: {
                        required: true,
                    }


                },
                messages:
                    {
                        title:
                       {
                           required: "Please enter a Country Title.",
                           maxlength: "Max Length 45"
                       },

                        isActive:
                        {
                            required: "Please select record status."
                        }
                    }
            });
        }
    }

    //when the dom has loaded setup form validation rules
    $(D).ready(function ($) {
        JQUERY4LVITS.UTIL.setupFormValidation();
    });

})(jQuery, window, document);





/************************ AngularJs Code for Insert Update Delete Starts here**************************/

var uid = -1;
var dataAngularObject = loadAngularObject();
var app = angular.module('countryCreateAppAng', ['ui']);

app.controller('countryCreateControllerAng', function ($scope, $http) {

    var idupdateCountry;

    // Following is for load data from view models to angularjs scope variables and list

    $scope.country = dataAngularObject.country;
    $scope.countryLists = dataAngularObject.countryLists;
    $scope.InsertCountryLists = dataAngularObject.InsertCountryLists;
    $scope.UpdateCountryLists = dataAngularObject.UpdateCountryLists;
    $scope.DeleteCountryLists = dataAngularObject.DeleteCountryLists;



    // Following is for set bydefault disable buttons , textboxes etc controls when page is first time load
    $scope.buttonNew = false;
    $scope.buttonInsert = true;
    $scope.buttonUpdate = true;
    $scope.buttonDelete = true;
    $scope.buttonCancel = false;
    $scope.AddNewPortion = true;
    $scope.buttonSubmitSave = true;
    $scope.buttonDeleteAll = true;

   

    $scope.sortType = 'title'; // set the default sort type
    $scope.sortReverse = false;  // set the default sort order
    $scope.search = '';     // set the default search/filter term

    /* Following is for load coutry list in the search autocomplete text box*/
    $(function () {
        var data = loadAngularObject();
        var country = [];
        for (var i = 0; i < data.countryLists.length; i++) {
            country[i] = data.countryLists[i].title;

        }

        $("#txtSearch").autocomplete({
            source: country
        });

    });




    /*Following is for button code like new,add,update,delete etc when these buttons are clicked than some other buttons or controls gets disabled*/

    $scope.NewEntry = function () {
        $scope.buttonNew = true;
        $scope.buttonInsert = false;
        $scope.buttonUpdate = true;
        $scope.buttonDelete = true;
        $scope.buttonCancel = false;
        $scope.AddNewPortion = false;


        $scope.country = {};
        $scope.country.isActive = true;


    }
    $scope.CancelEntry = function () {
        $scope.buttonNew = false;
        $scope.buttonInsert = true;
        $scope.buttonUpdate = true;
        $scope.buttonDelete = true;
        $scope.buttonCancel = false;
        $scope.AddNewPortion = true;
        $scope.country = {};
        $scope.country.isActive = true;

        $scope.countryLists = dataAngularObject.countryLists;

    }

    $scope.CancelSearch = function () {

        document.getElementById('txtSearch').value = null;

        $scope.countryLists = dataAngularObject.countryLists;

    }


    /********************************Following is for insert new record data in grid******************************/
    $scope.AddInGrid = function () {

        // Following is for checking duplicate record using coutnry title
        var flag = true;

        for (var i = 0; i < $scope.countryLists.length; i++) {

            if (angular.lowercase($scope.country.title) == angular.lowercase($scope.countryLists[i].title)) {
                flag = false;

            }

        }


        //if this is new country, add it in contacts array
        if ($("#idCreateCountryForm").valid()) {

            if (flag == true) {

                $scope.country.idcountry = uid--;
                $scope.countryLists.push($scope.country);
                $scope.InsertCountryLists.push($scope.country);

                //clear the add country form
                $scope.buttonNew = false;
                $scope.buttonInsert = true;
                $scope.buttonUpdate = true;
                $scope.buttonDelete = true;
                $scope.buttonCancel = false;
                $scope.AddNewPortion = true;
                $scope.buttonSubmitSave = false;
                $scope.country = {};
                $scope.country.isActive = true;


                // Following is for Display Country Grid Section Div

                $("#CountryGridDiv").removeClass("class : DivHide");

                // Following code is for check to Country title is empty or not if it is not empty than page goes to scroll down
                if ($scope.txtCountryTitle != "undefined") {
                    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
                }


            }
            else {


                alertDialog.init('Duplicate Country Found');
                $scope.country = {};
                $scope.country.isActive = true;

            }
        }
    }


    /*******************Following is for load data into form controls like textboxes , radio buttons etc ****************/

    $scope.edit = function (idcountry) {

        // Following is to set idcountry for idupdate global variable than this variable is used in $scope.UpdateInGrid for check duplicate entry by country title
        idupdateCountry = idcountry;

        //search country with given id and update it
        for (i in $scope.countryLists) {
            if ($scope.countryLists[i].idcountry == idcountry) {
                //we use angular.copy() method to create 
                //copy of original object
                $scope.country = angular.copy($scope.countryLists[i]);
            }
        }
        //clear the add country form
        $scope.buttonNew = true;
        $scope.buttonInsert = true;
        $scope.buttonUpdate = false;
        $scope.buttonDelete = true;
        $scope.buttonCancel = false;
        $scope.AddNewPortion = false;

        // Following is for Display Insert Country Section Div

        $("#countryTitle").parent().addClass("active");
        $("#countryTitle").parent().find("label").css("top", "-16px");
        $("#countryTitle").parent().find("label").css("font-size", "14px");


    }



    /*******************Following is for update record in grid table ****************/

    $scope.UpdateInGrid = function () {


        // Following is for Checking Duplicate Country 
        var flag = true;
        var flag2 = true;
        for (var i = 0; i < $scope.countryLists.length; i++) {
            if ($scope.country.idcountry == $scope.countryLists[i].idcountry && angular.lowercase($scope.country.title) != angular.lowercase($scope.countryLists[i].title)) {
                continue;
            }
            else if ($scope.country.idcountry == $scope.countryLists[i].idcountry && angular.lowercase($scope.country.title) == angular.lowercase($scope.countryLists[i].title) && angular.lowercase($scope.country.isActive) == angular.lowercase($scope.countryLists[i].isActive)) {
                flag2 = false;
            }
            else if ($scope.country.idcountry != $scope.countryLists[i].idcountry && angular.lowercase($scope.country.title) == angular.lowercase($scope.countryLists[i].title)) {
                flag = false;
            }

        }


        //for existing country, find this country using id
        //and update it.
        if (flag == true) {
            if (flag2 == true) {
                if ($("#idCreateCountryForm").valid()) {


                    for (var i = 0; i < $scope.countryLists.length; i++) {
                        if ($scope.countryLists[i].idcountry == $scope.country.idcountry) {
                            // Following is for update record in table
                            $scope.countryLists[i] = $scope.country;

                            if ($scope.country.idcountry < 0) {
                                for (var j = 0; j < $scope.InsertCountryLists.length; j++) {
                                    if ($scope.InsertCountryLists[j].idcountry == $scope.country.idcountry) {
                                        $scope.InsertCountryLists[j] = $scope.country;
                                        break;
                                    }
                                }
                            }
                            else {
                                // Following code is for checking duplicate id for update record if found it will take last modified record and add to updateCountyrList
                                var k = 0;
                                var Flag = 0;
                                for (; k < $scope.UpdateCountryLists.length; k++) {
                                    if ($scope.UpdateCountryLists[k].idcountry == $scope.country.idcountry) {
                                        Flag = 1;
                                        break;
                                    }
                                }
                                if (Flag == 0) {
                                    
                                    $scope.UpdateCountryLists.push($scope.country);
                                }
                                else {
                                    
                                    $scope.UpdateCountryLists[k] = $scope.country;
                                }
                            }
                        }
                    }
                    //clear the add country form
                    $scope.buttonNew = false;
                    $scope.buttonInsert = true;
                    $scope.buttonUpdate = true;
                    $scope.buttonDelete = true;
                    $scope.buttonCancel = false;
                    $scope.AddNewPortion = true;
                    $scope.buttonSubmitSave = false;
                    //  $scope.country = {};
                    $scope.country = {};
                    $scope.country.isActive = true;

                }
                else {
                    alertDialog.init('Check Input validation !');
                }
            }
            else {
                //clear the add country form
                $scope.buttonNew = false;
                $scope.buttonInsert = true;
                $scope.buttonUpdate = true;
                $scope.buttonDelete = true;
                $scope.buttonCancel = false;
                $scope.AddNewPortion = true;
                $scope.buttonSubmitSave = true;
                //  $scope.country = {};
                $scope.country = {};
                $scope.country.isActive = true;
            }
        }
        else {

            alertDialog.init('Duplicate Country Found Entry Aborted!');
            $scope.countryLists = dataAngularObject.countryLists;
        }
    }



    /*******************Following is for load data into form controls like textboxes , radio buttons etc ****************/
    $scope.delete = function (idcountry) {

        //search country with given id and update it
        for (i in $scope.countryLists) {
            if ($scope.countryLists[i].idcountry == idcountry) {
                //we use angular.copy() method to create 
                //copy of original object
                $scope.country = angular.copy($scope.countryLists[i]);
            }

        }
        //clear the add country form
        $scope.buttonNew = true;
        $scope.buttonInsert = true;
        $scope.buttonUpdate = true;
        $scope.buttonDelete = false;
        $scope.buttonCancel = false;
        $scope.AddNewPortion = false;

        // Following is for Display Insert Country Section Div

        $("#countryTitle").parent().addClass("active");
        $("#countryTitle").parent().find("label").css("top", "-16px");
        $("#countryTitle").parent().find("label").css("font-size", "14px");
    }

    /*******************Following is for delete record from grid table ****************/

    $scope.DeleteInGrid = function () {

        //search country with given id and delete it
        if ($("#idCreateCountryForm").valid()) {
            for (i in $scope.countryLists) {

                if ($scope.countryLists[i].idcountry == $scope.country.idcountry) {

                    if ($scope.country.idcountry < 0) {
                        for (var j = 0; j < $scope.InsertCountryLists.length; j++) {
                            if ($scope.InsertCountryLists[j].idcountry == $scope.country.idcountry) {

                                $scope.InsertCountryLists.splice(j, 1);

                                $scope.countryLists.splice(i, 1);
                                break;
                            }
                        }
                    }
                    else {
                        $scope.countryLists.splice(i, 1);
                        $scope.UpdateCountryLists.splice(j, 1);
                        $scope.DeleteCountryLists.push($scope.country);
                    }



                    $scope.country = {};
                }
            }
            //clear the add country form
            $scope.buttonNew = false;
            $scope.buttonInsert = true;
            $scope.buttonUpdate = true;
            $scope.buttonDelete = true;
            $scope.buttonCancel = false;
            $scope.AddNewPortion = true;
            $scope.buttonSubmitSave = false;
            $scope.country = {};
        }
    }





    /*******************Following is for search record from grid table ****************/

    //$scope.search = function () {

    //    var searchText = document.getElementById('txtSearch').value;

    //    var temp = dataAngularObject.countryLists;

    //    $scope.countryLists = [];

    //    for (var i = 0; i < temp.length; i++) {
    //        if (angular.lowercase(temp[i].title) == angular.lowercase(searchText)) {
    //            $scope.countryLists.push(temp[i]);
    //        }
    //    }

    //    // Following is for Display Country Grid Section Div
    //    $("#CountryGridDiv").removeClass("class : DivHide");



    //    // Following is for scroll page down
    //    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
    //}

    $scope.DeleteAll = function () {
        $.each($(".selectAllCheckBoxClass").find("input"), function (index, val) {
            var idchk = $(val).attr("id");
            if ($("input[id=" + idchk + "]:checked").length > 0) {
                var id = parseInt(idchk.replace("chk",""));
                for (i in $scope.countryLists) {

                    if ($scope.countryLists[i].idcountry == id) {

                        if ($scope.country.idcountry < 0) {
                            for (var j = 0; j < $scope.InsertCountryLists.length; j++) {
                                if ($scope.InsertCountryLists[j].idcountry == id) {
                                    $scope.InsertCountryLists.splice(j, 1);
                                    $scope.countryLists.splice(i, 1);
                                    break;
                                }
                            }
                        }
                        else {
                            $scope.DeleteCountryLists.push($scope.countryLists[i]);
                            $scope.countryLists.splice(i, 1);
                            $scope.UpdateCountryLists.splice(j, 1);
               
                        }
                    }
                }
            }
        });
        $scope.buttonNew = false;
        $scope.buttonInsert = true;
        $scope.buttonUpdate = true;
        $scope.buttonDelete = true;
        $scope.buttonCancel = false;
        $scope.AddNewPortion = true;
        $scope.buttonSubmitSave = false;
        $scope.country = {};
    }


    $("#idCreateCountryForm").submit(function () {
        var CountryViewModel = new Object();
        CountryViewModel.InsertCountryLists = $scope.InsertCountryLists;
        CountryViewModel.UpdateCountryLists = $scope.UpdateCountryLists;
        CountryViewModel.DeleteCountryLists = $scope.DeleteCountryLists;
        $("#angularModel").val(JSON.stringify(CountryViewModel));

    });


    $(document).on("click", ".radioer", function () {
        $(this).find("input")[0].click();
    });
    var checkBoxFlag = true;
    $(document).on("click", ".checkboxer", function (e) {
        if (checkBoxFlag == true) {
            $(this).find("input")[0].click();
        }
    });
    $scope.SelectedDelete = function ($event) {
       
            if ($(".selectAllCheckBoxClass").find("input[type=checkbox]:checked").length>0) {
                $scope.buttonDeleteAll = false;
            }
            else {
                $scope.buttonDeleteAll = true;
            }
        
            if (checkBoxFlag == true) {
                checkBoxFlag = false;
                
                if ($($event.target).attr("id") == "chkSelectAllData") {
                    //$event.target.find("input")[0].click();
                    var checked = $("input[id=chkSelectAllData]:checked").length;
                    $.each($(".selectAllCheckBoxClass").find("input"), function (index, val) {
                        if (checked == 1 && $("input[id=" + $(val).attr("id") + "]:checked").length <= 0) {
                            $(val).click();
                        }
                        else if (checked == 0 && $("input[id=" + $(val).attr("id") + "]:checked").length > 0) {
                            $(val).click();
                        }
                    });

                    checkBoxFlag = true;
                }
                else {
                    //$($event.target).find("input")[0].click();
                    if ($(".selectAllCheckBoxClass").find("input").length == $(".selectAllCheckBoxClass").find("input[type=checkbox]:checked").length) {
                        if ($("input[id=chkSelectAllData]:checked").length <= 0) {
                            $("#chkSelectAllData").click();
                        }
                    }
                    else {
                        if ($("input[id=chkSelectAllData]:checked").length > 0) {
                            $("#chkSelectAllData").click();
                        }
                    }
                    checkBoxFlag = true;
                }

            }
    }
    //$(document).on("click", ".checkboxer", function (e) {
    //    
 
    //});






    //$(document).on("click", ".checkboxer", function (e) {
    //    
    //    if ($(".selectAllCheckBoxClass").find("input").length != $(".selectAllCheckBoxClass").find("input[type=checkbox]:checked").length) {
    //        $scope.buttonDeleteAll = false;
    //    }
    //    else {
    //        $scope.buttonDeleteAll = true;
    //    }

    //    if (checkBoxFlag == true) {
    //        checkBoxFlag = false;
    //        if ($($(this).find("input")[0]).attr("id") == "chkSelectAllData") {
    //            $(this).find("input")[0].click();
    //            var checked = $("input[id=chkSelectAllData]:checked").length;
    //            $.each($(".selectAllCheckBoxClass").find("input"), function (index, val) {
    //                if (checked == 1 && $("input[id=" + $(val).attr("id") + "]:checked").length <= 0) {
    //                    $(val).click();
    //                }
    //                else if (checked == 0 && $("input[id=" + $(val).attr("id") + "]:checked").length > 0) {
    //                    $(val).click();
    //                }
    //            });

    //            checkBoxFlag = true;
    //        }
    //        else {
    //            $(this).find("input")[0].click();
    //            if ($(".selectAllCheckBoxClass").find("input").length == $(".selectAllCheckBoxClass").find("input[type=checkbox]:checked").length) {
    //                if ($("input[id=chkSelectAllData]:checked").length <= 0) {
    //                    $("#chkSelectAllData").click();
    //                }
    //            }
    //            else {
    //                if ($("input[id=chkSelectAllData]:checked").length > 0) {
    //                    $("#chkSelectAllData").click();
    //                }
    //            }
    //            checkBoxFlag = true;
    //        }
    //    }
    //});


});


// Following is Code  for Display BootstrapJquery Dialog Model 

var alertDialog = {

    init: function (msg) {

        var dialogDiv = $('<div id="sessionTimeout-dialog" class="modal fade" role="dialog" aria-labelledby="gridSystemModalLabel"><div class="modal-dialog" role="document"> <div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title" id="gridSystemModalLabel">Warning!!!</h4></div><div class="modal-body">' + msg + '</div><div class="modal-footer"><button type="button" class="btn btn-default" data-dismiss="modal">Close</button></div></div><!-- /.modal-content --></div><!-- /.modal-dialog --></div><!-- /.modal -->');

        dialogDiv.appendTo('body');

        $("#sessionTimeout-dialog").modal("show");

    }

}






