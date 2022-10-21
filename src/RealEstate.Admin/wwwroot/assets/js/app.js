window.onBlazorReady = function () {

    tooltip();
}

window.getBlazorCulture = function() {
    return localStorage['BlazorCulture'];
}

window.setBlazorCulture = function(value) {
    localStorage['BlazorCulture'] = value;
}

window.hideCollapse = function (elementId) {
    var collapse = bootstrap.Collapse.getInstance(document.getElementById(elementId));
    if (collapse)
        collapse.hide();
}

window.showModal = function (modalId) {
    var modal = new bootstrap.Modal(document.getElementById(modalId));
    modal.show();
}

window.hideModal = function (modalId) {
    var modal = bootstrap.Modal.getInstance(document.getElementById(modalId))
    modal.hide();
}

window.setNumber = function (id, number) {
    const input = document.querySelector(`#${id} input[type="tel"]`);
    const iti = window.intlTelInputGlobals.getInstance(input);
    iti.setNumber(number);
}

window.setStyleFormPhoneInput = function(){
    const inputs = document.querySelectorAll('.form-phone-input .iti > input');
    for (const input of inputs) {
        input.classList.add('form-control');
    }
}

window.setReadOnly = function (id, readOnly) {
    const input = document.querySelector(`#${id} input[type="tel"]`);
    if(readOnly){
        input.setAttribute("readonly", "readonly");
    }
    else{
        input.removeAttribute("readonly");
    }
}

window.initPasswordVisibilityToggle = function (){
    var elements = document.querySelectorAll('.password-toggle');

    var _loop2 = function _loop2(i) {
        var passInput = elements[i].querySelector('.form-control'),
            passToggle = elements[i].querySelector('.password-toggle-btn');
        passToggle.addEventListener('click', function (e) {
            if (e.target.type !== 'checkbox') return;

            if (e.target.checked) {
                passInput.type = 'text';
            } else {
                passInput.type = 'password';
            }
        }, false);
    };

    for (var i = 0; i < elements.length; i++) {
        _loop2(i);
    }
}

// https://www.syncfusion.com/faq/blazor/javascript-interop/how-do-i-get-a-browsers-culture-in-blazor-webassembly
window.getBrowserLanguage = function () {
    return (navigator.languages && navigator.languages.length) ? navigator.languages[0] :
        navigator.userLanguage || navigator.language || navigator.browserLanguage || 'en'
}

var tooltip = function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        console.log(tooltipTriggerEl);
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};