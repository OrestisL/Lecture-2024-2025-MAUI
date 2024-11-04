using Lecture_2024_2025_Notes.Utilities;
using NTUA_Notes.Models;
using NTUA_Notes.Source;
using System.Windows.Input;

namespace NTUA_Notes.UI;

public partial class FullNotePage : ContentPage
{
	private bool _isEditMode = false;
	private NoteViewModel _viewModel;

	private Entry _header;
	private Label _date;
	private Editor _body;

	public FullNotePage()
	{
		_viewModel = AppData.CurrentNoteModel;
		Title = "Note";
		//Main page grid, will hold:
		//First row: Header + edit + delete buttons
		//Second row: date
		//Third row: body
		Grid mainGrid = new Grid() 
		{
            RowDefinitions = 
			[
                new RowDefinition(new GridLength(60, GridUnitType.Absolute)),
                new RowDefinition(new GridLength(30, GridUnitType.Absolute)),
                new RowDefinition(new GridLength(1, GridUnitType.Star))
			],
			ColumnDefinitions = 
			[
				new ColumnDefinition(new GridLength(1, GridUnitType.Star)) 
			],
			Padding = 20,
		};

		Grid firstRow = new Grid()
		{
			RowDefinitions = [new RowDefinition(new GridLength(1, GridUnitType.Star))],
            ColumnDefinitions = 
			[
				new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
				new ColumnDefinition(new GridLength(50, GridUnitType.Absolute)),
				new ColumnDefinition(new GridLength(50, GridUnitType.Absolute)),
				new ColumnDefinition(new GridLength(50, GridUnitType.Absolute)),
			],
			ColumnSpacing = 5
        };

		_header = new Entry() 
		{
			Placeholder = "Header",
            FontSize = 20,
			FontAttributes = FontAttributes.Bold,
            Background = Colors.Transparent,
            IsReadOnly = !string.IsNullOrWhiteSpace(_viewModel.Header),
			Text = _viewModel.Header
		};
		_header.SetAppThemeColor(Entry.TextColorProperty, AppData.TextColorLightTheme, AppData.TextColorDarkTheme);
        _header.TextChanged += Header_TextChanged;

        Button editButton = new Button()
        {
            BackgroundColor = Colors.Transparent,
        };
		editButton.SetAppTheme(Button.ImageSourceProperty, "editlight.png", "editdark.png");
		editButton.Clicked += EditButton_Clicked;

		Button deleteButton = new Button()
		{
			BackgroundColor = Colors.Transparent,
		};
		deleteButton.SetAppTheme(Button.ImageSourceProperty, "deletelight.png", "deletedark.png");
        deleteButton.Clicked += DeleteButton_Clicked;

		Button saveButton = new Button()
		{
			BackgroundColor = Colors.Transparent,
		};
		saveButton.SetAppTheme(Button.ImageSourceProperty, "savelight.png", "savedark.png");
		saveButton.Clicked += (s, e) => SaveNote();

		firstRow.Add(_header, 0, 0);
		firstRow.Add(editButton, 1, 0);
		firstRow.Add(deleteButton, 2, 0);
		firstRow.Add(saveButton, 3, 0);

		mainGrid.Add(firstRow, 0, 0);

		_date = new Label()
        {
            FontSize = 14,
            TextColor = Colors.LightGray,
            Text = _viewModel.Date
        };
        mainGrid.Add(_date, 0, 1);

		_body = new Editor()
		{
			FontSize = 16,
			Placeholder = "Body",
			Background = Colors.Transparent,
			IsReadOnly = !string.IsNullOrWhiteSpace( _viewModel.Body),
			Text = _viewModel.Body,
			AutoSize = EditorAutoSizeOption.TextChanges
		};
		_body.SetAppThemeColor(Editor.TextColorProperty, AppData.TextColorLightTheme, AppData.TextColorDarkTheme);
		_body.TextChanged += Body_TextChanged;

        //Body should be inside a scroll view
        ScrollView bodyScroll = new ScrollView()
        {
            Content = _body,
			VerticalScrollBarVisibility = ScrollBarVisibility.Never
		};

        mainGrid.Add(bodyScroll, 0, 2);

		_isEditMode = AppData.CurrentNoteModel.Id == Guid.Empty;

		//BackButtonBehavior backButtonBehavior = new BackButtonBehavior()
		//{
		//	Command = new Command(() => OnBackButtonPressed()),
		//};

		//Shell.SetBackButtonBehavior(this, backButtonBehavior);

		Content = mainGrid;
	}

    private void Header_TextChanged(object? sender, TextChangedEventArgs e)
    {
		if (!_isEditMode)
			return;

		AppData.CurrentNoteModel.UpdateValues(e.NewTextValue, _body.Text, _date.Text);
    }

    private void Body_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (!_isEditMode)
            return;

        AppData.CurrentNoteModel.UpdateValues(_header.Text, e.NewTextValue, _date.Text);
    }

    private async void DeleteButton_Clicked(object? sender, EventArgs e)
    {
		bool answer = await DisplayAlert("Delete Note", "Are you sure you want to delete this note?", "Yes", "No");
		if (!answer)
			return;

		AppData.CurrentNoteModel.ToDelete = true;
		await Shell.Current.GoToAsync("..");
    }

    private void EditButton_Clicked(object? sender, EventArgs e)
	{
        _isEditMode = true;
		_header.IsReadOnly = !_isEditMode;
		_body.IsReadOnly = !_isEditMode;
    }

	
    private bool SaveNote()
    {
        Guid guid = Guid.Empty;
        string filename;

        if (AppData.CurrentNoteModel.Id == Guid.Empty)
        {
			if (string.IsNullOrWhiteSpace(AppData.CurrentNoteModel.Header) &
				string.IsNullOrWhiteSpace(AppData.CurrentNoteModel.Body))
                return base.OnBackButtonPressed();

            //file doesnt exist
            guid = Guid.NewGuid();
            AppData.CurrentNoteModel.Id = guid;
            filename = guid.ToString();
            Utilities.SaveDataToJson(filename, AppData.CurrentNoteModel);
            MainPage.OnNoteModelSaved.Invoke(AppData.CurrentNoteModel);
        }
        else
        {
            filename = AppData.CurrentNoteModel.Id.ToString();
            Utilities.SaveDataToJson(filename, AppData.CurrentNoteModel);
        }

		Shell.Current.GoToAsync("..", true);
		return base.OnBackButtonPressed();
    }

	protected override bool OnBackButtonPressed()
	{
		Guid guid = Guid.Empty;
		string filename;

		if (AppData.CurrentNoteModel.Id == Guid.Empty)
		{
			if (string.IsNullOrWhiteSpace(AppData.CurrentNoteModel.Header) &
				string.IsNullOrWhiteSpace(AppData.CurrentNoteModel.Body))
				return base.OnBackButtonPressed();

			//file doesnt exist
			guid = Guid.NewGuid();
			AppData.CurrentNoteModel.Id = guid;
			filename = guid.ToString();
			Utilities.SaveDataToJson(filename, AppData.CurrentNoteModel);
			MainPage.OnNoteModelSaved.Invoke(AppData.CurrentNoteModel);
		}
		else
		{
			filename = AppData.CurrentNoteModel.Id.ToString();
			Utilities.SaveDataToJson(filename, AppData.CurrentNoteModel);
		}
		return base.OnBackButtonPressed();
	}
}