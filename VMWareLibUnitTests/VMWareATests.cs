using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Vestris.VMWareLib;
using System.Configuration;

namespace Vestris.VMWareLibUnitTests
{
    [TestFixture]
    public class VMWareATests
    {
        [Test]
        public async void Test()
        {
            var virtualHost = new VMWareVirtualHost();
            virtualHost.ConnectToVMWareWorkstation();
            var virtualMachine = virtualHost.Open(@"C:\Users\LionSoft\Documents\Virtual Machines\Windows XP Professional\Windows XP Professional.vmx");
            //virtualMachine.PowerOn();
            await virtualMachine.PowerOnAsync();


            // login to the virtual machine
            await virtualMachine.LoginAsync("Lion", "noilsoft");

            var y = virtualMachine.GuestEnvironmentVariables["Path"];
            
            // run notepad
            virtualMachine.DetachProgramInGuest("notepad.exe", string.Empty);
            // create a new snapshot
            var name = "New Snapshot";
            // take a snapshot at the current state
            virtualMachine.Snapshots.CreateSnapshot(name, "test snapshot");
            // power off
            virtualMachine.PowerOff();
            // find the newly created snapshot
            var snapshot = virtualMachine.Snapshots.GetNamedSnapshot(name);
            // revert to the new snapshot
            snapshot.RevertToSnapshot();
            // delete snapshot
            snapshot.RemoveSnapshot();
        }

        private Task GetTask(int timeout)
        {
            return new Task(() =>
            {
                Thread.Sleep(timeout);
                Console.WriteLine(timeout);
            });
        }

        private IEnumerable<Task> GetTasks()
        {
            yield return GetTask(3000);
            yield return GetTask(2000);
            yield return GetTask(1000);
        }

        private Task RunTasks(IEnumerable<Task> tasks)
        {
            return tasks
                .Aggregate((Task)null, (current, task) =>
                {
                    var res = task;
                    if (current == null)
                        task.Start();
                    else
                        res = current.ContinueWith(x => { task.Start(); return task; }).Unwrap();
                    return res;
                });
        }
            
            
        [Test]
        public async void TestAsyncChain()
        {
            await RunTasks(GetTasks());
        }
    }
}
