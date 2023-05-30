using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui;
using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;

namespace WaterTrackerMaui2;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void ResetButton_Clicked(object sender, EventArgs e)
    {
        Data.Save(true);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Deletes '\user.txt' from the folder location
        //Idk why it does that I fucked it up somewhere in the code and won't ever fix it...
        FolderPickerInput.Text = Data.savePath.EndsWith(@"\user.txt") ? Data.savePath.Remove(Data.savePath.Length - 9) : Data.savePath;
    }

    private async void FolderPickerButton_Clicked(object sender, EventArgs e)
    {
        //Opens the folder picker menu
        //Starts at default initialPath if the Entry is empty
        //otherwise it starts at the path given in the Entry
        var folder = await FolderPicker.PickAsync(default);

        //Preserves the Entry text if populated and selection was cancelled
        //Handles edge-case when user would abort selection and Entry text was null
        if (FolderPickerInput.Text == null)
        {
            //Sets the Entry text depending on if the user has aborted the process or not
            //For some reason this is necessary since otherwise the app would just crash...
            FolderPickerInput.Text = folder.Folder == null ? FolderPickerInput.Text = null : FolderPickerInput.Text = folder.Folder.Path.ToString();
        }
        else
        {
            //Sets the Entry text depending on if the user has aborted the process or not
            //This also preserves the text that is already inside of the Entry if the process is aborted
            FolderPickerInput.Text = folder.Folder == null ? FolderPickerInput.Text = FolderPickerInput.Text : FolderPickerInput.Text = folder.Folder.Path.ToString();
            
            //This checks whether the new location already contains a user.txt file and then loads that file
            var path = FolderPickerInput.Text + @"\user.txt";
            Data.SaveConfig(path);
            if (File.Exists(path)) Data.Load();
        }
    }

    private void FolderPickerInput_TextChanged(object sender, TextChangedEventArgs e)
    {
        Data.savePath = FolderPickerInput.Text;
    }
}