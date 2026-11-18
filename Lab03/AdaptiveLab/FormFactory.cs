namespace AdaptiveLab;

public static class FormFactory
{
    // N rows -> N + 1 layouts: an outer stack holding one horizontal stack per row.
    public static (View Root, View Last) BuildNested(int rows)
    {
        var outer = new VerticalStackLayout { Spacing = 8, Padding = 16 };
        View last = outer;
        for (int i = 0; i < rows; i++)
        {
            var row = new HorizontalStackLayout { Spacing = 12 };
            row.Children.Add(new Label { Text = $"Field {i + 1}", VerticalOptions = LayoutOptions.Center });
            var entry = new Entry { Placeholder = $"Value {i + 1}" };
            row.Children.Add(entry);
            outer.Children.Add(row);
            last = entry;
        }
        return (outer, last);
    }

    // N rows -> 1 layout: one Grid, label in column 0, entry in column 1.
    public static (View Root, View Last) BuildFlat(int rows)
    {
        var grid = new Grid { ColumnSpacing = 12, RowSpacing = 8, Padding = 16 };
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
        grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        View last = grid;
        for (int i = 0; i < rows; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.Add(new Label { Text = $"Field {i + 1}", VerticalOptions = LayoutOptions.Center }, 0, i);
            var entry = new Entry { Placeholder = $"Value {i + 1}" };
            grid.Add(entry, 1, i);
            last = entry;
        }
        return (grid, last);
    }
}
