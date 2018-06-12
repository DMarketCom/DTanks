using System.Collections;
using DMarketSDK.Widget;
using DMarketSDK.Widget.Forms;
using TankGame.Shop;
using UnityEngine;

namespace DevInstruments.AutoFlow
{
    public class AutoWidgetLogin : AutoFlowBase<ShopSceneController>
    {
        private IWidget Widget { get { return SceneController.Widget; } }
        
        protected override void ApplyFlowOperation()
        {
            if (!Widget.IsLogged)
            {
                StartCoroutine(MakeWidgetStepByStepAutorization());
            }
        }

        private IEnumerator MakeWidgetStepByStepAutorization()
        {
            SceneController.View.SendMessage("OnBasicWidgetClicked");
            yield return new WaitWhile(() => !SceneController.Widget.IsInitialize);
            yield return new WaitForSeconds(0.1f);
            var loginForm = GameObject.FindObjectOfType<WidgetLoginForm>();
            loginForm.LoginField.text = Settings.LoginMarket;
            loginForm.PasswordField.text = Settings.PasswordMarket;
            yield return new WaitForSeconds(0.3f);
            Widget.LoginEvent += OnWidgetLogin;
            loginForm.SendButton.onClick.Invoke();
            yield return null;
        }

        private void OnWidgetLogin(LoginEventData loginEventData)
        {
            Widget.LoginEvent -= OnWidgetLogin;
            StartCoroutine(CloseWidget());
        }

        private IEnumerator CloseWidget()
        {
            yield return new WaitForSeconds(0.1f);
            Widget.Close();
        }
    }
}
