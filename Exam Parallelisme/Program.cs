/*
 * Evaluation sur le parallélisme
 * REMY Vincent
 */

/*
 * En utilisant la class Task et la class Parralel, faire :
 * 1/ Calcul la somme des nombres de 1 -> 3000
 * 2/ une fois fini, faire en même temps :
 *  a/ Traiter 2 fichier Eval_file1.txt et Eval_file2.txt pour calculer leurs nombre de mots
 *  b/ Traiter 2 fichier Eval_file1.txt et Eval_file2.txt pour calculer le nombre de "Lorem"
 * 3/ Une fois fini afficher la somme total des 5 résultats 
 * ( somme 1-3000, mot fichier 1, mot fichier 2, lorem fichier 1, lorem fichier 2)
 */

/*
 * Précision, il faut utiliser un maximum le paralelisme
 * Il faut aussi afficher le temps de tous ce traitement et le résultat final
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