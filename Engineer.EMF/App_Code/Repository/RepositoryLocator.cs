namespace Engineer.EMF
{
    public class RepositoryLocator
    {
        private RepositoryLocator() { }
        private static Entities db = null;
    
        public static Entities loadDBContext()
        {
            if (db == null)
                db = new Entities();
            return db;
        }
    }
}