(function ($) {
    $.fn.extend({
        menu: function (options) {
            var defaults = {
                corner: null
            };

            var options = $.extend(defaults, options);

            return this.each(function () {
                var o = options;

                var obj = $(this);

                if (o.corner) {
                    obj.corner(o.corner);
                }

                $('.menugroup', obj).each(function () {
                    var group = $(this);
                    var sub = $('.submenu', this);

                    $(this).hover(
                        function (e) {
                            group.removeClass('menugroup_off').addClass('menugroup_on');
                            $('.topmenu', group).removeClass('topmenu_off').addClass('topmenu_on');
                            $('.menubtn', group).removeClass('menubtn_off').addClass('menubtn_on');
                            $('.submenu', group).removeClass('submenu_off').addClass('submenu_on');
                            $('.menulbl', group).removeClass('menulbl_off').addClass('menulbl_on');
                        },
                        function (e) {
                            group.removeClass('menugroup_on').addClass('menugroup_off');
                            $('.topmenu', group).removeClass('topmenu_on').addClass('topmenu_off');
                            $('.menubtn', group).removeClass('menubtn_on').addClass('menubtn_off');
                            $('.submenu', group).removeClass('submenu_on').addClass('submenu_off');
                            $('.menulbl', group).removeClass('menulbl_on').addClass('menulbl_off');
                        }
                    );

                    $('> div > a', sub).hover(
                        function (e) {
                            $(e.target).removeClass('menuitem_off').addClass('menuitem_on');
                        },
                        function (e) {
                            $(e.target).removeClass('menuitem_on').addClass('menuitem_off');
                        }
                    );

                    $('.topmenu:last', obj).addClass('topmenu_last');
                    $('.menulbl:last', obj).addClass('menulbl_last');
                    obj.children(':last').after('<div style="clear: both;"></div>');
                });
            });
        }
    });
})(jQuery);