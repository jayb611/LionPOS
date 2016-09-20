
/// <reference path="../jquery-2.2.2.js" />


$(document).ready(function () {

    $.each(FilterList, function (index, filter) {
        $(document).on("click", "#filter" + filter.name + "Add", function () {
            var optionFields = optionFields + '<option value="select" data-type="select"></option>'
            $.each(filter.FilterFieldModelJson(), function (index, val) {
                optionFields = optionFields + '<option value="' + val.field + '" data-type="' + val.valueType + '">' + val.fieldTitle + '</option>'
            });

            var control = '<div class="row" id="FilterLayout" >' +
                                   '<div class="col-xs-12 col-md-3">' +
                                       '<select class="" id="filter' + filter.name + 'Fileds" style="width:100%;padding-top:3px;margin-bottom:3px" >' +
                                           optionFields +
                                       '</select>' +
                                   '</div>' +
                                   '<div class="col-xs-12 col-md-3" id="filterOperationsParent">' +
                                       '<select id="filter' + filter.name + 'Operations" class="" style="width:100%;padding-top:3px;margin-bottom:3px">' +
                                           
                                       '</select>' +
                                   '</div>' +
                                   '<div class="col-xs-12 col-md-3">' +
                                       '<div class="input-wrapper" id="filterInputParent">' +
                                           
                                       '</div>' +
                                   '</div>' +
                                   '<div class="col-xs-12 col-md-2">' +
                                       '<select id="filter' + filter.name + 'Joints" class="" style="width:100%;padding-top:3px;margin-bottom:3px">' +
                                           '<option value="And">And</option>' +
                                           '<option value="Or">Or</option>' +
                                       '</select>' +
                                   '</div>' +

                                   '<div class="col-xs-12 col-md-1 text-center">' +
                                       '<a class="btn btn-floating btn-deep-purple btn-ripple" id="filter' + filter.name + 'Remove" ><i class="ion-android-remove"></i></a>' +
                                   '</div>' +
                               '</div>';
            $("#filter" + filter.name + "Pane").append(control);
            $('#filterFilterPane').find(".selecter").selectpicker("refresh");
        });
        $(document).on("click", "#filter" + filter.name + "Remove", function () {
            $(this).parent().parent().remove();
        });

      
        $(document).on("change", "#filter" + filter.name + "Fileds", function () {
        
            
            var type = $(this).find(':selected').data('type');

            var inputField = '<input @Type class="form-control" placeholder="Place Serach Term Here" id="filter' + filter.name + 'Input" >';
            var optionFields = '<select id="filter' + filter.name + 'Operations" class="" style="width:100%;padding-top:3px;margin-bottom:3px">';
            $.each(SQLDataTypeJosn, function (index, val) {
                if (val.type == type) {
                    $.each(val.operations, function (indexj, valj) {
                        optionFields = optionFields + '<option value="' + valj + '">' + val.operationsTitles[indexj] + '</option>'
                    });

                    if (type == "Numeric") {
                        inputField = inputField.replace("@Type", 'type="number"')
                    }
                    else if (type == "Text") {
                        inputField = inputField.replace("@Type", 'type="text"')
                    }
                    else if (type == "DateTime") {
                        inputField = inputField.replace("@Type", 'type="date"')
                    }
                }
            });
            optionFields = optionFields + '</select>' +

            $(this).parent().parent().find("#filterOperationsParent").html(optionFields);
            $(this).parent().parent().find("#filterInputParent").html(inputField);
            //initLayout();
            $('#FilterLayout').find(".selecter").selectpicker("refresh");

        });
        
        filter.FilterFetchedFired = false;
        $(document).on("click", "#btn" + filter.name + "Filter", function () {
            LionFilter(false);
        });
            
        $(document).on("click", "#btn" + filter.name + "SaveAsDefault", function () {
            
            LionFilter(true);
        });
        function LionFilter(saveAsDefault) {
            if (filter.FilterFetchedFired == false) {
                filter.FilterFetchedFired = true;
                filter.whereClauseStream = "";
                $.each($("#filter" + filter.name + "Pane .row"), function (indexi, vali) {
                    filter.whereClauseStream = filter.whereClauseStream + $(vali).find("#filter" + filter.name + "Fileds").find(":selected").val() + "|";
                    filter.whereClauseStream = filter.whereClauseStream + $(vali).find("#filter" + filter.name + "Operations").find(":selected").val() + "|";
                    filter.whereClauseStream = filter.whereClauseStream + $(vali).find("#filter" + filter.name + "Input").val() + "|";
                    filter.whereClauseStream = filter.whereClauseStream + $(vali).find("#filter" + filter.name + "Joints").find(":selected").val() + "|";
                });
                filter.OrderByStream = "";
                $.each($("#filter" + filter.name + "OrderBy li select").find(":selected"), function (indexi, vali) {
                    if ((filter.OrderByStream != "" && filter.OrderByStream != null && filter.OrderByStream != undefined) && vali.value != "") {
                        filter.OrderByStream = filter.OrderByStream + ",";
                    }
                    filter.OrderByStream = filter.OrderByStream + vali.value;
                });
                $("#ldrfilter" + filter.name + "Placeholder").show();
                $("#filter" + filter.name + "Container").html("");
               

                $.ajax({
                    url: filter.FilterURLLink,
                    data: { filterFieldsAndValues: filter.whereClauseStream, orderByField: filter.OrderByStream, PageNumber: 1, SaveAsDefaultFilter: saveAsDefault, LoadAsDefaultFilter:false },
                    type: "POST",
                    cache: false,
                    async: true,
                    success: function (data) {
                        $.each(InfinityList, function (index, infinity) {
                            if (infinity.name == filter.name) {
                                infinity.InfiniteDataFetchedNextPageNumber = 2;
                                infinity.TotalPage = data.TotalPage;
                                filter.LoadAsDefaultFilter = false;
                            }
                        });
                        alert("asd");
                        $("#filter" + filter.name + "Container").html("");
                        $("#filter" + filter.name + "Container").html(data.view);
                        $("#ldrfilter" + filter.name + "Placeholder").hide();
                        filter.FilterFetchedFired = false;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $("#filter" + filter.name + "Container").html("");
                        $("#ldrfilter" + filter.name + "Placeholder").hide();
                        filter.FilterFetchedFired = false;
                        alert(thrownError.toString());
                    }
                });
            }
        }
    });

});
