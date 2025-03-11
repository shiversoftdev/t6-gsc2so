using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Irony.Parsing;

public static class Extensions
{
    public static void Replace<T>(this List<T> list, int index, List<T> input)
    {
        list.RemoveRange(index, input.Count);
        list.InsertRange(index, input);
    }
}

namespace GameScriptCompiler_v3
{
    internal class Program
    {
        public static IntPtr ProcessHandle = IntPtr.Zero;
        private static int ProcessID = -1;
        private static void WriteInternal(int address, byte[] bytes)
        {
            I.WriteProcessMemory(ProcessHandle, (IntPtr) address, bytes, (uint) bytes.Length, 0);
        }

        private static bool ProcessLoad()
        {
            Process[] processesByName = Process.GetProcessesByName("t6zm");
            if (processesByName.Length != 0)
            {
                ProcessID = processesByName[0].Id;
                ProcessHandle = I.OpenProcess(0x1f0fff, false, ProcessID);
                return true;
            }
            return false;
        }
    }

    public class ExternalEntry
    {
        public string Compile(string path, Dictionary<string, List<string>> available_keys, GSC2SO.Form1 ths)
        {
            var gameScript = new GSCGrammar();
            var parser = new Parser(gameScript);
            var compiler = new ScriptCompiler(parser.Parse(File.ReadAllText(path)), path, available_keys, ths);
            compiler.Init();
            return ScriptCompiler.ERROR_MSG;
        }
    }
}