namespace DataAccess
{
    public interface IConfigurationRepository
    {
        Task<ICollection<Configuration>> GetConfigurationAsync();
    }
}