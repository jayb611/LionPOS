$(document).ready(function () {
    //$(".hideable-wrapper").hide();
    $(".radio-option input[type='radio']").click(function (e) {
        $(".hideable-wrapper").hide();
        if($(this).prop("checked"))
        {
            
            $(this).parent().find(".hideable-wrapper").stop().animate({ height: "toggle", opacity: "toggle" }, 300);
        }
       
    });

});