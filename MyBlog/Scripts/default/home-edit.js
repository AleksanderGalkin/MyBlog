function HomeEdit() {
    _this = this;

    this.init = function () {

            if (!Modernizr.inputtypes.date) {
                $('.datepicker').datepicker(
                    {
                        format: "dd/mm/yyyy",
                        autoclose: true,
                        keyboardNavigation: true,
                        language: "ru",
                        orientation: "auto",
                        keyboardNavigation: true,
                        todayHighlight: true

                    }); //Initialise any date pickers
            }
        
    }

    /* другие публичные методы*/
    //this.saySomething = function (id) {
    //    alert("Пыщь-пыщь! : " + id);
    //}

    /* другие приватные методы */
    //function saySomething(id) {
    //    alert("Пыщь-пыщь! Но тссс!: " + id);
    //}

}

var homeEdit = null;
$().ready(function () {
    homeEdit = new HomeEdit();
    homeEdit.init();
});