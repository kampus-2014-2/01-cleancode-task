using System;
using System.IO;
using NUnit.Framework;

namespace CleanCode
{
    [TestFixture]
    public class Chess_Test
    {
        [Test]
        public void Test()
        {
            int testsCount = 0;
            foreach (var file in Directory.GetFiles("ChessTests"))
            {
                if (Path.GetExtension(file) != string.Empty) continue;
                using (var f = File.OpenText(file))
                {
                    var actualAnswer = new Chess(new Board(f)).getWhiteStatus();
                    var expectedAnswer = File.ReadAllText(file + ".ans").Trim();
                    Assert.AreEqual(expectedAnswer, actualAnswer, "error in file " + file);
                }
                testsCount++;
            }
            Console.WriteLine("Tests count: " + testsCount);
        }
    }
}