/// <reference path="../jquery-2.2.2.js" />






$(document).ready(function () {
    $.each(InfinityList, function (index, infinity) {
       var InfiniteDataFetchedFired = false;
        $(infinity.scrollBarContainer).scroll(function () {
            if (infinity.InfiniteDataFetchedNextPageNumber <= infinity.TotalPage) {
                var currY = $(this).scrollTop();
                var scrollHeight = $(infinity.documentHeight).height();
                var postHeight =  $(infinity.areaHeight).height();
              
                // Current percentual position
                var scrollPercent = (currY / (scrollHeight - postHeight));
                scrollPercent = Math.round(scrollPercent * 100);
                //$("#btnAddNew").css("z-index", 99999);
                //$("#btnAddNew").css("left", 0);
                //$("#btnAddNew").css("position", "fixed");
                //$("#btnAddNew").html("scrollTop: " + currY + ",documentHeight: " + scrollHeight + ", areaHeight:" + postHeight + " , value : " + scrollPercent);
                
                if (scrollPercent >= 99) {
                    if (InfiniteDataFetchedFired == false) {
                        InfiniteDataFetchedFired = true;
                        scrollPercent = 0;
                        $("#filter" + infinity.name + "Container").append($("#filter" + infinity.name + "LoaderContainer").html());
                       
                        var whereClauseStream="";
                        var OrderByStream = "";
                        var LoadAsDefaultFilter = true;
                        $.each(FilterList, function (index, filter) {
                            
                            if(filter.name == infinity.name)
                            {
                                whereClauseStream = filter.whereClauseStream;
                                OrderByStream = filter.OrderByStream;
                                LoadAsDefaultFilter = filter.LoadAsDefaultFilter;
                            }
                        });
                        
                        $.ajax({
                            url: infinity.InfinitePageFetchURLink,
                            data: { filterFieldsAndValues: whereClauseStream, orderByField: OrderByStream, PageNumber: infinity.InfiniteDataFetchedNextPageNumber, SaveAsDefaultFilter: false, LoadAsDefaultFilter: LoadAsDefaultFilter },
                            type: "POST",
                            cache: false,
                            async: true,
                            success: function (data) {
                                
                                if (infinity.InfiniteDataFetchedNextPageNumber <= infinity.TotalPage) {
                                    infinity.InfiniteDataFetchedNextPageNumber = infinity.InfiniteDataFetchedNextPageNumber + 1;
                                }

                                $("#filter" + infinity.name + "Container").find("#filter" + infinity.name + "Loader").remove();
                                $("#filter" + infinity.name + "Container").append(data.view);
                                InfiniteDataFetchedFired = false;
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                $("#filter" + infinity.name + "Loading").hide();
                                InfiniteDataFetchedFired = false;
                                alert(thrownError.toString());
                            }
                        });
                    }
                }
            }
        });
    });

    
});
