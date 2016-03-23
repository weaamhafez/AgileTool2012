<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="EngineerWeb.Search.User.Form" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="panel-body" id="searchDiv">
        <div class="form-group">
			<label for="Users" class="col-md-2 control-label">Select Diagram</label>
			<div class="col-md-5">
			    <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="Diagrams" runat="server" DataTextField="Name" DataValueField="Id" data-live-search="true" required/>
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
                                User Name
                            </th>
                            <th style="width: 30%">
                                Email
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
