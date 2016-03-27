using Engineer.EMF;
using Engineer.EMF.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Engineer.Service
{
    public class UserStoryService
    {
        UserStoryRepository uRepository = new UserStoryRepository();
        public void FinishStory(UserStory story,string userId)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    story.state = AppConstants.USERSTORY_STATUS_FINISIHED;
                    uRepository.UpdateState(story);

                    #region Save to history
                    uRepository.SaveToHistory(story, userId);
                    #endregion
                    #region lock diagrams
                    DiagramService dService = (DiagramService)new ServiceLocator<Attachment>().locate();
                    var diagrams = dService.FindByUserStory(story.Id);
                    dService.LockDiagrams(diagrams);
                    #endregion


                    sc.Complete();
                }
                catch(Exception ex)
                {
                    throw new Exception(AppConstants.EXCEPTION_GLOBAL);
                }
                finally
                {
                    sc.Dispose();
                }
            }
                
        }

        public List<UserStory> FindBySprint(int sprintId)
        {
            return uRepository.FindBySprint(sprintId);
        }
        public List<UserStory> FindByUser(string userId)
        {
            return uRepository.FindByUser(userId);
        }

        public void SaveOrUpdate(UserStory storyObject, string userId,string storyUsers,string projectId)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    #region add users and project
                    if (!string.IsNullOrEmpty(storyUsers))
                    {
                        storyObject.AspNetUsers = new List<AspNetUser>();
                        UserRepository userR = new UserRepository();
                        foreach (string user in storyUsers.Split(','))
                        {
                            var exisUser = userR.FindById(user);
                            if (exisUser != null)
                                storyObject.AspNetUsers.Add(exisUser);
                        }

                    }
                    storyObject.projectId = !string.IsNullOrEmpty(projectId) ? int.Parse(projectId) : (int?)null;
                    #endregion

                    uRepository.SaveOrUpdate(storyObject, userId);
                    sc.Complete();
                }
                catch (BadRequestException e)
                {
                    throw new Exception(e.ErrorMessage);
                }
                catch (NotExistItemException e)
                {
                    throw new Exception(e.ErrorMessage);
                }
                catch(Exception ex)
                {
                    throw new Exception(AppConstants.EXCEPTION_GLOBAL);
                }
                finally
                {
                    sc.Dispose();
                }
            }
                
        }

        public List<UserStory> ListAll()
        {
            return uRepository.ListAll();
        }

        public void Delete(UserStory userStory)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    userStory.state = AppConstants.USERSTORY_STATUS_DELETED;
                    var exist = uRepository.Get(userStory);
                    uRepository.UpdateState(userStory);

                    #region all diagrams should be deleted
                    if (exist != null)
                    {
                        exist.UserStoryAttachments.ToList().ForEach(d =>
                        {
                            UserStoryAttachmentRepository dR = new UserStoryAttachmentRepository();
                            exist.state = AppConstants.DIAGRAM_STATUS_FINISIHED;
                            dR.UpdateStatus(d);
                        });
                    }
                    #endregion
                    sc.Complete();
                }
                catch (BadRequestException e)
                {
                    throw new Exception(e.ErrorMessage);
                }
                catch (NotExistItemException e)
                {
                    throw new Exception(e.ErrorMessage);
                }
                finally
                {
                    sc.Dispose();
                }
            }
                
        }

        public List<UserStory> FindByDiagramID(int diagramId)
        {
            try
            {
                return uRepository.FindByDiagramID(diagramId);
            }
            catch (Exception e)
            {
                throw new NotExistItemException(AppConstants.EXCEPTION_RETREIVE_STORY_OF_DIAGRAMS);
            }
        }
    }
}
