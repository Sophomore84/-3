<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Add.aspx.cs" Inherits="Maticsoft.Web.tb_KuCun.Add" Title="增加页" %>

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
		<asp:TextBox id="txtHuoJiaHao" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		CengHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtCengHao" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		LieHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtLieHao" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		BianMa
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtBianMa" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		MingCheng
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtMingCheng" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		GuiGe
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtGuiGe" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		XingHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtXingHao" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		HeTongHao
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtHeTongHao" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		ShuLiang
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtShuLiang" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		X
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtX" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		Y
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtY" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		Z
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtZ" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
	<tr>
	<td height="25" width="30%" align="right">
		CaoZuoYuan
	：</td>
	<td height="25" width="*" align="left">
		<asp:TextBox id="txtCaoZuoYuan" runat="server" Width="200px"></asp:TextBox>
	</td></tr>
</table>

            </td>
        </tr>
        <tr>
            <td class="tdbg" align="center" valign="bottom">
                <asp:Button ID="btnSave" runat="server" Text="保存"
                    OnClick="btnSave_Click" class="inputbutton" onmouseover="this.className='inputbutton_hover'"
                    onmouseout="this.className='inputbutton'"></asp:Button>
                <asp:Button ID="btnCancle" runat="server" Text="取消"
                    OnClick="btnCancle_Click" class="inputbutton" onmouseover="this.className='inputbutton_hover'"
                    onmouseout="this.className='inputbutton'"></asp:Button>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>
