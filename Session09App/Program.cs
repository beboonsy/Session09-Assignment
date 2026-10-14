// ======================================================================
// Session09 Assignment - everything in one Program.cs
// Requires: .NET 8 console project (ImplicitUsings enabled, Nullable enabled)
// ======================================================================

// ---------------------------- Patient.cs ----------------------------
namespace Session09App
{
    // Q1: class with a PRIMARY CONSTRUCTOR (C# 12).
    // Primary-constructor parameters are NOT properties in a class,
    // so we expose them through properties initialised from the parameters.
    public class Patient(int id, string fullName, string phoneNumber, string medicalHistory)
    {
        public int Id { get; } = id;
        public string FullName { get; } = fullName;
        public string PhoneNumber { get; } = phoneNumber;
        public string MedicalHistory { get; } = medicalHistory;

        public override string ToString()
        {
            return $"Id: {Id} :: FullName: {FullName} :: PhoneNumber: {PhoneNumber} :: MedicalHistory: {MedicalHistory}";
        }
    }
}

// ---------------------------- PatientDto.cs ----------------------------
namespace Session09App
{
    // Q3: positional record that exposes only non-sensitive data
    // (no MedicalHistory). The compiler generates properties, Equals,
    // GetHashCode, ToString, deconstruction and "with" support for us.
    public record PatientDto(int Id, string FullName, string PhoneNumber);
}

// ---------------------------- PatientMapper.cs ----------------------------
namespace Session09App
{
    // Q4
    public static class PatientMapper
    {
        public static PatientDto MapFromModelToDto(Patient patient)
        {
            return new PatientDto(patient.Id, patient.FullName, patient.PhoneNumber);
        }
    }
}

// ---------------------------- AppLogger.cs ----------------------------
namespace Session09App
{
    // Q5: Singleton pattern
    public class AppLogger
    {
        // Holds the single instance (starts as null)
        private static AppLogger? _instance;

        // Private constructor => nobody outside can write "new AppLogger()"
        private AppLogger()
        {
            Console.WriteLine("AppLogger instance created (this message prints only once).");
        }

        // Creates the instance on the first call only, then always returns the same one
        public static AppLogger GetLogger()
        {
            if (_instance is null)
            {
                _instance = new AppLogger();
            }
            return _instance;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[LOG] {message}");
        }
    }
}

// ---------------------------- TextHelper.cs ----------------------------
namespace Session09App
{
    // Q9: extension methods must live in a NON-generic STATIC class,
    // and the first parameter uses the "this" keyword.
    public static class TextHelper
    {
        // Bonus - step 1: the normal static method looked like this:
        //     public static bool IsShorterThan(string value, int length) => value.Length < length;
        //     usage: TextHelper.IsShorterThan("Stethoscope", 5);
        //
        // Step 2: adding "this" turns it into an extension method:
        public static bool IsShorterThan(this string value, int length)
        {
            return value.Length < length;
        }

        // "Ha".Repeat(3) => "HaHaHa"
        public static string Repeat(this string value, int times)
        {
            if (times < 0)
                throw new ArgumentOutOfRangeException(nameof(times), "times cannot be negative.");

            return string.Concat(Enumerable.Repeat(value, times));
        }
    }
}

// ---------------------------- FineCalculator.cs ----------------------------
namespace Session09App
{
    // Q11
    public class FineCalculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int Divide(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }
}

