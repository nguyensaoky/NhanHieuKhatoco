<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Admin.Vendors.EditVendors"
    CodeFile="EditVendors.ascx.cs" %>
<%@ Reference Control="~/admin/vendors/affiliates.ascx" %>
<%@ Reference Control="~/admin/vendors/banners.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Address" Src="~/controls/Address.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="Portal" TagName="URL" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<table cellspacing="0" cellpadding="4" border="0" summary="Edit Vendors Design Table">
    <tr>
        <td align="top">
            <table cellspacing="0" cellpadding="0" border="0" summary="Edit Vendors Design Table">
                <tr>
                    <td valign="top" width="560">
                        <dnn:SectionHead id="dshSettings" runat="server" cssclass="Head" includerule="True"
                            resourcekey="Settings" section="tblSettings" text="Vendor Details" />
                        <table id="tblSettings" runat="server" cellspacing="2" cellpadding="2" border="0"
                            summary="Edit Vendors Design Table">
                            <tr valign="top">
                                <td class="SubHead" width="120">
                                    <dnn:Label id="plVendorName" runat="server" controlname="txtVendorName" suffix=":">
                                    </dnn:Label></td>
                                <td align="left" class="NormalBold" nowrap>
                                    <asp:TextBox ID="txtVendorName" runat="server" CssClass="NormalTextBox" Width="200px"
                                        MaxLength="50" TabIndex="1"></asp:TextBox>&nbsp;*
                                    <asp:RequiredFieldValidator ID="valVendorName" runat="server" CssClass="NormalRed"
                                        Display="Dynamic" ErrorMessage="<br>Company Name Is Required" ControlToValidate="txtVendorName"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <br>
                        <dnn:SectionHead id="dshOther" runat="server" cssclass="Head" includerule="True"
                            resourcekey="Other" section="tblOther" text="Other Details" />
                        <table id="tblOther" runat="server" cellspacing="2" cellpadding="2" border="0" summary="Edit Vendors Design Table">
                            <tr id="rowVendor2" runat="server" valign="top">
                                <td class="SubHead" width="120">
                                    <dnn:Label id="plAuthorized" runat="server" controlname="chkAuthorized" suffix=":">
                                    </dnn:Label></td>
                                <td>
                                    <asp:CheckBox ID="chkAuthorized" runat="server" TabIndex="22"></asp:CheckBox></td>
                            </tr>
                        </table>
                        <br>
                        <asp:PlaceHolder ID="pnlBanners" runat="server">
                            <dnn:SectionHead id="dshBanners" runat="server" cssclass="Head" includerule="True"
                                isexpanded="False" resourcekey="BannerAdvertising" section="divBanners" text="Banner Advertising" />
                            <div id="divBanners" runat="server" />
                        </asp:PlaceHolder>
                        <br>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<p>
    <asp:LinkButton cssclass="CommandButton" ID="cmdUpdate" resourcekey="cmdUpdate" runat="server"
        Text="Update" BorderStyle="none" OnClick="cmdUpdate_Click"></asp:LinkButton>&nbsp;
    <asp:LinkButton cssclass="CommandButton" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
        Text="Cancel" BorderStyle="none" CausesValidation="False" OnClick="cmdCancel_Click"></asp:LinkButton>&nbsp;
    <asp:LinkButton cssclass="CommandButton" ID="cmdDelete" resourcekey="cmdDelete" runat="server"
        Text="Delete" BorderStyle="none" CausesValidation="False" OnClick="cmdDelete_Click"></asp:LinkButton>
</p>