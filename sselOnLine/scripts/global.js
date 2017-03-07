$(document).ready(function () {
    global.Page.Init();
});

var global = {
    Page: {
        Init: function () {
            //global.Page.Menu.Init();
            //global.Page.Clock.Init();
            global.App.Title.ApplyCornerStyle();
            //global.App.Grid.FixHeaderColumns();
        },
        Menu: {
            Init: function () {
                $('#menu1').menu();
            }
        },
        Clock: {
            Init: function () {
                $('.jclock').jclock({
                    "format": "Time: %i:%M:%S %P on %A, %B %d"
                });
            }
        },
        Uploadify: {
            Init: function (id, options) {
                var defaults = {
                    uploader: '/sselOnLine/scripts/uploadify/uploadify.swf',
                    script: '/sselOnLine/scripts/uploadify/uploadify.ashx',
                    cancelImg: '/sselOnLine/images/cancel.png',
                    folder: '/sselOnLine/files/temp/',
                    queueID: 'file_queue',
                    auto: true,
                    multi: true,
                    onComplete: function (event, queueID, fileObj, response, data) {
                        //nothing
                    },
                    onError: function (event, queueID, fileObj, errorObj) {
                        //nothing
                    }
                }

                var o = $.extend(defaults, options);

                $(id).uploadify(o);
            }
        }
    },
    App: {
        Session: function (key) {
            var val = null;
            if ($('.session').find('#' + key)) {
                val = $('.session').find('#' + key).find('input[type="hidden"]:first').val();
            }
            return val;
        },
        Title: {
            ApplyCornerStyle: function () {
                $('.app .title').corner();
            }
        },
        Grid: {
            FixHeaderColumns: function () {
                if (!$('.app .grid th:first-child').hasClass('first_header_column')) { $('.app .grid th:first-child').addClass('first_header_column'); }
                if (!$('.app .grid th:last-child').hasClass('last_header_column')) { $('.app .grid th:last-child').addClass('last_header_column'); }
            }
        }
    }
};