window.onBlazorReady = function () {

    tooltip();
}

window.getBlazorCulture = function() {
    return localStorage['BlazorCulture'];
}

window.setBlazorCulture = function(value) {
    localStorage['BlazorCulture'] = value;
}

window.stripeCheckout = function (publishableKey, sessionId) {

    var stripe = Stripe(publishableKey);

    stripe.redirectToCheckout({
        sessionId: sessionId
    }).then(function (result) {
        console.log(result.error.message);
    });
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

window.openCart = function () {
    var cart = new bootstrap.Offcanvas(document.getElementById('shoppingCart'));
    cart.show();
}

window.hideCart = function () {
    var cart = bootstrap.Offcanvas.getInstance(document.getElementById('shoppingCart'))
    cart.hide();
}

window.scaleBannerPreview = function () {
    const bannerWidth = 1920;
    const bannerHeight = 275;

    let wrap = document.querySelector(".wrap");
    let frame = document.querySelector(".frame");

    let w = wrap.parentElement.clientWidth - 48;
    let h = w / (bannerWidth / bannerHeight);

    wrap.style.width = w + "px";
    wrap.style.height = h + "px"

    frame.style.width = bannerWidth + "px";
    frame.style.height = bannerHeight + "px"

    frame.style.transform = 'scale(' + (w / bannerWidth) + ')';

    window.onresize = scaleBannerPreview;
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

var tooltip = function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        console.log(tooltipTriggerEl);
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};