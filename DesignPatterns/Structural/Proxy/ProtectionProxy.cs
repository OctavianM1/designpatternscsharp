namespace DesignPatterns.Structural.Proxy;

public static class ProtectionProxy
{
    public interface ICar
    {
        void Drive();
    }

    public class Driver
    {
        public int Age { get; set; }
    }

    public class CarProxy : ICar
    {
        private readonly Driver _driver;
        private readonly Car _car = new Car();

        public CarProxy(Driver driver)
        {
            _driver = driver;
        }

        public void Drive()
        {
            if (_driver.Age >= 16)
            {
                _car.Drive();
            }
            else
            {
                Console.WriteLine("Too young");
            }
        }
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("Car is being driven");
        }
    }

    public static void Render()
    {
        ICar car = new CarProxy(new Driver { Age = 19 });
        car.Drive();
    }
}
