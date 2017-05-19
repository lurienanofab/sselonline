<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestParser.aspx.cs" Inherits="sselOnLine.TestParser" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Test Parser</title>

    <!-- Bootstrap -->
    <link href="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="page-header">
                <h1>Test Parser</h1>
            </div>

            <div class="form-group">
                <label>Test</label>
                <asp:DropDownList runat="server" ID="ddlTests" CssClass="form-control" DataValueField="TestType" DataTextField="TestTitle"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label>File Upload</label>
                <asp:FileUpload runat="server" ID="fuTest" />
            </div>

            <asp:Button runat="server" ID="btnSubmit" Text="Parse" CssClass="btn btn-default" OnClick="btnParse_Click" />

            <asp:PlaceHolder runat="server" ID="phError" Visible="false">
                <div class="alert alert-danger" role="alert" style="margin-top: 20px;">
                    <asp:Literal runat="server" ID="litErrorMessage"></asp:Literal>
                </div>
            </asp:PlaceHolder>

            <div style="margin-top: 20px;">
                <textarea class="form-control" rows="30" spellcheck="false"><asp:Literal runat="server" ID="litOutput"></asp:Literal></textarea>
            </div>
        </div>
    </form>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="//ssel-apps.eecs.umich.edu/static/lib/jquery/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="//ssel-apps.eecs.umich.edu/static/lib/bootstrap/js/bootstrap.min.js"></script>
</body>
</html>

<!DOCTYPE html>
