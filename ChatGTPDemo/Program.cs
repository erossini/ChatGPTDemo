using ChatGTPDemo;
using ChatGTPDemo.Models;
using Spectre.Console;

var chatGPTClient = new ChatGPTClient();
var chatActive = true;

// Load an image
var image = new CanvasImage("openai_logo.png");

// Set the max width of the image.
// If no max width is set, the image will take
// up as much space as there is available.
image.MaxWidth(20);

// Render the image to the console
//AnsiConsole.Write(image);

Console.WriteLine();

AnsiConsole.Write(
    new FigletText("C# ChatGPT client")
        .LeftJustified()
        .Color(Color.NavajoWhite1));

Console.WriteLine();

AnsiConsole.MarkupLine("[#ffffff]Write a ChatGPT client[/]");
AnsiConsole.MarkupLine("[Grey]Experiment with ChartGPT in a C# console application[/]");

Console.WriteLine();

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OpenAIAPIKey")))
{
    AnsiConsole.MarkupLine("[#af0000]The OpenAPI key is not specified.[/]");
    AnsiConsole.MarkupLine("[#af0000]Please add the key in your project in the[/] [red]environment variable[/] [#af0000]section.[/]");
    return;
}

var rule = new Rule();
while (chatActive)
{
    AnsiConsole.Write(rule);
    var userMessage = AnsiConsole.Prompt(new TextPrompt<string>("[Grey]What is your [/][green]question[/][Grey]?[/]").AllowEmpty());

    if (string.IsNullOrWhiteSpace(userMessage))
    {
        chatActive = false;
    }
    else
    {
        ChatResponse chatResponse = null;

        AnsiConsole.Status()
                   .Spinner(Spinner.Known.Balloon)
                   .Start("Thinking...", ctx => {
                       chatResponse = chatGPTClient.SendMessage(userMessage).Result;
                   });

        if (chatResponse != null)
        {
            Console.WriteLine();

            var table = new Table();
            AnsiConsole.Live(table)
                       .Start(ctx =>
                       {
                           table.AddColumn("[yellow]Answer[/]");
                           ctx.Refresh();

                           foreach (var assistantMessage in chatResponse.Choices!.Select(c => c.Message))
                           {
                               table.AddRow(new Markup("[Grey]" + assistantMessage!.Content!.Trim().Replace("\n", "") + "[/]"));
                               ctx.Refresh();
                           }
                       });

            Console.WriteLine();
        }
        else
        {
            AnsiConsole.MarkupLine("[#af0000]I received an error when calling the API.[/]");
            chatActive = false;
        }
    }
}

Console.WriteLine();

rule = new Rule("[Grey]Thank you for using this demo![/]");
rule.RuleStyle("Grey dim");
AnsiConsole.Write(rule);