using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class SignInWindow : AccountDataWindowBase
    {
        [SerializeField] private Button _signInButton;

        protected override void SubscriptionsElementsUi()
        {
            base.SubscriptionsElementsUi();
            _signInButton.onClick.AddListener(SignIn);
        }

        private void SignIn()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _password
            }, result =>
            {
                Debug.Log($"Success: {_username}");
                EnterInGameScene();
            }, error =>
            {
                Debug.LogError($"Fail: {error.ErrorMessage}");
            });
        }
    }
}