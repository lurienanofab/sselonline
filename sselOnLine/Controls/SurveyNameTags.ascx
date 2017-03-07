<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyNameTags.ascx.cs" Inherits="sselOnLine.Controls.SurveyNameTags" %>
<asp:Panel runat="server" ID="panMain" Visible="false">

    <div class="survey">
        <div class="survey-text">


            <p style="color: red">
                Attention: If you have already filled out this survey but it is still displayed please resubmit.An issue that prevented saving survey results for some users has been fixed.
            </p>
            <p>
                Reminder: All users in the LNF between 6pm and 6am and on weekends will have to wear a nametag on the back of their suits, both in the clean room and in the wet chemistry. Please provide your feedback below (even if you have already emailed Dennis Schweiger):
            </p>
            <table>
                <tr>
                    <td>
                        <input runat="server" type="checkbox" id="chkCleanRoom" class="chkbox" />
                        I need a name tag for the cleanroom
                    </td>
                </tr>
                <tr>
                    <td>
                        <input runat="server" type="checkbox" id="chkWetChemistry" class="chkbox" />
                        I need a name tag for the wet chemistry lab
                    </td>
                </tr>
                <tr>
                    <td>
                        <input runat="server" type="checkbox" id="chkDayTimeOnly" class="chkbox" />
                        I only work during day time (M-F 6am-6pm) and do not want a name tag
                    </td>
                </tr>
            </table>
            <div class="survey-text">
                <asp:Button ID="btnSubmit" runat="server" Text="Agree" CssClass="sursubmit" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('.sursubmit').attr("disabled", true);

            $('.chkbox').click(function () {
                var anythingChecked = false;
                $('.chkbox').each(function () {
                    if (this.checked) {
                        anythingChecked = true;
                    }
                });
                if (anythingChecked) {
                    $('.sursubmit').removeAttr("disabled");
                } else {
                    $('.sursubmit').attr("disabled", true);
                }
            });
        });
    </script>
</asp:Panel>
