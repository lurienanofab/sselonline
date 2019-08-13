<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="sselOnLine.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <%--<meta http-equiv="X-UA-Compatible" content="IE=edge" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>LNF Online Services</title>

    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/styles/bootstrap/navbar-fixed-top.css" />
    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/styles/bootstrap/menu.css" />
    <link rel="stylesheet" href="scripts/jquery.alerts/jquery.alerts.css?v=2" />

    <style>
        .flexbox html,
        .flexbox body,
        .flexbox form {
            display: flex;
        }

        html, body, form {
            width: 100%;
            height: 100%;
        }

        .flexbox.flexboxlegacy .content-wrapper {
            flex: 1;
        }

        .no-flexbox .content-wrapper,
        .no-flexboxlegacy .content-wrapper {
            height: 100%;
        }

        .content-wrapper {
            width: 100%;
        }

        #content {
            display: block;
            width: 100%;
            height: 100%;
            border: none;
        }

        .debug {
            width: 700px;
            height: 304px;
            border: 2px solid #808080;
            position: fixed;
            top: 50%;
            left: 50%;
            margin-left: -350px;
            margin-top: -152px;
            background-color: #f5f5f5;
        }

            .debug .messages {
                white-space: pre;
                overflow: auto;
                height: 260px;
                padding: 10px;
                font-family: monospace;
                margin-bottom: 5px;
            }
    </style>

    <script src="//ssel-apps.eecs.umich.edu/static/scripts/idle-timeout.js"></script>

    <script>
        var url = '<%=string.Format(ConfigurationManager.AppSettings["ScreensaverUrl"], Request.QueryString["timeout"], Request.QueryString["room"])%>';
        var idleTimeout = new IdleTimeout(url);

        function resizeWrapper() {
            var wrapperHeight = $(".content-wrapper").height();
            var navbarHeight = $(".navbar").height();
            var pad = 5;
            $(".content-wrapper").height(wrapperHeight - navbarHeight - pad);
        }

        function frameLoad(f) {
            try {
                // go to screensaver if there is no activity for awhile (only enabled when timeout parameter is in querystring)
                var fdoc = f.contentDocument || f.contentWindow.document;
                idleTimeout.watch(fdoc, "FRAME");

                // this will resize the frame every time the iframe loads
                var event = document.createEvent("HTMLEvents");
                event.initEvent("resize", true, false);
                window.dispatchEvent(event);

                // this will close an open dropdown menu when the frame document is clicked
                $(fdoc).on("click", function (e) {
                    $(".menu > nav .dropdown.open,.menu > nav .navbar-collapse.collapse.in").removeClass("open in");
                });
            } catch (err) {
                console.log(err.message);
            }
        }

        idleTimeout.watch(window, "PARENT");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="hidPasswordRedirect" class="password-redirect" value="false" />
        <input type="hidden" runat="server" id="hidViewUrl" class="view-url" />
        <input type="hidden" runat="server" id="hidIpAddr" class="ip-addr" />

        <lnf:SiteMenu runat="server" ID="SiteMenu1" Target="content" />
        <div class="alerts"></div>

        <div class="content-wrapper">
            <iframe id="content" name="content" src="Loader.ashx" onload="frameLoad(this);"></iframe>
        </div>

        <div class="debug" style="display: none;">
            <div class="messages"></div>
            <button type="button" class="debug-close-button" style="margin-left: 5px;">Close</button>
        </div>
    </form>

    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/js/bootstrap.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/lib/moment/moment.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/scripts/servertime.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/scripts/menu.js?v=20180404"></script>
    <script src="scripts/jquery.alerts/jquery.alerts.js?v=2"></script>
    <script src="scripts/modernizr-2.8.3.js"></script>

    <script>
        //display any alerts
        $('.alerts').alerts({
            "url": "/alerts.js",
            "location": "menu"
        });

        //called when menu items are selected - causes the iframe to navigate to the selected location
        function menuNav(url, target) {
            if (target)
                window.open(url, target);
            else
                $('#content').attr("src", url);
        }

        //when there is a ?view querystring parameter the path it is written to a hidden input, this will cause the iframe to navigate to this location
        function viewNav() {
            var url = $(".view-url").val();
            if (url) menuNav(url);
        }

        viewNav();

        if ($(".password-redirect").val() == "true") {
            $('#content').attr('src', '/sseluser/ChangePassword.aspx?ForceChange=True');
        }

        if (Modernizr.flexbox)
            $(".debug .messages").append("<div>flexbox</div>");
        else
            $(".debug .messages").append("<div>no-flexbox</div>");

        if (Modernizr.flexboxlegacy)
            $(".debug .messages").append("<div>flexboxlegacy</div>");
        else
            $(".debug .messages").append("<div>no-flexboxlegacy</div>");

        $(".debug .messages").append($("<div/>").html(window.navigator.userAgent));

        $(".debug .messages").append($("<div/>").html("[top] " + window.location.href));
        $(".debug .messages").append($("<div/>").html("[iframe] " + $("#content").prop("src")));
        $(".debug .messages").append($("<div/>").html($(".ip-addr").val()));

        $(".current-user").on("click", function (e) {
            $(".debug").show();
        });

        $(".debug-close-button").on("click", function (e) {
            $(".debug").hide();
        });
    </script>
</body>
</html>
