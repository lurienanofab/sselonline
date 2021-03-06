﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureContestSurvey.aspx.cs" Inherits="sselOnLine.PictureContestSurvey" %>

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

    <style>
        table.table td {
            border-bottom: 1px solid #ddd;
        }

        table.table tr:last-child td {
            border-bottom: none;
        }
    </style>
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
            <asp:PlaceHolder runat="server" ID="phNoVoting">
                <div class="alert alert-danger" role="alert">
                    Voting is not allowed at this time. <a href="PictureContest.aspx">Click here</a> to submit an image.
                </div>
            </asp:PlaceHolder>

            <asp:PlaceHolder runat="server" ID="phVoting">
                <div style="margin-bottom: 10px;">
                    <strong>Please select up to three pictures from below. You can click on a thumbnail to view a larger image and read a short description.</strong>
                </div>

                <asp:Repeater runat="server" ID="rptImages" Visible="true">
                    <HeaderTemplate>
                        <div style="border-bottom: solid 1px #ddd; margin-bottom: 10px;">
                            <table class="table">
                                <tbody>
                                    <tr>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <td style="vertical-align: middle; text-align: center;">
                            <p>
                                <a href="#" class="pop" data-description='<%#Eval("Description")%>'>
                                    <img class="cimgpath" style="width: 100px; height: 100px;" src='<%#Eval("ImageID", VirtualPathUtility.ToAbsolute("~/PictureContest.aspx") + "?view={0}")%>' />
                                </a>
                            </p>
                            <asp:HiddenField runat="server" ID="hiddenImageID" Value='<%#Eval("ImageID")%>' />
                            <input type="checkbox" runat="server" id="chkVote" class="chkVote" />
                        </td>
                        <%# Container.ItemIndex > 0 && Container.ItemIndex % 5 == 4 ? "</tr><tr>" : "" %>
                    </ItemTemplate>

                    <FooterTemplate>
                        </tr>
                    </tbody>
                        </table>
                    </div>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Button ID="btnSaveVoting" runat="server" OnClick="btnSaveVoting_Click" Text="Submit Voting" class="btn btn-primary" />
                <asp:Literal ID="litSaveMessage" runat="server"></asp:Literal>
            </asp:PlaceHolder>
        </form>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="imagemodal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <div style="text-align: center;">
                        <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNgYAAAAAMAASsJTYQAAAAASUVORK5CYII=" id="imagepreview" style="width: 100%; height: 100%;">
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

    <script>
        $(".pop").on("click", function (e) {
            e.preventDefault();

            $('#imagepreview').attr('src', $(this).find('.cimgpath').attr('src'));
            $('#imagemodal').modal('show');

            $("#imgdesc").html($(this).data('description'));
        });

        $('.chkVote').change(function () {
            if ($(this).prop("checked")) {
                var totalChecked = $('input.chkVote:checked').length;
                if (totalChecked > 3) {
                    alert('Please select only 3 pictures');
                    $(this).prop("checked", false);
                }
            }
        });

        /*$( "a.pop" ).each(function( index ) {  
          console.log( index + ": " + $( this ).attr('data-description') );
        });*/

        var rtd = function (ra) {
            $(ra).parent().parent().remove();
        };

        $("a.pop").each(function (index) {

            //console.log( index + ": " + $( this ).attr('data-description') );  
            var s = $(this).attr('data-description');
            if (s.match("^Modern art generated by")) { rtd($(this)); }
            if (s.match("^Montassar Aidi Sharif")) { rtd($(this)); }

        })

        var all = $("a.pop");
        var td17 = $(all[17]).closest('td');
        var tab = $(td17).closest('table');
        var firstrow = $(tab).children().children()[0];
        $(firstrow).append($(td17));
    </script>
</body>
</html>
