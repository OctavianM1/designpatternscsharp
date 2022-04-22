using Autofac;
using Autofac.Features.Metadata;
using MoreLinq;

namespace DesignPatterns.Structural.Adapter;

internal static class AdapterDI
{
    public interface ICommand
    {
        void Execute();
    }

    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving a file...");
        }
    }

    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file...");
        }
    }

    public class Button
    {
        private ICommand command;
        private readonly string _name;

        public Button(ICommand command, string name)
        {
            this.command = command;
            _name = name;
        }

        public void onClick()
        {
            command.Execute();
        }

        public void PrintMe()
        {
            Console.WriteLine($"I am a button called {_name}");
        }
    }

    public class Editor
    {
        private readonly IEnumerable<Button> _buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            this._buttons = buttons;
        }

        internal IEnumerable<Button> Buttons => _buttons;

        public void ClickAll()
        {
            Buttons.ForEach(x => x.onClick());
        }
    }

    public static void Render()
    {
        var b = new ContainerBuilder();
        b.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");
        b.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
        //b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd));
        b.RegisterAdapter<Meta<ICommand>, Button>(cmd => new Button(cmd.Value, (string)cmd.Metadata["Name"]));
        b.RegisterType<Editor>();

        using var c = b.Build();
        var editor = c.Resolve<Editor>();
        editor.ClickAll();
        editor.Buttons.ForEach(x => x.PrintMe());
    }
}
