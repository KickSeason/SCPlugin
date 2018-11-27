using System;
using System.Linq;
using System.Globalization;
using System.IO;
using Neo;
using Neo.VM;
using Neo.Plugins;
using Neo.SmartContract;

namespace Neo.Plugins
{
    public class SCPlugin : Plugin, ICmdPlugin
    {
        public override string Name { get { return "testsc"; } }
        public bool Exec(string[]args)
        {
            if (CheckArgs(args, 1))
            {
                Console.WriteLine(args[0]);
                var script = File.ReadAllBytes(args[0]);
                Console.WriteLine("script: {0}", script.ToHexString());
                return InvokeScript(script);
            }
            Console.WriteLine("wrong args count.");
            return false;
        }
        private bool CheckArgs(string[] args, int n)
        {
            if (args.Length < n) return false;
            return true;
        }
        private bool InvokeScript(byte[] script)
        {
            ApplicationEngine engine = ApplicationEngine.Run(script, null, null, true, new Fixed8(1000));
            Console.WriteLine("state:{0}", engine.State);
            Console.WriteLine("gas_consumed:{0}", engine.GasConsumed.ToString());
            Console.Write("stack: [\n");
            foreach (StackItem p in engine.ResultStack)
            {
                Console.Write(p.ToParameter().ToString());
                Console.Write(",\n");
            }
            Console.Write("]\n");
            return true;
        }
    }
}
