using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class Lesson
    {
        public double Homework1Percent; // Ödev 1 yüzdelik
        public double Homework2Percent; // Ödev 2 yüzdelik
        public double VisaPercent;      // Vize yüzdelik
        public double FinalPercent;     // Final yüzdelik
    }
    public class Students
    {
        public string Name;     // Öğrenci ismi
        public string Surname;  // Öğrenci soyismi
        public string StudentID; // Öğrenci numarası
        public double Homework1; // Öğrenci ödev 1 notu
        public double Homework2; // Öğrenci ödev 2 notu
        public double Visa;      // Öğrenci vize notu
        public double Final;    // Öğrenci final notu
        public string GradeNote;   // Öğrenci harf notu
        public double calculateGrade(double Homework1, double Homework2, double Visa, double Final, Lesson lesson) // Not hesaplayan fonksiyon
        {

            return ((Homework1 * lesson.Homework1Percent) + (Homework2 * lesson.Homework2Percent) + (Visa * lesson.VisaPercent) + (Final * lesson.FinalPercent)) / 100;
        }


    }
    public static class Operations
    {
        public static double CalcClassAvg(List<Students> students, Lesson lesson)
        {
            double allNotes = 0;

            foreach (var item in students) // Not hesaplıyor
            {
                double note = item.calculateGrade(item.Homework1, item.Homework2, item.Visa, item.Final, lesson);
                allNotes += note;
            }

            return allNotes / students.Count;
        }
        public static string CalcGradeNote(double decimalAverage) // Harf notu aralığı
        {
            if (decimalAverage <= 100 && decimalAverage >= 90)
                return "AA";
            else if (decimalAverage < 90 && decimalAverage >= 85)
                return "BA";
            else if (decimalAverage < 85 && decimalAverage >= 80)
                return "BB";
            else if (decimalAverage < 80 && decimalAverage >= 75)
                return "CB";
            else if (decimalAverage < 75 && decimalAverage >= 65)
                return "CC";
            else if (decimalAverage < 65 && decimalAverage >= 58)
                return "DC";
            else if (decimalAverage < 58 && decimalAverage >= 50)
                return "DD";
            else if (decimalAverage < 50 && decimalAverage >= 40)
                return "FD";
            else if (decimalAverage < 40 && decimalAverage >= 0)
                return "FF";
            else
            {
                return "Hatalı değerler mevcut";
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var NDP = new Lesson // NDP dersi için örnek yüzdelik oluşturuluyor.
            {
                Homework1Percent = 19,
                Homework2Percent = 11,
                VisaPercent = 25,
                FinalPercent = 45
            };

            string read;
            StreamReader file = new StreamReader(@"g181210092_text1_soru1.txt");
            List<Students> students = new List<Students>();
            while ((read = file.ReadLine()) != null) // Dosyayı okuyor.
            {
                var studentData = read.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var tmpStudent = new Students
                {
                    Name = studentData[0],
                    Surname = studentData[1],
                    StudentID = studentData[2],
                    Homework1 = Convert.ToDouble(studentData[3]),
                    Homework2 = Convert.ToDouble(studentData[4]),
                    Visa = Convert.ToDouble(studentData[5]),
                    Final = Convert.ToDouble(studentData[6])
                };

                tmpStudent.GradeNote = Operations.CalcGradeNote(tmpStudent.calculateGrade(tmpStudent.Homework1, tmpStudent.Homework2, tmpStudent.Visa, tmpStudent.Final, NDP));
                students.Add(tmpStudent);
            }
            foreach (var student in students) // Listedeki verileri yazdırıyor.
            {
                Console.WriteLine(student.Name + " " + student.Surname + " " + student.StudentID + " " + student.Homework1 + " " + student.Homework2 + " " + student.Visa + " " + student.Final + " " + student.GradeNote);
                Console.Write("\n");
            }


            double classPopulation = students.Count;
            double percent;
            var groupedByGradeNote = students.GroupBy(x => x.GradeNote).Select(y => new { y.Key, Count = y.Count() });

            StreamWriter streamWriter = new StreamWriter("g181210092_text2_soru2.txt");

            foreach (var item in groupedByGradeNote)  // Harf notunu, yüzdeliğini ve öğrenci sayısını yazdırıyor.
            {
                percent = (item.Count * 100.0) / classPopulation;
                streamWriter.WriteLine("Harf Notu: " + item.Key + " \nAlan Kişi Sayısı: " + item.Count + " \nHarf Notunun Sınıfa Göre Yüzdesi: %" + percent + "\n\n");
            }

            streamWriter.Close();

            Console.WriteLine("Sınıf Ortalaması:..." + Operations.CalcClassAvg(students, NDP));
        }
    }
}
