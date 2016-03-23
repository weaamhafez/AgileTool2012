<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListStories.aspx.cs" Inherits="EngineerWeb.Reports.ListStories" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="false" 
                                OnRowCommand="gvResult_RowCommand" AllowPaging="true" OnRowDataBound="gvResult_RowDataBound" PagerStyle-CssClass="pager" 
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" DataKeyNames="Id"  >
         <PagerSettings Mode="NumericFirstLast" />
        <Columns>
               <asp:TemplateField HeaderText="Name" Visible="false" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "Id")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "name")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "description")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "state")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Creator" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "AspNetUser.UserName")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Project" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "Project.name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action" meta:resourcekey="TemplateFieldResource5">
                <ItemTemplate>
                    <asp:Label ID="lblViewDiagrams" runat="server" />
                    <asp:Button CssClass="btn btn-primary" ID="btnViewDiagrams" CommandName="ViewDiagrams" runat="server"
                        Text="View Diagrams" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:GridView ID="Diagrams" runat="server" AutoGenerateColumns="false" 
                               AllowPaging="true" PagerStyle-CssClass="pager" 
            HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="false" Caption="Diagrams" EmptyDataText="No Diagrams shared in this story" >
        <Columns>
            <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <%#DataBinder.Eval(Container.DataItem, "name")%></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "state")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Locked" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "readonly")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
