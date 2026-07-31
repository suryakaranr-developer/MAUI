namespace CompanyName.AppName.Design.MAUI.Material.Enumerations;

/// <summary>
/// The five Material Design 3 common button variants, in descending order of emphasis.
/// </summary>
/// <remarks>
/// Emphasis is the whole basis for choosing between them: one filled button per screen carries
/// the main action, and everything else steps down from there.
/// </remarks>
public enum MaterialButtonVariant
{
    /// <summary>Highest emphasis. The one action a screen is asking for.</summary>
    Filled,

    /// <summary>A filled button that lifts off the surface, for use over busy backgrounds.</summary>
    Elevated,

    /// <summary>Between filled and outlined. For actions that matter but are not the main one.</summary>
    FilledTonal,

    /// <summary>Medium emphasis. For secondary actions that still need a clear boundary.</summary>
    Outlined,

    /// <summary>Lowest emphasis. For actions in dialogs and cards, or alongside other content.</summary>
    Text
}

/// <summary>
/// The Material Design 3 icon button variants.
/// </summary>
public enum MaterialIconButtonVariant
{
    /// <summary>No container. For icons alongside other content, such as in an app bar.</summary>
    Standard,

    /// <summary>A filled container, for the highest emphasis icon action.</summary>
    Filled,

    /// <summary>A tonal container, between filled and outlined in emphasis.</summary>
    Tonal,

    /// <summary>An outlined container, for medium emphasis on an unfilled surface.</summary>
    Outlined
}

/// <summary>
/// The size of a floating action button.
/// </summary>
public enum FabSize
{
    /// <summary>40dp, for compact layouts or alongside a larger action.</summary>
    Small,

    /// <summary>56dp. The default.</summary>
    Medium,

    /// <summary>96dp, for the primary action on a large screen.</summary>
    Large
}

/// <summary>
/// The colour role a floating action button draws itself in.
/// </summary>
public enum FabColor
{
    /// <summary>The default, using the primary container role.</summary>
    Primary,

    /// <summary>Uses the secondary container role.</summary>
    Secondary,

    /// <summary>Uses the tertiary container role.</summary>
    Tertiary,

    /// <summary>Uses a plain surface, for a low emphasis floating action.</summary>
    Surface
}

/// <summary>
/// The Material Design 3 card variants.
/// </summary>
public enum MaterialCardVariant
{
    /// <summary>Lifted off the surface with a shadow. For content that needs separation.</summary>
    Elevated,

    /// <summary>A filled container with no shadow. The most subtle of the three.</summary>
    Filled,

    /// <summary>A bordered container. For the clearest boundary without a shadow.</summary>
    Outlined
}

/// <summary>
/// The Material Design 3 chip variants.
/// </summary>
public enum MaterialChipVariant
{
    /// <summary>Triggers an action related to the content it sits with.</summary>
    Assist,

    /// <summary>Narrows a set of content. Selectable, and shows a check when selected.</summary>
    Filter,

    /// <summary>Represents a discrete piece of information the user entered.</summary>
    Input,

    /// <summary>Offers a suggestion the user can accept.</summary>
    Suggestion
}

/// <summary>
/// The Material Design 3 text field variants.
/// </summary>
public enum MaterialTextFieldVariant
{
    /// <summary>A filled container with an underline. The higher emphasis of the two.</summary>
    Filled,

    /// <summary>A bordered container with the label notched into the top edge.</summary>
    Outlined
}
