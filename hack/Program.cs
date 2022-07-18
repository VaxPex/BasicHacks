using Memory;
using System.Diagnostics;

namespace MC
{
    class modules
    {
        public class moduleBase
        {
            private bool enabled = false;
            protected static Mem mem = new Mem();
            protected static int PID = mem.GetProcIdFromName("Minecraft.Windows.exe");

            public void Sync()
            {
                enabled = true;
                if (PID > 0)
                {
                    mem.OpenProcess(PID);
                } else
                {
                    Console.WriteLine("Minecraft Not Found");
                }
            }

            public void UnSync()
            {
                enabled = false;
                if (PID > 0)
                {
                    mem.OpenProcess(PID);
                }
                else
                {
                    Console.WriteLine("Minecraft Not Found");
                }
            }

            public bool isEnabled()
            {
                return enabled == true;
            }
        }

        public class Reach : moduleBase
        {
            public void Synchronize(string value) : base()
            {
                mem.WriteMemory("Minecraft.Windows.exe+0x3A224B0", "float", value);
                Console.WriteLine("Reach Set");
            }

            public void UnSynchronize() : base()
            {
                mem.WriteMemory("Minecraft.Windows.exe+0x3A224B0", "float", "3.0");
                Console.WriteLine("Reach Module UnInitialized");
            }
        }

        public static void DisableAll()
        {
            new Reach().UnSynchronize();
        }
    }
}

namespace Program
{
    class Utils
    {
        public static void KillCurrentProcess()
        {
            Process.GetCurrentProcess().Kill();
        }
    }

    class Program
    {
       
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Help List:");
            Console.WriteLine("reach -> enable or disable reach");
            Console.WriteLine("disable -> disable all modules");
            Console.WriteLine("exit -> exit");
            var reachModule = new MC.modules.Reach();
            while (true)
            {
                var line = Console.ReadLine().ToLower();
                if (line.Split(" ")[0] == "reach")
                {
                    if (line.Split(" ").Count() == 2)
                    {
                        var amount = line.Split(" ")[1];
                        if (!amount.StartsWith("0"))
                        {
                            reachModule.Synchronize(amount);
                        }
                    } else
                    {
                        Console.WriteLine("Usage: reach <amount>");
                    }
                }
                else if (line == "disable")
                {
                    MC.modules.DisableAll();
                } else if (line == "exit")
                {
                    MC.modules.DisableAll();
                    Utils.KillCurrentProcess();
                } else
                {
                    Console.WriteLine("Unknown Command");
                }
            }
        }
    }
}
