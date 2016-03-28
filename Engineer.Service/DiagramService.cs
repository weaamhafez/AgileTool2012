
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
        public void Delete(UserStoryAttachment diagram)
        {
            diagram.state = AppConstants.DIAGRAM_STATUS_FINISIHED;
            rep.UpdateStatus(diagram);
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
                                state = AppConstants.DIAGRAM_STATUS_OPEN
                            });
                        }
                    }

                    id  = repository.Add(diagramObject, userId);
                    diagramObject.Id = id;
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

        public void Open(UserStoryAttachment diagramObject, string userId)
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
                    UnLockDiagrams(diagrams);
                    #endregion

                    diagramObject.state = AppConstants.DIAGRAM_STATUS_OPEN;
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
                    LockDiagrams(diagrams);
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
            #region send email notification in case diagram updated
                openDiagrams.ForEach(diagram =>
                {
                    repository.SaveToHistory(diagram, userId);
                    try
                    {
                        SendUpdateEmail(diagram, userId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot send email" + ex.Message);
                    }
                });
           
            
            #endregion
            return id;

        }

        private void SendUpdateEmail(UserStoryAttachment diagramObject,string userId)
        {
            MailService mailService = MailService.Instance;
            string _mailConfigFilePath = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\Mail.xml";
            mailService.LoadFile(_mailConfigFilePath);
            DTOMessage message = mailService.GetMessage("EditDiagram");
            string sendFrom = (ConfigurationSettings.AppSettings["MailFromAddress"] != null) ? ConfigurationSettings.AppSettings["MailFromAddress"].ToString() : "";
            UserRepository uRepositorty = new UserRepository();
            var actionUser = uRepositorty.FindById(userId);
            diagramObject.UserStory.AspNetUsers.ToList().ForEach(f =>
            {
                string messageSubject = MailService.formatMsg(message.Subject, new string[] { diagramObject.Attachment.name.ToString() });
                string bodyStr = MailService.formatMsg(message.Body, new string[] { diagramObject.Attachment.name.ToString(), f.UserName, actionUser.UserName,diagramObject.UserStory.name });
                MailService.SendMessageWithAttachment(sendFrom, f.Email, null, messageSubject, bodyStr, null);
            });
        }

        //public Attachment FindByID(int id)
        //{
        //    return repository.Get(new Attachment() { Id = id });
        //}

        public void LockDiagrams(List<UserStoryAttachment> diagrams)
        {
            foreach(UserStoryAttachment attach in diagrams)
            {
                attach.@readonly = true;
                repository.UpdateLock(attach);
                attach.state = AppConstants.DIAGRAM_STATUS_CLOSED;
                rep.UpdateStatus(attach);
            }
        }

        public void UnLockDiagrams(List<UserStoryAttachment> diagrams)
        {
            foreach (UserStoryAttachment attach in diagrams)
            {
                attach.@readonly = false;
                repository.UpdateLock(attach);
                attach.state = AppConstants.DIAGRAM_STATUS_OPEN;
                rep.UpdateStatus(attach);
            }
        }

        public List<UserStoryAttachment> FindBySprint(int sprintId)
        {
            return repository.FindBySprintID(sprintId);
        }
    }
}
