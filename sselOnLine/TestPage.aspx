<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="sselOnLine.TestPage" Async="true" %>

<!--%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %-->
<%@ Register TagPrefix="lnf" Namespace="LNF.Web.Controls.Tools" Assembly="LNF.Web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test - Home</title>

    <link rel="stylesheet" href="styles/default.css" />
    <link rel="stylesheet" href="styles/testing.css" />

    <style>
        body {
            font-family: Arial;
        }

        .middle > * {
            vertical-align: middle;
        }

        .middle > h2 {
            margin: 0;
            padding-left: 20px;
            font-size: 40pt;
            display: inline-block;
        }

        input {
            font-size: 12pt;
            padding: 4px;
        }

        .question {
            margin-bottom: 10px;
        }

        .choice {
            padding-left: 15px;
        }

            .choice.incorrect {
                font-weight: bold;
                color: #ff0000;
            }

            .choice.correct {
                font-weight: bold;
                color: #008000;
            }
    </style>

    <script src="//www.google.com/recaptcha/api.js"></script>
</head>
<body style="padding: 10px;">
    <div id="testdiv" style="">
        <form id="form1" runat="server">
            <div class="middle">
                <img alt="LNF" src="//ssel-apps.eecs.umich.edu/static/images/lnf-logo.png" />
                <h2>
                    <asp:Literal runat="server" ID="litTestTitle"></asp:Literal>
                </h2>
            </div>
            <hr />
            <asp:Panel runat="server" ID="panTestStart" Visible="true">
                <div>
                    <table class="userinput">
                        <tr runat="server" id="trUserName">
                            <td style="vertical-align: middle;" class="col0">
                                <strong>LNF Username:</strong>
                            </td>
                            <td style="vertical-align: middle;" class="col1">
                                <asp:Literal runat="server" ID="litUserName"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle;" class="col0">
                                <strong>First Name:</strong>
                            </td>
                            <td style="vertical-align: middle;" class="col1">
                                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="rfv2" runat="server" ErrorMessage="* Required" ControlToValidate="txtFirstName" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle;" class="col0">
                                <strong>Last Name:</strong>
                            </td>
                            <td style="vertical-align: middle;" class="col1">
                                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* Required" ControlToValidate="txtLastName" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle;" class="col0">
                                <strong>Email:</strong>
                            </td>
                            <td style="vertical-align: middle;" class="col1">
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* Required" ControlToValidate="txtEmail" CssClass="validation-error"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle;" class="col0">
                                <strong>Organization or Research Group:</strong>
                            </td>
                            <td style="vertical-align: middle;" class="col1">
                                <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>&nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-bottom: 20px;">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red" Font-Bold="true"></asp:Label><br />
                    <p>This is an open book exam. All of the materials you will need to complete the test are located on the <a href="http://lnf-wiki.eecs.umich.edu/wiki/User_Resources">LNF Wiki</a>.</p>
                    <p><strong>Attention:</strong> Each test page has a <%=GetExamTime()%> minute limit. If you do not submit your answers within <%=GetExamTime()%> minutes, the session will time out and your data will be lost.</p>
                    <p>If you are NOT planning to use the LNF, please email Sandrine Martin (<a href="mailto:sandrine@umich.edu">sandrine@umich.edu</a>) and let her know why you need the test.</p>
                </div>
                <div runat="server" id="panRecaptcha" class="captcha" style="width: 650px;">
                    <div style="margin-bottom: 10px;">
                        We are using <a href="https://www.google.com/recaptcha">Google reCAPTCHA</a> to prevent spam and other unwanted access.
                    </div>
                    <lnf:GoogleRecaptcha runat="server" ID="GoogleRecaptcha1" />
                    <div style="margin-top: 10px;">
                        <asp:Label runat="server" ID="lblCaptchaError" ForeColor="#FF0000" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div style="padding-top: 20px;">
                    <asp:Button runat="server" ID="btnStart" Text="Start the Test" OnClick="btnStart_Click" />
                </div>
                <div id="email_test" style="padding-top: 20px; display: none;">
                    <asp:Button runat="server" ID="btnSendTestEmail" Text="Send Test Email" OnClick="btnSendTestEmail_Click" />
                    <asp:Literal runat="server" ID="litSendTestEmailMessage"></asp:Literal>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panTestPage" Visible="false">
                <div class="test-warning" style="margin-bottom: 20px; color: Red;">
                    Please note: You must finish this test in <%=GetExamTime()%> minutes. If you use the arrow keys while taking the test, it will change your selected answers
                </div>
                <asp:Repeater runat="server" ID="rptQuestions" OnItemDataBound="rptQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <ol class="test-list" start="<%#GetQuestionStartNumber()%>">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li>
                            <span><%# Eval("QuestionText")%></span>
                            <asp:RadioButtonList runat="server" ID="rblChoices" DataTextField="ChoiceDisplayText" DataValueField="ChoiceIndex">
                            </asp:RadioButtonList>
                            <asp:HiddenField runat="server" ID="hidQuestionNumber" />
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ol>
                    </FooterTemplate>
                </asp:Repeater>
                <div style="margin: 10px;">
                    <asp:Button ID="btnNext" runat="server" Text="Submit" OnClick="btnNext_Click" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panTimeLimit" Visible="false">
                <div class="error">
                    You took longer than <%=GetExamTime()%> minutes to complete this page. Please
                    <asp:HyperLink runat="server" ID="hypStartOver1">click here to start over</asp:HyperLink>.
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panWrongAnswers" Visible="false">
                <div class="error">
                    <h2><asp:Literal runat="server" ID="litFailedMessage"></asp:Literal></h2>
                    <asp:HyperLink runat="server" ID="hypStartOver2">Click here to start over</asp:HyperLink>.
                <asp:Literal runat="server" ID="litWrongAnswers"></asp:Literal>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="panPassedMessage" Visible="false">
                <asp:Literal runat="server" ID="litTestResults"></asp:Literal>
                <br />
                <br />
                <asp:Literal runat="server" ID="litPassedMessage"></asp:Literal>
            </asp:Panel>
        </form>
    </div>

    <script src="<%=GetStaticUrl("lib/jquery/jquery.min.js")%>"></script>
    <script src="scripts/main.js"></script>
</body>
</html>
