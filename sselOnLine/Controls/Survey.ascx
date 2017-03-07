<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Survey.ascx.cs" Inherits="sselOnLine.Controls.Survey" %>
<div class="applet-control">
    <asp:Panel runat="server" ID="panAppletDisabled" Visible="false">
        <div class="applet-disabled">
            <asp:LinkButton runat="server" ID="btnDisabledAdmin" Text="Survey Administration" OnClick="btnDisabledAdmin_Click" Visible="false" CssClass="admin-link"></asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="panAppletEnabled">
        <table class="applet-table">
            <tr>
                <td class="applet-container">
                    <asp:Panel runat="server" ID="panApplet" Visible="true">
                        <div class="applet">
                            <div class="title">
                                <asp:Literal runat="server" ID="litAppletTitle"></asp:Literal>
                            </div>
                            <div class="question">
                                <asp:Literal runat="server" ID="litSurveyQuestion"></asp:Literal>
                            </div>
                            <div class="answer">
                                <table class="survey-answer-table">
                                    <tr>
                                        <td style="vertical-align: bottom;">
                                            <asp:RadioButtonList runat="server" ID="rblAnswer" RepeatDirection="Vertical" RepeatLayout="Table">
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAnswer" TextMode="MultiLine" CssClass="answer-text"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnAnswser" Text="Submit Answer" OnClick="btnAnswer_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <div class="applet-error">
                                    <asp:Literal runat="server" ID="litErrorMessage"></asp:Literal>&nbsp;
                                </div>
                                <asp:Panel runat="server" ID="panAnonMessage" Visible="false">
                                    <div style="font-style: italic;">
                                        <table>
                                            <tr>
                                                <td style="vertical-align: middle;">Attach my identity to this submission</td>
                                                <td style="vertical-align: middle;">
                                                    <asp:CheckBox runat="server" ID="chkNotAnonymous" Checked="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel runat="server" ID="panAdminLink" Visible="false">
                                <div class="footer">
                                    <asp:LinkButton runat="server" ID="btnAdmin" Text="Administration" OnClick="btnAdmin_Click"></asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="panAdministration" Visible="false">
                        <div class="applet-admin">
                            <div class="admin-title">
                                <asp:Literal runat="server" ID="litAppletAdminTitle"></asp:Literal>
                                :: Administration
                            </div>
                            <div class="admin-question">
                                <div style="font-weight: bold;">
                                    New Question:
                                </div>
                                <table style="border-collapse: collapse;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="txtNewQuestion" TextMode="MultiLine" CssClass="admin-new-question-text"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">Answers:
                                            <ol class="survey-admin-answer-list">
                                                <li>
                                                    <asp:TextBox runat="server" ID="txtAnswer1" Width="250"></asp:TextBox></li>
                                                <li>
                                                    <asp:TextBox runat="server" ID="txtAnswer2" Width="250"></asp:TextBox></li>
                                                <li>
                                                    <asp:TextBox runat="server" ID="txtAnswer3" Width="250"></asp:TextBox></li>
                                                <li>
                                                    <asp:TextBox runat="server" ID="txtAnswer4" Width="250"></asp:TextBox></li>
                                                <li>
                                                    <asp:TextBox runat="server" ID="txtAnswer5" Width="250"></asp:TextBox></li>
                                            </ol>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom: 10px; width: 70px;">
                                            <asp:Button runat="server" ID="btnAddQuestion" Text="Add" Width="70" OnCommand="admin_Command" CommandName="add_question" />
                                        </td>
                                        <td class="applet-error">
                                            <asp:Label runat="server" ID="lblAddErrorMsg"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel runat="server" ID="panAdminCurrentQuestion" Visible="false">
                                <div class="admin-section">
                                    <div style="font-weight: bold;">
                                        Current Question:
                                    </div>
                                    <div>
                                        <table class="survey-admin-current-question">
                                            <tr>
                                                <th style="width: 30px;">ID
                                                </th>
                                                <th style="width: 250px;">Text
                                                </th>
                                                <th style="width: 145px;">Created
                                                </th>
                                                <th style="width: 145px;">Expiration
                                                </th>
                                                <th style="width: 50px;">Active
                                                </th>
                                                <th style="width: 70px;">&nbsp;
                                                </th>
                                            </tr>
                                            <tr runat="server" id="trCurrentQuestion" visible="true">
                                                <td style="vertical-align: top;">
                                                    <asp:Literal runat="server" ID="litAdminCurrentQuestionID"></asp:Literal>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <asp:TextBox runat="server" ID="txtAdminCurrentQuestionText" TextMode="MultiLine" CssClass="admin-current-question-text"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right; vertical-align: top;">
                                                    <asp:Literal runat="server" ID="litAdminCurrentQuestionCreatedDate"></asp:Literal>
                                                </td>
                                                <td style="text-align: right; vertical-align: top;">
                                                    <asp:TextBox runat="server" ID="txtAdminCurrentQuestionExpirationDate"></asp:TextBox>
                                                </td>
                                                <td style="text-align: center; vertical-align: top;">
                                                    <asp:CheckBox runat="server" ID="chkAdminCurrentQuestionActive" />
                                                </td>
                                                <td style="text-align: center; vertical-align: top;">
                                                    <asp:Button runat="server" ID="btnAdminCurrentQuestionSave" Text="Save" OnCommand="admin_Command" CommandName="save_current" />
                                                </td>
                                                <td style="text-align: center; vertical-align: top;"></td>
                                            </tr>
                                            <tr runat="server" id="trNoData" visible="false">
                                                <td colspan="7" style="font-style: italic;">There is no current question available.</td>
                                            </tr>
                                        </table>
                                        <asp:Literal runat="server" ID="litAdminQuestionSaveError"></asp:Literal>
                                    </div>
                                </div>
                                <div class="admin-section">
                                    <div style="font-weight: bold; display: inline;">
                                        Report:
                                    </div>
                                    <asp:LinkButton runat="server" ID="btnAdminAllQuestionsReport" Text="All" OnCommand="report_Command" CommandName="all" />
                                    |
                                    <asp:LinkButton runat="server" ID="btnAdminCurrentQuestionReport" Text="Current" OnCommand="report_Command" CommandName="byid" />
                                    |
                                    <asp:DropDownList runat="server" ID="ddlReportSelectQuestion" CssClass="report-question-select" AutoPostBack="true" OnSelectedIndexChanged="ddlReportSelectQuestion_SelectedIndexChanged" DataTextField="QuestionText" DataValueField="SurveyQuestionID">
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>
                            <div class="admin-section">
                                <div style="font-weight: bold;">
                                    Properties:
                                </div>
                                <table class="survey-admin-current-question">
                                    <tr>
                                        <th style="text-align: right; width: 100px; vertical-align: middle;">Anonymous</th>
                                        <td style="border: solid 1px #D2D2D2; vertical-align: middle;">
                                            <asp:CheckBox runat="server" ID="chkAnonymous" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="text-align: right; vertical-align: middle;">Enabled</th>
                                        <td style="border: solid 1px #D2D2D2; vertical-align: middle;">
                                            <asp:CheckBox runat="server" ID="chkEnabled" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center; padding-left: 0px; border: solid 1px #D2D2D2; vertical-align: middle;">
                                            <asp:Button runat="server" ID="btnSaveProperties" Text="Save" OnCommand="admin_Command" CommandName="save_properties" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="admin-section">
                                <asp:LinkButton runat="server" ID="btnAdminReturn" Text="Return" OnClick="btnAdminReturn_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script>
        $(document).ready(function () {
            $(window).focus(function () {
                $('.report-question-select').attr('selectedIndex', 0);
            });
        });
    </script>
</div>
