using System;

public delegate void MessageDelegate(string message);
public delegate double CalculationDelegate(double a, double b);
class Student
{
    public string Name { get; set; }
    public int Grade { get; set; }
    public int Age { get; set; }
}


public class NotificationService
{
    // TODO: Deklarasi delegate untuk event handler
    // public delegate void NotifyDelegate(string message);
    // public event NotifyDelegate OnNotification;
    public delegate void NotifyDelegate(string message);
    public event NotifyDelegate OnNotification;
    
    public void SendNotification(string message)
    {
        Console.WriteLine($"Mengirim: {message}");
        // TODO: Trigger event jika ada subscriber
    }
}





public class Program
{
    public static void ShowMessage(string text)
    {
        Console.WriteLine($"Pesan: {text}");
    }
    static double Add(double a, double b) => a + b;
    static double Subtract(double a, double b) => a - b;
    static double Multiply(double a, double b) => a * b;
    static double Divided(double a, double b) => b != 0 ? a / b : 0;
    static double Power(double a, double b) => a * b * b;

    static bool GradeFilter(Student number)
    {
        return number.Grade > 80;
    }
    static bool AgeFilter(Student number)
    {
        return number.Age < 21;
    }
    static List<Student> FilterStudents(List<Student> students, Predicate<Student> filter)
    {
        List<Student> result = new List<Student>();
        foreach (var student in students)
        {
            if (filter(student))
                result.Add(student);
        }
        return result;
    }
    static void PrintStudent(List<Student> students)
    {
        if (students.Count == 0)
        {
            System.Console.WriteLine("tidak ada data yang sesuai filter");
            return;
        }

        foreach (var student in students)
        {
            System.Console.WriteLine($"- {student.Name}, Grade: {student.Grade}, Age: {student.Age}");
        }
    }


     static void EmailNotification(string msg)
    {
        Console.WriteLine($"Email: {msg}");
    }
    
    static void SMSNotification(string msg)
    {
        Console.WriteLine($"SMS: {msg}");
    }


    public static void Main()
    {
        // List<Student> students = new List<Student>
        // {
        //     new Student { Name = "Alice", Grade = 85, Age = 20 },
        //     new Student { Name = "Bob", Grade = 92, Age = 22 },
        //     new Student { Name = "Charlie", Grade = 78, Age = 19 },
        //     new Student { Name = "Diana", Grade = 95, Age = 21 }
        // };

        // var highAchivers = FilterStudents(students, GradeFilter);
        // PrintStudent(highAchivers);
        // System.Console.WriteLine();
        // List<Student> youngStudent = FilterStudents(students, AgeFilter);
        // PrintStudent(youngStudent);
        // System.Console.WriteLine();

        // var topStudent = FilterStudents(students, s => s.Grade >= 90 && s.Age < 25);
        // PrintStudent(topStudent);
        // System.Console.WriteLine();

        // var alternativeFilter = students.FindAll(s => s.Grade < 80);
        // Console.WriteLine("\nSiswa dengan Grade < 80 (menggunakan FindAll):");
        // PrintStudent(alternativeFilter);






        // MessageDelegate messager = ShowMessage;
        // messager("hallo dunia");
        // CalculationDelegate calc1 = Add;
        // Console.WriteLine($"5 + 3 = {calc1(5, 3)}");
        // CalculationDelegate calc2 = Subtract;
        // Console.WriteLine($"5 - 3 = {calc2(5, 3)}");
        // CalculationDelegate calc3 = Multiply;
        // Console.WriteLine($"5 * 3 = {calc3(5, 3)}");
        // CalculationDelegate calc4 = Divided;
        // Console.WriteLine($"5 / 3 = {calc4(5, 3)}");
        // CalculationDelegate calc5 = Power;
        // Console.WriteLine($"5 ** 3 = {calc5(5, 3)}");



        NotificationService service = new NotificationService();

        // TODO: Subscribe method ke event
        service.OnNotification += EmailNotification;
        service.OnNotification += SMSNotification;
        // service.OnNotification += EmailNotification;
        // service.OnNotification += SMSNotification;
        
        service.SendNotification("Hello World!");
    }
}
