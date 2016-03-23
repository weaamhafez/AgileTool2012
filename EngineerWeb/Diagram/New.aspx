<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="New.aspx.cs" Inherits="EngineerWeb.Diagram.New" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        
        <div id="diagram_div" class="container-fluid" style="padding-left: 0; padding-right: 0;">
            <div class="row">
                <div class="col-md-12">
                    <nav class="navbar navbar-default" id="navbarForm" style="margin-left: 0; margin-bottom: 0">
                        <div class="container-fluid">
                            <div class="navbar-header">
                                <span class="navbar-brand" href="#">Diagram-<span id="dialogNameTitle"
                                                                               style="font-size: 16px;">Untitled</span></span>
                            </div>
                            <!-- Collect the nav links, forms, and other content for toggling -->
                            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                                <ul class="nav navbar-nav">
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                                           aria-haspopup="true" aria-expanded="false">File
                                            <span class="caret"></span></a>
                                        <ul class="dropdown-menu">
                                            <li><a data-target="#renameModal" data-toggle="modal">
                                                Rename</a></li>
                                            <li class="dropdown-divider"></li>
                                            <li><a onclick="save()"><!-- <i class="fa fa-cloud-upload"></i>-->
                                                Save</a></li>
                                        </ul>
                                    </li>
                                </ul>
                                <%--<ul class="nav navbar-nav">
                                    <li class="dropdown">
                                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button"
                                           aria-haspopup="true" aria-expanded="false">Edit
                                            <span class="caret"></span></a>
                                        <ul class="dropdown-menu">
                                            <li><a onclick="deleteSelContol();"> Delete</a>
                                            </li>
                                            <li class="dropdown-divider"></li>
                                        </ul>
                                    </li>
                                </ul>--%>
                            </div>
                            <!-- /.navbar-collapse -->
                        </div>
                        <!-- /.container-fluid -->
                    </nav>

                </div>
            </div>
            <div class="row">
            <div class="form-group col-md-5">
                    <asp:Label runat="server" Text="User Story" CssClass="col-md-3 control-label">

                    </asp:Label>
                    <asp:ListBox runat="server" ID="UserStoriesList" DataTextField="name" DataValueField="Id" CssClass="selectpicker" data-live-search="true" SelectionMode="Multiple" ClientIDMode="Static"/>
             </div>
         </div>
            <div class="row">
                <div class="col-xs-4 border-right full-height">
                    <h2>Builder</h2>
                    <h4>Drag Activity and drop to right panel</h4>
                    <asp:HiddenField ID="Templates" runat="server" />
                    <div class="panel-group" id="toolbox" role="tablist" aria-multiselectable="true">
                        <div class="panel panel-default">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="panel-title">
                                    <a role="button" data-toggle="collapse" data-parent="#toolbox" href="#collapseOne"
                                       aria-expanded="true" aria-controls="collapseOne">
                                        Activity Diagram controls
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel"
                                 aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <div class="toolboxItem short col-xs-6" data-type="activity" ><a href=""
                                                                                                   class="btn btn-success btn-block"><i
                                            class="glyphicon glyphicon-share-alt" ></i> Activity</a></div>

                                    <%--<div class="toolboxItem short col-xs-6" data-type="activity"><a href=""
                                                                                                   class="btn btn-warning" aria-disabled="true"><i
                                            class="fa fa-file-text-o"></i> Condition</a></div>--%>
                            </div>
                        </div>
                    </div>
                </div>
               
            </div>
            <div style="overflow-y:scroll; max-height:600px;min-height: 300px" class="col-xs-4 sortable border-right full-height" id="preview"></div>
                <div class="col-xs-4 border-right full-height">
                    <div id="prop"></div>
                </div>
        </div>
       </div>
        <div class="modal fade" id="renameModal" tabindex="-1" role="dialog" aria-labelledby="renameModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title" id="projectModalLabel">Rename Diagram</h4>
                        </div>
                        <div class="modal-body" id="project_div">
                                <input type="hidden" id="story-id" name="Id" value="0"/>
                                <input type="hidden" id="state" name="state" />
                                <div class="form-group">
                                    <asp:HiddenField runat="server" ID="diagramID" ClientIDMode="Static" />
                                    <asp:HiddenField runat="server" ID="diagramGraph" ClientIDMode="Static" />
                                    <label for="story-name" class="col-md-3 control-label">Name</label>
                                    <div class="col-md-8">
                                        <asp:TextBox CssClass="form-control" ID="diagramName" ClientIDMode="Static" required runat="server" ></asp:TextBox>
                                    </div>
                                </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                            <button id="renameBtn" type="button" class="btn btn-primary">Rename</button>
                        </div>
                    </div>
                </div>
            </div>
    <script type="text/javascript">
        var templatesClientId = "<%=Templates.ClientID%>";
        var saveOrUpdateURL = '<%=ResolveUrl("New.aspx/SaveOrUpdate")%>';
    </script>
</asp:Content>
