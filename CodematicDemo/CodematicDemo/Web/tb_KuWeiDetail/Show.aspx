<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="Maticsoft.Web.tb_KuWeiDetail.Show" Title="显示页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>                   
                    <td class="tdbg">
                               
<table cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
	<td height="25" width="30%" align="right">
		HuoJiaHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblHuoJiaHao" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		CengHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblCengHao" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		LieHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblLieHao" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		XuHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblXuHao" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		RuKuTime
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblRuKuTime" runat="server"></asp:Label>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		CaoZuoYuan
	：</td>
	<td height="25" width="*" align="left">
		<asp:Label id="lblCaoZuoYuan" runat="server"></asp:Label>
	</td></tr>
</table>

                    </td>
                </tr>
            </table>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>




