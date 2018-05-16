<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="sselOnLine.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>LNF Online Services</title>

    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/styles/bootstrap/navbar-fixed-top.css" />
    <link rel="stylesheet" href="//ssel-apps.eecs.umich.edu/static/styles/bootstrap/menu.css" />
    <link rel="stylesheet" href="scripts/jquery.alerts/jquery.alerts.css?v=2" />

    <style>
        html, body, form {
            height: 100%;
        }

        .content-wrapper {
            position: fixed;
            width: 100%;
            height: 100%;
        }

        #content {
            display: block;
            width: 100%;
            height: 100%;
            border: none;
        }
    </style>

    <script src="//ssel-apps.eecs.umich.edu/static/scripts/idle-timeout.js"></script>

    <script>
        var url = '<%=string.Format(ConfigurationManager.AppSettings["ScreensaverUrl"], Request.QueryString["timeout"], Request.QueryString["room"])%>';
        var idleTimeout = new IdleTimeout(url);

        function frameLoad(f) {
            // go to screensaver if there is no activity for awhile (only enabled when timeout parameter is in querystring)
            var fdoc = f.contentDocument || f.contentWindow.document;
            idleTimeout.watch(fdoc, "FRAME");

            // this will resize the frame every time the iframe loads
            var event = document.createEvent("HTMLEvents");
            event.initEvent("resize", true, false);
            window.dispatchEvent(event);
        }

        idleTimeout.watch(window, "PARENT");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="hidPasswordRedirect" class="password-redirect" value="false" />
        <input type="hidden" runat="server" id="hidViewUrl" class="view-url" />

        <lnf:SiteMenu runat="server" ID="SiteMenu1" Target="content" />
        <div class="alerts"></div>

        <div class="content-wrapper">
            <iframe id="content" name="content" src="Loader.ashx" onload="frameLoad(this);" style="width: 100%; border: none;"></iframe>
        </div>
    </form>

    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/js/bootstrap.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/lib/moment/moment.min.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/scripts/servertime.js"></script>
    <script src="//ssel-apps.eecs.umich.edu/static/scripts/menu.js?v=20180404"></script>
    <script src="scripts/jquery.alerts/jquery.alerts.js?v=2"></script>

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
    </script>
</body>
</html>
