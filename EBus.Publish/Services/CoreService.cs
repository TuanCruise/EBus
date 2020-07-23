namespace EBus.Core.Services
{
    public interface ICoreService
    {
        string WelcomeEquinox();
    }
    public class CoreService : ICoreService
    {
        public string welcomeEquinoxStr { get; set; }
        public CoreService()
        {
            welcomeEquinoxStr = "Welcome Folks in .Net Core presentation!"; //Configuration["WelcomeEquinox"];  
        }
        public string WelcomeEquinox()
        {
            return welcomeEquinoxStr;
        }
    }

}
