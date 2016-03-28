<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="EngineerWeb.User_Story.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-top: 30px">
                <div class="row"><button type="button" class="btn btn-success custom-button-width navbar-right" id="add" data-toggle="modal" data-target="#projectModal">
                    <span class="glyphicon glyphicon-plus" aria-hidden="true" ></span> New User Story
              </button></div>
                <div class="col-md-10 col-md-offset-1">
                    <table id="diagramsTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
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
                            <h4 class="modal-title" id="projectModalLabel">Create / Update Story</h4>
                        </div>
                        <div class="modal-body" id="project_div">
                                <input type="hidden" id="story-id" name="Id" value="0"/>
                                <input type="hidden" id="state" name="state" />
                                <div class="form-group">
                                    <label for="story-name" class="col-md-3 control-label">Name</label>
                                    <div class="col-md-8">
                                        <input type="text" class="form-control" id="story-name" name="name" required>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="description" class="col-md-3 control-label">Description</label>
                                    <div class="col-md-8">
                                        <textarea class="form-control" id="description" name="description" required style="width:280px!important"></textarea>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="description" class="col-md-3 control-label">Project</label>
                                    <div class="col-md-8">
                                        <asp:DropDownList CssClass="selectpicker" ClientIDMode="Static" ID="projectId" runat="server" DataTextField="name" DataValueField="Id" data-live-search="true" style="width:280px!important"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="description" class="col-md-3 control-label">Users</label>
                                    <div class="col-md-8">
                                        <asp:ListBox CssClass="selectpicker" ClientIDMode="Static" ID="AspNetUsers" runat="server" DataTextField="UserName" DataValueField="Id" SelectionMode="Multiple" data-live-search="true"/>
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
            <div class="modal fade" id="removeModal" tabindex="-1" role="dialog" aria-labelledby="removeModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="removeModalLabel">Confirm Delete Story</h4>
                        </div>
                        
                        <div class="modal-body" id="delete_div">
                                <div class="col-md-10 col-md-offset-1">
                                    <table id="userStoriesTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                                        <thead>
                                        <tr>
                                            <th style="width: 20%">
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
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <button data-loading-text="Deleting..." id="deleteBtn" type="button" class="btn btn-danger">Delete</button>
                        </div>
                    </div>
                </div>
            </div>
    <script type="text/javascript">
        var datatableURL = '<%=ResolveUrl("List.aspx/GetAllStories")%>';
        var saveOrUpdateURL = '<%=ResolveUrl("List.aspx/SaveOrUpdate")%>';
        var deleteURL = '<%=ResolveUrl("List.aspx/Delete")%>';
        var finishURL = '<%=ResolveUrl("List.aspx/Finish")%>';
        var loadDiagramsURL = '<%=ResolveUrl("List.aspx/FindDiagramsByStory")%>';
        var openURL = '<%=ResolveUrl("List.aspx/Open")%>';
    </script>
</asp:Content>
