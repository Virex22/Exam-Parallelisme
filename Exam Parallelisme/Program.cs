/*
 * Evaluation sur le parallélisme
 * REMY Vincent
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Exam_Parallelisme
{
    class Program
    {
        static async Task<int> SumWordAsync(string path)
         {
              var content = await File.ReadAllTextAsync(path);
              var words = content.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
              return words.Length;
         }

        static async Task<int> SumLoremAsync(string path)
        {
             var content = await File.ReadAllTextAsync(path);
             var lorem = content.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
             return lorem.Count(x => x == "Lorem");
        }
        
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var sum = Enumerable.Range(1, 3000).AsParallel()
                             .Sum();

            var sumWord1 = SumWordAsync("Eval_file1.txt");
            var sumWord2 = SumWordAsync("Eval_file2.txt");
            var sumLorem1 = SumLoremAsync("Eval_file1.txt");
            var sumLorem2 = SumLoremAsync("Eval_file2.txt");

            Task.WaitAll(sumWord1, sumWord2, sumLorem1, sumLorem2);

            Console.WriteLine($"Somme de 1 à 3000 : {sum}");
            Console.WriteLine($"Nombre de mots dans le fichier 1 : {sumWord1.Result}");
            Console.WriteLine($"Nombre de mots dans le fichier 2 : {sumWord2.Result}");
            Console.WriteLine($"Nombre de Lorem dans le fichier 1 : {sumLorem1.Result}");
            Console.WriteLine($"Nombre de Lorem dans le fichier 2 : {sumLorem2.Result}");
            Console.WriteLine($"Total des résultats : {sum + sumWord1.Result + sumWord2.Result + sumLorem1.Result + sumLorem2.Result}");
   
            sw.Stop();
            
            Console.WriteLine($"Temps d'exécution : {sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}