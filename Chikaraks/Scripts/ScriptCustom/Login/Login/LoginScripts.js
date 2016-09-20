$(document).ready(function () {
    //$("body").height(window.innerHeight);
    //$(window).change(function () {
    //    $("body").height(window.innerHeight);
    //});
    var parent, ink, d, x, y;
    $(".show-hide-quote").click(function (e) {
        $("footer").stop().animate({ height: "toggle", opacity: "toggle" }, 300);
    });
    
    function directloginWindow()
    {
        //Find container in which you want to put ripple effect ink
        parent = $(".surface-login-container");
        //Remove any previous states
        parent.removeClass("ink-container-after-animation");
        //Set back to default state
        parent.addClass("ink-container-before-animation");
        //create .ink element if it doesn't exist
        if (parent.find(".ink").length == 0) {
            parent.prepend("<span class='ink'></span>");
        }
        ink = parent.find(".ink");
        //remove animation
        ink.removeClass("ripple-out-animate");
        var max = Math.max(parent.height(), parent.width());
        ink.css({ height: parent.height(), width: parent.width() });
        $(".user-icon").removeClass("circle-grow-animate");
        $(".user-icon").addClass("circle-shrink-animate");
        //activate animation after circle shring animation complete
        setTimeout(function () {
            $(".user-icon").parent().parent().hide();
            ink.addClass("ripple-out-animate");
            //activate animation after riple out animation complete 
            setTimeout(function () {
                //Set after animation state
                parent.addClass("ink-container-after-animation");
                //Remoave ink
                ink.remove();
                //Show Card
                $(".card-background").show();
                $(".card").show();

            }, 300)
        }, 600)
    }
    //Defined in Chikaraks\Views\Login\Login\Index.cshtml
    if (directloginWindowExecute == true) {
        directloginWindow();
    }

    $(".user-icon").click(function (e) {

        $("#Username").val($(this).data("username"));
        $("#profilepicture").attr("src", $(this).data("profilepicture"));


        //Find container in which you want to put ripple effect ink
        parent = $(".surface-login-container");
        //Remove any previous states
        parent.removeClass("ink-container-after-animation");
        //Set back to default state
        parent.addClass("ink-container-before-animation");
        //create .ink element if it doesn't exist
        if (parent.find(".ink").length == 0) {
            parent.prepend("<span class='ink'></span>");
        }
        ink = parent.find(".ink");
        //remove animation
        ink.removeClass("ripple-out-animate");
         var max= Math.max(parent.height(),parent.width());
         ink.css({ height: parent.height(), width: parent.width() });
        $(".user-icon").removeClass("circle-grow-animate");
        $(".user-icon").addClass("circle-shrink-animate");
        //activate animation after circle shring animation complete
        setTimeout(function () {
            $(".user-icon").parent().parent().hide();
            ink.addClass("ripple-out-animate");
            //activate animation after riple out animation complete 
            setTimeout(function () {
                //Set after animation state
                parent.addClass("ink-container-after-animation");
                //Remoave ink
                ink.remove();
                //Show Card
                $(".card-background").show();
                $(".card").show();

            }, 300)
        }, 600)


    });

    $('.footer a').on('click', function () {
        $(".surface-forgot-password-container").css("background", "transparent");
        $(".surface-forgot-password-container").css("box-shadow", "none");
        $(".surface-forgot-password-container").show();
        parent = $(".surface-forgot-password-container");
        parent.addClass("ink-container-before-animation");
        //create .ink element if it doesn't exist
        if (parent.find(".ink").length == 0) {
            parent.prepend("<span class='ink'></span>");
        }
        ink = parent.find(".ink");
        ink.css("background", "#5677fc");
        //incase of quick double clicks stop the previous animation
        ink.removeClass("animate");
        ink.css({ height: parent.height(), width: parent.width() });

        setTimeout(function () {
            ink.addClass("ripple-out-animate");
            setTimeout(function () {
                $(".surface-forgot-password-container").css("box-shadow", "0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24)");
                parent.addClass("ink-container-after-animation");
                $(".surface-forgot-password-container").css("background", "#5677fc");
                ink.remove();
                $('.login-container').stop().addClass('active');
            }, 300)
        }, 600)




    });

    $('.close').on('click', function () {
        $('.login-container').stop().removeClass('active');
        $(".surface-forgot-password-container").hide();

    });
    $('.close-login').on('click', function () {
        $(".card-background").hide();
        $(".card").hide();
        parent = $(".surface-login-container ");
        parent.removeClass("ink-container-after-animation");
        parent.removeClass("ink-container-before-animation");
        
        if (parent.find(".ink").length == 0) {
            parent.prepend("<span class='ink'></span>");
        }
        ink = parent.find(".ink");
        ink.css("transform", "scale(1)");
        ink.removeClass("animate");
        var max= Math.max(parent.height(),parent.width());
        ink.css({ height: parent.height(), width: parent.width() });
        ink.addClass("ripple-in-animate");
        
        
        setTimeout(function () {
            ink.remove();
            $(".user-icon").addClass("circle-grow-animate");
           
            $(".user-icon").parent().parent().show();
        }, 250)
    });

  
});

////<!-- BEGIN Google Analytics -->
//(function (i, s, o, g, r, a, m) {
//    i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
//        (i[r].q = i[r].q || []).push(arguments)
//    }, i[r].l = 1 * new Date(); a = s.createElement(o),
//    m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
//})(window, document, 'script', '../Content/assets/globals/js/analytics.js', 'ga');
//ga('create', Pleasure.settings.ga.urchin, Pleasure.settings.ga.url);
//ga('send', 'pageview');
////<!-- END Google Analytics -->