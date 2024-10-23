using Lecture_2024_2025_Notes.Utilities;
using NTUA_Notes.Models;
using NTUA_Notes.Source;
using NTUA_Notes.UI;

namespace NTUA_Notes;

public partial class MainPage : ContentPage
{
    int count = 0;
    public static Action<NoteViewModel> OnNoteModelSaved = delegate { };
    public MainPage()
    {
        InitializeComponent();

        InitializeNoteViews();

        NoteView.OnNoteRemoved += DeleteNoteView;

        OnNoteModelSaved += AddNewNote;
    }

    private async void AddNoteButton_Clicked(object sender, EventArgs e)
    {
        AppData.CurrentNoteModel = new();
        await Shell.Current.GoToAsync(nameof(FullNotePage));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateNotes();
    }

    public void UpdateNotes() 
    {
        foreach (IView note in NotesStackLayout.Children)
        {
            NoteView view = note as NoteView;

            if (view == null) continue;

            if (view.ViewModel.ToDelete)
            {
                DeleteNoteView(view);
                break;
            }
            if (view.ViewModel.IsDirty)
            {
                view.UpdateView();
                NotesStackLayout.Children.Remove(view);
                NotesStackLayout.Children.Insert(0, view);
                break;
            }
        }
    }

    private void AddNewNote(NoteViewModel noteViewModel) 
    {
        NoteView note = new NoteView(noteViewModel);
        NotesStackLayout.Children.Insert(0, note);
    }

    private void InitializeNoteViews() 
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Utilities.SaveDataPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(file => file.CreationTime).ToArray();
        foreach (FileInfo file in fileInfos)
        {
            string fileName = file.Name;
            if (!Utilities.LoadDataFromJson<NoteViewModel>(fileName, out NoteViewModel noteViewModel))
                return;

            NotesStackLayout.Children.Add(new NoteView(noteViewModel));
        }
    }

    private void DeleteNoteView(NoteView view) 
    {
        NotesStackLayout.Children.Remove(view);
        DirectoryInfo directoryInfo = new DirectoryInfo(Utilities.SaveDataPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(file => file.CreationTime).ToArray();
        foreach (FileInfo file in fileInfos)
        {
            if (file.Name != view.ViewModel.Id.ToString())
                continue;

            File.Delete(file.FullName);
            return;
        }
    }
}
