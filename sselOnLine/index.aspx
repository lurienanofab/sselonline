<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="sselOnLine.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LNF Online Services</title>
    <link rel="stylesheet" href="styles/default.css?v=2" />
    <link rel="stylesheet" href="scripts/jquery.alerts/jquery.alerts.css?v=2" />
    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/styles/navigation.css?v=2" />
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="hidPasswordRedirect" value="false" class="password-redirect" />
        <div class="wrapper">
            <div class="site-header">
                <input type="hidden" runat="server" id="hidSeedTime" class="seed-time" />
                <lnf:DropDownMenu runat="server" ID="DropDownMenu1" CssClass="menu-nav" UseJavascriptNavigation="false" Target="content"></lnf:DropDownMenu>
                <div class="alerts"></div>
            </div>
            <div class="site-content">
                <input type="hidden" class="view-url" id="hidViewUrl" runat="server" />
                <iframe id="content" name="content" src="Loader.ashx" style="width: 100%; border: none;"></iframe>
            </div>
        </div>
    </form>

    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/lib/moment/moment.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/data/scripts/jquery.servertime.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/scripts/jquery/jquery.menu.js"></script>
    <script src="scripts/jquery.alerts/jquery.alerts.js?v=2"></script>

    <script>
        //sets up the menu to show/hide on hover events
        $(".menu-root").menu();

        //starts the menu clock
        $('#jclock').servertime({ "format": "hh:mm:ss A on dddd, MMMM DD", "url": "//ssel-sched.eecs.umich.edu/time.aspx" });

        //resizes the iframe when the window resizes
        $(window).on("resize", function (e) {
            var headerHeight = $(".site-header").outerHeight();
            $(".site-content").css("top", headerHeight + "px");
        }).trigger("resize");

        //display any alerts
        $('.alerts').alerts({
            "url": "/alerts.js",
            "location": "menu",
            "callback": function (d) {
                d.always(function (data) {
                    $(window).trigger("resize");
                });
            }
        });

        //this will resize the frame every time the iframe loads
        $("#content").on("load", function (e) {
            $(window).trigger("resize");
        });

        viewNav();

        //when there is a ?view querystring parameter the path is written to a hidden input - this will cause the iframe to navigate to this location
        function viewNav() {
            var url = $(".view-url").val();
            if (url) menuNav(url);
        }

        //called when menu items are selected - causes the iframe to navigate to the selected location
        function menuNav(url, target) {
            if (target)
                window.open(url, target);
            else
                $('#content').attr("src", url);
        }

        if ($(".password-redirect").val() == "true")
            $('#content').attr('src', '/sseluser/ChangePassword.aspx?ForceChange=True');
    </script>
</body>
</html>
