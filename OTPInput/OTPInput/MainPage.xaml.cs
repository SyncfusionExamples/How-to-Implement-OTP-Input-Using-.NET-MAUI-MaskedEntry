using Syncfusion.Maui.Inputs;

namespace OTPInput
{
    public partial class MainPage : ContentPage
    {
       private List<SfMaskedEntry> otpEntries = new();

       public MainPage()
       {
           InitializeComponent();
           InitializeOTPEntries();
       }

       /// <summary>
       /// Initialize OTP entry fields collection
       /// </summary>
       private void InitializeOTPEntries()
       {
           otpEntries = new List<SfMaskedEntry>
           {
               OTPEntry1, OTPEntry2, OTPEntry3, 
               OTPEntry4, OTPEntry5, OTPEntry6
           };
       }

       /// <summary>
       /// Handle text changes in OTP entry fields
       /// Auto-move to next field when digit is entered
       /// </summary>
       private void OnOTPEntryTextChanged(object? sender, MaskedEntryValueChangedEventArgs e)
       {
           if (sender is not SfMaskedEntry entry)
               return;

           // Check if text was entered
           if (e.NewValue?.ToString() != "#" && e.NewValue?.ToString()?.Length == 1)
           {
               // Find current entry index
               int currentIndex = otpEntries.IndexOf(entry);

               // Move to next entry if available
               if (currentIndex < otpEntries.Count - 1)
               {
                   MainThread.BeginInvokeOnMainThread(() =>
                   {
                       otpEntries[currentIndex + 1].Focus();
                   });
               }
               else
               {
                   // Last entry filled, validate OTP
                   ValidateOTP();
               }
           }
           else if (e.NewValue?.ToString() == "#" && e.OldValue?.ToString() != "#")
           {
               // Clear button or backspace - move to previous entry
               int currentIndex = otpEntries.IndexOf(entry);
               if (currentIndex > 0)
               {
                   MainThread.BeginInvokeOnMainThread(() =>
                   {
                       otpEntries[currentIndex - 1].Focus();
                   });
               }
           }

           // Enable verify button if all fields are filled
           UpdateVerifyButtonState();
       }

       /// <summary>
       /// Update verify button state based on OTP completion
       /// </summary>
       private void UpdateVerifyButtonState()
       {
           bool allFilled = otpEntries.All(entry => 
               !string.IsNullOrEmpty(entry.Value?.ToString()) && entry.Value?.ToString()?.Length == 1);
           
           VerifyButton.IsEnabled = allFilled;

           if (allFilled)
           {
               VerifyButton.BackgroundColor = Color.FromArgb("#512BD4");
           }
           else
           {
               VerifyButton.BackgroundColor = Color.FromArgb("#D3D3D3");
           }
       }

       private string GetCompleteOTP()
       {
            return string.Concat(otpEntries.Select(entry => entry.Value ?? ""));
       }
   
       private void ValidateOTP()
       {
            string otp = GetCompleteOTP();
        
            if (otp.Length == 6)
            {
                if (otp == "123456")
                {
                    ResultLabel.Text =
                        "✓ OTP Verified Successfully!";
        
                    ResultLabel.TextColor =
                        Color.FromArgb("#00AA00");
                }
                else
                {
                    ResultLabel.Text =
                        "✗ Invalid OTP. Please try again.";
        
                    ResultLabel.TextColor =
                        Color.FromArgb("#FF0000");
                }
            }
        }

        private async void OnVerifyClicked(object? sender,EventArgs e)
        {
            string otp = GetCompleteOTP();

            if (otp.Length == 6)
            {
                ValidateOTP();

                await DisplayAlert("OTP Verification",$"Entered OTP: {otp}","OK");
            }
        }

        private async void OnResendClicked(object? sender,EventArgs e)
        {
            ClearOTPEntries();

            await DisplayAlert("OTP Resent","Your OTP is 123456","OK");

            OTPEntry1.Focus();
        } 

        private void ClearOTPEntries()
        {
            foreach (var entry in otpEntries)
            {
                entry.Value = string.Empty;
            }

            ResultLabel.Text = string.Empty;

            UpdateVerifyButtonState();
        } 
    }
}
