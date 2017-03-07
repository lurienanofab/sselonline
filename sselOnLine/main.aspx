<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="sselOnLine.main" %>

<%@ Import Namespace="LNF" %>
<%@ Import Namespace="LNF.Repository.Data" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>LNF Online Services</title>

    <!-- Bootstrap -->
    <link href="<%=GetStaticUrl("styles/bootstrap/themes/lnf/bootstrap.min.css")%>" rel="stylesheet" />

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

    <style>
        .navbar.navbar-lnf {
            min-height: 68px;
            margin-bottom: 0;
            color: #304797;
            font-family: Arial;
            font-weight: bold;
            font-size: 10pt;
            background: url(images/menubg.gif) repeat-x scroll 0% 50% #ffffff;
        }

            .navbar.navbar-lnf .navbar-brand {
                padding: 0;
            }

            .navbar.navbar-lnf .navbar-nav > li > a {
                color: #304797;
                line-height: 33px;
            }

            .navbar.navbar-lnf .user-info {
                padding: 10px 10px 10px 30px;
                text-align: center;
            }

        .navbar.navbar-default.navbar-lnf {
            background-color: #fff;
        }

        .navbar.navbar-lnf.navbar-default .navbar-nav > .open > a,
        .navbar.navbar-lnf.navbar-default .navbar-nav > .open > a:hover,
        .navbar.navbar-lnf.navbar-default .navbar-nav > .open > a:focus {
            background-color: rgba(255,255,0, 0.15);
        }

        .navbar.navbar-default.navbar-lnf .navbar-toggle {
            margin-top: 15px;
            border-color: #999;
        }

        .navbar.navbar-default.navbar-lnf .navbar-toggle:hover,
        .navbar.navbar-default.navbar-lnf .navbar-toggle:focus{
            background-color: rgba(221,221,221,0.5);
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-default navbar-lnf">
        <div class="container-fluid">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="<%=VirtualPathUtility.ToAbsolute("~") %>">
                    <img alt="LNF" src="<%=GetStaticUrl("images/lnfbanner.jpg")%>" />
                </a>
            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Applications <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#">Scheduler</a></li>
                            <li><a href="#">Store</a></li>
                            <li><a href="#">IOF 2.0</a></li>
                            <li><a href="#">Control</a></li>
                            <li><a href="#">Inventory</a></li>
                            <li><a href="#">User Data</a></li>
                            <li><a href="#">Ext Website</a></li>
                            <li><a href="#">Feedback</a></li>
                            <li><a href="#">Mass Email</a></li>
                            <li><a href="#">Dry Box</a></li>
                            <li><a href="#">File Storage</a></li>
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Reports <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#">User Reports</a></li>
                            <li><a href="#">Resource Reports</a></li>
                            <li><a href="#">Store Reports</a></li>
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Administration <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#">Data Entry</a></li>
                            <li><a href="#">Fin Ops</a></li>
                            <li><a href="#">Control Admin</a></li>
                            <li><a href="#">Misc</a></li>
                            <li><a href="#">IT Task Manager</a></li>
                        </ul>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">Help <span class="caret"></span></a>
                        <ul class="dropdown-menu" role="menu">
                            <li><a href="#">General Help</a></li>
                            <li><a href="#">User Fees</a></li>
                            <li><a href="#">User Committee</a></li>
                            <li><a href="#">Staff Directory</a></li>
                            <li><a href="#">Staff Calendar</a></li>
                            <li><a href="#">Facility Calendar</a></li>
                            <li><a href="#">Helpdesk</a></li>
                        </ul>
                    </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li><a href="<%=Providers.Context.LoginUrl%>">Logout</a></li>
                    <li>
                        <div class="user-info">
                            <div class="user-name"><%=Client.Current.DisplayName%></div>
                            <div class="server-time"></div>
                        </div>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->

            <div class="collapse navbar-collapse navbar-right" id="bs-example-navbar-collapse-2">
                test
            </div>

        </div>
        <!-- /.container-fluid -->
    </nav>

    <iframe src="Blank.aspx" style="width: 100%;" id="frame1"></iframe>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="<%=GetStaticUrl("lib/jquery/jquery.min.js")%>"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="<%=GetStaticUrl("lib/bootstrap/js/bootstrap.min.js")%>"></script>

    <script src="<%=GetStaticUrl("lib/jclock/jquery.jclock.js")%>"></script>

    <script>
        //adding here so intellisense doesn't complain
        $('#frame1').prop("frameborder", "0");

        //starts the menu clock
        var seed = new Date();
        $('.server-time').jclock({ "format": "%i:%M:%S %P on %A, %B %d", "seedTime": seed.getTime() });
    </script>
</body>
</html>
