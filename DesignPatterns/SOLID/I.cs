namespace DesignPatterns.SOLID;

public static class I
{
    public interface IMachine
    {
        public void Fax(Document d);
        public void Print(Document d);
        public void Scan(Document d);
    }
    public interface IPrinter
    {
        public void Print(Document d);
    }
    public interface IScanner
    {
        public void Scan(Document d);
    }
    public interface IFax
    {
        public void Fax(Document d);
    }

    public class Document
    {

    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Fax(Document d)
        {
            Console.WriteLine("FAX");
        }

        public void Print(Document d)
        {
            Console.WriteLine("PRINT");
        }

        public void Scan(Document d)
        {
            Console.WriteLine("SCAN");
        }
    }

    public class SimplePrinter : IMachine
    {
        public void Fax(Document d)
        {
            throw new NotImplementedException();
        }

        public void Print(Document d)
        {
            Console.WriteLine("Old - PRINT");
        }

        public void Scan(Document d)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMultiFunctionalPrinter : IScanner, IPrinter, IFax
    {

    }

    public class BetterMultiFunctionalPrinter : IMultiFunctionalPrinter
    {
        public void Fax(Document d)
        {
            Console.WriteLine("FAX");
        }

        public void Print(Document d)
        {
            Console.WriteLine("PRINT");
        }

        public void Scan(Document d)
        {
            Console.WriteLine("SCAN");
        }
    }

    public class BetterPrinter : IPrinter
    {
        public void Print(Document d)
        {
            Console.WriteLine("PRINT");
        }
    }

    public class BetterScanner : IScanner
    {
        public void Scan(Document d)
        {
            Console.WriteLine("SCAN");
        }
    }

    public class DecoratorMultiFunctionalMachine : IPrinter, IScanner
    {
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;

        public DecoratorMultiFunctionalMachine(IPrinter printer, IScanner scanner)
        {
            _printer = printer;
            _scanner = scanner;
        }

        public void Print(Document d)
        {
            _printer.Print(d);
        }

        public void Scan(Document d)
        {
            _scanner.Scan(d);
        }
    }

    public static void Render()
    {
        var document = new Document();
        IMachine simple = new SimplePrinter();
        IMachine multiFunctional = new MultiFunctionPrinter();

        IMultiFunctionalPrinter betterMultiFunctional = new BetterMultiFunctionalPrinter();
        IPrinter betterPrinter = new BetterPrinter();

        betterMultiFunctional.Fax(document);
        betterPrinter.Print(document);

        multiFunctional.Fax(document);
        simple.Fax(document);

        IScanner betterScanner = new BetterScanner();
        var decorator = new DecoratorMultiFunctionalMachine(betterPrinter, betterScanner);
        decorator.Scan(document);
        decorator.Print(document);
    }
}
