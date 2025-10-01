using System;
using System.Globalization;
public interface IVehicle
{
    void Drive();
    void Refuel();
}

public class Car : IVehicle
{
    public string Make { get; }
    public string Model { get; }
    public string FuelType { get; }

    public Car(string make, string model, string fuelType)
    {
        Make = make; Model = model; FuelType = fuelType;
    }

    public void Drive()  => Console.WriteLine($"Car: {Make} {Model} едет.");
    public void Refuel() => Console.WriteLine($"Car: заправка ({FuelType}).");
}

public class Motorcycle : IVehicle
{
    public string MotoType { get; }
    public int EngineCc { get; } 

    public Motorcycle(string motoType, int engineCc)
    {
        MotoType = motoType; EngineCc = engineCc;
    }

    public void Drive()  => Console.WriteLine($"Motorcycle: {MotoType} {EngineCc}cc едет.");
    public void Refuel() => Console.WriteLine("Motorcycle: заправка бензином.");
}

public class Truck : IVehicle
{
    public double CapacityTons { get; }
    public int Axles { get; }

    public Truck(double capacityTons, int axles)
    {
        CapacityTons = capacityTons; Axles = axles;
    }

    public void Drive()  => Console.WriteLine($"Truck: {CapacityTons} т, осей: {Axles} - едет.");
    public void Refuel() => Console.WriteLine("Truck: заправка дизелем.");
}

public class Bus : IVehicle
{
    public int Seats { get; }
    public string FuelType { get; }

    public Bus(int seats, string fuelType)
    {
        Seats = seats; FuelType = fuelType;
    }

    public void Drive()  => Console.WriteLine($"Bus: мест {Seats} - едет.");
    public void Refuel() => Console.WriteLine($"Bus: заправка ({FuelType}).");
}

public class EScooter : IVehicle
{
    public int BatteryWh { get; }

    public EScooter(int batteryWh) => BatteryWh = batteryWh;

    public void Drive()  => Console.WriteLine($"E-Scooter: батарея {BatteryWh} Wh - едет.");
    public void Refuel() => Console.WriteLine("E-Scooter: зарядка аккумулятора.");
}

public abstract class VehicleFactory
{
    public abstract IVehicle CreateVehicle();
}
public class CarFactory : VehicleFactory
{
    private readonly string _make, _model, _fuelType;
    public CarFactory(string make, string model, string fuelType)
    {
        _make = make; _model = model; _fuelType = fuelType;
    }
    public override IVehicle CreateVehicle() => new Car(_make, _model, _fuelType);
}

public class MotorcycleFactory : VehicleFactory
{
    private readonly string _motoType; private readonly int _engineCc;
    public MotorcycleFactory(string motoType, int engineCc)
    {
        _motoType = motoType; _engineCc = engineCc;
    }
    public override IVehicle CreateVehicle() => new Motorcycle(_motoType, _engineCc);
}

public class TruckFactory : VehicleFactory
{
    private readonly double _capacityTons; private readonly int _axles;
    public TruckFactory(double capacityTons, int axles)
    {
        _capacityTons = capacityTons; _axles = axles;
    }
    public override IVehicle CreateVehicle() => new Truck(_capacityTons, _axles);
}

public class BusFactory : VehicleFactory
{
    private readonly int _seats; private readonly string _fuelType;
    public BusFactory(int seats, string fuelType)
    {
        _seats = seats; _fuelType = fuelType;
    }
    public override IVehicle CreateVehicle() => new Bus(_seats, _fuelType);
}

public class EScooterFactory : VehicleFactory
{
    private readonly int _batteryWh;
    public EScooterFactory(int batteryWh) => _batteryWh = batteryWh;
    public override IVehicle CreateVehicle() => new EScooter(_batteryWh);
}

public static class VehicleConsoleFactory
{
    public static IVehicle CreateFromUserInput()
    {
        Console.Write("Тип (car/moto/truck/bus/escooter): ");
        var type = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

        switch (type)
        {
            case "car":
                Console.Write("Марка: "); var make = Console.ReadLine();
                Console.Write("Модель: "); var model = Console.ReadLine();
                Console.Write("Топливо (бензин/дизель/электро): "); var fuel = Console.ReadLine();
                return new CarFactory(make ?? "", model ?? "", fuel ?? "бензин").CreateVehicle();

            case "moto":
                Console.Write("Тип (спортивный/туристический/...): "); var mtype = Console.ReadLine();
                Console.Write("Объем двигателя (cc): "); int cc = ReadInt();
                return new MotorcycleFactory(mtype ?? "спортивный", cc).CreateVehicle();

            case "truck":
                Console.Write("Грузоподъемность (тонн): "); double tons = ReadDouble();
                Console.Write("Количество осей: "); int axles = ReadInt();
                return new TruckFactory(tons, axles).CreateVehicle();

            case "bus":
                Console.Write("Мест: "); int seats = ReadInt();
                Console.Write("Топливо: "); var bfuel = Console.ReadLine();
                return new BusFactory(seats, bfuel ?? "дизель").CreateVehicle();

            case "escooter":
                Console.Write("Емкость батареи (Wh): "); int wh = ReadInt();
                return new EScooterFactory(wh).CreateVehicle();

            default:
                throw new ArgumentException("Неизвестный тип транспорта.");
        }
    }
    private static int ReadInt()
    {
        while (true)
        {
            var s = Console.ReadLine();
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)) return v;
            Console.Write("Введите целое число: ");
        }
    }
    private static double ReadDouble()
    {
        while (true)
        {
            var s = Console.ReadLine();
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v)) return v;
            Console.Write("Введите число: ");
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Создание транспорта по Фабричному методу.");
        try
        {
            IVehicle v = VehicleConsoleFactory.CreateFromUserInput();
            v.Drive();
            v.Refuel();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }

        var fleet = new IVehicle[]
        {
            new CarFactory("Toyota","Camry","бензин").CreateVehicle(),
            new MotorcycleFactory("туристический", 1200).CreateVehicle(),
            new TruckFactory(10.5, 3).CreateVehicle(),
            new BusFactory(45,"дизель").CreateVehicle(),
            new EScooterFactory(480).CreateVehicle()
        };

        Console.WriteLine("\nПримеры:");
        foreach (var veh in fleet) { veh.Drive(); veh.Refuel(); }
    }
}