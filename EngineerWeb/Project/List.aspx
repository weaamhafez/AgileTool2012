<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="EngineerWeb.Project.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="margin-top: 30px">
                <div class="row"><button type="button" class="btn btn-success custom-button-width navbar-right" data-toggle="modal" data-target="#projectModal">
                    <span class="glyphicon glyphicon-plus" aria-hidden="true" ></span> New Project
              </button></div>
                <div class="col-md-10 col-md-offset-1">
                    <table id="diagramsTable" class="table table-striped table-bordered" cellspacing="0" width="100%">
                        <thead>
                        <tr>
                            <th style="width: 10%">
                                Name
                            </th>
                            <th style="width: 30%">
                                Description
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
                            <h4 class="modal-title" id="projectModalLabel">Create Project</h4>
                        </div>
                        <div class="modal-body" id="project_div" data-toggle="validator" role="textbox">
                                <input type="hidden" id="project-id" name="Id" value="0"/>
                                <div class="form-group">
                                    <label for="project-name" class="col-md-3 control-label">Project Name</label>
                                    <div class="col-md-8">
                                        <input type="text" class="form-control" id="project-name" name="name" required data-error="*">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="description" class="col-md-3 control-label">Description</label>
                                    <div class="col-md-8">
                                        <textarea class="form-control" id="description" name="description" style="width:280px!important"></textarea>
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
        var datatableURL = '<%=ResolveUrl("List.aspx/GetAllProjects")%>';
        var saveOrUpdateURL = '<%=ResolveUrl("List.aspx/SaveOrUpdate")%>';
        var deleteURL = '<%=ResolveUrl("List.aspx/Delete")%>';
    </script>
</asp:Content>
