<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginRequirement.ascx.cs" Inherits="sselOnLine.Controls.LoginRequirement" %>
<asp:HiddenField runat="server" ID="hidClientLoginRequirementID" />
<asp:Panel runat="server" ID="RequiredQuestions" Visible="false" CssClass="required_questions">
    <ol>
        <li>Do you have publications (conferences, presentations, peer-reviewed journals, etc.) and/or patents for the July 2009 - June 2010 period, related to or making use of work performed in the LNF?
            <asp:RadioButtonList runat="server" ID="rblQ1">
                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
            </asp:RadioButtonList>
        </li>
        <li>Have you submitted a project summary (using <a href="/sselOnLine/files/NNIN_template_for_highlights_2010.ppt">this</a> template) in the last six months?
            <asp:RadioButtonList runat="server" ID="rblQ2">
                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Text="No" Value="No" Selected="True"></asp:ListItem>
            </asp:RadioButtonList>
        </li>
    </ol>
    <div style="margin: 20px 0px 10px 40px;">
        <asp:Button runat="server" ID="btnRequiredOK" Text="OK" Width="80" OnClick="btnRequiredOK_Click" />
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="RequiredInput" CssClass="required_questions" Visible="false">
    <div class="required_input">
        <div style="padding: 10px 20px 0px 0px;">
            Dear LNF Users:<br />
            <br />
            A few weeks ago you received <a href="#" onclick="show_msg();">this email message</a> requesting updated information on publications, presentations, or summaries.<br />
            <br />
            So far we have received few responses.<br />
            <br />
            We need this information and need your help in getting this information. We are required by NSF to report on all publications that made use of the LNF, and all such publications should acknowledge NSF/NNIN support. Without the NNIN, much of what you are trying to do in the LNF would not be possible.<br />
            <br />
            <span style="color: #FF0000;">I encourage you to send the requested information, asap, but no later than Monday August 9, since we are already past the deadline for getting this information. One of the conditions of using the LNF is to acknowledge the NSF support and to supply this information. Without it we might lose the NNIN support, and this will raise lab fees for everyone who is working in the lab.</span><br />
            <br />
            Thank you for your help!<br />
            <br />
            Khalil Najafi<br />
            NNIN Director
        </div>
        <div style="margin-top: 20px;">
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:CheckBox runat="server" ID="chkAcknowledge" AutoPostBack="true" OnCheckedChanged="chkAcknowledge_CheckChanged" />
                    </td>
                    <td style="padding-right: 10px;">I acknowledge that I have read the above message and will provide the requested information using the form below or by email to <a href="mailto:LNFreports@umich.edu?subject=Publications, Presentations and Summaries - <%= DisplayName %>">LNFreports@umich.edu</a>.<br />
                        <b>I understand that failure to respond will result in automatic suspension of my OnLine Services account.</b>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 10px; margin-top: 10px; margin-left: 10px; background-color: #CCCCCC; border: 1px solid #808080; width: 420px;">
            You may upload your project summary (use <a href="/sselOnLine/files/NNIN_template_for_highlights_2010.ppt">this</a> template) and/or publication or manualy enter citations below.
            <div style="margin-top: 10px;">
                <div>
                    <asp:FileUpload runat="server" ID="fupResponse1" />
                </div>
                <div style="margin-top: 5px;">
                    <asp:FileUpload runat="server" ID="fupResponse2" />
                </div>
            </div>
            <div style="margin-top: 20px;">
                <asp:TextBox runat="server" ID="txtResponse" TextMode="MultiLine" Width="400" Height="300"></asp:TextBox>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px;">
            <div>
                <asp:Literal runat="server" ID="litConfirmCheckbox"></asp:Literal>I confirm that all requested information has been submitted.
            </div>
            <div>
                <asp:Button runat="server" ID="btnConfirmOK" CssClass="confirm_btn" Text="OK" Width="80" Enabled="false" OnClick="btnConfirmOK_Click" />
            </div>
        </div>
    </div>
    <div class="email" style="display: none; background-color: #F5F5F5; z-index: 999;">
        <div style="padding: 10px;">
            Dear LNF Users:<br />
            <br />
            As part of the National Nanotechnology Infrastructure Network (NNIN) program, the NSF requests every year updated information about the work being done in the LNF. It is very critical for us that these lists accurately reflect the work that our lab users are doing in the facility, both onsite and remotely. Please note that in the past, we were criticized for low reporting levels and we need to do a much better job.<br />
            <br />
            <b>Here is what we need:</b><br />
            <br />
            <div style="padding-left: 40px;">
                1) an updated list of all publications (conferences presentations, peer-reviewed journals, etc.) and patents for the July 2009-June 2010 period, related to or making use of work performed in the LNF. In addition, if some of your publications are highlighted on a journal cover, please also send it to us.
                <div style="padding-left: 40px;">
                    <b>Proper format:</b><br />
                    <br />
                    H. Lin, H. Liu, A. Kumar, U. Avci, J. S. Van Delden and S. Tiwari, ”Strained Si Channel Super-Self-Aligned Back-Gate/Double-Gate Planar Transistors,” IEEE Electron Device Letters, 28, June, 506(2007). <i>Be sure to include the full citation, including title and date.</i><br />
                    <br />
                </div>
                2) an updated project description using the <a href="/sselOnLine/files/NNIN_template_for_highlights_2010.ppt">attached template</a>.
            </div>
            <br />
            <br />
            Please send me this information by email to LNFreports@umich.edu before <b>Monday August 2nd.</b><br />
            <br />
            This applies to any work that fully or partially used the LNF resources. If you have not published anything in this period, or if none of your publications make use of the work that you performed at the LNF, please let us know anyway, to avoid multiple reminders.<br />
            <br />
            This is a very important and time critical request and we very appreciate your attention to it.<br />
            <br />
            Thank you for your help and support!
            <div style="text-align: center; padding-top: 10px;">
                <input type="button" value="Close" onclick="close_msg();" />
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="KioskMessage" Visible="false" CssClass="kiosk_message">
    <div style="text-align: center; color: #FF0000; padding-bottom: 5px;">
        **** ATTENTION ****
    </div>
    An urgent message must be acknowledged by logging into your online services account from a non-kiosk terminal.<br />
    <br />
    You have
    <asp:Literal runat="server" ID="litRemainingLoginAttempts"></asp:Literal>&nbsp; kiosk logins remaining to complete this task before your account will be locked out.
    <div style="text-align: center; padding: 10px 10px 0px 10px;">
        <asp:Button runat="server" ID="btnKioskOK" Text="OK" Width="60" OnClick="btnKioskOK_Click" />
    </div>
</asp:Panel>

<script>
    function show_msg() {
        $('.email').css({
            display: 'block',
            position: 'absolute',
            width: '700px',
            border: 'outset',
            top: '50px',
            left: '50px'
        });
    }

    function close_msg() {
        $('.email').css('display', 'none');
    }

    function confirm_change(obj) {
        if ($(obj).attr('checked')) {
            $('.confirm_btn').removeAttr('disabled');
        }
        else {
            $('.confirm_btn').attr('disabled', 'disabled');
        }
    }
</script>
