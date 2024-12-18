namespace videotheque.Services
{
    public interface IRetourService
    {
        Task<LocationDetail> GetLocationDetailByDVDId(int dvdId);
        Task<bool> ProcessDVDReturn(int dvdId);
        Task<(bool success, string message)> ValidateReturn(int dvdId);
    }
}
