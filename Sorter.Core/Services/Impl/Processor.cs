using Microsoft.Extensions.Logging;
using Sorter.Core.Model;
using System.Diagnostics;
using System.Text;

namespace Sorter.Core.Services
{
    public class Processor : IProcessor
    {
        private ITreeGenerator _treeGenerator;
        private ITreeDataSaver _dataSaver;

        private ILogger<Processor> _logger;

        public Processor(ITreeGenerator treeGenerator, ITreeDataSaver dataSaver, ILogger<Processor> logger)
        {
            _treeGenerator = treeGenerator;
            _dataSaver = dataSaver;
            _logger = logger;
        }

        public void Process(string inputFile, string outputFile, int batchSizeInMegabytes)
        {
            if (!Directory.Exists("temp"))
                Directory.CreateDirectory("temp");

            var stopWatch = new Stopwatch();
            stopWatch.Start();


            var treeMaxStringLength = 0;
            var interationNum = 0;
            var fileBuffer = new byte[(batchSizeInMegabytes + 1) * 1024 * 1024];
            using (var reader = File.OpenText(inputFile))
            {

                while (true)
                {
                    _logger.LogInformation($"Tree generation started: ");
                    var tree = _treeGenerator.GenerateTree(reader, fileBuffer);

                    if (tree == null)
                        break;

                    if (tree.GetMaxStringLength() > treeMaxStringLength)
                        treeMaxStringLength = tree.GetMaxStringLength();

                    var seconds = stopWatch.ElapsedMilliseconds / 1000;
                    Console.WriteLine($"Tree generation done in {seconds} секунд");

                    using (var fileWriter = new FileWriter(Path.Combine("temp", interationNum.ToString()), fileBuffer))
                    {
                        _dataSaver.SaveSortedData(tree, fileWriter);
                    }

                    seconds = stopWatch.ElapsedMilliseconds / 1000;
                    Console.WriteLine($"Sorting done in {seconds} seconds");

                    interationNum++;
                }
            }

            Console.WriteLine($"Temp files generated...");

            if (interationNum == 1)
            {
                File.Move(Path.Combine("temp", "0"), outputFile, true);

                var seconds = stopWatch.ElapsedMilliseconds / 1000;
                Console.WriteLine($"All done in {seconds} seconds");

                return;
            }

            using (var fileWriter = new FileWriter(outputFile, fileBuffer))
            {
                _logger.LogInformation($"File created '{outputFile}'");



                //var tree = new SmallFileTree();
                //var treeRowSaver = new TreeRowSaver(tree, fileWriter, 100);
                var smallFiles = new SmallFileReader[interationNum];
                var rows = new List<FileRow>(interationNum);
                // инициализация дерева
                for (var i = 0; i < interationNum; i++)
                {
                    smallFiles[i] = new SmallFileReader();
                    smallFiles[i].Open(i, (double)batchSizeInMegabytes / interationNum);
                    rows.Add(new FileRow(smallFiles[i].GetNextString(), i));
                }

                var comparer = new FileRowComparer();
                while (true)
                {
                    var minRow = rows.Min(comparer);

                    if (minRow == null)
                        break;

                    fileWriter.WriteToFile(minRow.Number, minRow.String, minRow.String.Length);
                    rows.Remove(minRow);

                    if (!smallFiles[minRow.FileNum].IsClosed())
                    {
                        var row = smallFiles[minRow.FileNum].GetNextString();
                        if (row != null)
                            rows.Add(new FileRow(row, minRow.FileNum));
                    }
                        
                }


                stopWatch.Stop();

                var seconds = stopWatch.ElapsedMilliseconds / 1000;
                Console.WriteLine($"File '{outputFile}' filled in {seconds.ToString("F")} seconds");
            }

        }


    }
}
