using System.Diagnostics;
using System.Text.RegularExpressions;
using Syncfusion.Maui;

namespace WaterTrackerMaui2;

public partial class MainPage : ContentPage
{
    enum TextFieldType
    {
        Requirement,
        Level
    }

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!Data.loaded) Data.Load();
        ForceUIUpdate();
    }

    public void OnDestroying()
    {
        Data.Save();
        ForceUIUpdate();
    }

    private void ForceUIUpdate()
    {
        Lena.Text = FormatString(Data.waterLevel);
        Maximilian.Text = FormatString(Data.requirement, TextFieldType.Requirement);
        Tobias.Text = Data.lastInput;
        Justus.Progress = (Data.waterLevel / Data.requirement) * 100;
    }

    private string FormatString(float input, TextFieldType textFieldType = TextFieldType.Level)
    {
        var output = "";


        /////////////Ignore this//////////////
        if (input >= 20000)
        {
            if (textFieldType == TextFieldType.Level)
            {
                output = "dead";
                return output;
            }
            else
            {
                output = "stop";
                return output;
            }
        }

        if (input < 0)
        {
            output = "dried out";
            return output;
        }
        //////////////////////////////////////
        

        if (input < 1000)
        {
            output = input + "ml";
        }
        else
        {
            output = input / 1000 + "L";
        }
        return output;
    }

    private void IncrementBtn1_Clicked(object sender, EventArgs e)
    {
        if (Tobias.Text == null) return;
        if (Regex.IsMatch(Tobias.Text, @"^\d+"))
        {
            if (Tobias.Text.ToLower().EndsWith("ml"))
            {
                Data.waterLevel += float.Parse(Tobias.Text.Remove(Tobias.Text.Length - 2));
            }
            else if (Tobias.Text.EndsWith("L"))
            {
                Data.waterLevel += float.Parse(Tobias.Text.Remove(Tobias.Text.Length - 1)) * 1000;
            }
            else
            {
                return;
            }
        }
        ForceUIUpdate();
    }

    private void DecrementBtn1_Clicked(object sender, EventArgs e)
    {

        if (Regex.IsMatch(Tobias.Text, @"^\d+"))
        {
            if (Tobias.Text.ToLower().EndsWith("ml"))
            {
                Data.waterLevel -= float.Parse(Tobias.Text.Remove(Tobias.Text.Length - 2));
            }
            else if (Tobias.Text.EndsWith("L"))
            {
                Data.waterLevel -= float.Parse(Tobias.Text.Remove(Tobias.Text.Length - 1)) * 1000;
            }
            else
            {
                return;
            }
        }
        ForceUIUpdate();
    }

    private void RequirementDecrease_Clicked(object sender, EventArgs e)
    {
        if (Data.requirement > 0) Data.requirement -= 250;
        ForceUIUpdate();
    }

    private void RequirementIncrease_Clicked(object sender, EventArgs e)
    {
        Data.requirement += 250;
        ForceUIUpdate();
    }

    private void Tobias_Focused(object sender, FocusEventArgs e)
    {
        Tobias.CursorPosition = 0;
        Tobias.SelectionLength = Tobias.Text != null ? Tobias.Text.Length : 0;
    }

    private void Tobias_TextChanged(object sender, TextChangedEventArgs e)
    {
        Data.lastInput = Tobias.Text;
    }

    private async void Stefan_Clicked(object sender, EventArgs e)
    {
        SettingsPage settingsPage = new SettingsPage();
        await Navigation.PushAsync(settingsPage);
    }

    private void Tobias_Unfocused(object sender, FocusEventArgs e)
    {
        //Makes sure that the input is empty if being emptied
        //it also adds L to the value if the number inside of
        //it is only one decimal place long and vice verca
        if (Tobias.Text == null || Tobias.Text == "") return;
        if (Tobias.Text.EndsWith("ml") || Tobias.Text.EndsWith("L")) return;
        if (Tobias.Text.Length == 1) Tobias.Text += "L";
        else Tobias.Text += "ml";
        Data.lastInput = Tobias.Text;
    }

    private void Justus_ProgressChanged(object sender, Syncfusion.Maui.ProgressBar.ProgressValueEventArgs e)
    {
        if (Justus.Progress >= 100) Justus.ProgressFill = Color.FromHex("#6DBC6D");
        else Justus.ProgressFill = Color.FromHex("#2c99c4");
    }

    private void Tobias_Completed(object sender, EventArgs e)
    {
        Tobias.Unfocus();
    }
}

