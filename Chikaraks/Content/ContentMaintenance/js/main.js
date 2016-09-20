( function( $ ) {
    "use strict";

        /*-------------------------------------------------------------------*/
        /*      Email validator
        /*-------------------------------------------------------------------*/
    var emailValidator = function(email){

            var emailReg = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
            var valid = emailReg.test(email);

            return valid;
        };


    if ( bigHero === true ) {

        /*-------------------------------------------------------------------*/
        /*      Modify the HTML dom to create the big hero header
        /*-------------------------------------------------------------------*/
        $('body').addClass('big-hero').find('.header-wrapper').prepend('<a id="go-down" href="#"></a>');

        /*-------------------------------------------------------------------*/
        /*      Scroll to the main content
        /*-------------------------------------------------------------------*/
        $('#go-down').click(function(e) {
            e.preventDefault();
            $('html,body').animate({ scrollTop: $('header').next().offset().top },
                1000);           
        });

    }

    /*-------------------------------------------------------------------*/
    /*      Forms 
    /*          1. Form Messages
    /*          2. Form Processor 
    /*          3. Close Form Message
    /*-------------------------------------------------------------------*/

    /* 1. Form Messages
    -------------------------------------------------------------------*/
    var formMessages = {
          subscribed    : 'Thanks for your subscription!',
          duplicate     : 'You\'re already subscribed!',
          fail          : 'Oops! Sending error. Please try again',
          failEmail     : 'Invalid Email!',
          emptyField    : 'Empty field!',
        },
        $form = $('.newsletter-form');

    /* 2. Form Processor
    -------------------------------------------------------------------*/
    // Add form message container
    $form.append('<div class="form-msg" style="display:none"><a href="#"></a></div>');

    $form.submit(function(e){
        e.preventDefault();

        var $that = $(this),
            emailField = $that.find(':text').val(),
            postData = { newsletter: emailField },
            checkEmpty = false,
            $msgForm = $that.find('.form-msg'),
            $msgText = $msgForm.find('a');
        
        $msgForm.removeClass('fail success duplicate');
        $msgText.text('');
        
        // Check if the field is not empty. Otherwise stop all
        if($.trim(emailField) === '') {
            $msgText.text(formMessages.emptyField).parent().addClass('fail').fadeIn('fast');
            return false;
        }

        // Check if the email is valid. Otherwise stop all
        if ( ! emailValidator(emailField) ) {
            $msgText.text(formMessages.failEmail).parent().addClass('fail').fadeIn('fast');
            return false;
        }

        // Send data to the corresponding processing file
        $.post($that.attr('action'), postData, function(result){
            if (result == 'success') {
                $msgText.text(formMessages.subscribed);         // success
                $that.find(':text').val('');                    // reset field
            } else if (result == 'duplicate') {
                $msgText.text(formMessages.duplicate);          // duplicate email   
            } else {
                $msgText.text(formMessages.fail);               // fail
            }       
        }).fail(function() {
            $msgText.text(formMessages.fail);                   // fail (problem with sending data)
        }).always(function(result) {
            $msgForm.addClass(result).fadeIn('fast');           // show form message
        });

    });

    /* 3. Close form messages
    -------------------------------------------------------------------*/
    $(document).on('click','.form-msg a', function(e){
        e.preventDefault();
        $(this).parent().fadeOut();
    });


    /*-------------------------------------------------------------------*/
    /*      Window load scripts
    /*-------------------------------------------------------------------*/
    $(window).load(function() {

        /*-------------------------------------------------------------------*/
        /*      Countdown 
        /*-------------------------------------------------------------------*/
        var $countdown = $('#countdown'),
            dateCountdown = $countdown.data('countdown'),
            endCountdownMSg = 'We are ready!',
            countdownComp = {
                weeks   : '<span class="name-counter">%!w:week,weeks;</span> <span class="num-counter">%-w</span> ',
                days    : '<span class="name-counter">%!d:day,days;</span> <span class="num-counter">%-d</span>',
                hours   : '<span class="name-counter">%!H:hour,hours;</span><span class="num-counter">%-H</span>',
                mins    : '<span class="name-counter">%!M:minute,minutes;</span><span class="num-counter">%-M</span>',
                sec     : '<span class="name-counter">%!S:second,seconds;</span><span class="num-counter">%-S</span>'
            };
        
        $countdown.addClass('with-weeks').countdown(dateCountdown)
        .on('update.countdown', function(event) {
            var format = '';      
            if(event.offset.weeks > 0) {
                $('.count-el.weeks').html(event.strftime(countdownComp.weeks));
            } else {
                $(this).removeClass('with-weeks');
                $('.count-el.weeks').remove()
                $('.count-el').css('width','25%');
            }
            $('.count-el.days').html(event.strftime(countdownComp.days));
            $('.count-el.hours').html(event.strftime(countdownComp.hours));
            $('.count-el.minutes').html(event.strftime(countdownComp.mins));
            $('.count-el.seconds').html(event.strftime(countdownComp.sec));

        })
        .on('finish.countdown', function(event) {
            $(this).addClass('end-countdown').html('<span>'+endCountdownMSg+'</span>');
        });

        /*-------------------------------------------------------------------*/
        /*      Remove page loader to the DOM 
        /*-------------------------------------------------------------------*/
        setTimeout(function() {

            $('.content-loader').fadeOut(400, function(){
                $(this).remove();
            });
            $('body').removeClass('loading');

        }, 400);

    });

} )( jQuery );