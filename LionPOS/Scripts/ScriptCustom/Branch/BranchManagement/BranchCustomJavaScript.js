var BranchformID = "#Branchform"
$(document).ready(function () {
    tinymce.init({
        selector: ".TinyMCE",
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks  fullscreen",
            "insertdatetime media table contextmenu paste textcolor "
        ],
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | forecolor backcolor"
    });
   
    $(document).on("submit", BranchformID, function (e) {
        debugger
        $("#remarks").val(tinyMCE.get('remarks').getContent());
        if ($(BranchformID).valid()) {

        }
        else {
            e.preventDefault();
        }
    });
});


(function ($, W, D) {
    var JQUERY4LVITS = {};
    JQUERY4LVITS.UTIL =
    {
        setupFormValidation: function () {
            //form validation rules             
            $(BranchformID).validate({
                rules: {
                    branchCode: {
                        required: true,
                        maxlength: '45'
                    },
                    branchName: {
                        required: true,
                        maxlength: '45'
                    },
                    branchType: {
                        required: true,
                        maxlength: '45'
                    },
                    contactNo1: {
                        required: true,
                        maxlength: '10'
                    },
                    email: {
                        required: true,
                        email: true
                    }
                },
                messages: {
                    contactname:
                        {
                            required: "Please enter branch code.",
                            maxlength: "You can enter maximum 45 character."
                        },
                    branchName:
                       {
                           required: "Please enter branch name.",
                           maxlength: "You can enter maximum 45 character."
                       },
                    branchType: {
                        required: "Please enter branch type.",
                        maxlength: "You can enter maximum 45 character."
                    },
                    contactNo1: {
                        required: "Only 10 Digit Allowed"
                    },
                   
                    email: {
                        required:"Please enter branch email address.",
                        email: "please enter valid email address."
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



