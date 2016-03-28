<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="EngineerWeb.Diagram.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <div style="margin-top: 30px">
                <div class="row"><button type="button" class="btn btn-success custom-button-width navbar-right" id="addDiagram">
                    <span class="glyphicon glyphicon-plus" aria-hidden="true" ></span> New Diagram
              </button></div>
                <div class="col-md-10 col-md-offset-1">
                    <table id="diagramsTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                        <thead>
                        <tr>
                            <th style="width: 30%">
                                Name
                            </th>
                            <th style="width: 5%">
                                Locked
                            </th>
                            <th style="width: 35%">
                                User Story
                            </th>
                            <th style="width: 30%">
                                Action
                            </th>
                        </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>

            </div>
            <div class="modal fade" id="removeModal" tabindex="-1" role="dialog" aria-labelledby="removeModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="removeModalLabel">Confirm Delete Diagram</h4>
                        </div>
                        
                        <div class="modal-body" id="delete_div">
                                <input type="hidden" id="sprint-id" name="Id" value="0"/>
                                <div class="col-md-10 col-md-offset-1">
                                    <table id="userStoriesTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                                        <thead>
                                        <tr>
                                            <th style="width: 20%">
                                                Name
                                            </th>
                                            <th style="width: 30%">
                                                Description
                                            </th>
                                            <th style="width: 10%">
                                                State
                                            </th>
                                        </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <button data-loading-text="Deleting..." id="deleteBtn" type="button" class="btn btn-danger">Delete</button>
                        </div>
                    </div>
                </div>
            </div>
    <script type="text/javascript">
        var datatableURL = '<%=ResolveUrl("List.aspx/GetAllDiagrams")%>';
        var deleteURL = '<%=ResolveUrl("List.aspx/Delete")%>';
        var loadUserStoriesURL = '<%=ResolveUrl("List.aspx/FindStoriesByDiagram")%>';
        var openURL = '<%=ResolveUrl("List.aspx/Open")%>';
        var closeURL = '<%=ResolveUrl("List.aspx/Close")%>';
    </script>
</asp:Content>
