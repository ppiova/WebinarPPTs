#pragma checksum "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\Pages\Review.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa56039bec1d5465f10bd7165e3e3dfec321bd35"
// <auto-generated/>
#pragma warning disable 1591
namespace BlazorSentiment.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#line 1 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#line 2 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#line 3 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#line 4 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#line 5 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#line 6 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using BlazorSentiment.Client;

#line default
#line hidden
#line 7 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\_Imports.razor"
using BlazorSentiment.Client.Shared;

#line default
#line hidden
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Review : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h2>Live Sentiment</h2>\r\n\r\n");
            __builder.OpenElement(1, "p");
            __builder.OpenElement(2, "textarea");
            __builder.AddAttribute(3, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#line 6 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\Pages\Review.razor"
                       UpdateScoreAsync

#line default
#line hidden
            ));
            __builder.AddAttribute(4, "cols", "45");
            __builder.AddAttribute(5, "placeholder", "Type any text like a short product review");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(6, "\r\n\r\n");
            __builder.OpenComponent<BlazorSentiment.Client.Shared.HappinessScale>(7);
            __builder.AddAttribute(8, "Value", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Single>(
#line 8 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\Pages\Review.razor"
                       happiness

#line default
#line hidden
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#line 10 "E:\GitMLDotNet\03-ScalableSentimentAnalysisBlazorWebApp\BlazorSentiment.Client\Pages\Review.razor"
       
    [Parameter]
    public float happiness { get; set; } = 50; // 0=worst, 100=best

    private async Task UpdateScoreAsync(ChangeEventArgs e)
    {
        string targetText = (string)e.Value;

        // Make a real call to Sentiment service
        happiness = await PredictSentimentAsync(targetText);
    }

    private async Task<float> PredictSentimentAsync(string targetText)
    {
        string url = $"api/Sentiment/sentimentprediction?sentimentText={targetText}";

        float percentage = await Http.GetJsonAsync<float>(url);

        return percentage;
    }

#line default
#line hidden
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
    }
}
#pragma warning restore 1591
