(function ($) {
    "use strict";

    $(function () {
        for (
            var nk = window.location,
            patharray = nk.pathname.split('/'),
            o = $("ul#menu-accordion a").filter(function () {
                var thisPath = this.pathname.split('/')[1];
                return this.href == nk || (thisPath != '' && patharray.indexOf(thisPath) > 0);
            })
                .addClass("active")
                .parent()
                .addClass("active"); ;) {
            if (!o.is("li")) break;
            var parent = o.parent()
                .parent();
            if (parent.hasClass("submenu")) {
                parent.addClass("show");
            }
            parent = parent.prev()
            if (parent.hasClass("submenu-toggle")) {
                parent.attr("aria-expanded", 'true');
                parent.removeClass("collapsed");
                parent.addClass("active");

            }
            o = parent
        }
    });
})(jQuery);