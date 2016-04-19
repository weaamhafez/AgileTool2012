<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EngineerWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
    <asp:LoginView runat="server" ViewStateMode="Disabled">
                <RoleGroups>
                    <asp:RoleGroup Roles="Admin">
                        <ContentTemplate>
                             <div class="col-lg-4 col-md-6">
                                <a href='<%=ResolveUrl("Project/List") %>'>
                                   <div class="panel panel-green">
                                        <div class="panel-heading">
                                            <div class="row">
                                                <div class="col-xs-3">
                                                  <i class="glyphicon glyphicon-wrench glyphicon-5x"></i>
                                                </div>
                                                <div class="col-xs-9 text-right">
                                                  <div class="huge">Projects</div>
                                                  <div>Add new projects</div>
                                                </div>
                                            </div>
                                         </div>
                                       <div class="panel-footer"> 
                                            <a href="#">
                                               <div class="panel panel-green"> <span class="pull-left"><button type="button" class="btn btn-primary btn-sl" data-toggle="modal" data-target="#myModal1">View Details</button></span></div>
                                            </a>
                                           <div class="clearfix"></div>
                                       </div>
                                   </div>
                                </a>
            
                            </div>
                        </ContentTemplate>
                     </asp:RoleGroup>
                </RoleGroups>
        </asp:LoginView>
        <div class="col-lg-4 col-md-6">
            <a href='<%=ResolveUrl("User_Story/List") %>'>
               <div class="panel panel-green">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                              <i class="glyphicon glyphicon-book glyphicon-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                              <div class="huge">Stories</div>
                              <div>Create User stories and assign users.</div>
                            </div>
                        </div>
                     </div>
                    <div class="panel-footer"> 
                        <a href="#">
                           <div class="panel panel-green"> <span class="pull-left"><button type="button" class="btn btn-primary btn-sl" data-toggle="modal" data-target="#myModal2">View Details</button></span></div>
                        </a>
                       <div class="clearfix"></div>
                    </div>
               </div>
            </a>
        </div>
        <div class="col-lg-4 col-md-6">
            <a href='<%=ResolveUrl("Diagram/List") %>'>
               <div class="panel panel-green">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                              <i class="glyphicon glyphicon-edit glyphicon-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                              <div class="huge">Diagrams</div>
                              <div>Add Diagram to user Story</div>
                            </div>
                        </div>
                     </div>
                   <div class="panel-footer"> 
                        <a href="#">
                           <div class="panel panel-green"> <span class="pull-left"><button type="button" class="btn btn-primary btn-sl" data-toggle="modal" data-target="#myModal3">View Details</button></span></div>
                        </a>
                       <div class="clearfix"></div>
                    </div>
               </div>
            </a>
        </div>
        </div>
    <div class="row"></div>
    <div class="row">
        <div class="col-lg-4 col-md-6">
            <a href='<%=ResolveUrl("Sprint/List") %>'>
               <div class="panel panel-green">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-xs-3">
                              <i class="glyphicon glyphicon-share-alt glyphicon-5x"></i>
                            </div>
                            <div class="col-xs-9 text-right">
                              <div class="huge">Sprints</div>
                              <div>Add Sprint ,assign user story</div>
                            </div>
                        </div>
                     </div>
                   <div class="panel-footer"> 
                        <a href="#">
                           <div class="panel panel-green"> <span class="pull-left"><button type="button" class="btn btn-primary btn-sl" data-toggle="modal" data-target="#myModal4">View Details</button></span></div>
                        </a>
                       <div class="clearfix"></div>
                    </div>
               </div>
            </a>
        </div>
        </div>


    <div class="modal fade" id="myModal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="myModalLabel">Projects help</h4>
        </div>
        <div class="modal-body">
          <ul> <li>Add New project</li>
            <li>list projects created by logged in user</li>
            <li>list projects having user stories assigned to you</li></ul>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

        </div>
      </div>
    </div>
  </div>

    <div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel1" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="myModalLabel1">User Stories help</h4>
        </div>
        <div class="modal-body">
          <ul> <li>Add New User Story and assign users</li>
            <li>list user stories you have been assigned to</li>
           </ul>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

        </div>
      </div>
    </div>
  </div>

    <div class="modal fade" id="myModal3" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="myModalLabel2">Diagrams</h4>
        </div>
        <div class="modal-body">
          <ul> <li>Add New Diagram and add user stories to it</li>
            <li>list diagrams</li>
           </ul>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

        </div>
      </div>
    </div>
  </div>
    <div class="modal fade" id="myModal4" tabindex="-1" role="dialog" aria-labelledby="myModalLabel3" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="myModalLabel3">Sprints</h4>
        </div>
        <div class="modal-body">
          <ul> <li>Add New Sprint and add user stories to it</li>
            <li>list sprints</li>
           </ul>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>

        </div>
      </div>
    </div>
  </div>

</asp:Content>
