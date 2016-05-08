$(document).ready(function ($) {

    $('#txtNewPwd').strength({
        strengthClass: 'strength',
        strengthMeterClass: 'strength_meter',
        strengthButtonClass: 'button_strength',
        strengthButtonText: 'Show Password',
        strengthButtonTextToggle: 'Hide Password'
    });
     $('#txtNewPwdConfirm').strength({
        strengthClass: 'strength',
        strengthMeterClass: 'strength_meter',
        strengthButtonClass: 'button_strength',
        strengthButtonText: 'Show Password',
        strengthButtonTextToggle: 'Hide Password'
    });

});