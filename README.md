# How-to-Implement-OTP-Input-Using-.NET-MAUI-MaskedEntry

This section explains how to build an OTP (One-Time Password) input form in .NET MAUI using the SfMaskedEntry control. By using individual masked input fields for each OTP digit, you can create a secure and user-friendly verification experience. The implementation automatically moves focus between fields, validates the entered OTP, enables the verification button only when all digits are entered, and supports OTP resending functionality.

# XAML

Define six SfMaskedEntry controls, each configured to accept a single numeric digit. The controls are arranged horizontally to create a familiar OTP input experience. A Verify button, status label, and resend option are also included.

```xml
    <Grid RowDefinitions="Auto,*" 
          Padding="20">
       <!-- Header Section -->
       <VerticalStackLayout Spacing="10" 
                            Padding="0,20">
           <Label Text="Enter Your OTP" 
                  FontSize="24" 
                  FontAttributes="Bold"
                  HorizontalTextAlignment="Center"/>
           <Label Text="A 6-digit code has been sent to your registered email" 
                  FontSize="14"
                  TextColor="Gray"
                  HorizontalTextAlignment="Center"/>
       </VerticalStackLayout>

       <!-- OTP Input Section -->
       <VerticalStackLayout Grid.Row="1" 
                            Spacing="30" 
                            VerticalOptions="Center">
           <!-- OTP Input Fields -->
           <HorizontalStackLayout Spacing="12" 
                                  HorizontalOptions="Center">
               <editors:SfMaskedEntry x:Name="OTPEntry1"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/>

               <editors:SfMaskedEntry x:Name="OTPEntry2"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/>

               <editors:SfMaskedEntry x:Name="OTPEntry3"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/>

               <editors:SfMaskedEntry x:Name="OTPEntry4"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/> 

               <editors:SfMaskedEntry x:Name="OTPEntry5"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/>

               <editors:SfMaskedEntry x:Name="OTPEntry6"
                                      WidthRequest="50"
                                      HeightRequest="50"
                                      MaskType="Simple"
                                      Mask="0"
                                      Placeholder="0"
                                      HorizontalTextAlignment="Center"
                                      FontSize="18"
                                      FontAttributes="Bold"
                                      ClearButtonVisibility="Never"
                                      PromptChar="#"
                                      Stroke="LightGray"
                                      ValueChanged="OnOTPEntryTextChanged"/>
           </HorizontalStackLayout>

           <!-- Verify Button -->
           <Button x:Name="VerifyButton"
                   Text="Verify OTP"
                   Padding="20,12"
                   CornerRadius="8"
                   FontSize="16"
                   FontAttributes="Bold"
                   BackgroundColor="#512BD4"
                   TextColor="White"
                   HorizontalOptions="Center"
                   WidthRequest="200"
                   Clicked="OnVerifyClicked"
                   IsEnabled="False"/>

           <!-- Result Label -->
           <Label x:Name="ResultLabel"
                  HorizontalTextAlignment="Center"
                  FontSize="14"
                  Margin="0,20"/>

           <!-- Resend Section -->
           <HorizontalStackLayout Spacing="5" 
                                  HorizontalOptions="Center">
               <Label Text="Didn't receive the code?" 
                      TextColor="Gray"/>
               <Label Text="Resend" 
                      TextColor="#512BD4" 
                      FontAttributes="Bold">
                   <Label.GestureRecognizers>
                       <TapGestureRecognizer Tapped="OnResendClicked"/>
                   </Label.GestureRecognizers>
               </Label>
           </HorizontalStackLayout>
       </VerticalStackLayout>
   </Grid>
```

# C#

Handle the ValueChanged event of each SfMaskedEntry control to automatically move focus between OTP fields as digits are entered. The Verify button is enabled only when all six fields contain valid input. When the OTP is complete, it can be validated and displayed to the user.

```csharp
using Syncfusion.Maui.Inputs;

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
private void OnOTPEntryTextChanged(
    object? sender,
    MaskedEntryValueChangedEventArgs e)
{
    if (sender is not SfMaskedEntry entry)
        return;

    if (e.NewValue?.ToString() != "#" &&
        e.NewValue?.ToString()?.Length == 1)
    {
        int currentIndex = otpEntries.IndexOf(entry);

        if (currentIndex < otpEntries.Count - 1)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                otpEntries[currentIndex + 1].Focus();
            });
        }
        else
        {
            ValidateOTP();
        }
    }
    else if (e.NewValue?.ToString() == "#" &&
             e.OldValue?.ToString() != "#")
    {
        int currentIndex = otpEntries.IndexOf(entry);

        if (currentIndex > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                otpEntries[currentIndex - 1].Focus();
            });
        }
    }

    UpdateVerifyButtonState();
}

/// <summary>
/// Update verify button state
/// </summary>
private void UpdateVerifyButtonState()
{
    bool allFilled = otpEntries.All(entry =>
        !string.IsNullOrEmpty(entry.Value?.ToString()) &&
        entry.Value?.ToString()?.Length == 1);

    VerifyButton.IsEnabled = allFilled;

    VerifyButton.BackgroundColor = allFilled
        ? Color.FromArgb("#512BD4")
        : Color.FromArgb("#D3D3D3");
}

private string GetCompleteOTP()
{
    return string.Concat(
        otpEntries.Select(entry => entry.Value ?? ""));
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

private async void OnVerifyClicked(
    object? sender,
    EventArgs e)
{
    string otp = GetCompleteOTP();

    if (otp.Length == 6)
    {
        ValidateOTP();

        await DisplayAlert(
            "OTP Verification",
            $"Entered OTP: {otp}",
            "OK");
    }
}

private async void OnResendClicked(
    object? sender,
    EventArgs e)
{
    ClearOTPEntries();

    await DisplayAlert(
        "OTP Resent",
        "Your OTP is 123456",
        "OK");

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
```

The SfMaskedEntry control simplifies OTP entry by restricting each field to a single numeric digit and enforcing consistent input. It automatically advances focus to the next field when a digit is entered, supports backward navigation when characters are removed, and enables OTP validation once all fields are completed. Combined with verification and resend functionality, this approach creates a smooth and intuitive OTP authentication experience while improving input accuracy and usability.