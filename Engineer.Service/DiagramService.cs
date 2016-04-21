
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engineer.EMF;
using Engineer.EMF.Utils.Exceptions;
using System.Transactions;
using Engineer.EMF.Utils;
using System.Configuration;
using Engineer.EMF.Models;

namespace Engineer.Service
{
    public class DiagramService
    {
        DiagramRepository repository = new DiagramRepository();
        UserStoryAttachmentRepository rep = new UserStoryAttachmentRepository();
        public void Delete(UserStoryAttachment diagram,string userId)
        {
            diagram.state = AppConstants.DIAGRAM_STATUS_FINISIHED;
            rep.UpdateStatus(diagram);

            #region send email notification / save to history in case diagram updated
            try
            {
                diagram = rep.Get(diagram);
                SendEmail(diagram, userId, "DeleteDiagram");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot send email" + ex.Message);
            }
            #endregion
        }

        public AttachmentHistory FindByHistoryID(int historyId)
        {
            try
            {
                return repository.FindByHistoryID(historyId);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(AppConstants.EXCEPTION_RETREIVE_DIAGRAMS);
            }
        }

        public List<UserStoryAttachment> FindByStoryID(int Id)
        {
            try
            {
                return repository.FindByStoryID(Id);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(AppConstants.EXCEPTION_RETREIVE_DIAGRAMS);
            }
        }

        public AttachmentHistory FindHistoryByIDAndUserStory(int historyId)
        {
            try
            {
                return rep.GetHistory(historyId);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(AppConstants.EXCEPTION_RETREIVE_DIAGRAMS);
            }
        }

        public List<AttachmentHistory> FindHistory(UserStoryAttachment diagramObject)
        {
            try
            {
                return repository.FindDiagramHistory(diagramObject);
            }
            catch (Exception ex)
            {
                throw new BadRequestException(AppConstants.EXCEPTION_RETREIVE_DIAGRAM_HISTORY);
            }
            
        }

        public List<UserStoryAttachment> FindByUsers(string[] users)
        {
            return repository.FindByUsers(users);
        }

        public UserStoryAttachment FindByIDAndUserStory(int id, int userStoryId)
        {
            return rep.Get(new UserStoryAttachment() { attachId = id,userStoryId=userStoryId });
        }

        public List<UserStoryAttachment> ListAll()
        {
            try
            {
                return repository.FindAll();
            }
            catch (Exception ex)
            {
                throw new BadRequestException(AppConstants.EXCEPTION_RETREIVE_DIAGRAMS);
            }
        }

        public List<UserStoryAttachment> FindByUserStory(int id)
        {
            return repository.FindByStoryID(id);
        }

        public List<DiagramSearchModel> FindByUsersAndStories(string[] users, string[] stories,string sprint,string diagramName)
        {
            return repository.FindByUsersAndStories(users, stories,sprint,diagramName);
        }

