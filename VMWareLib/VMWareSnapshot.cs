using VixCOM;

namespace Vestris.VMWareLib
{
    /// <summary>
    /// A VMWare snapshot.
    /// </summary>
    public class VMWareSnapshot : VMWareVixHandle<ISnapshot>
    {
        private readonly IVM _vm;
        private VMWareSnapshotCollection _childSnapshots;

        /// <summary>
        /// A VMWare snapshot constructor.
        /// </summary>
        /// <param name="vm">virtual machine</param>
        /// <param name="snapshot">snapshot</param>
        /// <param name="parent">parent snapshot</param>
        public VMWareSnapshot(IVM vm, ISnapshot snapshot, VMWareSnapshot parent) : base(snapshot)
        {
            _vm = vm;
            Parent = parent;
        }

        /// <summary>
        /// Parent snapshot.
        /// </summary>
        /// <remarks>
        /// Root snapshots have a null parent.
        /// </remarks>
        public VMWareSnapshot Parent { get; set; }

        /// <summary>
        /// Restores the virtual machine to the state when the specified snapshot was created.
        /// </summary>
        /// <param name="powerOnOptions">additional power-on options</param>
        /// <param name="timeoutInSeconds">timeout in seconds</param>
        public void RevertToSnapshot(int powerOnOptions, int timeoutInSeconds)
        {
            var callback = new VMWareJobCallback();
            var job = new VMWareJob(_vm.RevertToSnapshot(Handle, powerOnOptions, null, callback), callback);
            job.Wait(timeoutInSeconds);
        }

        /// <summary>
        /// Restores the virtual machine to the state when the specified snapshot was created.
        /// </summary>
        /// <param name="timeoutInSeconds">timeout in seconds</param>
        public void RevertToSnapshot(int timeoutInSeconds)
        {
            RevertToSnapshot(Constants.VIX_VMPOWEROP_NORMAL, timeoutInSeconds);
        }

        /// <summary>
        /// Restores the virtual machine to the state when the specified snapshot was created.
        /// </summary>
        public void RevertToSnapshot()
        {
            RevertToSnapshot(VMWareInterop.Timeouts.RevertToSnapshotTimeout);
        }

        /// <summary>
        /// Remove/delete this snapshot.
        /// </summary>
        public void RemoveSnapshot()
        {
            RemoveSnapshot(VMWareInterop.Timeouts.RemoveSnapshotTimeout);
        }

        /// <summary>
        /// Remove/delete this snapshot.
        /// </summary>
        /// <remarks>
        /// If the snapshot is a member of a collection, the latter is updated with orphaned
        /// snapshots appended to the parent.
        /// </remarks>
        /// <param name="timeoutInSeconds">timeout in seconds</param>
        public void RemoveSnapshot(int timeoutInSeconds)
        {
            // remove the snapshot
            var callback = new VMWareJobCallback();
            var job = new VMWareJob(_vm.RemoveSnapshot(Handle, 0, callback), callback);
            job.Wait(timeoutInSeconds);
            if (Parent != null)
            {
                // child snapshots from this snapshot have now moved one level up
                Parent.ChildSnapshots.Remove(this);
            }
        }

        /// <summary>
        /// Child snapshots.
        /// </summary>
        public VMWareSnapshotCollection ChildSnapshots
        {
            get
            {
                if (_childSnapshots == null)
                {
                    var childSnapshots = new VMWareSnapshotCollection(_vm, this);
                    int nChildSnapshots;
                    VMWareInterop.Check(Handle.GetNumChildren(out nChildSnapshots));
                    for (int i = 0; i < nChildSnapshots; i++)
                    {
                        ISnapshot childSnapshot;
                        VMWareInterop.Check(Handle.GetChild(i, out childSnapshot));
                        childSnapshots.Add(new VMWareSnapshot(_vm, childSnapshot, this));
                    }
                    _childSnapshots = childSnapshots;
                }
                return _childSnapshots;
            }
        }

        /// <summary>
        /// Display name of the snapshot.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return GetProperty<string>(Constants.VIX_PROPERTY_SNAPSHOT_DISPLAYNAME);
            }
        }

        /// <summary>
        /// Display name of the snapshot.
        /// </summary>
        public string Description
        {
            get
            {
                return GetProperty<string>(Constants.VIX_PROPERTY_SNAPSHOT_DESCRIPTION);
            }
        }

        /// <summary>
        /// Complete snapshot path, from root.
        /// </summary>
        public string Path
        {
            get
            {
                ISnapshot parentSnapshot;
                ulong ulError;
                switch ((ulError = Handle.GetParent(out parentSnapshot)))
                {
                    case Constants.VIX_OK:
                        return System.IO.Path.Combine(new VMWareSnapshot(_vm, parentSnapshot, null).Path, DisplayName);
                    case Constants.VIX_E_SNAPSHOT_NOTFOUND: // no parent
                        return DisplayName;
                    case Constants.VIX_E_INVALID_ARG: // root snapshot
                        return string.Empty;
                    default:
                        throw new VMWareException(ulError);
                }
            }
        }

        /// <summary>
        /// The power state of this snapshot, an OR-ed set of VIX_POWERSTATE_* values.
        /// </summary>
        public int PowerState
        {
            get
            {
                return GetProperty<int>(Constants.VIX_PROPERTY_SNAPSHOT_POWERSTATE);
            }
        }

        /// <summary>
        /// Returns true if the snapshot is replayable.
        /// </summary>
        public bool IsReplayable
        {
            get
            {
                //return GetProperty<bool>(Constants.VIX_PROPERTY_SNAPSHOT_IS_REPLAYABLE);
                return true;
            }
        }
    }
}
