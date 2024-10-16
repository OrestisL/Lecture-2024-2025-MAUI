using NTUA_Notes.Models;
using NTUA_Notes.UI;

namespace NTUA_Notes;

public partial class MainPage : ContentPage
{
    int count = 0;
    public static MainPage Instance { get ; private set; }
    public MainPage()
    {
        InitializeComponent();
        Instance = this;

        NoteView.OnNoteRemoved += RemoveNote;
    }

    private void AddNoteButton_Clicked(object sender, EventArgs e)
    {
        //setup notes here       
        for (int i = 0; i < 5; i++)
        {
            NoteViewModel viewModel = new NoteViewModel();
            viewModel.Header = "Test";
            viewModel.Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            //viewModel.Date = DateTime.Now;
            viewModel.Header += Random.Shared.NextInt64();
            NoteView noteView = new NoteView(viewModel);
            NotesStackLayout.Children.Add(noteView);
        }
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
