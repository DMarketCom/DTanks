using DMarketSDK.Market.Commands.API;
using System;
using DMarketSDK.Common.Sound;
using DMarketSDK.IntegrationAPI;
using DMarketSDK.WidgetCore.Forms;
using SHLibrary.StateMachine;

namespace DMarketSDK.Market.States
{
    public abstract class MarketApiStateBase : MarketStateBase
    {
        protected WidgetWaitingForm WaitingForm { get { return Controller.GetForm<WidgetWaitingForm>(); } }
        protected WidgetErrorMessageBoxForm ErrorForm { get { return Controller.GetForm<WidgetErrorMessageBoxForm>(); } }
        protected WidgetMessageBoxForm MessageBoxForm { get { return Controller.GetForm<WidgetMessageBoxForm>(); } }

        public override void Finish()
        {
            base.Finish();
            WaitingForm.Hide();
            ErrorForm.Hide();
            MessageBoxForm.Hide();
        }

        protected void SendApiCommand(ApiCommandBase command, 
            bool blockUI = true,
            Action<ApiResponse> callback = null,
            bool showSuccessPopUp = false)
        {
            Action<CommandBase> finishHandler = null;
            finishHandler = delegate (CommandBase targetCommand)
            {
                targetCommand.CommandFinished -= finishHandler;
                if (blockUI)
                {
                    WaitingForm.Hide(targetCommand);
                }
                if (command.Response.IsSuccessful)
                {
                    if (showSuccessPopUp)
                    {
                        Controller.SoundManager.Play(MarketSoundType.SuccessNotification);
                        MessageBoxForm.Show("Success", "Request sent");
                    }
                }
                else if (command.Response.ErrorCode == ErrorCode.DMarketAccountNotVerified)
                {
                    Controller.SoundManager.Play(MarketSoundType.ErrorNotification);
                    MessageBoxForm.Show("Verify your account", "Your DMarket account is not verified so you can't perform this action. Please verify your account via email", "OK"); //TODO: refactor this: add catalog and enum of messages for message box
                }
                else
                {
                    Controller.SoundManager.Play(MarketSoundType.ErrorNotification);
                    ErrorForm.Show("Error", command.Response.ErrorTxt);
                }
                callback.SafeRaise(command.Response);
            };
            command.CommandFinished += finishHandler;

            if (blockUI)
            {
                WaitingForm.Show(command);
            }

            ApplyCommand(command);
        }
    }
}