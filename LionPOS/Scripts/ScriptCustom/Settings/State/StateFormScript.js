
(function ($, W, D) {
    var JQUERY4LVITS = {};
    
    JQUERY4LVITS.UTIL =
    {
        
        setupFormValidation: function () {

            //form validation rules
            
            $("#idSubmitForm").validate({
                rules: {
                    autoTextidcountry: {
                        required: true,
                        maxlength: '45'
                    },
                    autoTextTitle: {
                        required: true,
                        maxlength: '45'
                    },
                    isActive: {
                        required: true,
                    }


                },
                messages:
                    {
                        autoTextidcountry:
                       {
                           required: "Please select country.",
                           maxlength: "Max Length 45"
                       },
                        autoTextTitle:
                      {
                          required: "Please select state.",
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

var SubmitProcessEventStop = false;
$(".Submit_anchor").click(function () {
    SubmitProcessEventStop = true;
    $("#hSave").click();
    SubmitProcessEventStop = false;
    });

$(document).on("click", ".radioer", function () {
    if (SubmitProcessEventStop == false) {
        $(this).find("input")[0].click();
    }
    });
    var checkBoxFlag = true;
    $(document).on("click", ".checkboxer", function (e) {
        if (SubmitProcessEventStop == false) {
            
                $(this).find("input")[0].click();
            
        }
    });
  