        public int Add(Attachment diagramObject, string[] userStoriesIds, string graph, string userId,string svg)
        {
            int id = 0;
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    #region adding user stories
                    if (userStoriesIds != null)
                    {
                        UserStoryRepository usRepository = new UserStoryRepository();
                        foreach (string userStoryId in userStoriesIds)
                        {
                            var userStory = usRepository.FindByID(userStoryId);
                            if (userStory == null)
                                throw new BadRequestException(AppConstants.EXCEPTION_USER_STORY_CANNOT_FIND);

                            diagramObject.UserStoryAttachments.Add(new UserStoryAttachment() {
                                UserStory = userStory,
                                activties = graph,
                                SVG = svg,
                                state = AppConstants.DIAGRAM_STATUS_OPEN,
                                version = 1
                            });
                        }
                    }

                    id  = repository.Add(diagramObject, userId);
                    diagramObject.Id = id;
                    #endregion
                    #region save to history
                    diagramObject.UserStoryAttachments.ToList().ForEach(f =>
                    {
                        repository.SaveToHistory(f, userId);
                    });
                    #endregion
                    sc.Complete();
                }
                catch (Exception ex)
                {
                    throw new BadRequestException(AppConstants.EXCEPTION_DIAGRAM_SAVING_ERROR);
                }
                finally
                {
                    sc.Dispose();
                }
                
            }
            return id;

        }

        public void Share(Attachment diagramObject,int userStoryId, string userId)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    UserStoryAttachment attachment = rep.Get(new UserStoryAttachment() { attachId = diagramObject.Id, userStoryId = userStoryId });
                    diagramObject.UserStoryAttachments.ToList().ForEach(attach =>
                    {
                        attach.activties = attachment.activties;
                        attach.state = AppConstants.DIAGRAM_STATUS_OPEN;
                        attach.SVG = attachment.SVG;
                        attach.update_by = userId;
                        attach.update_date = DateTime.Now;
                        attach.version = attachment.version;
                    });
                    rep.Add(diagramObject.UserStoryAttachments.ToList());
                    sc.Complete();
                }
                catch (Exception ex)
                { }
                finally
                {
                    sc.Dispose();
                }
            }
        }

        public void Open(UserStoryAttachment diagramObject, string userId)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    diagramObject.state = AppConstants.DIAGRAM_STATUS_OPEN;
                    diagramObject = rep.UpdateStatus(diagramObject); 

                    #region lock
                    var diagrams = new List<UserStoryAttachment>();
                    diagrams.Add(diagramObject);
                    UnLockDiagrams(diagrams,userId);
                    #endregion

                    
                    sc.Complete();

                }
                catch (Exception ex)
                {
                    throw new Exception(AppConstants.EXCEPTION_GLOBAL);
                }
                finally
                {
                    sc.Dispose();
                }
            }
        }

        public void Close(UserStoryAttachment diagramObject, string userId)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    #region lock
                    var diagrams = new List<UserStoryAttachment>();
                    diagrams.Add(diagramObject);
                    LockDiagrams(diagrams,userId);
                    #endregion

                    diagramObject.state = AppConstants.DIAGRAM_STATUS_CLOSED;
                    rep.UpdateStatus(diagramObject);

                    sc.Complete();

                }
                catch (Exception ex)
                {
                    throw new Exception(AppConstants.EXCEPTION_GLOBAL);
                }
                finally
                {
                    sc.Dispose();
                }
            }
        }
        public int Update(UserStoryAttachment diagramObject, string graph, string userId, string svg)
        {
            int id = 0;
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            List<UserStoryAttachment> openDiagrams = new List<UserStoryAttachment>();
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                
                try
                {
                    diagramObject.activties = graph;
                    diagramObject.SVG = svg;
                    diagramObject.state = AppConstants.DIAGRAM_STATUS_OPEN;

                    openDiagrams = repository.Update(diagramObject, userId);
                    sc.Complete();
                }
                catch (Exception ex)
                {
                    throw new BadRequestException(AppConstants.EXCEPTION_DIAGRAM_SAVING_ERROR);
                }
                finally
                {
                    sc.Dispose();
                }

            }
            #region send email notification / save to history in case diagram updated
                openDiagrams.ForEach(diagram =>
                {
                    repository.SaveToHistory(diagram, userId);
                    try
                    {
                        SendEmail(diagram, userId, "EditDiagram");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot send email" + ex.Message);
                    }
                });
           
            
            #endregion
            return id;

        }

        private void SendEmail(UserStoryAttachment diagramObject,string userId,string template)
        {
            MailService mailService = MailService.Instance;
            string _mailConfigFilePath = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\Mail.xml";
            mailService.LoadFile(_mailConfigFilePath);
            DTOMessage message = mailService.GetMessage(template);
            string sendFrom = (ConfigurationSettings.AppSettings["MailFromAddress"] != null) ? ConfigurationSettings.AppSettings["MailFromAddress"].ToString() : "";
            UserRepository uRepositorty = new UserRepository();
            var actionUser = uRepositorty.FindById(userId);
            diagramObject.UserStory.AspNetUsers.ToList().ForEach(f =>
            {
                string messageSubject = MailService.formatMsg(message.Subject, new string[] { diagramObject.Attachment.name.ToString() });
                string bodyStr = MailService.formatMsg(message.Body, new string[] { diagramObject.Attachment.name.ToString(), f.UserName, actionUser.UserName,diagramObject.UserStory.name });
                try
                {
                    MailService.SendMessageWithAttachment(sendFrom, f.Email, null, messageSubject, bodyStr, null);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            });
        }


        public void LockDiagrams(List<UserStoryAttachment> diagrams,string userId)
        {
            foreach(UserStoryAttachment attach in diagrams)
            {
                attach.@readonly = true;
                if (repository.UpdateLock(attach, userId,true))
                {
                    attach.state = AppConstants.DIAGRAM_STATUS_CLOSED;
                    var exist = rep.UpdateStatus(attach);
                    repository.SaveToHistory(exist, userId);
                }
                else
                    Console.WriteLine("Already locked");
            }
        }

        public void UnLockDiagrams(List<UserStoryAttachment> diagrams, string userId)
        {
            foreach (UserStoryAttachment attach in diagrams)
            {
                attach.@readonly = false;
                repository.UpdateLock(attach,userId,false);
                attach.state = AppConstants.DIAGRAM_STATUS_OPEN;
                attach.version = ++attach.version;
                rep.UpdateStatusAndVersion(attach);
            }
        }

        public List<UserStoryAttachment> FindBySprint(int sprintId)
        {
            return repository.FindBySprintID(sprintId);
        }
    }
}
