namespace PolyLab3DStudio.Controls;

/// <summary>Picks the row template for a <see cref="CommandRow"/> by its kind.</summary>
public sealed class CommandRowTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CommandTemplate { get; set; }

    public DataTemplate? CommentTemplate { get; set; }

    public DataTemplate? SpacerTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container) =>
        item is CommandRow row
            ? row.Kind switch
            {
                CommandRowKind.Comment => CommentTemplate,
                CommandRowKind.Spacer => SpacerTemplate,
                _ => CommandTemplate,
            }
            : base.SelectTemplate(item, container);
}
