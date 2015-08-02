function LandingIndex() {
    _this = this;

    this.init = function () {
        $('.carousel').carousel({
            interval: 3000,
        });
    }
    /* другие публичные методы*/
    //this.saySomething = function (id) {
    //    alert("Пыщь-пыщь! : " + id);
    //}

}

var landingIndex = null;
$().ready(function () {
    landingIndex = new LandingIndex();
    landingIndex.init();
});