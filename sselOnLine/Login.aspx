<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="sselOnLine.Login" %>

<%@ Register Src="~/Controls/AjaxDataSource.ascx" TagName="AjaxDataSource" TagPrefix="uc" %>
<%@ Register Src="~/Controls/Meter.ascx" TagName="Meter" TagPrefix="uc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title>LNF Online Services</title>

    <link rel="stylesheet" href="styles/default.css" />
    <link rel="stylesheet" href="styles/meter.css" />
</head>
<body style="background-color: #5050ff; text-align: center; margin: 0px auto; width: 100%; height: 100%; position: fixed; display: table;">
    <div style="display: table-cell; vertical-align: middle;">
        <uc:AjaxDataSource runat="server" ID="AjaxDataSource1" URL="/data/?q=meter" RefreshInterval="2000" Enabled="false" />
        <uc:Meter runat="server" ID="diwater_meter" Group="DI Water" GroupTag="DiWater" AjaxDataSourceID="AjaxDataSource1" Visible="false" />
        <div style="position: relative; visibility: visible; top: 0px; left: 0px; display: block; margin: 0px auto; width: 1024px; height: 768px; background-image: url('images/LNFLoginScreen.jpg'); background-repeat: no-repeat;">
            <form id="form1" runat="server">
                <div style="position: absolute; left: 480px; top: 140px; color: #FFFFFF; font-size: 14pt; font-family: Arial; font-weight: bold;">
                    <asp:CheckBox runat="server" ID="chkKiosk" Visible="false" Text="Kiosk Login" />
                </div>
                <div style="text-align: left; position: absolute; left: 480px; top: 174px; margin: 0px auto;">
                    <asp:TextBox ID="txtUsername" TabIndex="1" runat="server" MaxLength="25" Width="180px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ForeColor="#ff7f7f" CssClass="WarningText" ControlToValidate="txtUsername" ErrorMessage="RequiredFieldValidator" Width="232px" Height="24px">Please enter your username</asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" MaxLength="25" TextMode="Password" Width="180px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv2" runat="server" ForeColor="#ff7f7f" CssClass="WarningText" ControlToValidate="txtPassword" ErrorMessage="RequiredFieldValidator" Width="234px" Height="24px">Please enter your password</asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:ImageButton ID="btnEnter" ImageUrl="~/images/enter_button.jpg" TabIndex="3" runat="server" OnClick="btnEnter_Click"></asp:ImageButton>
                    <br />
                    <br />
                    <asp:Label ID="lblError" runat="server" CssClass="WarningText" Width="754px" Height="8px" Visible="False">Login failure</asp:Label>
                    <asp:Label ID="lblKiosk" runat="server" CssClass="KioskLabel" Width="216px" Visible="False">Label</asp:Label>
                    <asp:Literal runat="server" ID="litDebug"></asp:Literal>
                </div>
                <asp:Panel runat="server" ID="panLoginError" Visible="false">
                    <div style="background-color: #FFFFFF; border: 2px solid #808080; font-family: arial; font-size: 12pt; position: relative; top: 140px; margin: 0 auto; z-index: 999; width: 500px;">
                        <div style="padding: 5px; font-weight: bold; background-color: #D4D4D4; border-bottom: 1px solid #808080; text-align: left;">
                            Login Error
                        </div>
                        <div style="padding: 5px; border-top: solid 1px #DADADA;">
                            <asp:Literal runat="server" ID="litLoginErrorMessage"></asp:Literal>
                            <div style="padding-top: 10px; padding-bottom: 10px;">
                                <asp:Button runat="server" ID="btnLoginErrorOK" Text="OK" OnClick="btnLoginErrorOK_Click" Width="100" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
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
        </div>
    </div>

    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>

    <script>
        //this ensures the menu will not be displayed above the login form
        if (window.location != window.top.location) {
            window.top.location = window.location;
        }
    </script>
</body>
</html>
