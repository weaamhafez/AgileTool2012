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
    public class SprintService
    {
        SprintRepository uRepository = new SprintRepository();
        public List<Sprint> ListAll()
        {
            return uRepository.ListAll();
        }
        public List<UserStoryAttachment> CloseSprint(Sprint sprint)
        {
            var diagrams = new List<UserStoryAttachment>();
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    sprint.state = AppConstants.SPRINT_STATUS_CLOSED;
                    uRepository.UpdateState(sprint);
                    #region lock diagrams
                    DiagramService dService = (DiagramService)new ServiceLocator<Attachment>().locate();
                    diagrams = dService.FindBySprint(sprint.Id);
                    dService.LockDiagrams(diagrams);
                    #endregion
                    sc.Complete();
                }
                catch(Exception ex)
                {
                    throw new BadRequestException(AppConstants.EXCEPTION_SPRINT_CLOSE_ERROR);
                }
                finally
                {
                    sc.Dispose();
                }

            }
            return diagrams;
        }

        public void OpenSprint(Sprint sprint)
        {
            sprint.state = AppConstants.SPRINT_STATUS_OPEN;
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    uRepository.UpdateState(sprint);
                    #region lock diagrams
                    DiagramService dService = (DiagramService)new ServiceLocator<Attachment>().locate();
                    var diagrams = dService.FindBySprint(sprint.Id);
                    dService.UnLockDiagrams(diagrams);
                    #endregion
                    sc.Complete();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
                finally
                {
                    sc.Dispose();
                }
            }
               
        }


        public List<Sprint> FindByUser(string userId)
        {
            return uRepository.FindByUser(userId);
        }

        public void SaveOrUpdate(Sprint sprint, string userId,string userStories)
        {
            TransactionOptions _transcOptions = new TransactionOptions();
            _transcOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (TransactionScope sc = new TransactionScope(TransactionScopeOption.Required, _transcOptions, EnterpriseServicesInteropOption.Full))
            {
                try
                {
                    #region add user stories
                    if (!string.IsNullOrEmpty(userStories))
                    {
                        UserStoryRepository userR = new UserStoryRepository();
                        foreach (string user in userStories.Split(','))
                        {
                            var exisUser = userR.FindByID(user);
                            if (exisUser != null)
                                sprint.UserStories.Add(exisUser);
                        }

                    }
                    #endregion
                    uRepository.SaveOrUpdate(sprint, userId);
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

        public void Delete(Sprint sprint)
        {
            try
            {
                sprint.state = AppConstants.SPRINT_STATUS_DELETED;
                uRepository.UpdateState(sprint);
            }
            catch (BadRequestException e)
            {
                throw new Exception(e.ErrorMessage);
            }
            catch (NotExistItemException e)
            {
                throw new Exception(e.ErrorMessage);
            }
        }
    }
}
