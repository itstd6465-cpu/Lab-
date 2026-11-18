using System.Diagnostics;

namespace AdaptiveLab;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
    }

    void OnShowPasswordClicked(object? sender, EventArgs e)
    {
        bool wasHidden = PasswordEntry.IsPassword;
        PasswordEntry.IsPassword = !wasHidden;
        ConfirmEntry.IsPassword = !wasHidden;
        // Part C: the button's spoken name must match what it will do next.
        SemanticProperties.SetDescription(ShowPasswordButton,
            wasHidden ? "Hide password" : "Show password");
    }

    void OnTermsTapped(object? sender, TappedEventArgs e) =>
        TermsCheckBox.IsChecked = !TermsCheckBox.IsChecked;

    void OnCreateClicked(object? sender, EventArgs e)
    {
        string message;
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrEmpty(PasswordEntry.Text))
        {
            message = "Please fill in name, email and password.";
        }
        else if (PasswordEntry.Text != ConfirmEntry.Text)
        {
            message = "The two passwords do not match.";
        }
        else if (!TermsCheckBox.IsChecked)
        {
            message = "Please accept the terms to continue.";
        }
        else
        {
            message = $"Account created for {NameEntry.Text.Trim()}.";
        }

        StatusLabel.Text = message;
        SemanticScreenReader.Default.Announce(message); // TalkBack reads the result aloud
    }

    // Logs the width the layout actually receives. You need these numbers in Part B.
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        Debug.WriteLine($"[Layout] page {width:F0} x {height:F0}, window {Window?.Width:F0}");
    }
}
