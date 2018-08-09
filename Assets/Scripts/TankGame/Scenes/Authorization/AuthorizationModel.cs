using SHLibrary.ObserverView;

namespace TankGame.Authorization
{
    public class AuthorizationModel : ObservableBase
    {
        public string UserName;
        public string Password;

        public bool IsLogged;

        public AuthorizationModel(bool isLogged, string userName, string password)
        {
            IsLogged = isLogged;
            UserName = userName;
            Password = password;
        }
    }
}