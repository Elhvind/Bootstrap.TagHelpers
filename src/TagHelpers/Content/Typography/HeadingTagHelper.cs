﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelpers.Content.Typography;

[HtmlTargetElement(tag: "heading", ParentTag = null)]
public class HeadingTagHelper : BootstrapTagHelperBase
{
    /// <summary>
    /// The level of the rendered HTML heading element.
    /// </summary>
    [HtmlAttributeName("level")]
    public HeadingLevel Level { get; set; } = HeadingLevel.H1;

    /// <summary>
    /// Imitates the heading level, for when you want to match the font styling of a heading but cannot use the associated HTML element.
    /// </summary>
    [HtmlAttributeName("imitate")]
    public bool Imitate { get; set; }

    /// <summary>
    /// Traditional heading elements are designed to work best in the meat of your page content.
    /// When you need a heading to stand out, consider using a display heading—a larger, slightly more opinionated heading style.
    /// </summary>
    [HtmlAttributeName("display-level")]
    public DisplayLevel? DisplayLevel { get; set; }

    /// <summary>
    /// Changes the color of the heading.
    /// </summary>
    [HtmlAttributeName("color")]
    public ThemeColor? Color { get; set; }

    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;

        if (!string.IsNullOrEmpty(Href))
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", Href);
            output.AddCssClasses(Level.ToString().ToLower());

            if (!Imitate)
            {
                output.Attributes.SetAttribute("role", "heading");
                output.Attributes.SetAttribute("aria-level", (int)Level);
            }
        }
        else if (Imitate)
        {
            output.TagName = "p";
            output.AddCssClasses(Level.ToString().ToLower());
        }
        else
        {
            output.TagName = Level.ToString().ToLower();
        }

        output.AddCssClasses(GetDisplayClass());

        if (Color.HasValue)
            output.AddCssClasses(GetColorClassName("text", Color.Value));
    }

    private string GetDisplayClass()
    {
        if (!DisplayLevel.HasValue)
            return "";

        return DisplayLevel.Value switch
        {
            Content.DisplayLevel.D1 => "display-1",
            Content.DisplayLevel.D2 => "display-2",
            Content.DisplayLevel.D3 => "display-3",
            Content.DisplayLevel.D4 => "display-4",
            _ => ""
        };
    }
}
