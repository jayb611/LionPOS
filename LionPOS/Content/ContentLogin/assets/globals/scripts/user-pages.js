var UserPages = {

    lockScreen: function () {
        $('.bg-blur').addClass('active');
        $('#user-password').focus();
    },

    login: function () {
        $('.bg-blur').addClass('active');

        $('.show-pane-forgot-password').click(function () {
            $('.panel-body').hide();
            $('#pane-forgot-password').fadeIn(1000);
            $('.login-screen').addClass('forgot-password');
        });
        $('.show-pane-create-account').click(function () {
            $('.panel-body').hide();
            $('#pane-create-account').fadeIn(1000);
            $('.login-screen').addClass('create-account');
        });
        $('.show-pane-login').click(function () {
            $('.panel-body').hide();
            $('#pane-login').fadeIn(1000);
            $('.login-screen').removeClass('forgot-password create-account');
        });

    },

    // Following Function is for Enable / Disable Social Medai Button in Creat New Account

    socialButtonEnableDisable: function () {

        // Following is for when page is load then social media button is disabled
        $('.btnSocialButtonGroup').attr("disabled", "disabled");


        // Following is when Terms & Condition checkbox is click then social buttong is enable / disable
        $('#chkTermsAndCondition').click(function () {

            if ($('#chkTermsAndCondition').is(":checked")) {
                $(".btnSocialButtonGroup").removeAttr("disabled");

            }
            else if ($('#chkTermsAndCondition').not(':checked')) {

                $('.btnSocialButtonGroup').attr("disabled", "disabled");
            }

        });
    },

    profile: function () {
        var $masonryContainer = $('.masonry');

        setTimeout(function () {
            $masonryContainer.masonry({
                /* 'isOriginLeft': false // RTL support */
            });
        }, 500);

        $('.toggle-sidebar-menu').click(function () {
            setTimeout(function () {
                $masonryContainer.masonry();
            }, 100);
        });

        var gallery = $('#blueimp-gallery').data('gallery');
    }

}


