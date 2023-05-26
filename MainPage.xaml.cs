using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WaterTrackerMAUI;

public partial class MainPage : ContentPage
{

    enum DataMethod
    {
        Save,
        Load
    }

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
        LoadAndSave(DataMethod.Load);
    }

    public void OnDestroying()
    {
        LoadAndSave(DataMethod.Save);
    }

    private void ForceUIUpdate()
    {
        Lena.Text = FormatString(Data.waterLevel);
        Maximilian.Text = FormatString(Data.requirement, TextFieldType.Requirement);
    }

    private void LoadAndSave(DataMethod method)
    {
        var appDataPath = FileSystem.Current.AppDataDirectory;
        var path = appDataPath + @"\user.txt";
        
        if (method == DataMethod.Load)
        {
            if (!File.Exists(path)) return;
            var split = File.ReadAllText(path).Split("|");
            Data.savedDate = DateTime.Parse(split[0]);
            Trace.WriteLine(Data.savedDate);

            if (CheckForDayDifference())
            {
                Trace.WriteLine("Date difference found!");
                Data.requirement = float.Parse(split[2]);
                File.Delete(path);
            }
            else
            {
                Trace.WriteLine("File Already Exists");
                Data.waterLevel = float.Parse(split[1]);
                Data.requirement = float.Parse(split[2]);
                Trace.WriteLine(split[1]);
                Trace.WriteLine(float.Parse(split[1]));                
                Trace.WriteLine(Data.waterLevel);
            }
            ForceUIUpdate();
        }
        else
        {
            if (File.Exists(path)) File.Delete(path);

            Trace.WriteLine("File Created");
            Trace.WriteLine(Data.waterLevel);

            File.Create(path).Dispose();
            File.WriteAllText(path, DateTime.Today + "|" + Data.waterLevel + "|" + Data.requirement);
        }
        
    }

    private bool CheckForDayDifference()
    {
        //Using month here as well since the user might log in on the same day of a different month
        //Let's just hope no one attempts to open the same app exactly one year late,
        //because then we'd have to implement a reset button...
        var today = DateTime.Today;

        var sameDay = (today.Day == Data.savedDate.Day) ? true : false;
        var sameMonth = (today.Month == Data.savedDate.Month) ? true : false;

        Trace.WriteLine(sameDay);
        Trace.WriteLine(sameMonth);

        if ((!sameDay && !sameMonth) || (!sameDay && sameMonth)) return true;
        return false;
    }

    private string FormatString(float input, TextFieldType textFieldType = TextFieldType.Level)
    {
        var output = "";

        if (input >= 20000)
        {
            if (textFieldType == TextFieldType.Level)
            {
                output = "lol ur dead";
                return output;
            }
            else
            {
                output = "no stop";
                return output;
            }
        }

        if (input < 0)
        {
            output = "ur dry";
            return output;
        }

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
        var factor = 1;
        var formatted_string = Tobias.Text;

        if (Tobias.Text.StartsWith("-"))
        {
            factor = -1;
            formatted_string = formatted_string.Split("-")[1];
        }

        if (Regex.IsMatch(formatted_string, @"^\d+"))
        {
            if (formatted_string.ToLower().EndsWith("ml"))
            {
                Data.waterLevel += float.Parse(formatted_string.Remove(formatted_string.Length - 2)) * factor;
            }
            else if (formatted_string.EndsWith("L"))
            {
                Data.waterLevel += float.Parse(formatted_string.Remove(formatted_string.Length - 1)) * 1000 * factor;
            }
            else
            {
                return;
            }
        }
        Trace.WriteLine(Data.waterLevel);
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
}

