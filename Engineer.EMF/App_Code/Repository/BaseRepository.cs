namespace Engineer.EMF
{
    public class BaseRepository
    {
        protected Entities db = RepositoryLocator.loadDBContext();
    }
}