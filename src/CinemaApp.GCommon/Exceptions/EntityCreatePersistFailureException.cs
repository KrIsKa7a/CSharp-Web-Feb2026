namespace CinemaApp.GCommon.Exceptions
{
    public class EntityCreatePersistFailureException : Exception
    {
        public EntityCreatePersistFailureException()
        {
            
        }

        public EntityCreatePersistFailureException(string message)
            : base(message)
        {
            
        }
    }
}
