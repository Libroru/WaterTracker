using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterTrackerMaui2
{
    internal static class Data
    {
        public static float waterLevel = 0;
        public static float requirement = 2500;
        public static string lastInput;
        public static DateTime savedDate = new DateTime();
        public static string savePath = FileSystem.Current.AppDataDirectory;
        public static bool loaded = false;

        /// <summary>
        /// Checks whether todays date and the date inside of user.txt matches or not.
        /// Depending on that information, it either returns true or false.
        /// </summary>
        /// <returns>Returns true if the date is mismatched and vice versa.</returns>
        private static bool CheckForDayDifference()
        {
            //Using month here as well since the user might log in on the same day of a different month
            //Let's just hope no one attempts to open the same app exactly one year late,
            //because then we'd have to implement a reset button...
            var today = DateTime.Today;

            var sameDay = (today.Day == Data.savedDate.Day) ? true : false;
            var sameMonth = (today.Month == Data.savedDate.Month) ? true : false;

            if ((!sameDay && !sameMonth) || (!sameDay && sameMonth)) return true;
            return false;
        }

        /// <summary>
        /// It does what it says.
        /// </summary>
        public static void Load()
        {
            var configPath = FileSystem.Current.AppDataDirectory + @"\config.txt";
            if (File.Exists(configPath))
            {
                savePath = File.ReadAllText(configPath);
            }
            else
            {
                savePath = FileSystem.Current.AppDataDirectory;
            }

            if (!File.Exists(savePath)) return;
            var split = File.ReadAllText(savePath).Split("|");
            Data.savedDate = DateTime.Parse(split[0]);

            if (CheckForDayDifference())
            {
                Data.requirement = float.Parse(split[2]);
                Data.lastInput = split[3];
                File.Delete(savePath);
            }
            else
            {
                Data.waterLevel = float.Parse(split[1]);
                Data.requirement = float.Parse(split[2]);
                Data.lastInput = split[3];
            }
            loaded = true;
        }
        /// <summary>
        /// It does what it says.
        /// </summary>
        /// <param name="reset">If set to true deletes the user.txt file, no matter if the file already exists or not.</param>
        public static void Save(bool reset = false)
        {
            var path = savePath.EndsWith(@"\user.txt") ? savePath : savePath + @"\user.txt";

            if (File.Exists(path) || reset) File.Delete(path);

            File.Create(path).Dispose();
            if (!reset) File.WriteAllText(path, DateTime.Today + "|" + Data.waterLevel + "|" + Data.requirement + "|" + Data.lastInput);
            else
            {
                File.WriteAllText(path, DateTime.Today + "|" + 0 + "|" + 3000 + "|");
                loaded = false;
            }
            SaveConfig(path);
        }

        /// <summary>
        /// This method saves the new save path to the config files inside of AppDataLocal.
        /// </summary>
        /// <param name="newPath">A new path where the new config file is or should be stored in.</param>
        public static void SaveConfig(string newPath)
        {
            var configPath = FileSystem.Current.AppDataDirectory + @"\config.txt";
            if (File.Exists(configPath)) File.Delete(configPath);
            File.Create(configPath).Dispose();
            File.WriteAllText(configPath, newPath);
        }
    }
}