using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NettyBaseReloaded.Main.objects
{
    class Cronjob
    {
        public static DebugLog Log = new DebugLog("cronlog");

        /// <summary>
        /// Cronjob ID - used for SQL identification
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Time schuelded for execution of the cronjob
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// If it's gonna ever happen again
        /// </summary>
        public bool RepeatedTask { get; set; }

        /// <summary>
        /// Time between executions / Delay (0 if a one time only execution)
        /// </summary>
        public int Intervals { get; set; }

        public string ExecuteStr { get; set; }

        protected Cronjob() { }

        public void Execute()
        {
            if (ExecuteStr.StartsWith("run"))
            {
                var p = new Process();
                p.StartInfo = FormatToProcessStartInfo();
                p.OutputDataReceived += (sender, args) => Log.Write($"Data received from process: {args.Data} [{p.Id}]");
                p.ErrorDataReceived += (sender, args) => Log.Write($"Error: {args.Data} [{p.Id}]");
                p.Exited += (sender, args) => Log.Write($"Process exited [{p.Id}]");
                p.Start();
                Log.Write($"{p.StandardOutput.ReadToEndAsync().Result} [{p.Id}]");
                Log.Write($"Process started with ID: {p.Id}");
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
            }
            ExecutionTime = DateTime.Now;
            PostExecution();
        }

        private void PostExecution()
        {
            if (RepeatedTask)
                Global.QueryManager.UpdateCronjob(this);
            else
                Remove();
        }

        /// <summary>
        /// Example run cronjob:
        /// run>cmd.exe -ping 213.32.95.48
        /// </summary>
        /// <returns></returns>
        public string ReformatExecuteStr()
        {
            return ExecuteStr.Contains('>') ? ExecuteStr.Split(' ')[1] : ExecuteStr;
        }

        public ProcessStartInfo FormatToProcessStartInfo()
        {
            var fullRunStr = ReformatExecuteStr();
            var path = fullRunStr.Split(' ')[0];
            var args = fullRunStr.Replace($"{path} ", "");
            var pInfo = new ProcessStartInfo(path,args);
            pInfo.CreateNoWindow = true;
            pInfo.RedirectStandardOutput = true;
            return pInfo;
        }

        public void Tick()
        {
            if (ExecutionTime.AddMinutes(Intervals) < DateTime.Now) Execute();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
