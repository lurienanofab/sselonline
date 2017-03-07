<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Meter.ascx.cs" Inherits="sselOnLine.Controls.Meter" %>
<div id='<%= ID %>'>
    <asp:Panel ID="panel" runat="server" CssClass="meter">
        <table>
            <tr>
                <td class="group" style="vertical-align: middle;">
                    <%= Group %>
                </td>
                <td>
                    <asp:Repeater ID="repeater" runat="server" OnItemDataBound="repeater_ItemDataBound">
                        <HeaderTemplate>
                            <table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class='<%# DataBinder.Eval(Container.DataItem, "MeterTag") %>'>
                                <td class="name" style="background-color: Red; vertical-align: middle;">
                                    <%# DataBinder.Eval(Container.DataItem, "MeterName") %>
                                </td>
                                <td class="value" style="background-color: Blue; vertical-align: middle;">&nbsp;</td>
                                <td class="display" style="vertical-align: middle;">
                                    <div class="level">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
