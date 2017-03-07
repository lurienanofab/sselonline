<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewVideo.aspx.cs" Inherits="sselOnLine.ViewVideo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Video</title>

    <link rel="stylesheet" href="styles/default.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family: ''Arial Calibri''; font-size: medium; font-weight: bold;">
            <h3 style="color: #FF0000;">Attention!</h3>
            You have been directed to this page because all LNF lab users are required to view training 
            video & slides related to social and ethical issues in Nanotechnology.  
            <br />
            You MUST view the video no later than Apr 30, 2008.
            <br />
            <br />
            If you choose to view the video and slides now (you need to have speakers ready, and it will take about 20 minutes), please click 
            <asp:LinkButton ID="btnView" runat="server" Text="here" OnClick="btnView_Click"></asp:LinkButton>.<br />
            <br />
            If you wish to view it later, please click <a href="/sselOnLine">here</a>.
        </div>
    </form>
</body>
</html>
