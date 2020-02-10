(function($){

  $.fn.fullHeight1 = function(){

    var self = this;
    var windowHeight = $(window)[0].innerHeight;

    var fullHeightFunction = function(){
      return self.each(function() {
        self.css({
          'height': windowHeight -112
        });
      });
    }

    $(window).on('resize', function(){
      windowHeight = $(window)[0].innerHeight;
      fullHeightFunction();
    });

    fullHeightFunction();
    return self;
    
  }

})(jQuery);