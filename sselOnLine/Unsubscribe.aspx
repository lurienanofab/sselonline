<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unsubscribe.aspx.cs" Inherits="sselOnLine.Unsubscribe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LNF User List: Unsubscribe</title>
    
    <link rel="stylesheet" href="styles/default.css" />

    <style>
        .box {
            background-color: #CCCCCC;
            padding: 10px;
            border: 1px solid #808080;
            margin: 0 auto;
            margin-top: 50px;
            width: 500px;
            font-family: Arial;
        }

            .box textarea {
                width: 400px;
                height: 120px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="box">
            <h3>You are about to unsubscribe from LNF User List</h3>
            Sometimes we have to send emergency emails to all active users of lab.<br />
            Please tell us why you want to be removed from our email list:<br />
            <asp:TextBox ID="txtReason" runat="server" TextMode="multiLine"></asp:TextBox>
            <br />
            <br />
            Email:&nbsp;<asp:TextBox ID="txtEmail" runat="server" Width="350" />
            <br />
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Unsubscribe" OnClick="btnSubmit_Click" />
            <br />
            <br />
            <div style="color: #FF0000;">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