// ---------------------------- Program.cs ----------------------------
namespace Session09App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // =====================================================
            // Part 1 - Primary Constructor & Records
            // =====================================================
            Console.WriteLine("========== Part 1: Patient (class) ==========");

            // Q2: two patients with the SAME values
            Patient patient01 = new Patient(1, "Ahmed Ali", "01012345678", "Diabetes");
            Patient patient02 = new Patient(1, "Ahmed Ali", "01012345678", "Diabetes");

            Console.WriteLine(patient01);

            // 1) GetHashCode
            Console.WriteLine($"patient01.GetHashCode() = {patient01.GetHashCode()}");
            Console.WriteLine($"patient02.GetHashCode() = {patient02.GetHashCode()}");
            // Answer: the hash codes are DIFFERENT. Patient is a class (reference type) and
            // does not override GetHashCode, so the default implementation from System.Object
            // is used, which is based on the object's identity (its reference), not its values.

            // 2) Equals
            Console.WriteLine($"patient01.Equals(patient02) = {patient01.Equals(patient02)}");
            // Answer: False. The default Object.Equals for classes compares REFERENCES.
            // patient01 and patient02 are two different objects in memory,
            // even though all their property values are identical.

            // 3) Assign patient01 = patient02, then Equals again
            patient01 = patient02;
            Console.WriteLine($"After patient01 = patient02 => Equals = {patient01.Equals(patient02)}");
            // Answer: True. Now both variables point to the SAME object in memory
            // (same reference), so the reference comparison succeeds.

            // ------------------------------------------------------
            // Q3: positional record PatientDto
            // ------------------------------------------------------
            Console.WriteLine("\n========== Part 1: PatientDto (record) ==========");

            PatientDto dto01 = new PatientDto(1, "Ahmed Ali", "01012345678");
            PatientDto dto02 = new PatientDto(1, "Ahmed Ali", "01012345678");

            Console.WriteLine(dto01); // records generate a readable ToString automatically
            Console.WriteLine($"dto01.GetHashCode() = {dto01.GetHashCode()}");
            Console.WriteLine($"dto02.GetHashCode() = {dto02.GetHashCode()}");
            Console.WriteLine($"dto01.Equals(dto02) = {dto01.Equals(dto02)}");
            Console.WriteLine($"dto01 == dto02      = {dto01 == dto02}");

            dto01 = dto02;
            Console.WriteLine($"After dto01 = dto02 => Equals = {dto01.Equals(dto02)}");
            // Difference between class and record:
            // - class  : Equals/GetHashCode use REFERENCE equality by default. Two objects with
            //            identical data are "not equal" and have different hash codes.
            // - record : Equals/GetHashCode use VALUE equality (the compiler compares every
            //            property). Two different objects with identical data are equal
            //            and have the SAME hash code. The == operator also compares values.
            // So we get value-based comparison for free with a record, and it stays true
            // even after the assignment.

            // ------------------------------------------------------
            // Q4: mapper
            // ------------------------------------------------------
            Console.WriteLine("\n========== Part 1: Mapper ==========");
            Patient fullPatient = new Patient(2, "Mona Hassan", "01198765432", "Asthma");
            PatientDto mappedDto = PatientMapper.MapFromModelToDto(fullPatient);
            Console.WriteLine(mappedDto); // MedicalHistory is not exposed

            // =====================================================
            // Part 2 - Singleton
            // =====================================================
            Console.WriteLine("\n========== Part 2: Singleton ==========");

            // Q6: call GetLogger() four times
            AppLogger logger1 = AppLogger.GetLogger();
            AppLogger logger2 = AppLogger.GetLogger();
            AppLogger logger3 = AppLogger.GetLogger();
            AppLogger logger4 = AppLogger.GetLogger();

            Console.WriteLine($"logger1 hash = {logger1.GetHashCode()}");
            Console.WriteLine($"logger2 hash = {logger2.GetHashCode()}");
            Console.WriteLine($"logger3 hash = {logger3.GetHashCode()}");
            Console.WriteLine($"logger4 hash = {logger4.GetHashCode()}");
            logger1.Log("Singleton works!");
            // Observation: all four hash codes are IDENTICAL.
            // Why: the constructor is private, so the only way to get an object is GetLogger().
            // The first call creates the instance and stores it in the private static field;
            // every later call finds the field is not null and returns the SAME object,
            // so all four variables reference one single instance.

            // =====================================================
            // Part 3 - var & dynamic
            // =====================================================
            Console.WriteLine("\n========== Part 3: var & dynamic ==========");

            // 1) target-typed new: the type is written on the left, "new" infers it
            Patient p = new(10, "Khaled Samy", "01255555555", "None");

            // 2) var: the compiler infers the type from the right side
            var p2 = new Patient(11, "Nour Adel", "01066666666", "Allergy");

            // 3) dynamic
            dynamic d = new Patient(12, "Salma Omar", "01577777777", "Hypertension");
            Console.WriteLine(p);
            Console.WriteLine(p2);
            Console.WriteLine(d);
            Console.WriteLine($"d.FullName = {d.FullName}"); // member lookup happens at run time

            // 4) var vs dynamic:
            // - var     : the type is resolved at COMPILE time (implicitly typed, but still
            //             strongly typed). After "var p2 = new Patient(...)", p2 IS a Patient
            //             forever; you get IntelliSense and compile-time errors.
            // - dynamic : the type is resolved at RUN time. The compiler does NOT check the
            //             members you call; a wrong member name (d.Foo) compiles fine and
            //             fails with a RuntimeBinderException while the program is running.

            // =====================================================
            // Part 4 - Anonymous Types
            // =====================================================
            Console.WriteLine("\n========== Part 4: Anonymous Types ==========");

            var doctor01 = new { Name = "Sara", Specialty = "Cardiology", ExperienceYears = 8, Salary = 25_000 };
            var doctor02 = new { Name = "Sara", Specialty = "Cardiology", ExperienceYears = 8, Salary = 25_000 };

            Console.WriteLine($"1) {doctor01.Name} - {doctor01.Specialty}");
            Console.WriteLine($"2) doctor01 hash = {doctor01.GetHashCode()} | doctor02 hash = {doctor02.GetHashCode()}");
            Console.WriteLine($"3) Type: {doctor01.GetType()}");
            Console.WriteLine($"4) doctor01.Equals(doctor02) = {doctor01.Equals(doctor02)}");
            Console.WriteLine($"5) ToString: {doctor01.ToString()}");
            // Comparison:
            // - Anonymous type: the compiler overrides Equals/GetHashCode/ToString to use VALUE
            //   equality, so two anonymous objects with the same property names, types, order
            //   and values are equal and have the same hash code (like a record).
            //   It is also immutable (read-only properties) and its type name is compiler-generated.
            // - class : reference equality by default (equal only if it is the same object),
            //           unless you override Equals/GetHashCode yourself.
            // - record: value equality generated by the compiler, and it is a named type you
            //           can use as a parameter or return type (anonymous types cannot).

            // =====================================================
            // Part 5 - Extension Methods
            // =====================================================
            Console.WriteLine("\n========== Part 5: Extension Methods ==========");

            // Q10
            bool isShorter = "Stethoscope".IsShorterThan(5);
            string repeated = "Ab".Repeat(4);
            Console.WriteLine($"\"Stethoscope\".IsShorterThan(5) = {isShorter}");
            Console.WriteLine($"\"Ab\".Repeat(4) = {repeated}");

            // =====================================================
            // Part 6 - FineCalculator (tested in the xUnit project)
            // =====================================================
            Console.WriteLine("\n========== Part 6: FineCalculator ==========");
            FineCalculator calculator = new FineCalculator();
            Console.WriteLine($"10 + 5 = {calculator.Add(10, 5)}");
            Console.WriteLine($"10 - 5 = {calculator.Subtract(10, 5)}");
            Console.WriteLine($"10 * 5 = {calculator.Multiply(10, 5)}");
            Console.WriteLine($"10 / 5 = {calculator.Divide(10, 5)}");
            Console.WriteLine("Run the tests from Test Explorer (Ctrl+E, T).");
        }
    }
}