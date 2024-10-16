using Lecture_2024_2025_Notes.Utilities;
using NTUA_Notes.Models;
using NTUA_Notes.Source;
using NTUA_Notes.UI;

namespace NTUA_Notes;

public partial class MainPage : ContentPage
{
    int count = 0;
    public MainPage()
    {
        InitializeComponent();

        NoteView.OnNoteRemoved += RemoveNote;

        DirectoryInfo directoryInfo = new DirectoryInfo(Utilities.SaveDataPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles().OrderByDescending(file => file.CreationTime).ToArray();
        foreach (FileInfo file in fileInfos)
        {
            string fileName = file.Name;
            Utilities.LoadDataFromJson<NoteViewModel>(fileName, out NoteViewModel noteViewModel);
            NoteView noteView = new(noteViewModel);
            NotesStackLayout.Children.Add(noteView);
        }
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
                NotesStackLayout.Children.Remove(view);
                //delete related file
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

    private void RemoveNote(NoteView note)
    {
        NotesStackLayout.Children.Remove(note);
    }
}
