<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureContest.aspx.cs" Inherits="sselOnLine.PictureContest" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->

    <title>LNF Picture Contest</title>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="<%=GetStaticUrl("lib/bootstrap/css/bootstrap.min.css")%>">

    <!-- Optional theme -->
    <link rel="stylesheet" href="<%=GetStaticUrl("lib/bootstrap/css/bootstrap-theme.min.css")%>">

    <link rel="stylesheet" href="styles/picturecontest.css">
</head>
<body>

    <div class="container">
        <div class="jumbotron" style="margin-top: 10px;">
            <h1>
                <asp:Literal runat="server" ID="litContestTitle"></asp:Literal>
            </h1>
            <p>Upload your picture and vote for your favorite</p>
        </div>
        <form id="form1" runat="server">
            <asp:Panel runat="server" ID="panAdmin" Visible="false" CssClass="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Contest Admin</h3>
                </div>
                <div class="panel-body">

                    <div style="margin-bottom: 20px;">
                        <em class="text-muted">You are a contest administrator so you can select other users.</em>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-1">User</label>
                            <div class="col-sm-6">
                                <asp:DropDownList runat="server" ID="ddlUser" DataTextField="DisplayName" DataValueField="ClientID" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"></asp:DropDownList>
                                <div style="margin-top: 10px;">
                                    <asp:RadioButtonList runat="server" ID="rblUsersOption" AutoPostBack="true" OnSelectedIndexChanged="rblUsersOption_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Selected="True">All users</asp:ListItem>
                                        <asp:ListItem Value="2">Users with image uploads</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <a href="PictureContestResults.aspx" class="btn btn-default">View Results</a>
                </div>
            </asp:Panel>

            <h3>Contest Rules</h3>
            <ul>
                <li>You may upload
                    <asp:Literal runat="server" ID="litMaxUploads"></asp:Literal>. To replace an image you must delete it and then upload a new one. (Note: you can combine multiple images into one)
                </li>
                <li>The following file types are allowed:
                    <asp:Literal runat="server" ID="litAllowedFileTypes"></asp:Literal>
                </li>
                <li>The maximum file size is 30 MB.</li>
                <li>There is a
                    <asp:Literal runat="server" ID="litMaxDescriptionLength"></asp:Literal>
                    character limit for descriptions. Any text beyond this will be truncated.
                </li>
                <li>Descriptions should not contain any personal identifying information, such as your name.</li>
            </ul>

            <hr />

            <h3>Current Uploads for
                <asp:Literal runat="server" ID="litDisplayName"></asp:Literal></h3>
            <asp:Panel runat="server" ID="panNoData" Visible="false">
                <em class="text-muted">You have not uploaded an image yet. Use the uploader below to submit an image.</em>
            </asp:Panel>
            <asp:Repeater runat="server" ID="rptImages" Visible="true">
                <HeaderTemplate>
                    <table class="table table-striped">
                        <tbody>
                </HeaderTemplate>

                <ItemTemplate>
                    <tr>
                        <td style="vertical-align: middle; text-align: center; width: 30px;">
                            <%#string.Format("{0}.", Container.ItemIndex + 1)%>
                        </td>

                        <td style="vertical-align: middle; text-align: center; width: 110px;">
                            <a href="#" class="pop" data-description='<%#Eval("Description")%>'>
                                <img class="cimgpath" style="width: 100px; height: 100px;" src='<%#Eval("ImageID", VirtualPathUtility.ToAbsolute("~/PictureContest.aspx") + "?view={0}")%>' />
                            </a>
                        </td>

                        <td style="vertical-align: middle;">
                            <asp:TextBox runat="server" ID="txtDescription" class="form-control" TextMode="multiline" Columns="5" Rows="4" Text='<%#Eval("Description")%>' />
                        </td>

                        <td style="vertical-align: middle; text-align: center; width: 100px;">
                            <asp:Button ID="btnDelete" runat="server" OnCommand="btnDelete_Command" Text="Delete" class="btn btn-danger" CommandArgument='<%#Eval("ImageID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>

                <FooterTemplate>
                    </tbody>
                        </table>
                </FooterTemplate>

            </asp:Repeater>

            <asp:Button ID="btnSaveDesc" runat="server" OnClick="btnSaveDesc_Click" Text="Save Image Descriptions" class="btn btn-primary" />

            <asp:Panel runat="server" ID="panSaveMessage" Visible="false">
                <div class="alert alert-success" role="alert" style="margin-top: 20px;">
                    <asp:Literal runat="server" ID="litSaveMessageText"></asp:Literal>
                </div>
            </asp:Panel>

            <hr />

            <h3>Upload your pictures for the contest </h3>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Image Uploader</h3>
                </div>
                <div class="panel-body">
                    <asp:FileUpload ID="fileImages" AllowMultiple="true" runat="server" ErrorMessage="Only .jpg, .jpeg, .gif, .png"
                        ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([Jj][Pp][Ee][Gg])|.*\.([gG][iI][fF])|.*\.([pP][nN][gG])" class="btn btn-success" />
                    
                    <br />

                    <div style="margin-bottom: 20px;">
                        <strong>New Image Description</strong>
                        <asp:TextBox runat="server" ID="txtNewImageDescription" class="form-control" TextMode="multiline" Columns="5" Rows="4" />
                    </div>

                    <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" class="btn btn-primary" />
                    <asp:Panel runat="server" ID="panUploadError" Visible="false">
                        <div class="alert alert-danger" role="alert" style="margin-top: 20px;">
                            <asp:Literal runat="server" ID="litUploadErrorText"></asp:Literal>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </form>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="imagemodal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <div style="text-align: center;">
                        <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNgYAAAAAMAASsJTYQAAAAASUVORK5CYII=" id="imagepreview" style="width: 500px; height: 464px;">
                    </div>
                    <hr />
                    <div class="text-muted" id="imgdesc">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="<%=GetStaticUrl("lib/jquery/jquery.min.js")%>"></script>

    <!-- Latest compiled and minified JavaScript -->
    <script src="<%=GetStaticUrl("lib/bootstrap/js/bootstrap.min.js")%>"></script>

    <script src="<%=GetStaticUrl("lib/jquery-ui/jquery-ui.min.js")%>"></script>

    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script>
        $(".pop").on("click", function (e) {
            e.preventDefault();

            //$('#imagepreview').attr('src', $('.cimgpath').attr('src'));
            $('#imagepreview').attr('src', $(this).find('.cimgpath').attr('src'));
            $('#imagemodal').modal('show');

            $("#imgdesc").html($(this).data('description'));
        });
    </script>
</body>
</html>
