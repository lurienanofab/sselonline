<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="sselOnLine.Test" %>

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

        ul {
            font-size: 16pt;
        }

            ul > li {
                padding-bottom: 15px;
            }

                ul > li > a:link {
                    text-decoration: none;
                }

                ul > li > a:hover {
                    text-decoration: underline;
                }
    </style>
</head>
<body style="padding: 10px;">
    <form id="form1" runat="server">
        <img alt="LNF" src="//ssel-apps.eecs.umich.edu/static/images/lnf-logo.png" />

        <hr />

        <asp:PlaceHolder runat="server" ID="phErrorMessage" Visible="false">
            <div>
                <h3>Available Tests:</h3>
                <ul>
                    <li><a href="Test.aspx?testname=TestHF">HF Test</a></li>
                    <li><a href="Test.aspx?testname=TestLNF">LNF Test</a></li>
                    <li><a href="Test.aspx?testname=TestOSEH">OSEH Test</a></li>
                </ul>
            </div>
        </asp:PlaceHolder>

        <asp:Panel class="div-full" runat="server" ID="divfull">
            <p>Are you in the process of gaining LNF access or are you taking this safety training to renew your access?</p>
            <ul style="font-size: 16pt;">
                <li>
                    <asp:LinkButton runat="server" ID="btnNewUser" OnClick="btnNewUser_Click">I am not yet an LNF user</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton runat="server" ID="btnExistingUser" OnClick="btnExistingUser_Click">I am an existing LNF user renewing my access</asp:LinkButton>
                </li>
            </ul>
        </asp:Panel>
    </form>

    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>
    <script src="scripts/main.js"></script>
</body>
</html>
