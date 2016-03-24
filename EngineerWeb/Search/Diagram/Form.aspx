<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="EngineerWeb.Search.Diagram.Form" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-body" id="searchDiv">
            <div class="form-group">
			    <label for="Users" class="col-md-2 control-label">Select Sprint</label>
			    <div class="col-md-5">
			        <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="Sprint" runat="server" DataTextField="number" DataValueField="Id" data-live-search="true" />
			    </div>
		    </div>
            <div class="form-group">
			    <label for="Users" class="col-md-2 control-label">Diagram</label>
			    <div class="col-md-5">
                     <asp:TextBox CssClass="form-input" runat="server" ID="DiagramName" ClientIDMode="Static"></asp:TextBox>
			    </div>
		    </div>
            <div class="form-group">
			    <label for="Users" class="col-md-2 control-label">Select Users</label>
			    <div class="col-md-5">
			        <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="Users" runat="server" DataTextField="UserName" DataValueField="Id" data-live-search="true" SelectionMode="Multiple" required/>
			    </div>
		    </div>
            <div class="form-group">
			    <label for="Users" class="col-md-2 control-label">Select User Story</label>
			    <div class="col-md-5">
			        <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="Stories" runat="server" DataTextField="name" DataValueField="Id" data-live-search="true" required/>
			    </div>
		    </div>
         <div class="form-group">
			<button type="button" class="btn btn-success custom-button-width navbar-center" id="search">
                    <span class="glyphicon glyphicon-search" aria-hidden="true" ></span> Search
              </button>
		</div>

        <div style="margin-top: 30px" class="form-group">
                <div class="col-md-10 col-md-offset-1">
                    <table id="diagramsTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                        <thead>
                        <tr>
                            <th style="width: 30%">
                                Name
                            </th>
                            <th style="width: 30%">
                                Locked
                            </th>
                        </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>

            </div>
     </div>
    <script type="text/javascript">
        var searchURL = '<%=ResolveUrl("Form.aspx/Search")%>';
    </script>
</asp:Content>
