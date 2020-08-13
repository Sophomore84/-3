﻿<%@ Page Title="tb_KuCunRecords" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Maticsoft.Web.tb_KuCunRecords.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" src="/js/CheckBox.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!--Title -->

            <!--Title end -->

            <!--Add  -->

            <!--Add end -->

            <!--Search -->
            <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
                <tr>
                    <td style="width: 80px" align="right" class="tdbg">
                         <b>关键字：</b>
                    </td>
                    <td class="tdbg">                       
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSearch" runat="server" Text="查询"  OnClick="btnSearch_Click" >
                    </asp:Button>                    
                        
                    </td>
                    <td class="tdbg">
                    </td>
                </tr>
            </table>
            <!--Search end-->
            <br />
            <asp:GridView ID="gridView" runat="server" AllowPaging="True" Width="100%" CellPadding="3"  OnPageIndexChanging ="gridView_PageIndexChanging"
                    BorderWidth="1px" DataKeyNames="HuoJiaHao,CengHao,LieHao" OnRowDataBound="gridView_RowDataBound"
                    AutoGenerateColumns="false" PageSize="10"  RowStyle-HorizontalAlign="Center" OnRowCreated="gridView_OnRowCreated">
                    <Columns>
                    <asp:TemplateField ControlStyle-Width="30" HeaderText="选择"    >
                                <ItemTemplate>
                                    <asp:CheckBox ID="DeleteThis" onclick="javascript:CCA(this);" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            
		<asp:BoundField DataField="HuoJiaHao" HeaderText="HuoJiaHao" SortExpression="HuoJiaHao" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CengHao" HeaderText="CengHao" SortExpression="CengHao" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="LieHao" HeaderText="LieHao" SortExpression="LieHao" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="BianMa" HeaderText="BianMa" SortExpression="BianMa" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="MingCheng" HeaderText="MingCheng" SortExpression="MingCheng" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="GuiGe" HeaderText="GuiGe" SortExpression="GuiGe" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="XingHao" HeaderText="XingHao" SortExpression="XingHao" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="HeTongHao" HeaderText="HeTongHao" SortExpression="HeTongHao" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="X" HeaderText="X" SortExpression="X" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Y" HeaderText="Y" SortExpression="Y" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="Z" HeaderText="Z" SortExpression="Z" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="RuKuTime" HeaderText="RuKuTime" SortExpression="RuKuTime" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="ChuKuTime" HeaderText="ChuKuTime" SortExpression="ChuKuTime" ItemStyle-HorizontalAlign="Center"  /> 
		<asp:BoundField DataField="CaoZuoYuan" HeaderText="CaoZuoYuan" SortExpression="CaoZuoYuan" ItemStyle-HorizontalAlign="Center"  /> 
                            
                            <asp:HyperLinkField HeaderText="详细" ControlStyle-Width="50" DataNavigateUrlFields="HuoJiaHao,CengHao,LieHao" DataNavigateUrlFormatString="Show.aspx?id0={0}&id1={1}&id2={2}"
                                Text="详细"  />
                            <asp:HyperLinkField HeaderText="编辑" ControlStyle-Width="50" DataNavigateUrlFields="HuoJiaHao,CengHao,LieHao" DataNavigateUrlFormatString="Modify.aspx?id0={0}&id1={1}&id2={2}"
                                Text="编辑"  />
                            <asp:TemplateField ControlStyle-Width="50" HeaderText="删除"   Visible="false"  >
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                         Text="删除"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                </asp:GridView>
               <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                <tr>
                    <td style="width: 1px;">                        
                    </td>
                    <td align="left">
                        <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"/>                       
                    </td>
                </tr>
            </table>
</asp:Content>
<%--<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>--%>
