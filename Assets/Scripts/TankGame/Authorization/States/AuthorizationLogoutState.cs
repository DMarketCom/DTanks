using Networking.Msg;
using SHLibrary.Utils;

namespace TankGame.Authorization.States
{
    public class AuthorizationLogoutState : AuthorizationStateBase
    {
        public override void Start(object[] args = null)
        {
            base.Start(args);

            Model.IsLogged = false;
            Model.SetChanges();
            Controller.LogOut.SafeRaise();
            Client.Send(new LogoutMessage());

            ApplyState<AuthorizationLoginState>();
        }
    }
}