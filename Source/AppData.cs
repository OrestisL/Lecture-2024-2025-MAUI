using NTUA_Notes.Models;

namespace NTUA_Notes.Source;

public static class AppData
{
    #region data
    public static NoteViewModel CurrentNoteModel { get; set; } = new NoteViewModel();
    #endregion

    #region colors
    public static readonly Color TextColorLightTheme = Colors.Black;
    public static readonly Color TextColorDarkTheme = Colors.White;

    public static readonly Color NoteBGColorLightTheme = Colors.White;
    public static readonly Color NoteBGColorDarkTheme = Colors.Black;

    public static readonly Color BorderColorLightTheme = Colors.Black;
    public static readonly Color BorderColorDarkTheme = Colors.LightGray;

    public static readonly Color ButtonColorLightTheme = Colors.BlueViolet;
    public static readonly Color ButtonColorDarkTheme = Colors.YellowGreen;
    #endregion
}
