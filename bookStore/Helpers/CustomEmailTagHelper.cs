using Microsoft.AspNetCore.Razor.TagHelpers;


namespace bookStore.Helpers {

    public class CustomEmailTagHelper  : TagHelper {

        // dynamic
        public string MyEmail { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            
            // name of attribute
            output.TagName = "a";
            // setting
            // output.Attributes.SetAttribute("href", "mailto:githaigageorge96@gmail.com");
            output.Attributes.SetAttribute("href", "$mailto:{MyEmail}");
            // adding another attribute
            // output.Attributes.Add("Id", "my-email-id");
            // output.Content.SetContent("my-email-->githaigageorge96@gmail.com");
            output.Content.SetContent("my-email");
            // base.Process(context, output);
        }
    }
}