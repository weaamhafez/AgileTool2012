<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="EngineerWeb.History.List" %>
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
            <div class="modal fade" id="showHistoryModal" tabindex="-1" role="dialog" aria-labelledby="showHistoryLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="showHistoryLabel">View diagram history</h4>
                        </div>
                        
                        <div class="modal-body" id="history_div">
                                <input type="hidden" id="sprint-id" name="Id" value="0"/>
                                <div class="col-md-10 col-md-offset-1">
                                    <table id="diagramHistoryTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                                        <thead>
                                        <tr>
                                            <th style="width: 35%">
                                                User
                                            </th>
                                            <th style="width: 35%">
                                                Date
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
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
    <script type="text/javascript">
        var datatableURL = '<%=ResolveUrl("List.aspx/GetAllDiagrams")%>';
        var loadHistoryURL = '<%=ResolveUrl("List.aspx/ShowHistory")%>';
        var viewURL = '<%=ResolveUrl("View")%>';
    </script>
</asp:Content>
