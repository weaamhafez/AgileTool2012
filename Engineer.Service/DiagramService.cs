
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

namespace Engineer.Service
{
    public class DiagramService
    {
        DiagramRepository repository = new DiagramRepository();
        public void Delete(Attachment diagram)
        {
            diagram.state = AppConstants.DIAGRAM_STATUS_FINISIHED;
            repository.UpdateStatus(diagram);
        }

        public List<Attachment> FindByStoryID(int Id)
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

        public List<Attachment> FindByUsers(string[] users)
        {
            return repository.FindByUsers(users);
        }

        public List<Attachment> ListAll()
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

        public List<Attachment> FindByUserStory(int id)
        {
            return repository.FindByStoryID(id);
        }

        public List<Attachment> FindByUsersAndStories(string[] users, string[] stories)
        {
            return repository.FindByUsersAndStories(users, stories);
        }

        public int SaveOrUpdate(Attachment diagramObject, string[] userStoriesIds, string graph, string userId)
        {
            int id = 0;
            bool isNew = diagramObject.Id <= 0;
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

                            diagramObject.UserStories.Add(userStory);
                        }
                    }
                    if (!string.IsNullOrEmpty(graph))
                        diagramObject.activties = graph;

                    id  = repository.SaveOrUpdate(diagramObject, userId);
                    diagramObject.Id = id;
                    repository.SaveToHistory(diagramObject, userId);
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
            #region send email notification in case diagram updated
            try
            {
                if (!isNew)
                    SendUpdateEmail(diagramObject, userId);
            }
            catch (Exception ex)
            {


                Console.WriteLine("Cannot send email: " + ex.Message);
            }
            #endregion
            return id;

        }

        private void SendUpdateEmail(Attachment diagramObject,string userId)
        {
            MailService mailService = MailService.Instance;
            string _mailConfigFilePath = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\Mail.xml";
            mailService.LoadFile(_mailConfigFilePath);
            DTOMessage message = mailService.GetMessage("EditDiagram");
            var users = new List<AspNetUser>();
            string sendFrom = (ConfigurationSettings.AppSettings["MailFromAddress"] != null) ? ConfigurationSettings.AppSettings["MailFromAddress"].ToString() : "";
            diagramObject.UserStories.ToList().ForEach(f =>
            {
                users.AddRange(f.AspNetUsers.Except(users, new UserComparer()));
            });
            UserRepository uRepositorty = new UserRepository();
            var actionUser = uRepositorty.FindById(userId);
            foreach (AspNetUser user in users)
            {
                string messageSubject = MailService.formatMsg(message.Subject, new string[] { diagramObject.Id.ToString() });
                string bodyStr = MailService.formatMsg(message.Body, new string[] { diagramObject.Id.ToString(), user.UserName, actionUser.UserName });
                MailService.SendMessageWithAttachment(sendFrom, user.Email, null, messageSubject, bodyStr, null);
            }
        }

        public Attachment FindByID(int id)
        {
            return repository.Get(new Attachment() { Id = id });
        }

        public void LockDiagrams(List<Attachment> diagrams)
        {
            foreach(Attachment attach in diagrams)
            {
                attach.@readonly = true;
                repository.UpdateLock(attach);
            }
        }

        public List<Attachment> FindBySprint(int sprintId)
        {
            return repository.FindBySprintID(sprintId);
        }
    }
}
