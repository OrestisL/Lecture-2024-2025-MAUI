﻿using NTUA_Notes.UI;

namespace NTUA_Notes;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FullNotePage), typeof(FullNotePage));
    }
}
