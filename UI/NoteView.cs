using NTUA_Notes.Models;
using Microsoft.Maui.Controls.Shapes;
using NTUA_Notes.Source;

namespace NTUA_Notes.UI;

public class NoteView : ContentView
{
    private Label _header;
    private Label _body;
    private Label _date;
    private NoteViewModel _viewModel;

    public NoteViewModel ViewModel => _viewModel;

    public NoteView(NoteViewModel model)
    {
        _viewModel = model;

        _header = new Label();
        _header.FontSize = 20;
        _header.Text = model.Header;
        _header.SetAppThemeColor(Label.TextColorProperty, AppData.TextColorLightTheme, AppData.TextColorDarkTheme);

        _body = new Label();
        _body.FontSize = 16;
        _body.LineBreakMode = LineBreakMode.TailTruncation;
        _body.TextColor = Colors.White;
        _body.Text = model.Body;
        _body.MaximumHeightRequest = 60;
        _body.SetAppThemeColor(Label.TextColorProperty, AppData.TextColorLightTheme, AppData.TextColorDarkTheme);

        _date = new Label();
        _date.FontSize = 14;
        _date.TextColor = Colors.LightGray;
        _date.Text = model.Date;
        _date.SetAppThemeColor(Label.TextColorProperty, AppData.TextColorLightTheme, AppData.TextColorDarkTheme);

        Grid grid = new Grid()
        {
            RowDefinitions =
            [
                new RowDefinition(new GridLength(1.2, GridUnitType.Star)),
                new RowDefinition(new GridLength(1, GridUnitType.Star)),
                new RowDefinition(new GridLength(0.6, GridUnitType.Star)),
            ],
            ColumnDefinitions =
            [
                new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
            ],
            RowSpacing = 5,
            Padding = 5,
            BackgroundColor = Colors.Transparent,
        };

        grid.Add(_header, 0, 0);
        grid.Add(_body, 0, 1);
        grid.Add(_date, 0, 2);

        Border noteView = new Border()
        {
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(8)
            },
            Content = grid,
            Stroke = new SolidColorBrush(Colors.Gray),
        };
        noteView.SetAppThemeColor(Border.StrokeProperty, AppData.BorderColorLightTheme, AppData.BorderColorDarkTheme);

        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnNoteTapped;
        grid.GestureRecognizers.Add(tapGestureRecognizer);

        SwipeItem swipeDelete = new SwipeItem()
        {
            Text = "Delete",
            BackgroundColor = Colors.LightPink,        
        };

        swipeDelete.SetAppTheme(SwipeItem.IconImageSourceProperty, "deletelight.png", "deletedark.png");
        swipeDelete.Invoked += SwipeDelete_Invoked;

        SwipeView swipeView = new SwipeView()
        {
            LeftItems = [swipeDelete],
            Content = noteView,
        };

        Border swipeBorder = new Border()
        {
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(8, 0, 8, 0)
            },
            Content = swipeView,
        };

        Content = swipeBorder;
    }

    private void SwipeDelete_Invoked(object? sender, EventArgs e)
    {
        _viewModel.ToDelete = true;
        MainPage.Instance.UpdateNotes();
    }

    public void UpdateView()
    {
        _header.Text = _viewModel.Header;
        _body.Text = _viewModel.Body;
        _date.Text = _viewModel.Date;
        _viewModel.IsDirty = false;
    }

    private async void OnNoteTapped(object? sender, EventArgs e)
    {
        AppData.CurrentNoteModel = _viewModel;
        await Shell.Current.GoToAsync(nameof(FullNotePage));
    }
}