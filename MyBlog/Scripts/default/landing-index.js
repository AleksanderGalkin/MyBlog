function LandingIndex() {
    _this = this;

    this.init = function () {

        (function ($) {
            $.fn.slideshow = function (options) {
                options = $.extend({
                    slides: ".slide",
                    speed: 3000,
                    easing: "swing"

                }, options);

                var timer = null; // Таймер
                var index = 0;    // Курсор

                var slideTo = function (slide, element) {
                    var $currentSlide = $(options.slides, element).eq(slide);
                    var imageUrl = "url(" + $currentSlide.data('image') + ")";
                    element.stop(true, true)
                        .css("opacity", 0.1)
                        .css('background-image', imageUrl)
                        .css('background-size', 'cover')
                        .animate({
                            opacity: 1
                        }, options.speed, options.easing);
                };

                var autoSlide = function (element) {
                    // Инициализируем последовательность
                    timer = setInterval(function () {
                        index++; // Увеличим курсор на 1
                        if (index == $(options.slides, element).length) {
                            index = 0; // Обнулим курсор
                        }
                        slideTo(index, element);
                    }, options.speed); // Тот же интервал, что и в методе .animate() 
                };

                var startStop = function (element) {
                    element.hover(function () { // Останавливаем анимацию
                        clearInterval(timer);
                        timer = null;
                    }, function () {
                        autoSlide(element); // Возобновляем анимацию	
                    });
                };

                return this.each(function () {
                    var $element = $(this);
                    autoSlide($element);
                    startStop($element);
                    slideTo(0, $element);
                });
            }
        })(jQuery);
                 $('#content').slideshow();
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