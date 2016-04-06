<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="EngineerWeb.Sprint.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-top: 30px">
                <div class="row"><button type="button" class="btn btn-success custom-button-width navbar-right" data-toggle="modal" data-target="#projectModal">
                    <span class="glyphicon glyphicon-plus" aria-hidden="true" ></span> New Sprint
              </button></div>
                <div class="col-md-10 col-md-offset-1">
                    <table id="diagramsTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                        <thead>
                        <tr>
                            <th style="width: 10%">
                                Number
                            </th>
                            <th style="width: 20%">
                                Topic
                            </th>
                             <th style="width: 20%">
                                State
                            </th>
                            <%--<th style="width: 20%">
                                Version
                            </th>--%>
                             <th style="width: 30%">
                                Action
                            </th>
                        </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>

            </div>
            <div class="modal fade" id="projectModal" tabindex="-1" role="dialog" aria-labelledby="projectModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="projectModalLabel">Create / Update Sprint</h4>
                        </div>
                        <div class="modal-body" id="project_div">
                                <input type="hidden" id="sprint-id" name="Id" value="0"/>
                                <input type="hidden" id="state" name="state" />
                                <div class="form-group">
                                    <label for="sprint-number" class="col-md-3 control-label">Number</label>
                                    <div class="col-md-8">
                                        <input type="text" class="form-control" id="sprint-number" name="number" required>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="description" class="col-md-3 control-label">Topic</label>
                                    <div class="col-md-8">
                                        <input type="text" class="form-control" id="topic" name="topic" required />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="UserStories" class="col-md-3 control-label">User Stories</label>
                                    <div class="col-md-8">
                                        <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="UserStories" runat="server" DataTextField="name" DataValueField="Id" data-live-search="true" SelectionMode="Multiple" required/>
                                    </div>
                                </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <button data-loading-text="Saving..." id="saveBtn" type="button" class="btn btn-primary">Save</button>
                        </div>
                    </div>
                </div>
            </div>
    <script type="text/javascript">
        var datatableURL = '<%=ResolveUrl("List.aspx/GetAllSprints")%>';
        var saveOrUpdateURL = '<%=ResolveUrl("List.aspx/SaveOrUpdate")%>';
        var deleteURL = '<%=ResolveUrl("List.aspx/Delete")%>';
        var closeURL = '<%=ResolveUrl("List.aspx/Close")%>';
        var openURL = '<%=ResolveUrl("List.aspx/Open")%>';
    </script>
</asp:Content>
