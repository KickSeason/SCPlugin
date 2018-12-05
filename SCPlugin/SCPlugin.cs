using System;
using System.Linq;
using System.Globalization;
using System.IO;
using Neo;
using Neo.VM;
using Neo.Plugins;
using Neo.SmartContract;
using Neo.IO.Json;

namespace Neo.Plugins
{
    public class SCPlugin : Plugin, ICmdPlugin
    {
        public SCPlugin()
        {
            StandardService.Notify += this.notify;
            StandardService.Log += this.log;
        }
        public override string Name
        {
            get
            {
                return "testsc";
            }
        }
        public bool Exec(string[]args)
        {
            if (checkArgs(args, 1))
            {
                try
                {
                    var script = File.ReadAllBytes(args[0]);
                    Console.WriteLine("sc path: {0}", args[0]);
                    Console.WriteLine("script: {0}", script.ToHexString());
                    return InvokeScript(script);
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return true;
                }
            }
            Console.WriteLine("wrong args count.");
            return true;
        }
        public string HelpStr
        {
            get
            {
                var str = "\t" + this.Name + " [.avm] test your smart contract.\n";
                return str;
            }
        }
        private bool checkArgs(string[] args, int n)
        {
            if (args.Length < n) return false;
            return true;
        }
        public void notify(Object o, NotifyEventArgs args)
        {
            Console.WriteLine("[Notify] ScriptHash: {0} State: {1}", args.ScriptHash, args.State.ToParameter().ToJson().ToString());
        }
        public void log(Object o, LogEventArgs args)
        {
            Console.WriteLine("[Log] ScriptHash: {0} Message: {1}", args.ScriptHash, args.Message);
        }
        private bool InvokeScript(byte[] script)
        {
            ApplicationEngine engine = ApplicationEngine.Run(script, null, null, true, default(Fixed8));
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
