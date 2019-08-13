<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Messenger.ascx.cs" Inherits="sselOnLine.Controls.Messenger" %>
<asp:Panel runat="server" ID="panMessenger" CssClass="messenger">
    <input type="hidden" runat="server" id="hidClientID" class="client-id" />
    <ul class="message-list" style="display: none;"></ul>
    <asp:Panel runat="server" ID="panAdmin" Visible="false" CssClass="admin">
        <asp:LinkButton runat="server" ID="btnCompose" Text="Messenger Administration" OnClick="BtnCompose_Click" CssClass="admin-link"></asp:LinkButton>
        <asp:Panel runat="server" ID="panCompose" Visible="false" CssClass="compose">
            <div class="label">
                Recipients
            </div>
            <div class="field">
                <input type="hidden" runat="server" id="hidSelectedTab" class="selected-tab" value="0" />
                <div class="tabs">
                    <ul>
                        <li><a href="#fragment-1">By Privilege</a></li>
                        <li><a href="#fragment-2">By Community</a></li>
                        <li><a href="#fragment-3">By Manager</a></li>
                        <li><a href="#fragment-4">By Tools</a></li>
                        <li><a href="#fragment-5">Currently In Lab</a></li>
                    </ul>
                    <div id="fragment-1">
                        <asp:CheckBoxList runat="server" ID="cblPrivs" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" DataValueField="PrivFlag" DataTextField="PrivType" CssClass="privs-list">
                        </asp:CheckBoxList>
                    </div>
                    <div id="fragment-2">
                        <asp:CheckBoxList runat="server" ID="cblCommunity" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" DataValueField="CommunityFlag" DataTextField="CommunityName" CssClass="communities-list">
                        </asp:CheckBoxList>
                    </div>
                    <div id="fragment-3">
                        <asp:DropDownList runat="server" ID="ddlManagers" DataValueField="ClientOrgID" DataTextField="DisplayName" CssClass="managers-list">
                        </asp:DropDownList>
                    </div>
                    <div id="fragment-4">
                        <asp:ListBox runat="server" ID="lbTools" DataValueField="ResourceID" DataTextField="ResourceName" Height="200" SelectionMode="Multiple" CssClass="tools-list"></asp:ListBox>
                        <div>
                            <span class="nodata">(Hold the "Ctrl" key to select multiple tools)</span>
                        </div>
                    </div>
                    <div id="fragment-5">
                        <asp:CheckBoxList runat="server" ID="cblAreas" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" DataValueField="AreaName" DataTextField="AreaName" CssClass="areas-list">
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div style="padding-top: 10px; padding-bottom: 10px;">
                    <input type="button" class="view-recipients" value="View Recipients" />
                    <div class="recipients-list" style="display: none;"></div>
                </div>
            </div>
            <div class="label">
                Subject
            </div>
            <div class="field">
                <input type="text" class="message-subject" style="width: 700px;" />
                <div class="subject-validation"></div>
            </div>
            <div class="label">
                Body
            </div>
            <div class="field">
                <textarea class="message-body" style="width: 700px; height: 200px;" cols="10" rows="5"></textarea>
                <div class="body-validation"></div>
                <div>
                    <label title="Message is displayed until user clicks a button">
                        <input type="checkbox" class="message-acknowledge-required" />
                        Acknowledgment Required
                    </label>
                    <label title="Only this message will be viewable until it is read">
                        <input type="checkbox" class="message-exclusive" />
                        Exclusive
                    </label>
                </div>
            </div>
            <div class="control">
                <input type="button" value="Send" class="send-message" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="BtnCancel_Click" />
                <div class="control-message"></div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Panel>
