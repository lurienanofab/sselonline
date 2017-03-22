<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Blank.aspx.cs" Inherits="sselOnLine.Blank" %>

<%@ Import Namespace="LNF" %>
<%@ Import Namespace="sselOnLine" %>

<%@ Register Src="~/Controls/ScrollPositionTool.ascx" TagName="ScrollPositionTool" TagPrefix="uc" %>
<%--<%@ Register Src="~/Controls/LoginRequirement.ascx" TagName="LoginRequirement" TagPrefix="uc" %>--%>
<%@ Register Src="~/controls/SurveyMOU.ascx" TagName="SurveyMOU" TagPrefix="uc" %>
<%--<%@ Register Src="~/controls/SurveyNameTags.ascx" TagName="SurveyNameTags" TagPrefix="uc" %>--%>
<%--<%@ Register Src="~/Controls/Messenger.ascx" TagName="Messenger" TagPrefix="uc" %>--%>
<%@ Register Src="~/Controls/Survey.ascx" TagName="Survey" TagPrefix="applet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LNF Online Services</title>

    <link rel="stylesheet" href="styles/default.css" />

    <style>
        .required_questions {
            border: solid 1px #808080;
            background-color: #F5F5F5;
        }

        .required_input {
            margin-left: 20px;
        }

        .kiosk_message {
            margin-left: 200px;
            width: 400px;
            padding: 20px;
            font-weight: bold;
            background-color: #FFFFFF;
            border: solid 1px #808080;
        }

        .blank-content {
            padding-top: 10px;
            padding-left: 10px;
        }

        .survey {
            width: 800px;
            margin: 0 auto;
            margin-bottom: 20px;
            border: solid 1px #0b61a4;
        }

        .survey-title {
            background-color: #0b61a4;
            padding: 15px;
            color: #ffffff;
            text-align: left;
            font-size: 18pt;
        }

        .survey-text {
            padding: 15px;
        }
    </style>

    <link rel="stylesheet" href="styles/applet.css" />
    <link rel="stylesheet" href="styles/survey.css" />
    <link rel="stylesheet" href="scripts/jquery.alerts/jquery.alerts.css" />
    <%--<link rel="stylesheet" href="scripts/jquery.messenger/jquery.messenger.css" />--%>
    <link rel="stylesheet" href="<%=GetStaticUrl("styles/master.css")%>" />
    <link rel="stylesheet" href="<%=GetStaticUrl("lib/jquery-ui/themes/smoothness/jquery-ui.min.css") %>" />

    <script src="<%=GetStaticUrl("lib/jquery/jquery.min.js")%>"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="blank-content">
            <uc:ScrollPositionTool runat="server" ID="ScrollPositionTool1" />

            <div class="alerts"></div>

            <asp:PlaceHolder runat="server" ID="phStaging" Visible="false">
                <div style="margin-bottom: 20px;">
                    <a href="http://staging.ssel-sched.eecs.umich.edu/sselscheduler" target="_top" style="font-size: 20px; font-weight: bold; line-height: 30px;">LNF Scheduler Staging Site</a>
                </div>
            </asp:PlaceHolder>

            <div style="margin-bottom: 20px;">
                <a href="/sselscheduler" target="_top" style="font-size: 20px; font-weight: bold; line-height: 30px;">LNF Scheduler for Users on Mobile Platforms</a>
            </div>

            <div style="margin-bottom: 20px;">
                <a style="font-size: 20px; font-weight: bold; line-height: 30px;" target="_blank" href="https://calendar.google.com/calendar/embed?src=lnf.umich.edu_0r8i90tksqtv0lm7e66olv3ri4@group.calendar.google.com&ctz=America/New_York&pli=1">After-Hour Buddy Calendar</a>
            </div>

            <div style="padding: 20px; margin-bottom: 20px; border: solid 1px #aaa; border-radius: 4px; width: 350px; text-align: center; background-color: #f5f5f5;">
                <a href="PictureContest.aspx" target="_blank" style="font-size: 20px; font-weight: bold; line-height: 30px;">
                    <img src="images/picture-contest-2017-small.jpg" /><br />
                    Click here to submit your image!<br />
                    Deadline to submit - April 7, 2017
                </a>
            </div>

            <%--<uc:Messenger runat="server" ID="Messenger1" />--%>
            <%--<uc:LoginRequirement runat="server" ID="LoginRequirement1" />--%>
            <%--<uc:SurveyNameTags runat="server" ID="SuveyNameTags1" SurveyType="NAMETAGS" />--%>
            <uc:SurveyMOU runat="server" ID="SurveyMOU1" SurveyType="MOU" CutoffClientID="2385" />
            <applet:Survey runat="server" ID="Survey1" Title="LNF User Committee Survey" />
        </div>
        <asp:Literal runat="server" ID="litDebug"></asp:Literal>
        <asp:Literal runat="server" ID="litCommonToolsVersion"></asp:Literal>

        <asp:PlaceHolder runat="server" ID="phGoogleAnalytics" Visible="false">
            <script>
                var _gaq = _gaq || [];
                _gaq.push(['_setAccount', 'UA-23459384-2']);
                _gaq.push(['_trackPageview']);

                (function () {
                    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                })();
            </script>
        </asp:PlaceHolder>
    </form>

    <script src="<%=GetStaticUrl("lib/jquery/jquery.min.js")%>"></script>
    <script src="<%=GetStaticUrl("lib/jquery-ui/jquery-ui.min.js")%>"></script>
    <script src="<%=GetStaticUrl("lib/watermark/ui.watermark.js")%>"></script>
    <script src="scripts/jquery.alerts/jquery.alerts.js"></script>
    <%--<script src="scripts/jquery.messenger/jquery.messenger.js"></script>--%>
    <script src="scripts/main.js"></script>

    <script>
        $('.alerts').alerts({ "url": "/alerts.js" });
        //$('.messenger').messenger();
    </script>
</body>
</html>
