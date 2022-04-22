namespace DesignPatterns.Creational.Builder;

public static class BuilderStepwise
{
    public enum CarType
    {
        Sedan,
        Crossover
    }

    public class Car
    {
        public CarType CarType { get; set; }
        public int WheelSize { get; set; }
    }

    public interface ISpecifyCarType
    {
        ISpecifyWheelSize OfType(CarType type); 
    }

    public interface ISpecifyWheelSize
    {
        IBuildCar WithWheels(int size);
    }

    public interface IBuildCar
    {
        public Car Build();
    }

    public class CarBuilder
    {
        private class Impl : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
        {
            private Car car = new();
            public Car Build()
            {
                return car;
            }

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.CarType = type;
                return this;
            }

            public IBuildCar WithWheels(int size)
            {
                switch (car.CarType)
                {
                    case CarType.Sedan when size < 17 || size > 20:
                    case CarType.Crossover when size < 15 || size > 17:
                        throw new ArgumentException($"Wrong size of wheel for {car.CarType}");
                }
                car.WheelSize = size;
                return this;
            }
        }

        public static ISpecifyCarType Create()
        {
            return new Impl();
        }
    }

    public static void Render()
    {
        var car = CarBuilder.Create().OfType(CarType.Crossover).WithWheels(18).Build();
    }
}
